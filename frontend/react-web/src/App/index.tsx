import {Route, BrowserRouter as Router, Routes} from "react-router-dom";
import RootLayout from "./RootLayout";
import Home from "../routes/Home";
import AddSocialEvent from "../routes/AddSocialEvent";
import Participants from "../routes/Participants";

export default function App() {

  return (
    <Router>
      <RootLayout>
        <Routes>
          <Route path="/" element={<Home/>}/>
          <Route path="/add-social-events" element={<AddSocialEvent/>}/>
          <Route path="/social-events/:eventId/participants" element={<Participants/>}/>
        </Routes>
      </RootLayout>
    </Router>
  )
}
