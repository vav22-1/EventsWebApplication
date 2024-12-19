import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import api from '../services/axiosConfig';
import Menu from '../components/Menu';
import Toast from '../components/Toast';

const UpdateEvent = () => {
  const { eventId } = useParams();
  const [event, setEvent] = useState({
    title: '',
    description: '',
    dateAndTime: '',
    location: '',
    category: '',
    maxParticipants: 0
  });
  const [image, setImage] = useState(null);

  const [validationErrors, setValidationErrors] = useState({});

  const [toastMessage, setToastMessage] = useState('');

  const navigate = useNavigate();

  useEffect(() => {
    const fetchEvent = async () => {
      try {
        const eventResponse = await api.get(`/Events/${eventId}`);
        setEvent(eventResponse.data);
      } catch (err) {
        setToastMessage('Не удалось загрузить событие');
      }
    };
    fetchEvent();
  }, [eventId]);

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

    try {
      const formattedEvent = {
        ...event,
        dateAndTime: new Date(event.dateAndTime),
        maxParticipants: parseInt(event.maxParticipants)
      };
      await api.put(`/Events/${eventId}`, formattedEvent);

      if (image) {
        const formData = new FormData();
        formData.append('image', image);

        await api.post(`/Events/${eventId}/upload-image`, formData, {
          headers: {
            'Content-Type': 'multipart/form-data'
          }
        });
      }

      setToastMessage("Событие успешно обновлено!");
      setTimeout(() => {
        navigate(`/events/${eventId}`);
      }, 3000);
    } catch (err) {
      if (err.response && err.response.status === 400 && err.response.data.errors) {
        setValidationErrors(err.response.data.errors);
        console.log(validationErrors);
      } else {
        setToastMessage('Произошла ошибка при обновлении события');
      }
    }
  };

  return (
    <div>
      {toastMessage && <Toast message={toastMessage} onClose={() => setToastMessage('')} />}
  
      <div>
        <Menu />
      </div>
  
      <div className="createOrUpdateEvent">
        <h2>Редактировать событие</h2>
        <form onSubmit={handleSubmit}>
          <div>
            <label>Название события:</label>
            <input
              type="text"
              name="title"
              value={event.title}
              onChange={handleChange}
            />
            {validationErrors.Title && (
              <div className="error">{validationErrors.Title[0]}</div>
            )}
          </div>
  
          <div>
            <label>Описание:</label>
            <textarea
              name="description"
              value={event.description}
              onChange={handleChange}
            />
            {validationErrors.Description && (
              <div className="error">{validationErrors.Description[0]}</div>
            )}
          </div>
  
          <div>
            <label>Дата и время:</label>
            <input
              type="datetime-local"
              name="dateAndTime"
              value={event.dateAndTime}
              onChange={handleChange}
            />
            {validationErrors.DateAndTime && (
              <div className="error">{validationErrors.DateAndTime[0]}</div>
            )}
          </div>
  
          <div>
            <label>Местоположение:</label>
            <input
              type="text"
              name="location"
              value={event.location}
              onChange={handleChange}
            />
            {validationErrors.Location && (
              <div className="error">{validationErrors.Location[0]}</div>
            )}
          </div>
  
          <div>
            <label>Категория:</label>
            <input
              type="text"
              name="category"
              value={event.category}
              onChange={handleChange}
            />
            {validationErrors.Category && (
              <div className="error">{validationErrors.Category[0]}</div>
            )}
          </div>
  
          <div>
            <label>Макс. количество участников:</label>
            <input
              type="number"
              name="maxParticipants"
              value={event.maxParticipants}
              onChange={handleChange}
              min="1"
            />
            {validationErrors.MaxParticipants && (
              <div className="error">{validationErrors.MaxParticipants[0]}</div>
            )}
          </div>
  
          <div>
            <label>Изображение:</label>
            <input
              type="file"
              onChange={handleImageChange}
              accept="image/*"
            />
          </div>
  
          <button type="submit">Обновить событие</button>
        </form>
      </div>
    </div>
  );
};  

export default UpdateEvent;
