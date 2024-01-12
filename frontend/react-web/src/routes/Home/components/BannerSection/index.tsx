import {Col, Row, Image} from "react-bootstrap";

export default function BannerSection() {
  return (
    <Row>
      <Col lg={12} className={"pe-lg-0"} style={{marginRight: '-12px'}}>
        <div className={"h-100 bg-primary p-4 d-flex align-items-center shadow-sm"}>
          <p className={"text-white fs-5"}>
            Sed nec elit vestibulum, <strong>tincidunt orci</strong> et, sagittis ex. Vestibulum rutrum <strong>neque suscipit</strong> ante mattis maximus.
            Nulla non sapien <strong>viverra, lobortis lorem non</strong>, accumsan metus.
          </p>
        </div>
      </Col>
      <Col lg={12} className={"px-lg-0"}>
        <Image src={"/pilt.jpg"} className={"w-100 shadow"}/>
      </Col>
    </Row>
  );
}
