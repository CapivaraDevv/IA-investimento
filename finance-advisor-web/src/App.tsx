import { Routes, Route } from "react-router-dom";
import Dashboard from "./pages/Dashboard";


function App() {
  return (
    <Routes>
      <Route path="/" element={<p>Cadastro em breve</p>}/>
      <Route path="/dashboard/:userId" element={<Dashboard/>}/>
    </Routes>
  );
}

export default App;
