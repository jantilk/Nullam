import {Route, BrowserRouter as Router, Routes} from "react-router-dom";
import RootLayout from "./RootLayout";
import Home from "../routes/Home";
import AddSocialEvent from "../routes/AddSocialEvent";
import Participants from "../routes/Participants";
import UpdateCompanyParticipant from "../routes/UpdateCompanyParticipant";
import UpdatePersonParticipant from "../routes/UpdatePersonParticipant";

export default function App() {

  return (
    <Router>
      <RootLayout>
        <Routes>
          <Route path="/" element={<Home/>}/>
          <Route path="/social-events" element={<AddSocialEvent/>}/>
          <Route path="/social-events/:eventId/participants" element={<Participants/>}/>
          <Route path="/social-events/:eventId/participants/companies/:companyId" element={<UpdateCompanyParticipant/>}/>
          <Route path="/social-events/:eventId/participants/persons/:personId" element={<UpdatePersonParticipant/>}/>
        </Routes>
      </RootLayout>
    </Router>
  )
}
