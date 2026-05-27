import { Routes, Route } from "react-router-dom";
import Dashboard from "./pages/Dashboard";
import RegisterPage from "./pages/RegisterPage";


function App() {
  return (
    <Routes>
      <Route path="/" element={<RegisterPage/>}/>
      <Route path="/dashboard/:userId" element={<Dashboard/>}/>
    </Routes>
  );
}

export default App;
