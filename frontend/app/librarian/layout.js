'use client';

import React, { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';

export default function UserLayout({ children }) {
  const router = useRouter();
  const [user, setUser] = useState(null);

  useEffect(() => {
    // Получаем данные пользователя из localStorage
    const userData = localStorage.getItem('userData');
    if (!userData) {
      router.push('/login');
    } else {
      const parsedUser = JSON.parse(userData);
      if (parsedUser.role !== 'Librarian') {
        router.push('/');
      } else {
        setUser(parsedUser);
      }
    }
  }, [router]);

  if (!user) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-white text-gray-500 font-sans">
        <p className="text-lg">Загрузка...</p>
      </div>
    );
  }

  return (
    <div>
      <main>
        {children}
      </main>
    </div>
  );
}

