import {Col, Container, Row, Stack, Table} from "react-bootstrap";
import {NavLink, useParams} from "react-router-dom";
import {useQuery} from "@tanstack/react-query";
import socialEventsApi from "../../api/socialEventsApi.ts";
import {toast} from "sonner";
import queryKeys from "../../api/QueryKeys.ts";
import {useLoader} from "../../contexts/LoaderContext.tsx";
import AddParticipants from "./components/AddParticipants";
import {format} from "date-fns";
import socialEventCompaniesApi from "../../api/socialEventCompaniesApi.ts";

export default function Participants() {
  const {eventId} = useParams();
  const {setLoading} = useLoader();

  const {data: socialEvent, error: getSocialEventError, isLoading: isLoading} = useQuery({
    queryKey: [queryKeys.SOCIAL_EVENT],
    queryFn: () => {
      return socialEventsApi.getById(eventId)
    },
    enabled: !!eventId
  });

  const {data: companies, error: getCompaniesError} = useQuery({
    queryKey: [queryKeys.COMPANIES_BY_SOCIAL_EVENT_ID],
    queryFn: () => {
      if (!socialEvent?.id) {
        return;
      }

      return socialEventCompaniesApi.getBySocialEventId(socialEvent?.id)
    }
  });

  if (getSocialEventError || getCompaniesError) {
    toast.error("Midagi läks valesti");
  }

  setLoading(isLoading);

  return (
    <Container className={"shadow-sm bg-white"}>
      <Row>
        <Col className={"bg-primary d-flex align-items-center justify-content-center"} md={6}>
          <h1 className={"text-white"}>Osavõtjad</h1>
        </Col>
        <Col className={"background-image-col"}/>
      </Row>
      <Row className={"justify-content-center"}>
        <Col md={16} lg={10} className={"p-4"}>
          <Row><Col><h2 className={"text-primary"}>Osavõtjad</h2></Col></Row>
          <Row>
            <Col>
              <Stack gap={3}>
                <Row>
                  <Col sm={24} md={8}>Ürituse nimi:</Col>
                  <Col md={16}>
                    {socialEvent?.name}
                  </Col>
                </Row>
                <Row>
                  <Col sm={24} md={8}>Toimumise aeg:</Col>
                  <Col md={16}>
                    {socialEvent?.date && format(new Date(socialEvent?.date), 'dd.mm.yyyy')}
                  </Col>
                </Row>
                <Row>
                  <Col sm={24} md={8}>Koht:</Col>
                  <Col md={16}>
                    {socialEvent?.location}
                  </Col>
                </Row>
                <Row>
                  <Col>
                    <Row>
                      <Col sm={24} md={8}>Osavõtjad:</Col>
                    </Row>
                    <Row>
                      <Col md={{span: 16, offset: 8}}>
                        <Table borderless>
                          <tbody>
                          {companies?.data.map((x, index) => (
                            <tr key={x.id}>
                              <th scope={"row"} className={"text-end px-0"}>{index + 1}.</th>
                              <td>{x.name}</td>
                              <td className={"col-4"}>{x.registerCode}</td>
                              <td className={"col-3"}>
                                <NavLink className={"nav nav-link p-0"} to={`/social-events/${x.id}`}>VAATA</NavLink>
                              </td>
                            </tr>
                          ))}
                          </tbody>
                        </Table>
                      </Col>
                    </Row>
                    <AddParticipants socialEvent={socialEvent}/>
                  </Col>
                </Row>
              </Stack>
            </Col>
          </Row>
        </Col>
      </Row>
    </Container>
  );
}
