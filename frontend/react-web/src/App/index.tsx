import {Route, BrowserRouter as Router, Routes} from "react-router-dom";
import RootLayout from "./RootLayout";
import Home from "../routes/Home";
import AddSocialEvent from "../routes/AddSocialEvent";
import Participants from "../routes/Participants";
import UpdatePersonParticipant from "../routes/Participants/UpdatePersonParticipant";
import UpdateCompanyParticipant from "../routes/Participants/UpdateCompanyParticipant";
import Settings from "../routes/Settings";

export default function App() {

  return (
    <Router>
      <RootLayout>
        <Routes>
          <Route path="/" element={<Home/>}/>
          <Route path="/settings" element={<Settings/>}/>
          <Route path="/social-events" element={<AddSocialEvent/>}/>
          <Route path="/social-events/:eventId/participants" element={<Participants/>}/>
          <Route path="/social-events/:eventId/participating-companies/:companyId" element={<UpdateCompanyParticipant/>}/>
          <Route path="/social-events/:eventId/participating-persons/:personId" element={<UpdatePersonParticipant/>}/>
        </Routes>
      </RootLayout>
    </Router>
  )
}
