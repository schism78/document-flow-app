'use client';

import React, { useState } from 'react';

const ReservationModal = ({ isOpen, onClose, bookId, onReserve }) => {
  const [returnBy, setReturnBy] = useState('');
  const [loading, setLoading] = useState(false);
  const [errorMsg, setErrorMsg] = useState('');

  if (!isOpen) return null;

  const handleSubmit = async (e) => {
    e.preventDefault();
    setErrorMsg('');

    if (!returnBy) {
      setErrorMsg('Пожалуйста, выберите дату возврата.');
      return;
    }

    const selectedDate = new Date(returnBy);
    const today = new Date();
    today.setHours(0,0,0,0); // обнуляем время для корректного сравнения

    if (selectedDate <= today) {
      setErrorMsg('Дата возврата должна быть позже текущей даты.');
      return;
    }

    setLoading(true);

    const reservationData = {
      bookId,
      returnBy: selectedDate.toISOString(),
      userId: 1, // TODO: заменить на реальный userId
    };

    try {
      await onReserve(reservationData);
      onClose();
    } catch (error) {
      setErrorMsg(error.message || 'Ошибка при бронировании');
    } finally {
      setLoading(false);
    }
  };

  // Чтобы не разрешать выбирать даты в прошлом, ограничим минимальное значение поля
  const todayYYYYMMDD = new Date().toISOString().split('T')[0];

  return (
    <div
      className="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-60 backdrop-blur-sm"
      onClick={onClose}
      aria-modal="true"
      role="dialog"
      aria-labelledby="reservation-modal-title"
    >
      <div
        className="bg-white dark:bg-gray-900 rounded-xl p-8 max-w-md w-full mx-4 relative"
        onClick={(e) => e.stopPropagation()}
      >
        <h2
          id="reservation-modal-title"
          className="text-2xl font-semibold text-gray-900 dark:text-white mb-6"
        >
          Забронировать книгу
        </h2>

        <form onSubmit={handleSubmit} className="space-y-6">
          <label
            htmlFor="returnBy"
            className="block text-sm font-medium text-gray-700 dark:text-gray-300"
          >
            Выберите дату возврата
          </label>
          <input
            type="date"
            id="returnBy"
            className="mt-1 block w-full rounded-md border border-gray-300 dark:border-gray-700 bg-white dark:bg-gray-800 py-2 px-3 shadow-sm focus:border-blue-500 focus:ring focus:ring-blue-300 dark:focus:ring-blue-600 focus:ring-opacity-50"
            value={returnBy}
            onChange={(e) => setReturnBy(e.target.value)}
            min={todayYYYYMMDD}
            required
          />

          {errorMsg && (
            <p className="text-red-600 text-sm font-medium">{errorMsg}</p>
          )}

          <div className="flex justify-end space-x-3">
            <button
              type="button"
              onClick={onClose}
              className="rounded-md border border-gray-300 dark:border-gray-700 px-4 py-2 text-gray-700 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-gray-800 transition"
              disabled={loading}
            >
              Отмена
            </button>

            <button
              type="submit"
              disabled={loading}
              className="rounded-md bg-blue-600 px-4 py-2 font-semibold text-white hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed transition"
            >
              {loading ? 'Бронирование...' : 'Забронировать'}
            </button>
          </div>

          <p className="text-xs text-gray-500 dark:text-gray-400 mt-4 italic">
            Время бронирования начнётся только после подтверждения библиотекарем.
          </p>
        </form>
      </div>
    </div>
  );
};

export default ReservationModal;
