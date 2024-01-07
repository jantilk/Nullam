import {
  Row,
  Col,
  Card,
  CardHeader,
  CardFooter,
  Table,
  CardBody
} from 'react-bootstrap';
import {useQuery} from "@tanstack/react-query";
import socialEventsApi, {SortingOption} from "../../../../api/socialEventsApi.ts";
import { NavLink } from "react-router-dom";
import queryKeys from "../../../../api/QueryKeys.ts";
import {ReactNode} from "react";
import RemoveIcon from '/public/remove.svg?react'

interface AppCardProps {
  children?: ReactNode;
  title: string;
}

const SocialEventCard = ({children, title} : AppCardProps) => {
  return (
    <Card className={"shadow-sm"} style={{height: '320px'}}>
      <CardHeader className="text-white bg-primary">
        <h2 className={"m-0"}>{title}</h2>
      </CardHeader>
      {children}
    </Card>
  );
}

const AppButton = () => {
  return (
    // TODO: move inline style
    <RemoveIcon
      color="#7E7E7E"
      style={{height: '18px'}}
      className={"d-flex icon-hover custom-icon"}
      type={"button"}
      onClick={() => {}}
    />
  );
}

export default function SocialEventsSection() {
  const { data: futureSocialEvents, error: futureError } = useQuery({
    queryKey: [queryKeys.FUTURE_SOCIAL_EVENTS],
    queryFn: () => {
      const today = new Date();
      const filter = { StartDate: today };
      const orderBy = SortingOption.DateAsc;
      return socialEventsApi.get({orderBy, filter});
    }
  });

  const { data: pastSocialEvents, error: pastError } = useQuery({
    queryKey: [queryKeys.PAST_SOCIAL_EVENTS],
    queryFn: () => {
      const today = new Date();
      const filter = { EndDate: today };
      const orderBy = SortingOption.DateDesc;
      return socialEventsApi.get({orderBy, filter});
    }
  });

  if (futureError || pastError) {
    return <div>Error: {futureError?.message || pastError?.message}</div>;
  }

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
                    <td className={"col-4"}>{new Date(x.date).toLocaleDateString()}</td>
                    <td className={"col-3"}>
                      <NavLink className={"nav nav-link p-0"} to={`/social-events/${x.id}`}>OSAVÕTJAD</NavLink>
                    </td>
                    <td className={"col-1"}>
                      <AppButton/>
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
    </Row>
  );
}
