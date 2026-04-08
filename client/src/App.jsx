import { Routes, Route, Navigate } from "react-router-dom";
import FishFarmList from "./pages/fishfarm/FishFarmList";

function App() {
  return (
    <>
      <div style={{ padding: 20 }}>
        <Routes>
          <Route path="/" element={<Navigate to="/fishfarms" />} />
          <Route path="/fishfarms" element={<FishFarmList />} />

          {/* Fallback page */}
          <Route path="*" element={<div>Page Not Found</div>} />
        </Routes>
      </div>
    </>
  )
}

export default App
