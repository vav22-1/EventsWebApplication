import React, { useState } from "react";
import NotificationsOverlay from "./NotificationsOverlay";
import bellIcon from "../assets/images/Bell.png";
import crossIcon from "../assets/images/Cross.png";

const NotificationsButton = () => {
  const [isOverlayOpen, setIsOverlayOpen] = useState(false);

  const toggleOverlay = () => {
    setIsOverlayOpen((prev) => !prev);
  };

  return (
    <div>
      <button className="notifications-button" onClick={toggleOverlay}>
        {!isOverlayOpen ? (
            <img src={bellIcon} alt="Уведомления" width={24} height={24} />
        ) : (
            <img src={crossIcon} alt="Закрыть уведомления" width={24} height={24} />
        )}
      </button>
      <NotificationsOverlay isOpen={isOverlayOpen} close={toggleOverlay} />
    </div>
  );
};

export default NotificationsButton;
