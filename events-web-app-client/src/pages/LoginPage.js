import React, { useState, useEffect } from 'react';
import api from "../services/axiosConfig";
import { useNavigate } from 'react-router-dom';
import { refreshAccessToken } from "../services/authService";

function LoginPage() {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [validationErrors, setValidationErrors] = useState({});
    const navigate = useNavigate();

    useEffect(() => {
        const checkRefreshToken = async () => {
            const refreshToken = localStorage.getItem('refreshToken');
            if (refreshToken) {
                const newAccessToken = await refreshAccessToken();
                if (newAccessToken) {
                    navigate('/events');
                }
            }
        };

        checkRefreshToken();
    }, [navigate]);

    const handleLogin = async () => {
        try {
            const response = await api.post('https://localhost:7178/api/User/login', {
                username,
                password,
            });
            localStorage.setItem('accessToken', response.data.accessToken);
            localStorage.setItem('refreshToken', response.data.refreshToken);
            localStorage.setItem('participantId', response.data.participantId);
            localStorage.setItem('role', response.data.role);

            navigate('/events');
        } catch (error) {
            if (error.response && error.response.status === 401) {
                const serverMessage = error.response.data;
                setValidationErrors({ general: [serverMessage] });
            } else if (error.response && error.response.status === 400 && error.response.data.errors) {
                setValidationErrors(error.response.data.errors);
            } else {
                setValidationErrors({ general: ['Ошибка при авторизации. Пожалуйста, попробуйте снова.'] });
            }
        }
    };

    return (
        <div className="login-container">
          <h1 className="login-title">Авторизация</h1>
      
          <div className="form-group">
            <input
              type="text"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              placeholder="Логин"
              className="input-field"
            />
            {validationErrors.Username && (
              <div className="error">{validationErrors.Username[0]}</div>
            )}
          </div>
      
          <div className="form-group">
            <input
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              placeholder="Пароль"
              className="input-field"
            />
            {validationErrors.Password && (
              <div className="error">{validationErrors.Password[0]}</div>
            )}
          </div>
      
          {validationErrors.general && (
            <div className="error general-error">{validationErrors.general[0]}</div>
          )}
      
          <div className="form-actions">
            <button onClick={handleLogin} className="login-button">
              Войти
            </button>
            <button onClick={() => navigate('/register')} className="register-button">
              Зарегистрироваться
            </button>
          </div>
        </div>
      );
    };      

export default LoginPage;
