import {Stack} from "react-bootstrap";
import SocialEventsSection from "./components/SocialEventsSection";
import BannerSection from "./components/BannerSection";

export default function Home() {
  return (
    <Stack gap={4}>
      <BannerSection/>
      <SocialEventsSection/>
    </Stack>
  )
}
