import {Col, Container, Row, Stack} from "react-bootstrap";
import {useParams} from "react-router-dom";
import {useQuery} from "@tanstack/react-query";
import socialEventsApi from "../../api/socialEventsApi.ts";
import {toast} from "sonner";
import queryKeys from "../../api/QueryKeys.ts";
import {useLoader} from "../../contexts/LoaderContext.tsx";

export default function Participants() {
  const {eventId} = useParams();
  console.log(eventId);
  const {setLoading} = useLoader();
  // TODO: get person and company participants

  const {data: socialEvent, error: error, isLoading: isLoading} = useQuery({
    queryKey: [queryKeys.SOCIAL_EVENT],
    queryFn: () => {
      return socialEventsApi.getById(eventId);
    },
    enabled: !!eventId
  });

  if (error) {
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
                    {socialEvent?.date}
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
                        Osavõtjate nimekiri
                      </Col>
                    </Row>
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
