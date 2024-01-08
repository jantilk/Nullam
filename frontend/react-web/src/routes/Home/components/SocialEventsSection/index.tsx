import {
  Row,
  Col,
  Card,
  CardHeader,
  CardFooter,
  Table,
  CardBody,
  Modal,
  Button
} from 'react-bootstrap';
import {useQuery} from "@tanstack/react-query";
import socialEventsApi, {SortingOption} from "../../../../api/socialEventsApi.ts";
import {NavLink} from "react-router-dom";
import queryKeys from "../../../../api/QueryKeys.ts";
import {ReactNode, useState} from "react";
import RemoveIcon from '/public/remove.svg?react'

interface AppCardProps {
  children?: ReactNode;
  title: string;
}

interface CurrentEvent {
  id: string | null;
  name: string;
}

const SocialEventCard = ({children, title}: AppCardProps) => {
  return (
    <Card className={"shadow-sm"} style={{height: '320px'}}>
      <CardHeader className="text-white bg-primary">
        <h2 className={"m-0"}>{title}</h2>
      </CardHeader>
      {children}
    </Card>
  );
}

export default function SocialEventsSection() {
  const [showModal, setShowModal] = useState(false);
  const [currentEvent, setCurrentEvent] = useState<CurrentEvent>({id: null, name: ""});

  const handleDelete = async () => {
    if (currentEvent.id) {
      try {
        await socialEventsApi.delete(currentEvent.id);
      } catch (error) {
        console.error('Delete operation failed:', error);
      }
    }
  };

  const openModal = (eventId: string, eventName: string) => {
    setCurrentEvent({id: eventId, name: eventName});
    setShowModal(true);
  };

  const ConfirmationModal = () => (
    <Modal show={showModal} onHide={() => setShowModal(false)}>
      <Modal.Header closeButton>
        <Modal.Title>Kustuta "<strong>{currentEvent.name}</strong>"</Modal.Title>
      </Modal.Header>
      <Modal.Body>Oled kindel, et soovid üritust kustutada?</Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={() => setShowModal(false)}>
          Tühista
        </Button>
        <Button variant="danger" onClick={handleDelete}>
          Kustuta
        </Button>
      </Modal.Footer>
    </Modal>
  );

  const {data: futureSocialEvents, error: futureError} = useQuery({
    queryKey: [queryKeys.FUTURE_SOCIAL_EVENTS],
    queryFn: () => {
      const today = new Date();
      const filter = {StartDate: today};
      const orderBy = SortingOption.DateAsc;
      return socialEventsApi.get({orderBy, filter});
    }
  });

  const {data: pastSocialEvents, error: pastError} = useQuery({
    queryKey: [queryKeys.PAST_SOCIAL_EVENTS],
    queryFn: () => {
      const today = new Date();
      const filter = {EndDate: today};
      const orderBy = SortingOption.DateDesc;
      return socialEventsApi.get({orderBy, filter});
    }
  });

  // if (futureError || pastError) {
  //   return <div>Error: {futureError?.message || pastError?.message}</div>;
  // }

  return (
    <Row>
      <Col lg={12}>
        <SocialEventCard title={"Tulevased üritused"}>
          <CardBody>
            <Table borderless>
              <tbody>
              {futureSocialEvents && futureSocialEvents.map((x, index) => (
                <tr key={x.id}>
                  <th scope={"row"} className={"px-0"}>{index + 1}.</th>
                  <td>{x.name}</td>
                  {/*TODO: fix formatting*/}
                  <td className={"col-4"}>{new Date(x.date).toLocaleDateString()}</td>
                  <td className={"col-3"}>
                    <NavLink className={"nav nav-link p-0"} to={`/social-events/${x.id}`}>OSAVÕTJAD</NavLink>
                  </td>
                  <td className={"col-1"}>
                    <RemoveIcon
                      color="#7E7E7E"
                      style={{height: '18px'}}
                      className={"d-flex icon-hover custom-icon"}
                      type={"button"}
                      onClick={() => openModal(x.id, x.name)}
                    />
                  </td>
                </tr>
              ))}
              </tbody>
            </Table>
          </CardBody>
          <CardFooter className={"d-flex justify-content-start bg-light"}>
            <NavLink className={"nav nav-link p-0"} to={"add-social-event"}>LISA ÜRITUS</NavLink>
          </CardFooter>
        </SocialEventCard>
      </Col>
      <Col lg={12}>
        <SocialEventCard title={"Toimunud üritused"}>
          <CardBody>
            <Table borderless>
              <tbody>
              {pastSocialEvents && pastSocialEvents.map((x, index) => (
                <tr key={x.id}>
                  <th scope={"row"} className={"text-end px-0"}>{index + 1}.</th>
                  <td>{x.name}</td>
                  <td className={"col-4"}>{new Date(x.date).toLocaleDateString()}</td>
                  <td className={"col-3"}>
                    <NavLink className={"nav nav-link p-0"} to={`/social-events/${x.id}`}>OSAVÕTJAD</NavLink>
                  </td>
                </tr>
              ))}
              </tbody>
            </Table>
          </CardBody>
        </SocialEventCard>
      </Col>
      <ConfirmationModal/>
    </Row>
  );
}