import React, { useState, useEffect } from "react";
import api from "../services/axiosConfig";
import { useNavigate, useParams } from 'react-router-dom';
import Menu from '../components/Menu';
import Toast from '../components/Toast';

const AccountDetails = () => {
  const { id } = useParams();
  const [participant, setParticipant] = useState({
    FirstName: "",
    LastName: "",
    DateOfBirth: "",
    Email: ""
  });
  const [loading, setLoading] = useState(true);
  
  const [validationErrors, setValidationErrors] = useState({});

  const [toastMessage, setToastMessage] = useState('');
  
  const navigate = useNavigate();

  useEffect(() => {

    if (!participant.DateOfBirth) {
      participant.DateOfBirth = new Date().toISOString().slice(0, 16)
    }
    const fetchParticipant = async () => {
      try {
        const response = await api.get(`/Participants/${id}`);
        setParticipant({
          FirstName: response.data.firstName || "",
          LastName: response.data.lastName || "",
          DateOfBirth: response.data.dateOfBirth ? response.data.dateOfBirth.split('T')[0] : new Date().toISOString().split('T')[0],
          Email: response.data.email || ""
        });
      } catch (err) {
        setToastMessage("Ошибка при загрузке данных.");
        console.error("Ошибка при запросе:", err);
      } finally {
        setLoading(false);
      }
    };

    fetchParticipant();
  }, [id]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setParticipant((prevState) => ({
      ...prevState,
      [name]: value
    }));
  };

  const handleSave = async () => {
    try {
      const formattedParticipant = {
        ...participant,
        FirstName: participant.FirstName.trim() === "" ? null : participant.FirstName,
        LastName: participant.LastName.trim() === "" ? null : participant.LastName,
        DateOfBirth: participant.DateOfBirth.trim() === "" ? null : participant.DateOfBirth,
        Email: participant.Email.trim() === "" ? null : participant.Email,
      };
  
      await api.put(`/Participants/${id}`, formattedParticipant);
      setToastMessage("Данные успешно сохранены!");
      setValidationErrors({});
    } catch (err) {
      if (err.response && err.response.status === 400 && err.response.data.errors) {
        setValidationErrors(err.response.data.errors);
      } else {
        setToastMessage("Ошибка при сохранении данных.");
        console.error("Ошибка при сохранении:", err);
      }
    }
  };
  
  const handleLogout = () => {
    localStorage.clear();
    navigate('/login');
  };

  if (loading) {
    return <div>Загрузка...</div>;
  }

  return (
    <div className="account-details">
      {toastMessage && (
        <Toast message={toastMessage} onClose={() => setToastMessage('')} />
      )}
      
      <div>
        <Menu />
      </div>
  
      <form className="account-form">
        <div className="form-group">
          <label>Имя:</label>
          <input
            type="text"
            name="FirstName"
            value={participant.FirstName}
            onChange={handleChange}
            className="input-field"
          />
          {validationErrors.FirstName && (
            <div className="error">{validationErrors.FirstName[0]}</div>
          )}
        </div>
  
        <div className="form-group">
          <label>Фамилия:</label>
          <input
            type="text"
            name="LastName"
            value={participant.LastName}
            onChange={handleChange}
            className="input-field"
          />
          {validationErrors.LastName && (
            <div className="error">{validationErrors.LastName[0]}</div>
          )}
        </div>
  
        <div className="form-group">
          <label>Дата рождения:</label>
          <input
            type="date"
            name="DateOfBirth"
            value={participant.DateOfBirth}
            onChange={handleChange}
            className="input-field"
          />
          {validationErrors.DateOfBirth && (
            <div className="error">{validationErrors.DateOfBirth[0]}</div>
          )}
        </div>
  
        <div className="form-group">
          <label>Email:</label>
          <input
            type="email"
            name="Email"
            value={participant.Email}
            onChange={handleChange}
            className="input-field"
          />
          {validationErrors.Email && (
            <div className="error">{validationErrors.Email[0]}</div>
          )}
        </div>
  
        <div className="form-actions">
          <button type="button" onClick={handleSave} className="save-button">
            Сохранить
          </button>
          <button onClick={handleLogout} className="logout-button">
            Выйти из аккаунта
          </button>
        </div>
      </form>
    </div>
  );
};  

export default AccountDetails;
