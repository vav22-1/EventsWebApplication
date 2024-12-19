import React, { useState, useEffect } from "react";
import api from "../services/axiosConfig";
import { useParams } from "react-router-dom";
import ConfirmationModal from "../components/ConfirmationModal";
import Menu from '../components/Menu';
import Toast from '../components/Toast';


const EventDetails = () => {
  const { id } = useParams();
  const [event, setEvent] = useState(null);
  const [loading, setLoading] = useState(true);

  const [error, setError] = useState(null);
  const [validationErrors, setValidationErrors] = useState({});

  const [isRegistered, setIsRegistered] = useState(false);

  const [isModalOpen, setIsModalOpen] = useState(false);

  const [toastMessage, setToastMessage] = useState('');

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
      setToastMessage("Вы успешно зарегистрировались на событие.");
      setIsRegistered(true);
    } catch (err) {
      if (err.response && err.response.status === 400 && err.response.data.errors) {
        setValidationErrors(err.response.data.errors);
      } else {
        setToastMessage("Ошибка при регистрации.");
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
      {toastMessage && (
        <Toast message={toastMessage} onClose={() => setToastMessage('')} />
      )}
  
      <div>
        <Menu selectedEventId={id} />
      </div>
  
      <div className="event-details">
        {event.imageUrl && (
          <img src={event.imageUrl} alt={event.title} />
        )}
  
        <h1>{event.title}</h1>
        <p>{event.description}</p>
        <p>Дата: {new Date(event.dateAndTime).toLocaleDateString()}</p>
        <p>Время: {new Date(event.dateAndTime).toLocaleTimeString()}</p>
        <p>Место проведения: {event.location}</p>
        <p>Категория: {event.category}</p>
  
        <div>
          {event.availableSeats <= 0 ? (
            <p>Свободных мест не осталось</p>
          ) : (
            <div>
              <p>Осталось мест: {event.availableSeats}</p>
            </div>
          )}
        </div>
  
        {isRegistered ? (
          <p>Вы уже зарегистрированы на это событие.</p>
        ) : (
          <div>
            <button onClick={openModal}>Зарегистрироваться</button>
            {validationErrors.registration && (
              <div className="error">{validationErrors.registration[0]}</div>
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
