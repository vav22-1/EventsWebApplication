import React, { useState, useEffect } from 'react';
import api from "../services/axiosConfig";
import { useNavigate } from 'react-router-dom';

function RegisterPage() {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [role, setRole] = useState('User');
    const [validationErrors, setValidationErrors] = useState({}); // Для хранения ошибок валидации
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
            alert('Регистрация успешна. Теперь вы можете войти в аккаунт');
            navigate('/login');
        } catch (error) {
            if (error.response && error.response.status === 400 && error.response.data.errors) {
                setValidationErrors(error.response.data.errors);
            } else {
                setValidationErrors({ general: ["Ошибка при регистрации. Пожалуйста, попробуйте снова."] });
            }
        }
    };

    return (
        <div>
            <h1>Регистрация</h1>
            <input
                type="text"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                placeholder="Логин"
            />
            {validationErrors.Username && <div className="error">{validationErrors.Username[0]}</div>} {/* Ошибка для поля Username */}

            <input
                type="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                placeholder="Пароль"
            />
            {validationErrors.Password && <div className="error">{validationErrors.Password[0]}</div>} {/* Ошибка для поля Password */}

            <input
                type="password"
                value={confirmPassword}
                onChange={(e) => setConfirmPassword(e.target.value)}
                placeholder="Повторите пароль"
            />
            {validationErrors.ConfirmPassword && <div className="error">{validationErrors.ConfirmPassword[0]}</div>} {/* Ошибка для поля ConfirmPassword */}

            <select value={role} onChange={(e) => setRole(e.target.value)}>
                <option value="User">Пользователь</option>
                <option value="Admin">Администратор</option>
            </select>

            {validationErrors.Role && <div className="error">{validationErrors.Role[0]}</div>} {/* Ошибка для поля Role */}

            {validationErrors.general && <div className="error">{validationErrors.general[0]}</div>} {/* Общие ошибки, если они есть */}
            
            <button onClick={handleRegister}>Зарегистрироваться</button>
            <button onClick={() => navigate("/login")}>Уже есть аккаунт?</button>
        </div>
    );
}

export default RegisterPage;
