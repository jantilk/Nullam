import {Col, Form, Row} from "react-bootstrap";
import "./index.scss";
import {SocialEvent} from "../../../../types/SocialEvent.ts";
import {useState} from "react";
import AddCompanyParticipant from "./components/AddCompanyParticipant";
import AddPersonParticipants from "./components/AddPersonParticipants";
import "./index.scss";

interface ComponentProps {
  socialEvent?: SocialEvent | null;
}

export default function AddParticipants({socialEvent}: ComponentProps) {
  const [participantType, setParticipantType] = useState("person");

  return (
    <>
      <Row className={"mb-3"}><Col><h2 className={"text-primary"}>Osavõtjate lisamine:</h2></Col></Row>
      <Row>
        <Col md={{span: 16, offset: 8}}>
          <Form>
            <Form.Check
              className={"form-check-inline"}
              type="radio"
              label="Eraisik"
              name="participantType"
              id="person"
              value="person"
              checked={participantType === "person"}
              onChange={() => setParticipantType("person")}
            />
            <Form.Check
              className={"form-check-inline"}
              type="radio"
              label="Ettevõte"
              name="participantType"
              id="company"
              value="company"
              checked={participantType === "company"}
              onChange={() => setParticipantType("company")}
            />
          </Form>
        </Col>
      </Row>
      <Row>
        <Col>
          {participantType === "person" ? (
            <AddPersonParticipants socialEvent={socialEvent}/>
          ) : (
            <AddCompanyParticipant socialEvent={socialEvent}/>
          )}
        </Col>
      </Row>
    </>
  );
}
