import { Routes, Route } from "react-router-dom";
import Dashboard from "./pages/Dashboard";
import RegisterPage from "./pages/RegisterPage";
import AppShell from './components/layout/AppShell'
import GoalsPage from './pages/GoalsPage'
import SimulationPage from './pages/SimulationPage'
import TransactionsPage from './pages/TransactionsPage'


function App() {
  return (
    <Routes>
      <Route path="/" element={<RegisterPage />} />
      <Route path="/:userId" element={<AppShell />}>
        <Route path="dashboard" element={<Dashboard />} /> {/* ← filha */}
        <Route path="goals" element={<GoalsPage />} />
        <Route path="simulate" element={<SimulationPage />} />
        <Route path="transactions" element={<TransactionsPage />} />
      </Route>
    </Routes>
  );
}

export default App;
