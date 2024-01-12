import {Col, Container, Row} from "react-bootstrap";

export default function Footer() {
  return (
    <Container>
      <Row style={{minHeight: '320px'}} className={"bg-dark p-5 text-opacity-75 text-white"}>
        <Col sm={12} md={12} lg={6} className={"p-3 d-flex flex-column justify-content-between"}>
          <h2 className={"fs-1 fw-normal"}>Curabitur</h2>
          <div>Emauris</div>
          <div>Kfringilla</div>
          <div>Oin magna sem</div>
          <div>Kelementum</div>
        </Col>
        <Col sm={12} md={12} lg={6} className={"p-3 d-flex flex-column justify-content-between"}>
          <h2 className={"fs-1 fw-normal"}>Fusce</h2>
          <div>Econsectetur</div>
          <div>Ksollicitudin</div>
          <div>Omvulputate</div>
          <div>Nunc fringilla tellu</div>
        </Col>
        <Col lg={12} className={"mt-4 mt-md-3"}>
          <Row>
            <Col sm={24} md={12} className={""}>
              <h2 className={"fs-1 fw-normal"}>Kontakt</h2>
            </Col>
          </Row>
          <Row className={"h-100"}>
            <Col sm={24} md={12} className={"pb-5 d-flex flex-column justify-content-between"}>
              <h5>Peakontor: Tallinnas</h5>
              <div>Väike- Ameerika 1, 11415 Tallinn</div>
              <div>Telefon: 605 4450</div>
              <div>Faks: 605 3186</div>
            </Col>
            <Col sm={24} md={12} className={"d-flex flex-column justify-content-end mt-1 mt-md-0 pb-5 justify-content-between"}>
              <h5>Harukontor: Võrus</h5>
              <div>Oja tn 7 (külastusaadress)</div>
              <div>Telefon: 605 3330</div>
              <div>Faks: 605 3155</div>
            </Col>
          </Row>
        </Col>
      </Row>
    </Container>
  );
}
