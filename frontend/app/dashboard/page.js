"use client";

export default function DashboardPage() {
  // Имитация текущего пользователя
  const currentUser = {
    id: 5,
    fullName: "Иван Петров",
    role: "Директор",
    department: { name: "Финансовый отдел" },
  };

  // Пример данных, позже придёт с API
  const documentsInHand = [
    { id: 1, title: "Письмо от МинФина", status: "InProgress" },
    { id: 2, title: "Смета по проекту", status: "ReturnedForRevision" },
  ];

  const documentsSent = [
    { id: 3, title: "Запрос на закупку", status: "SentToExecutor" },
    { id: 4, title: "Отчет за квартал", status: "Approved" },
  ];

  return (
    <div className="min-h-screen bg-gray-50 p-6 text-gray-900">
      <header className="mb-10">
        <h1 className="text-2xl font-bold">Здравствуйте, {currentUser.fullName}!</h1>
        <p className="text-sm text-gray-500">
          {currentUser.role} / {currentUser.department.name}
        </p>
      </header>

      <section className="mb-10">
        <h2 className="text-xl font-semibold mb-4">📄 Документы у вас</h2>
        <div className="grid gap-4 grid-cols-1 sm:grid-cols-2 md:grid-cols-3">
          {documentsInHand.map((doc) => (
            <DocumentCard key={doc.id} title={doc.title} status={doc.status} />
          ))}
        </div>
      </section>

      <section>
        <h2 className="text-xl font-semibold mb-4">📤 Документы, созданные вами</h2>
        <div className="grid gap-4 grid-cols-1 sm:grid-cols-2 md:grid-cols-3">
          {documentsSent.map((doc) => (
            <DocumentCard key={doc.id} title={doc.title} status={doc.status} />
          ))}
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
