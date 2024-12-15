import React, { useState, useEffect } from "react";
import api from "../services/axiosConfig";
import { useNavigate, useParams } from "react-router-dom";
import ConfirmationModal from "../components/ConfirmationModal";
import NotificationsButton from '../components/NotificationsButton';


const EventDetails = () => {
  const { id } = useParams();
  const [event, setEvent] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [validationErrors, setValidationErrors] = useState({});
  const [isRegistered, setIsRegistered] = useState(false);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchEvent = async () => {
      try {
        const response = await api.get(`/Events/${id}`);
        if (response.data.imagePath) {
          try {
            const imageResponse = await api.get(`/Events/${id}/image`, {
              responseType: "blob",
            });
            response.data.imageUrl = URL.createObjectURL(imageResponse.data);
          } catch (imageError) {
            console.error(`Ошибка загрузки изображения для события ${id}:`, imageError);
            response.data.imageUrl = null;
          }
        }

        const seatsResponse = await api.get(`/Events/${id}/available-seats`);
        response.data.availableSeats = seatsResponse.data.availableSeats;

        const participantId = localStorage.getItem("participantId");
        const participantResponse = await api.get(`/Participants/${participantId}`);
        const { eventIds } = participantResponse.data;

        if (eventIds.includes(parseInt(id))) {
          setIsRegistered(true);
        } else {
          setIsRegistered(false);
        }

        setEvent(response.data);
      } catch (error) {
        setError("Ошибка при загрузке данных.");
        console.error("Ошибка при запросе:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchEvent();
  }, [id]);

  const handleRegister = async () => {
    try {
      await api.post(`/Participants/register/${id}`);
  
      const seatsResponse = await api.get(`/Events/${id}/available-seats`);
      setEvent((prevEvent) => ({
        ...prevEvent,
        availableSeats: seatsResponse.data.availableSeats,
      }));
      setIsRegistered(true);
    } catch (err) {
      if (err.response && err.response.status === 400 && err.response.data.errors) {
        setValidationErrors(err.response.data.errors);
      } else {
        alert("Ошибка при регистрации.");
        console.error("Ошибка при регистрации:", err);
      }
    } finally {
      closeModal();
    }
  };
  
  const openModal = () => {
    setIsModalOpen(true);
  };
  
  const closeModal = () => {
    setIsModalOpen(false);
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
        <button onClick={() => navigate(`/account/${localStorage.getItem("participantId")}`)}>Личный кабинет</button>
        <button onClick={() => navigate(`/update-event/${id}`)}>Обновить событие</button>
        <NotificationsButton />
      </div>
      <div>
        {event.imageUrl && (
          <img
            src={event.imageUrl}
            alt={event.title}
            width="800"
          />
        )}
        <h1>{event.title}</h1>
        <p>Описание события: {event.description}</p>
        <p>Дата: {new Date(event.dateAndTime).toLocaleDateString()}</p>
        <p>Время: {new Date(event.dateAndTime).toLocaleTimeString()}</p>
        <p>Место проведения: {event.location}</p>
        <p>Категория: {event.category}</p>

        {isRegistered ? (
          <p>Вы уже зарегистрированы на это событие.</p>
        ) : (
          <div>
            {event.availableSeats <= 0 ? (
              <p>Свободных мест не осталось</p>
            ) : (
              <div>
                <p>Осталось мест: {event.availableSeats}</p>
                <button onClick={openModal}>Зарегистрироваться</button>
                {validationErrors.registration && <div className="error">{validationErrors.registration[0]}</div>}
              </div>
            )}
          </div>
        )}
      </div>
      <ConfirmationModal
          isOpen={isModalOpen}
          onConfirm={handleRegister}
          onCancel={closeModal}
          message="Вы уверены, что хотите зарегистрироваться на это событие?"
      />
    </div>
  );
};

export default EventDetails;
