import React, { useEffect, useState } from "react";
import api from '../services/axiosConfig';

const NotificationsOverlay = ({ isOpen, close }) => {
  const [notifications, setNotifications] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const loadNotifications = async () => {
      try {
        const response = await api.get("/Notifications");
        setNotifications(response.data);
        setLoading(false);
      } catch (err) {
        setError("Не удалось загрузить уведомления");
        setLoading(false);
      }
    };

    loadNotifications();
  }, []);

  const formatDate = (date) => {
    const options = {
      hour: '2-digit',
      minute: '2-digit',
      second: '2-digit',
    };
    return new Date(date).toLocaleTimeString([], options);
  };

  if (!isOpen) return null;

  return (
    <div className={`notifications-overlay ${isOpen ? 'open' : ''}`} onClick={close}>
      <div className="notifications-container" onClick={(e) => e.stopPropagation()}>
        <h2>Уведомления</h2>
        {loading ? (
          <p>Загрузка...</p>
        ) : error ? (
          <p>{error}</p>
        ) : notifications.length === 0 ? (
          <p>Нет новых уведомлений</p>
        ) : (
          <ul className="notifications-list">
            {notifications.map((notification) => (
              <li key={notification.id} className="notification-item">
                <p>{notification.message}</p>
                <p className="notification-time">{formatDate(notification.createdAt)}</p>
              </li>
            ))}
          </ul>
        )}
      </div>
    </div>
  );
};

export default NotificationsOverlay;
