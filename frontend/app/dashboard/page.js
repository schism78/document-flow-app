"use client";

import { useRouter } from "next/navigation";

export default function DashboardPage() {
  const router = useRouter();

  return (
    <div style={{ padding: 20 }}>
      <h1>Документооборот — Dashboard</h1>

      <nav style={{ marginBottom: 20 }}>
        <button onClick={() => router.push("/dashboard/inbox")} style={{ marginRight: 10 }}>
          Входящие
        </button>
        <button onClick={() => router.push("/dashboard/sent")} style={{ marginRight: 10 }}>
          Отправленные
        </button>
        <button onClick={() => router.push("/dashboard/profile")}>Профиль</button>
      </nav>

      <p>Здесь будет содержимое выбранного раздела.</p>
    </div>
  );
}