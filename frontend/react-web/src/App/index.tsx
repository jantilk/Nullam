import {Route, BrowserRouter as Router, Routes} from "react-router-dom";
import RootLayout from "./RootLayout";
import Home from "../routes/Home";
import AddSocialEvent from "../routes/AddSocialEvent";

export default function App() {

  return (
    <Router>
      <RootLayout>
        <Routes>
          <Route path="/" element={<Home/>}/>
          <Route path="/add-social-event" element={<AddSocialEvent/>}/>
        </Routes>
      </RootLayout>
    </Router>
  )
}
