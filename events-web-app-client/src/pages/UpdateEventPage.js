import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import api from '../services/axiosConfig';
import NotificationsButton from '../components/NotificationsButton';

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
  const [currentImage, setCurrentImage] = useState(null);
  const [error, setError] = useState(null);
  const [validationErrors, setValidationErrors] = useState({});
  const navigate = useNavigate();

  useEffect(() => {
    const fetchEvent = async () => {
      try {
        const eventResponse = await api.get(`/Events/${eventId}`);
        setEvent(eventResponse.data);
        if (eventResponse.data.imagePath) {
          try {
            const imageResponse = await api.get(`/Events/${eventId}/image`, {
              responseType: 'blob',
            });
            const imageUrl = URL.createObjectURL(imageResponse.data);
            setCurrentImage(imageUrl);
          } catch (imageError) {
            console.error(`Ошибка загрузки изображения для события ${eventId}:`, imageError);
            setCurrentImage(null);
          }
        }
      } catch (err) {
        setError('Не удалось загрузить событие');
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

      alert("Событие успешно обновлено!");
      navigate(`/events/${eventId}`);
    } catch (err) {
      if (err.response && err.response.status === 400 && err.response.data.errors) {
        setValidationErrors(err.response.data.errors);
      } else {
        setError('Произошла ошибка при обновлении события');
      }
    }
  };

  return (
    <div>
      <div>
        <button onClick={() => navigate('/events')}>К списку событий</button>
        <NotificationsButton />
      </div>
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
          {validationErrors.title && <div className="error">{validationErrors.title[0]}</div>}
        </div>
        <div>
          <label>Описание:</label>
          <textarea
            name="description"
            value={event.description}
            onChange={handleChange}
          />
          {validationErrors.description && <div className="error">{validationErrors.description[0]}</div>}
        </div>
        <div>
          <label>Дата и время:</label>
          <input
            type="datetime-local"
            name="dateAndTime"
            value={event.dateAndTime}
            onChange={handleChange}
          />
          {validationErrors.dateAndTime && <div className="error">{validationErrors.dateAndTime[0]}</div>}
        </div>
        <div>
          <label>Местоположение:</label>
          <input
            type="text"
            name="location"
            value={event.location}
            onChange={handleChange}
          />
          {validationErrors.location && <div className="error">{validationErrors.location[0]}</div>}
        </div>
        <div>
          <label>Категория:</label>
          <input
            type="text"
            name="category"
            value={event.category}
            onChange={handleChange}
          />
          {validationErrors.category && <div className="error">{validationErrors.category[0]}</div>}
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
          {validationErrors.maxParticipants && <div className="error">{validationErrors.maxParticipants[0]}</div>}
        </div>

        <div>
          <label>Изображение:</label>
          <input
            type="file"
            onChange={handleImageChange}
            accept="image/*"
          />
          {currentImage && <img src={currentImage} alt="Event" width="100" />}
          {error && <div className="error">{error}</div>}
        </div>

        <button type="submit">Обновить событие</button>
      </form>
      {error && <div className="error">{error}</div>}
    </div>
  );
};

export default UpdateEvent;
