"use client";

import { useEffect, useState } from "react";
import { useRouter, useParams } from "next/navigation";

export default function DocumentDetailsPage() {
  const router = useRouter();
  const { id } = useParams(); // получаем id документа из URL
  const [document, setDocument] = useState(null);
  const [routes, setRoutes] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  // Загрузка документа и маршрутов
  useEffect(() => {
    async function fetchData() {
      try {
        setLoading(true);

        const [docRes, routesRes] = await Promise.all([
          fetch(`http://localhost:5289/api/documents/${id}`),
          fetch(`http://localhost:5289/api/documentRoutes/document/${id}`)
        ]);

        if (!docRes.ok) throw new Error("Не удалось загрузить документ");
        if (!routesRes.ok) throw new Error("Не удалось загрузить маршруты");

        const docData = await docRes.json();
        const routesData = await routesRes.json();

        setDocument(docData);
        setRoutes(routesData);
      } catch (e) {
        setError(e.message);
      } finally {
        setLoading(false);
      }
    }

    fetchData();
  }, [id]);

  if (loading) return <p className="p-4">Загрузка документа...</p>;
  if (error) return <p className="p-4 text-red-500">Ошибка: {error}</p>;
  if (!document) return <p className="p-4">Документ не найден</p>;

  const statusMap = {
    ReceivedBySecretary: "Принят секретарем",
    SentToDirector: "Отправлен директору",
    SentToExecutor: "Отправлен исполнителю",
    InProgress: "В работе",
    ReturnedToDirector: "Возвращен директору",
    Approved: "Одобрен",
    ReturnedForRevision: "Возвращен на доработку"
  };

  const actionMap = {
    Forward: "Переслан",
    ReturnForRevision: "Возвращен на доработку",
    Approve: "Одобрен"
  };

  return (
    <div className="min-h-screen bg-gray-50 p-6 text-gray-900 max-w-4xl mx-auto">
      <button
        onClick={() => router.back()}
        className="mb-6 text-sm text-blue-600 hover:underline"
      >
        ← Назад
      </button>

      <h1 className="text-3xl font-bold mb-4">{document.title}</h1>

      <p className="mb-2">
        <strong>Статус:</strong> {statusMap[document.status] || "Неизвестен"}
      </p>
      <p className="mb-2">
        <strong>Отправитель:</strong> {document.senderUser.fullName}
      </p>
      <p className="mb-2">
        <strong>Текущий держатель:</strong> {document.currentUser ? document.currentUser.fullName : "Нет"}
      </p>
      <p className="mb-6">
        <strong>Дата создания:</strong>{" "}
        {new Date(document.createdAt).toLocaleString("ru-RU")}
      </p>

      <a
        href={document.fileUrl}
        target="_blank"
        rel="noopener noreferrer"
        className="inline-block mb-8 px-4 py-2 bg-black text-white rounded hover:bg-gray-800 transition"
      >
        Скачать файл
      </a>

      <section>
        <h2 className="text-2xl font-semibold mb-4">История маршрутов</h2>

        {routes.length === 0 && <p>Маршруты отсутствуют.</p>}

        <ul className="space-y-4">
          {routes.map((route) => (
            <li
              key={route.id}
              className="bg-white p-4 rounded-lg shadow border border-gray-200"
            >
              <p>
                <strong>Действие:</strong> {actionMap[route.action] || "Неизвестно"}
              </p>
              <p>
                <strong>От:</strong> {route.fromUser.fullName}{" "}
                <strong>→ Кому:</strong> {route.toUser.fullName}
              </p>
              <p>
                <strong>Дата:</strong>{" "}
                {new Date(route.sentAt).toLocaleString("ru-RU")}
              </p>
              {route.comment && (
                <p className="italic text-gray-600 mt-2">
                  Комментарий: {route.comment}
                </p>
              )}
            </li>
          ))}
        </ul>
      </section>
    </div>
  );
}