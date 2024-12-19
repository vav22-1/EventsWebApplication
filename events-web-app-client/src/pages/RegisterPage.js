import React, { useState, useEffect } from 'react';
import api from "../services/axiosConfig";
import { useNavigate } from 'react-router-dom';
import Toast from '../components/Toast';

function RegisterPage() {
    const [username, setUsername] = useState('');

    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');

    const [role, setRole] = useState('User');

    const [validationErrors, setValidationErrors] = useState({});

    const [toastMessage, setToastMessage] = useState('');

    const navigate = useNavigate();

    useEffect(() => {
            const checkRefreshToken = async () => {
                const refreshToken = localStorage.getItem('refreshToken');
                if (refreshToken) {
                    try {
                        const response = await api.post('https://localhost:7178/api/User/refresh', refreshToken, {
                            headers: {
                                'Content-Type': 'application/json',
                              },
                        });
                        localStorage.setItem('accessToken', response.data.accessToken);
                        localStorage.setItem('refreshToken', response.data.refreshToken);
                        localStorage.setItem('participantId', response.data.participantId);
                        navigate('/events');
                    } catch (error) {
                        console.error('Ошибка при обновлении токена:', error);
                    }
                }
            };
    
            checkRefreshToken();
        }, [navigate]);

    const handleRegister = async () => {
        if (password !== confirmPassword) {
            setValidationErrors({ ConfirmPassword: ["Пароли не совпадают. Пожалуйста, попробуйте снова."] });
            return;
        }

        try {
            const response = await api.post('https://localhost:7178/api/User/register', {
                username,
                password,
                role,
            });
            setToastMessage('Регистрация успешна. Теперь вы можете войти в аккаунт');
            setTimeout(() => {
                navigate('/login');
              }, 3000);
        } catch (error) {
            if (error.response && error.response.status === 400 && error.response.data.errors) {
                setValidationErrors(error.response.data.errors);
            } else {
                setValidationErrors({ general: ["Ошибка при регистрации. Пожалуйста, попробуйте снова."] });
            }
        }
    };

    return (
        <div className="register-container">
          {toastMessage && <Toast message={toastMessage} onClose={() => setToastMessage('')} />}
      
          <h1 className="register-title">Регистрация</h1>
      
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
      
          <div className="form-group">
            <input
              type="password"
              value={confirmPassword}
              onChange={(e) => setConfirmPassword(e.target.value)}
              placeholder="Повторите пароль"
              className="input-field"
            />
            {validationErrors.ConfirmPassword && (
              <div className="error">{validationErrors.ConfirmPassword[0]}</div>
            )}
          </div>
      
          <div className="form-group">
            <select
              value={role}
              onChange={(e) => setRole(e.target.value)}
              className="select-field"
            >
              <option value="User">Пользователь</option>
              <option value="Admin">Администратор</option>
            </select>
            {validationErrors.Role && <div className="error">{validationErrors.Role[0]}</div>}
          </div>
      
          {validationErrors.general && (
            <div className="error general-error">{validationErrors.general[0]}</div>
          )}
      
          <div className="form-actions">
            <button onClick={handleRegister} className="register-button">
              Зарегистрироваться
            </button>
            <button onClick={() => navigate("/login")} className="login-link-button">
              Уже есть аккаунт?
            </button>
          </div>
        </div>
      );
    };      

export default RegisterPage;
