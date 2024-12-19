import React, { useEffect } from 'react';
import "../css/Toast.css";

const Toast = ({ message, onClose }) => {
  useEffect(() => {
    const timer = setTimeout(() => {
      onClose();
    }, 5000);

    return () => clearTimeout(timer);
  }, [message, onClose]);

  return (
    <div className="toast">
      {message}
    </div>
  );
};

export default Toast;
