import {Container, Stack} from "react-bootstrap";
import {ReactNode} from "react";
import AppNavbar from "./components/AppNavbar";
import Footer from "./components/Footer";
import {Toaster} from "sonner";

interface RootLayoutProps {
  children?: ReactNode;
}

export default function RootLayout({children}: RootLayoutProps) {
  return (
    <Container className={"pt-4"}>
      <Toaster position={"top-right"} richColors/>
      <Stack gap={4}>
        <AppNavbar/>
        {children}
        <Footer/>
      </Stack>
    </Container>
  );
}
