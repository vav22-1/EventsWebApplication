import React, { useState, useEffect } from "react";
import api from "../services/axiosConfig";
import { useNavigate } from "react-router-dom";
import ConfirmationModal from "../components/ConfirmationModal";
import Menu from '../components/Menu';
import Toast from '../components/Toast';

const AccountEventsList = () => {
  const [events, setEvents] = useState([]);
  const [loading, setLoading] = useState(true);
  const [filter, setFilter] = useState({
    title: "",
    filterDate: "",
    filterLocation: "",
    filterCategory: ""
  });
  const [page, setPage] = useState(1);
  const [pageSize] = useState(3);

  const [debounceTimer, setDebounceTimer] = useState(null);

  const [toastMessage, setToastMessage] = useState('');

  const [isModalOpen, setIsModalOpen] = useState(false);
  const [eventToUnregister, setEventToUnregister] = useState(null);

  const navigate = useNavigate();

  useEffect(() => {
    const fetchMyEvents = async () => {
      try {
        const participantId = localStorage.getItem("participantId");
        const participantResponse = await api.get(`/Participants/${participantId}`);
        const { eventIds } = participantResponse.data;

        if (!eventIds || eventIds.length === 0) {
          setEvents([]);
          return;
        }

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
              setToastMessage(`Ошибка загрузки данных для события ${event.id}`);
              console.error(`Ошибка загрузки данных для события ${event.id}:`, error);
              event.imageUrl = null;
            }
            return event;
          })
        );
        const filteredEvents = eventsWithData.filter(event => eventIds.includes(event.id));
        setEvents(filteredEvents);
      } catch (error) {
        setToastMessage("Ошибка при загрузке данных.");
        console.error("Error during request:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchMyEvents();
  }, [filter, page, pageSize]);

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

  const openModalForUnregister = (eventId) => {
    setEventToUnregister(eventId);
    setIsModalOpen(true);
  };

  const handleUnregister = async () => {
    try {
      const participantId = localStorage.getItem("participantId");
      await api.delete(`/Participants/remove/${eventToUnregister}/${participantId}`);
      setEvents(prevEvents => prevEvents.filter(event => event.id !== eventToUnregister));
      setToastMessage("Вы успешно отменили участие в событии");
    } catch (error) {
      setToastMessage("Ошибка при отмене регистрации.");
      console.error("Error during unregistering:", error);
    } finally {
      closeModal();
    }
  };

  const closeModal = () => {
    setIsModalOpen(false);
    setEventToUnregister(null);
  };

  if (loading) {
    return <div>Загрузка...</div>;
  }

  return (
    <div>
      {toastMessage && (
        <Toast message={toastMessage} onClose={() => setToastMessage('')} />
      )}
  
      <div>
        <Menu />
      </div>
  
      <div className="eventlist">
        <div className="page-navigation">
          <button onClick={() => setPage(prevPage => prevPage - 1)} disabled={page <= 1}>
            Назад
          </button>
          <span>Страница {page}</span>
          {events.length === pageSize && (
            <button onClick={() => setPage(prevPage => prevPage + 1)}>
              Вперед
            </button>
          )}
        </div>
  
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
          <p>Вы не участвуете ни в одном событии.</p>
        ) : (
          <ul>
            {events.map(event => (
              <li key={event.id}>
                {event.imageUrl ? (
                  <img src={event.imageUrl} alt={event.title} width="200" />
                ) : (
                  <p>Изображение отсутствует</p>
                )}
                <p>{event.title}</p>
                <p>Дата: {new Date(event.dateAndTime).toLocaleDateString()}</p>
                <button onClick={() => navigate(`/events/${event.id}`)}>Подробнее</button>
                <button onClick={() => openModalForUnregister(event.id)}>Отменить участие</button>
              </li>
            ))}
          </ul>
        )}
      </div>
  
      <ConfirmationModal
        isOpen={isModalOpen}
        onConfirm={handleUnregister}
        onCancel={closeModal}
        message="Вы уверены, что хотите отменить участие в этом событии?"
      />
    </div>
  );
};
  
export default AccountEventsList;
