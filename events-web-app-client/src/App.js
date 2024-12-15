import React, { useEffect } from "react";
import { BrowserRouter as Router, Routes, Route, useNavigate } from "react-router-dom";
import routes from './services/routes';
import { refreshAccessToken } from './services/authService';

function App() {
  const navigate = useNavigate();

  useEffect(() => {
    const checkAuth = async () => {
      const currentPath = window.location.pathname;
      if (currentPath === '/') {
        const refreshToken = localStorage.getItem("refreshToken");
        if (refreshToken) {
          const newAccessToken = await refreshAccessToken();
          if (newAccessToken) {
            navigate("/events");
          } else {
            navigate("/login");
          }
        } else {
          navigate("/login");
        }
      }
    };

    checkAuth();
  }, [navigate]);

  return (
    <Routes>
      {routes.map((route, index) => (
        <Route key={index} path={route.path} element={<route.component />} />
      ))}
    </Routes>
  );
}

export default App;
