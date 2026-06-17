import { Navigate, Route, Routes } from "react-router-dom";
import Navbar from "./components/Navbar";
import FoodAlternativesPage from "./pages/FoodAlternativesPage";
import WeeklyPlansPage from "./pages/WeeklyPlansPage";
import WeeklyPlanDetailPage from "./pages/WeeklyPlanDetailPage";
import ReportPage from "./pages/ReportPage";

function App() {
  return (
    <div className="app-shell">
      <Navbar />
      <main>
        <Routes>
          <Route path="/" element={<Navigate to="/weekly-plans" replace />} />
          <Route path="/weekly-plans" element={<WeeklyPlansPage />} />
          <Route path="/weekly-plans/:id" element={<WeeklyPlanDetailPage />} />
          <Route path="/food-alternatives" element={<FoodAlternativesPage />} />
          <Route path="/report" element={<ReportPage />} />
          <Route path="*" element={<Navigate to="/weekly-plans" replace />} />
        </Routes>
      </main>
    </div>
  );
}

export default App;
