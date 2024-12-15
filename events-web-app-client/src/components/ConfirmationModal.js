import React from "react";
import "../css/ConfirmationModal.css";

const ConfirmationModal = ({ isOpen, onConfirm, onCancel, message }) => {
  if (!isOpen) return null;

  return (
    <div className="modal-overlay">
      <div className="modal-content">
        <p>{message}</p>
        <div className="modal-actions">
          <button className="btn-confirm" onClick={onConfirm}>Подтвердить</button>
          <button className="btn-cancel" onClick={onCancel}>Отмена</button>
        </div>
      </div>
    </div>
  );
};

export default ConfirmationModal;