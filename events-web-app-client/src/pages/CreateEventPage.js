import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../services/axiosConfig';
import NotificationsButton from '../components/NotificationsButton';

const CreateEvent = () => {
  const [event, setEvent] = useState({
    Title: '',
    Description: '',
    DateAndTime: '',
    Location: '',
    Category: '',
    MaxParticipants: 0
  });
  const [image, setImage] = useState(null);
  const [error, setError] = useState(null);
  const [validationErrors, setValidationErrors] = useState({});
  const navigate = useNavigate();

  useEffect(() => {
    const currentDateTime = new Date().toISOString().slice(0, 16);
    setEvent((prevState) => ({
      ...prevState,
      DateAndTime: currentDateTime
    }));
  }, []);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setEvent((prevState) => ({
      ...prevState,
      [name]: value
    }));
  };

  const handleImageChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      setImage(file);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    const newValidationErrors = {};
    if (!image) {
      newValidationErrors.Image = ["Изображение обязательно"];
    }

    if (Object.keys(newValidationErrors).length > 0) {
      setValidationErrors(prevErrors => ({
        ...prevErrors,
        ...newValidationErrors
      }));
    }

    try {
      const formattedEvent = {
        ...event,
        DateAndTime: new Date(event.DateAndTime),
        MaxParticipants: parseInt(event.MaxParticipants)
      };
      
      const eventResponse = await api.post('/Events', formattedEvent);

      const createdEventId = eventResponse.data.id;

      const formData = new FormData();
      formData.append('image', image);

      await api.post(`/Events/${createdEventId}/upload-image`, formData, {
        headers: {
          'Content-Type': 'multipart/form-data'
        }
      });

      alert("Событие успешно добавлено!");
      navigate('/events');
    } catch (err) {
      if (err.response && err.response.status === 400 && err.response.data.errors) {
        const serverValidationErrors = err.response.data.errors;
        setValidationErrors(prevErrors => ({
          ...prevErrors,  // Оставляем предыдущие ошибки
          ...serverValidationErrors  // Добавляем ошибки с сервера
        }));
      } else if (err.response && err.response.status === 403) {
        setError('У вас нет прав для добавления события');
      } else {
        setError('Произошла ошибка при добавлении события');
      }
    }
  };

  return (
    <div>
      <div>
        <button onClick={() => navigate('/events')}>К списку событий</button>
        <NotificationsButton />
      </div>
      <h2>Добавить новое событие</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label>Название события:</label>
          <input
            type="text"
            name="title"
            value={event.title}
            onChange={handleChange}
          />
          {validationErrors.Title && <div className="error">{validationErrors.Title[0]}</div>}
        </div>
        <div>
          <label>Описание:</label>
          <textarea
            name="description"
            value={event.description}
            onChange={handleChange}
          />
          {validationErrors.Description && <div className="error">{validationErrors.Description[0]}</div>}
        </div>
        <div>
          <label>Дата и время:</label>
          <input
            type="datetime-local"
            name="DateAndTime"
            value={event.DateAndTime}
            onChange={handleChange}
          />
          {validationErrors.DateAndTime && <div className="error">{validationErrors.DateAndTime[0]}</div>}
        </div>
        <div>
          <label>Местоположение:</label>
          <input
            type="text"
            name="location"
            value={event.location}
            onChange={handleChange}
          />
          {validationErrors.Location && <div className="error">{validationErrors.Location[0]}</div>}
        </div>
        <div>
          <label>Категория:</label>
          <input
            type="text"
            name="category"
            value={event.category}
            onChange={handleChange}
          />
          {validationErrors.Category && <div className="error">{validationErrors.Category[0]}</div>}
        </div>
        <div>
          <label>Макс. количество участников:</label>
          <input
            type="number"
            name="MaxParticipants"
            value={event.MaxParticipants}
            onChange={handleChange}
          />
          {validationErrors.MaxParticipants && <div className="error">{validationErrors.MaxParticipants[0]}</div>}
        </div>

        <div>
          <label>Изображение:</label>
          <input
            type="file"
            onChange={handleImageChange}
            accept="image/*"
          />
          {validationErrors.Image && <div className="error">{validationErrors.Image[0]}</div>}
        </div>

        <button type="submit">Добавить событие</button>
        {error && <div className="error">{error}</div>}
      </form>
    </div>
  );
};

export default CreateEvent;