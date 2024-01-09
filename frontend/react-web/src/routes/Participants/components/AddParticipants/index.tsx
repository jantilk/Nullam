import {Col, Form, Row} from "react-bootstrap";
import "./index.scss";
import {SocialEvent} from "../../../../types/SocialEvent.ts";
import AddCompanyParticipant from "../AddCompanyParticipant";
import AddPersonParticipants from "../AddPersonParticipants";
import {useState} from "react";

interface ComponentProps {
  socialEvent?: SocialEvent;
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
          {participantType === "company" ? (
            <AddCompanyParticipant socialEvent={socialEvent}/>
          ) : (
            <AddPersonParticipants socialEvent={socialEvent}/>
          )}
        </Col>
      </Row>
    </>
  );
}
