"use client";

import { useEffect, useState } from "react";
import Link from "next/link";

export default function DashboardPage() {
  const [currentUser, setCurrentUser] = useState(null);
  const [documentsInHand, setDocumentsInHand] = useState([]);
  const [documentsSent, setDocumentsSent] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const [showForm, setShowForm] = useState(false);
  const [title, setTitle] = useState("");
  const [receiverId, setReceiverId] = useState("");
  const [files, setFiles] = useState([]);
  const [users, setUsers] = useState([]);

  useEffect(() => {
    const storedUser = localStorage.getItem("currentUser");
    if (storedUser) {
      setCurrentUser(JSON.parse(storedUser));
    }
  }, []);

  useEffect(() => {
    if (!currentUser) return;

    async function fetchData() {
      try {
        const [docsRes, usersRes] = await Promise.all([
          fetch("http://localhost:5289/api/documents"),
          fetch("http://localhost:5289/api/users")
        ]);
        if (!docsRes.ok || !usersRes.ok) throw new Error("Ошибка при загрузке данных");

        const docs = await docsRes.json();
        const usersList = await usersRes.json();

        setDocumentsInHand(docs.filter(doc => doc.currentUserId === currentUser.id));
        setDocumentsSent(docs.filter(doc => doc.senderUserId === currentUser.id));
        setUsers(usersList.filter(u => u.id !== currentUser.id));
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    }

    fetchData();
  }, [currentUser]);

  async function handleCreateDocument(e) {
  e.preventDefault();
  if (!title || !receiverId || files.length === 0) return;

  try {
    const body = {
      title,
      senderUserId: currentUser.id,
      currentUserId: Number(receiverId),
      status: "SentToExecutor"
    };

    const docRes = await fetch("http://localhost:5289/api/documents", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(body)
    });

    if (!docRes.ok) {
      const errorData = await docRes.json();
      console.error("Ошибка сервера:", errorData);
      throw new Error(errorData.detail || errorData.title || "Ошибка при создании документа");
    }

    const createdDoc = await docRes.json();

    for (const file of files) {
      const formData = new FormData();
      formData.append("file", file);
      formData.append("documentId", createdDoc.id);

      await fetch("http://localhost:5289/api/documentfiles/upload", {
        method: "POST",
        body: formData
      });
    }

    setDocumentsSent((prev) => [...prev, createdDoc]);

    setShowForm(false);
    setTitle("");
    setFiles([]);
    setReceiverId("");

  } catch (err) {
    alert("Ошибка при создании документа: " + err.message);
  }
}

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
        <div className="flex justify-between items-center mb-4">
          <h2 className="text-xl font-semibold">📄 Исходящие</h2>
          <button
            onClick={() => setShowForm(true)}
            className="text-white bg-blue-600 hover:bg-blue-700 px-3 py-1 rounded"
          >
            + Создать
          </button>
        </div>
        <div className="grid gap-4 grid-cols-1 sm:grid-cols-2 md:grid-cols-3">
          {documentsInHand.length > 0 ? (
            documentsInHand.map((doc) => (
              <DocumentCard key={doc.id} id={doc.id} title={doc.title} status={doc.status} />
            ))
          ) : (
            <p>У вас нет документов.</p>
          )}
        </div>
      </section>

      <section>
        <h2 className="text-xl font-semibold mb-4">📤 Входящие</h2>
        <div className="grid gap-4 grid-cols-1 sm:grid-cols-2 md:grid-cols-3">
          {documentsSent.length > 0 ? (
            documentsSent.map((doc) => (
              <DocumentCard key={doc.id} id={doc.id} title={doc.title} status={doc.status} />
            ))
          ) : (
            <p>Вы не создавали документов.</p>
          )}
        </div>
      </section>

      {showForm && (
        <div className="fixed inset-0 bg-black bg-opacity-40 flex items-center justify-center z-50">
          <div className="bg-white p-6 rounded-md w-full max-w-lg">
            <h2 className="text-xl font-bold mb-4">Создать документ</h2>
            <form onSubmit={handleCreateDocument} className="space-y-4">
              <div>
                <label className="block text-sm font-medium">Заголовок</label>
                <input
                  type="text"
                  value={title}
                  onChange={(e) => setTitle(e.target.value)}
                  className="w-full border px-3 py-2 rounded"
                  required
                />
              </div>
              <div>
                <label className="block text-sm font-medium">Кому отправить</label>
                <select
                  value={receiverId}
                  onChange={(e) => setReceiverId(e.target.value)}
                  className="w-full border px-3 py-2 rounded"
                  required
                >
                  <option value="">Выберите пользователя</option>
                  {users.map((user) => (
                    <option key={user.id} value={user.id}>
                      {user.fullName} / {user.department?.name}
                    </option>
                  ))}
                </select>
              </div>
              <div>
                <label className="block text-sm font-medium">Файлы</label>
                <input
                  type="file"
                  multiple
                  onChange={(e) => setFiles(Array.from(e.target.files))}
                  className="w-full"
                  required
                />
              </div>
              <div className="flex justify-end gap-2">
                <button type="button" onClick={() => setShowForm(false)} className="px-4 py-2 bg-gray-200 rounded">
                  Отмена
                </button>
                <button type="submit" className="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700">
                  Отправить
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}

function DocumentCard({ id, title, status }) {
  const statusMap = {
    InProgress: "В работе",
    ReturnedForRevision: "Возвращено",
    Approved: "Одобрено",
    SentToExecutor: "Отправлено исполнителю",
  };

  return (
    <Link href={`/documents?id=${id}`} className="block bg-white p-4 rounded-lg shadow border border-gray-200 hover:shadow-md transition">
      <h3 className="font-medium text-lg mb-2">{title}</h3>
      <p className="text-sm text-gray-500">{statusMap[status] || "Статус неизвестен"}</p>
    </Link>
  );
}
