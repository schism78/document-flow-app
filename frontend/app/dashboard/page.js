"use client";

import { useEffect, useState } from "react";

export default function DashboardPage() {
  const [currentUser, setCurrentUser] = useState(null);
  const [documentsInHand, setDocumentsInHand] = useState([]);
  const [documentsSent, setDocumentsSent] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  // Загружаем пользователя из localStorage
  useEffect(() => {
    const storedUser = localStorage.getItem("currentUser");
    if (storedUser) {
      setCurrentUser(JSON.parse(storedUser));
    }
  }, []);

  // Загружаем документы после загрузки пользователя
  // Опять же, для отправления исключительно нужных документов добавить контроллер!!!!!!!!
  useEffect(() => {
    if (!currentUser) return;

    async function fetchDocuments() {
      try {
        const res = await fetch("http://localhost:5289/api/documents");
        if (!res.ok) throw new Error("Ошибка при загрузке документов");

        const data = await res.json();

        // Временно фильтруем по текущему пользователю епт
        setDocumentsInHand(data.filter(doc => doc.currentUserId === currentUser.id));
        setDocumentsSent(data.filter(doc => doc.senderUserId === currentUser.id));
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    }

    fetchDocuments();
  }, [currentUser]);

  if (!currentUser) return <p className="p-4">Загрузка пользователя...</p>;
  if (loading) return <p className="p-4">Загрузка документов...</p>;
  if (error) return <p className="p-4 text-red-500">Ошибка: {error}</p>;

  return (
    <div className="min-h-screen bg-gray-50 p-6 text-gray-900">
      <header className="mb-10">
        <h1 className="text-2xl font-bold">Здравствуйте, {currentUser.fullName}!</h1>
        <p className="text-sm text-gray-500">
          {currentUser.role} / {currentUser.department?.name || "Без отдела"}
        </p>
      </header>

      <section className="mb-10">
        <h2 className="text-xl font-semibold mb-4">📄 Документы у вас</h2>
        <div className="grid gap-4 grid-cols-1 sm:grid-cols-2 md:grid-cols-3">
          {documentsInHand.length > 0 ? (
            documentsInHand.map((doc) => (
              <DocumentCard key={doc.id} title={doc.title} status={doc.status} />
            ))
          ) : (
            <p>У вас нет документов.</p>
          )}
        </div>
      </section>

      <section>
        <h2 className="text-xl font-semibold mb-4">📤 Документы, созданные вами</h2>
        <div className="grid gap-4 grid-cols-1 sm:grid-cols-2 md:grid-cols-3">
          {documentsSent.length > 0 ? (
            documentsSent.map((doc) => (
              <DocumentCard key={doc.id} title={doc.title} status={doc.status} />
            ))
          ) : (
            <p>Вы не создавали документов.</p>
          )}
        </div>
      </section>
    </div>
  );
}

function DocumentCard({ title, status }) {
  const statusMap = {
    InProgress: "В работе",
    ReturnedForRevision: "Возвращено",
    Approved: "Одобрено",
    SentToExecutor: "Отправлено исполнителю",
  };

  return (
    <div className="bg-white p-4 rounded-lg shadow border border-gray-200 hover:shadow-md transition">
      <h3 className="font-medium text-lg mb-2">{title}</h3>
      <p className="text-sm text-gray-500">{statusMap[status] || "Статус неизвестен"}</p>
    </div>
  );
}
