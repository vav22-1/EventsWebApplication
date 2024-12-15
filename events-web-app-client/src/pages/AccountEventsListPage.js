import React, { useState, useEffect } from "react";
import api from "../services/axiosConfig";
import { useNavigate } from "react-router-dom";
import NotificationsButton from '../components/NotificationsButton';

const AccountEventsList = () => {
  const [events, setEvents] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [filter, setFilter] = useState({
    title: "",
    filterDate: "",
    filterLocation: "",
    filterCategory: ""
  });
  const [page, setPage] = useState(1);
  const [pageSize] = useState(3);

  const [debounceTimer, setDebounceTimer] = useState(null);

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
              console.error(`Ошибка загрузки данных для события ${event.id}:`, error);
              event.imageUrl = null;
            }
            return event;
          })
        );

        const filteredEvents = eventsWithData.filter(event => eventIds.includes(event.id));

        setEvents(filteredEvents);
      } catch (error) {
        setError("Ошибка при загрузке данных.");
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

  const handleUnregister = async (eventId) => {
    try {
      const participantId = localStorage.getItem("participantId");
      await api.delete(`/Participants/remove/${eventId}/${participantId}`);
      setEvents(prevEvents => prevEvents.filter(event => event.id !== eventId));
    } catch (error) {
      setError("Ошибка при отмене регистрации.");
      console.error("Error during unregistering:", error);
    }
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
        <NotificationsButton />
      </div>
      <div>
        <h1>Мои события</h1>
        
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
            {events.map((event, index) => (
              <div key={event.id}>
                <li>
                  <p>№ {index + 1 + (page - 1) * pageSize}</p> {/* Нумерация объектов */}
                  {event.imageUrl ? (
                    <img src={event.imageUrl} alt={event.title} width="200" />
                  ) : (
                    <p>Изображение отсутствует</p>
                  )}
                  <p>{event.title}</p>
                  <p>Дата: {new Date(event.dateAndTime).toLocaleDateString()}</p>
                  <button onClick={() => navigate(`/events/${event.id}`)}>Подробнее</button>
                  <button onClick={() => handleUnregister(event.id)}>Отменить участие</button>
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
    </div>
  );
};

export default AccountEventsList;
