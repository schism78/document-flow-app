'use client';

import { useState, useEffect } from 'react';
import { useRouter } from 'next/navigation';
import Header from '../components/header';
import Link from 'next/link';

export default function UserProfile() {
  const router = useRouter();
  const [user, setUser] = useState(null);
  const [error, setError] = useState('');

  useEffect(() => {
    const userData = localStorage.getItem('userData');

    if (userData) {
      setUser(JSON.parse(userData));
    } else {
      router.push('/auth');
    }
  }, []);

  if (!user) {
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

        {/* Greeting Section */}
        <section className="mb-20 text-center">
          <h1 className="text-5xl font-extrabold tracking-tight mb-3 text-gray-900">
            Привет, {user.username}!
          </h1>
          <p className="text-lg text-gray-500 max-w-xl mx-auto">
            Добро пожаловать в ваш личный кабинет LibraryLife. Здесь вы можете управлять своими бронированиями и просматривать историю.
          </p>
        </section>

        {/* Navigation Tabs */}
        <nav className="mb-20">
          <ul className="flex flex-col md:flex-row justify-center space-y-4 md:space-y-0 md:space-x-12 text-gray-600">
            <li>
              <Link
                href="/user/reserved"
                className="inline-block text-lg font-semibold hover:text-gray-900 transition-colors duration-300"
              >
                Забронированные книги
              </Link>
            </li>
            <li>
              <Link
                href="/user/borrowed"
                className="inline-block text-lg font-semibold hover:text-gray-900 transition-colors duration-300"
              >
                Взятые книги
              </Link>
            </li>
            <li>
              <Link
                href="/user/history"
                className="inline-block text-lg font-semibold hover:text-gray-900 transition-colors duration-300"
              >
                История бронирования
              </Link>
            </li>
          </ul>
        </nav>
      </main>
    </div>
  );
}
