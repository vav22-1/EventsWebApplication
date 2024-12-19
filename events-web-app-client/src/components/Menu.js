import React, { useState } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import NotificationsButton from "./NotificationsButton";
import ConfirmationModal from "./ConfirmationModal";
import api from "../services/axiosConfig";
import Toast from '../components/Toast';
import "../css/Menu.css";

const Menu = ({ selectedEventId }) => {
  const navigate = useNavigate();
  const location = useLocation();

  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);

  const [toastMessage, setToastMessage] = useState('');

  const showBackToEventsButton =
    location.pathname.includes("/events/") || 
    location.pathname.includes("/new-event") || 
    location.pathname.includes("/update-event/") ||
    location.pathname.includes("/account/") ||
    location.pathname.includes("/my-events");

  const showAddEventButton = location.pathname === "/events" && localStorage.getItem("role") === "Admin";

  const showUpdateEventButton = location.pathname.includes("/events/") && selectedEventId && localStorage.getItem("role") === "Admin";

  const showAccountButton = !location.pathname.includes("/account/");

  const showMyEvents = !location.pathname.includes("/my-events");

  const showDeleteEventButton = location.pathname.includes("/events") && selectedEventId && localStorage.getItem("role") === "Admin";

  const openDeleteModal = () => {
    setIsDeleteModalOpen(true);
  };

  const closeDeleteModal = () => {
    setIsDeleteModalOpen(false);
  };

  const handleDelete = async () => {
    try {
      await api.delete(`/Events/${selectedEventId}`);
      setToastMessage("Событие успешно удалено!");
      setIsDeleteModalOpen(false);
      setTimeout(() => {
        navigate('/events');
      }, 3000);
    } catch (err) {
      setToastMessage("Ошибка при удалении события.");
      console.error("Ошибка при удалении:", err);
    } finally {
      closeDeleteModal();
    }
  };

  return (
    <div className="menu">
        {toastMessage && <Toast message={toastMessage} onClose={() => setToastMessage('')} />}
      <div className="menu-left">
        {showAccountButton && (
          <button
            className="menu-button"
            onClick={() => navigate(`/account/${localStorage.getItem("participantId")}`)}
          >
            Личный кабинет
          </button>
        )}
        {showBackToEventsButton && (
          <button className="menu-button" onClick={() => navigate('/events')}>
            К списку событий
          </button>
        )}
        {showMyEvents && (
        <button className="menu-button" onClick={() => navigate('/my-events')}>
          Мои события
        </button>
        )}
        {showAddEventButton && (
          <button className="menu-button" onClick={() => navigate('/new-event')}>
            Добавить событие
          </button>
        )}
        {showUpdateEventButton && (
          <button
            className="menu-button"
            onClick={() => navigate(`/update-event/${selectedEventId}`)}
          >
            Обновить событие
          </button>
        )}
        {showDeleteEventButton && (
          <button
          className="menu-button"
          onClick={openDeleteModal}
        >
          Удалить событие
        </button>
        )}
      </div>
      <div className="menu-right">
        <NotificationsButton className="notifications-button"/>
      </div>
      <ConfirmationModal
        isOpen={isDeleteModalOpen}
        onConfirm={handleDelete}
        onCancel={closeDeleteModal}
        message="Вы уверены, что хотите удалить это событие?"
      />
    </div>
  );
};

export default Menu;
