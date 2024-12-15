import React, { useState, useEffect } from "react";
import api from "../services/axiosConfig";
import { useNavigate, useParams } from 'react-router-dom';
import NotificationsButton from '../components/NotificationsButton';

const AccountDetails = () => {
  const { id } = useParams();
  const [participant, setParticipant] = useState({
    FirstName: "",
    LastName: "",
    DateOfBirth: "",
    Email: ""
  });
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [validationErrors, setValidationErrors] = useState({});
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
        setError("Ошибка при загрузке данных.");
        console.error("Ошибка при запросе:", err);
      } finally {
        setLoading(false);
      }
    };

    fetchParticipant();
  }, [participant, id]);

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
      alert("Данные успешно сохранены!");
      setValidationErrors({});
    } catch (err) {
      if (err.response && err.response.status === 400 && err.response.data.errors) {
        setValidationErrors(err.response.data.errors);
      } else {
        alert("Ошибка при сохранении данных.");
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

  if (error) {
    return <div>{error}</div>;
  }

  return (
    <div>
      <div>
        <button onClick={() => navigate('/events')}>К списку событий</button>
        <button onClick={() => navigate('/my-events')}>Мои события</button>
        <NotificationsButton />
      </div>

      <form>
        <div>
          <label>Имя:</label>
          <input
            type="text"
            name="FirstName"
            value={participant.FirstName}
            onChange={handleChange}
          />
          {validationErrors.FirstName && <div className="error">{validationErrors.FirstName[0]}</div>}
        </div>
        <div>
          <label>Фамилия:</label>
          <input
            type="text"
            name="LastName"
            value={participant.LastName}
            onChange={handleChange}
          />
          {validationErrors.LastName && <div className="error">{validationErrors.LastName[0]}</div>}
        </div>
        <div>
          <label>Дата рождения:</label>
          <input
            type="date"
            name="DateOfBirth"
            value={participant.DateOfBirth}
            onChange={handleChange}
          />
          {validationErrors.DateOfBirth && <div className="error">{validationErrors.DateOfBirth[0]}</div>}
        </div>
        <div>
          <label>Email:</label>
          <input
            type="email"
            name="Email"
            value={participant.Email}
            onChange={handleChange}
          />
          {validationErrors.Email && <div className="error">{validationErrors.Email[0]}</div>}
        </div>
        <button type="button" onClick={handleSave}>Сохранить</button>
        <button onClick={handleLogout}>Выйти из аккаунта</button>
      </form>
    </div>
  );
};

export default AccountDetails;
