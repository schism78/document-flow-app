'use client';

import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import Header from '../components/header';

export default function LibrarianPage() {
  const router = useRouter();
  const [librarian, setLibrarian] = useState(null);
  const [error, setError] = useState('');

  useEffect(() => {
    const userData = localStorage.getItem('userData');

    if (userData) {
      const parsedUser  = JSON.parse(userData);
      if (parsedUser .role !== 'Librarian') {
        // Если роль не Librarian — редирект на главную страницу
        router.push('/');
      } else {
        setLibrarian(parsedUser );
      }
    } else {
      router.push('/login');
    }
  }, [router]);

  if (!librarian) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-white text-gray-500 font-sans">
        <p className="text-lg">Загрузка...</p>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-white font-sans text-gray-700">
      <Header />

      <main className="max-w-screen-lg mx-auto px-6 py-16">
        <h1 className="text-4xl font-bold mb-8 text-center">Добро пожаловать, {librarian.username}!</h1>

        <section className="mb-12">
          <h2 className="text-2xl font-semibold mb-4">Управление книгами</h2>
          <p className="text-gray-600 mb-4">Здесь вы можете добавлять, редактировать и удалять книги.</p>
          <button
            onClick={() => router.push('/librarian/books')}
            className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 transition"
          >
            Перейти к управлению книгами
          </button>
        </section>

        <section>
          <h2 className="text-2xl font-semibold mb-4">Управление бронированиями</h2>
          <p className="text-gray-600 mb-4">Здесь вы можете просматривать и управлять бронированиями пользователей.</p>
          <button
            onClick={() => router.push('/librarian/reservations')}
            className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 transition"
          >
            Перейти к управлению бронированиями
          </button>
        </section>
      </main>
    </div>
  );
}
