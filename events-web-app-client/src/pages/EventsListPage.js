import React, { useState, useEffect } from "react";
import api from "../services/axiosConfig";
import { useNavigate } from 'react-router-dom';
import ConfirmationModal from "../components/ConfirmationModal"
import NotificationsButton from '../components/NotificationsButton';

const EventsList = () => {
  const [events, setEvents] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [filter, setFilter] = useState({
    title: "",
    filterDate: "",
    filterLocation: "",
    filterCategory: ""
  });
  const [validationErrors, setValidationErrors] = useState({});
  const [page, setPage] = useState(1);
  const [pageSize] = useState(3);
  const navigate = useNavigate();

  const [debounceTimer, setDebounceTimer] = useState(null);

  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);

  const [selectedEventId, setSelectedEventId] = useState(null);

  const [registeredEventIds, setRegisteredEventIds] = useState([]);

  useEffect(() => {
    const fetchEvents = async () => {
      try {
        const participantId = localStorage.getItem("participantId");

        const participantResponse = await api.get(`/Participants/${participantId}`);
        setRegisteredEventIds(participantResponse.data.eventIds);

        const params = {
          ...filter,
          page,
          pageSize
        };

        const response = await api.get("/Events/filter", { params });
        const eventsWithData = await Promise.all(
          response.data.items.map(async (event) => {
            try {
              if (event.imagePath) {
                const imageResponse = await api.get(`/Events/${event.id}/image`, {
                  responseType: "blob",
                });
                event.imageUrl = URL.createObjectURL(imageResponse.data);
              } else {
                event.imageUrl = null;
              }
              const seatsResponse = await api.get(`/Events/${event.id}/available-seats`);
              event.availableSeats = seatsResponse.data.availableSeats;
            } catch (error) {
              console.error(`Ошибка загрузки данных для события ${event.id}:`, error);
              event.imageUrl = null;
            }
            return event;
          })
        );
        setEvents(eventsWithData);
      } catch (error) {
        setError("Ошибка при загрузке данных.");
        console.error("Error during request:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchEvents();
  }, [filter, page, pageSize]);

  const handleRegister = async (eventId) => {
    try {
      await api.post(`/Participants/register/${eventId}`);
      setRegisteredEventIds((prevIds) => [...prevIds, eventId]);
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

  const handleDelete = async () => {
    try {
      await api.delete(`/Events/${selectedEventId}`);
      setEvents(events.filter(event => event.id !== selectedEventId));
      alert("Событие успешно удалено!");
      setIsDeleteModalOpen(false);
    } catch (err) {
      alert("Ошибка при удалении события.");
      console.error("Ошибка при удалении:", err);
    }
  };

  const handleFilterChange = (e) => {
    const { name, value } = e.target;

    if (debounceTimer) {
      clearTimeout(debounceTimer);
    }
    const timer = setTimeout(() => {
      setFilter(prevFilter => ({
        ...prevFilter,
        [name]: value
      }));
    }, 1);

    setDebounceTimer(timer);
  };

  const openModal = (eventId) => {
    setSelectedEventId(eventId);
    setIsModalOpen(true);
  };

  const openDeleteModal = (eventId) => {
    setSelectedEventId(eventId);
    setIsDeleteModalOpen(true);
  };  

  const closeModal = () => {
    setIsModalOpen(false);
    setSelectedEventId(null);
  };

  const closeDeleteModal = () => {
    setIsDeleteModalOpen(false);
    setSelectedEventId(null);
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
        <button onClick={() => navigate(`/account/${localStorage.getItem("participantId")}`)}>Личный кабинет</button>
        {localStorage.getItem("role") === "Admin" && (
         <button onClick={() => navigate('/new-event')}>Добавить событие</button>
        )}
        <NotificationsButton />
      </div>
      <div>
        <h1>Список событий</h1>

        <div>
          <input
            type="text"
            name="title"
            value={filter.title}
            onChange={handleFilterChange}
            placeholder="Поиск по названию"
          />
          <input
            type="date"
            name="filterDate"
            value={filter.filterDate}
            onChange={handleFilterChange}
          />
          <input
            type="text"
            name="filterLocation"
            value={filter.filterLocation}
            onChange={handleFilterChange}
            placeholder="Поиск по локации"
          />
          <input
            type="text"
            name="filterCategory"
            value={filter.filterCategory}
            onChange={handleFilterChange}
            placeholder="Поиск по категории"
          />
        </div>

        {events.length === 0 ? (
          <p>События не найдены.</p>
        ) : (
          <ul>
            {events.map((event) => (
              <div key={event.id}>
                <li>
                  {event.imageUrl ? (
                    <img
                      src={event.imageUrl}
                      alt={event.title}
                      width="200"
                    />
                  ) : (
                    <p>Изображение отсутствует</p>
                  )}
                  <p>{event.title}</p>
                  <p>Дата: {new Date(event.dateAndTime).toLocaleDateString()}</p>
                  {registeredEventIds.includes(event.id) ? (
                    <p>Вы уже зарегистрированы на это событие.</p>
                  ) : event.availableSeats <= 0 ? (
                    <p>Свободных мест не осталось</p>
                  ) : (
                    <div>
                      <p>
                        Осталось мест: {event.availableSeats}
                      </p>
                      <button onClick={() => openModal(event.id)}>Зарегистрироваться</button>
                      {validationErrors.registration && (
                        <div className="error">{validationErrors.registration[0]}</div>
                      )}
                    </div>
                  )}
                  <button onClick={() => navigate(`/events/${event.id}`)}>Подробнее</button>
                  {localStorage.getItem("role") === "Admin" && (
                    <button onClick={() => openDeleteModal(event.id)}>Удалить событие</button>
                  )}
                </li>
              </div>
            ))}
          </ul>
        )}

        <div>
          {/* Кнопка назад */}
          {page > 1 && (
            <button onClick={() => setPage(prevPage => prevPage - 1)}>Назад</button>
          )}

          {/* Нумерация страниц */}
          <span>Страница {page}</span>

          {/* Кнопка вперед */}
          {events.length === pageSize && (
            <button onClick={() => setPage(prevPage => prevPage + 1)}>Вперед</button>
          )}
        </div>
      </div>
      <ConfirmationModal
        isOpen={isModalOpen}
        onConfirm={() => handleRegister(selectedEventId)}
        onCancel={closeModal}
        message="Вы уверены, что хотите зарегистрироваться на это событие?"
      />
      <ConfirmationModal
        isOpen={isDeleteModalOpen}
        onConfirm={handleDelete}
        onCancel={closeDeleteModal}
        message="Вы уверены, что хотите удалить это событие?"
      />
    </div>
  );
};

export default EventsList;

