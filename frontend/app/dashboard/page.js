'use client';

import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';

export default function DashboardPage() {
  const router = useRouter();
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const isAuth = localStorage.getItem('isAuthenticated');
    if (!isAuth) {
      router.replace('/login');
    } else {
      setLoading(false);
    }
  }, [router]);

  if (loading) {
    return <p>Загрузка...</p>;
  }

  return (
    <div style={{ maxWidth: 600, margin: 'auto', padding: 20 }}>
      <h1>Панель управления</h1>
      <p>Вы успешно вошли в систему!</p>
      <button
        onClick={() => {
          localStorage.removeItem('isAuthenticated');
          router.push('/login');
        }}
      >
        Выйти
      </button>
    </div>
  );
}