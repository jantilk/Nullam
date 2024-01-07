import {Nav, Navbar} from "react-bootstrap";
import {NavLink} from "react-router-dom";

const SymbolImage = () => {
  return (
    <img
      src="/symbol.svg"
      alt="logo with text NULLAM"
      width="auto"
      height={"100%"}
    />
  );
}

export default function AppNavbar() {
  return(
    <Navbar expand={"md"} bg="light" className={"shadow-sm"}>
      <Navbar.Brand className={"col-lg-6 d-flex px-md-4"} href="/">
        <div className={"d-block d-md-none pe-3"}><SymbolImage/></div>
        <img
          src="/logo.svg"
          alt="logo with text NULLAM"
          width="auto"
        />
      </Navbar.Brand>
      <Navbar.Toggle aria-controls="basic-navbar-nav"/>
      <Navbar.Collapse id="basic-navbar-nav" className={"mt-2 mt-md-0"}>
        <Nav variant={"tabs"} defaultActiveKey="/" className={"border-0"}>
          <Nav.Item>
            <NavLink to="/" className="nav-link">AVALEHT</NavLink>
          </Nav.Item>
          <Nav.Item>
            <NavLink to="/add-social-event" className="nav-link">ÜRITUSE LISAMINE</NavLink>
          </Nav.Item>
        </Nav>
      </Navbar.Collapse>
      <div className={"d-none d-md-block px-4"}><SymbolImage/></div>
    </Navbar>
  );
}
