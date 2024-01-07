import {Container, Stack} from "react-bootstrap";
import {ReactNode} from "react";
import AppNavbar from "./components/AppNavbar";
import Footer from "./components/Footer";

interface RootLayoutProps {
  children?: ReactNode;
}

export default function RootLayout({children}: RootLayoutProps) {
  return(
    <Container>
      <Stack gap={4}>
        <AppNavbar/>
        {children}
        <Footer/>
      </Stack>
    </Container>
  );
}
