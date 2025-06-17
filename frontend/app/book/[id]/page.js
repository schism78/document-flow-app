'use client';

import { useState, useEffect } from 'react';
import { useRouter, usePathname } from 'next/navigation';
import Link from 'next/link';
import Header from '../../components/header';

export default function BookPage() {
  const pathname = usePathname();
  const id = pathname.split('/').pop();

  const [book, setBook] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  // Заглушка похожих книг (для заполнения свободного места)
  const [relatedBooks, setRelatedBooks] = useState([]);

  useEffect(() => {
    if (!id) return;
    const fetchBook = async () => {
      setLoading(true);
      try {
        const response = await fetch(`http://localhost:5289/api/books/${id}`);
        if (!response.ok) throw new Error('Ошибка загрузки книги');
        const data = await response.json();
        setBook(data);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };

    const fetchRelatedBooks = async () => {
      // Позже реализовать логику рекомендации книг
      try {
        const response = await fetch('http://localhost:5289/api/books');
        if (!response.ok) throw new Error('Ошибка загрузки похожих книг');
        const data = await response.json();
        const booksArr = data.$values || [];
        // Отфильтровать текущую книгу и взять до 3х
        const related = booksArr.filter(b => b.id !== Number(id)).slice(0, 3);
        setRelatedBooks(related);
      } catch {
        setRelatedBooks([]);
      }
    };

    fetchBook();
    fetchRelatedBooks();
  }, [id]);

  if (loading) {
    return (
      <div className="min-h-screen bg-white font-sans text-gray-700">
        <Header />
        <main className="max-w-screen-lg mx-auto px-6 py-16 text-center">
          <p className="text-xl text-gray-500 animate-pulse">Загрузка информации о книге...</p>
        </main>
      </div>
    );
  }

  if (error) {
    return (
      <div className="min-h-screen bg-white font-sans text-gray-700">
        <Header />
        <main className="max-w-screen-lg mx-auto px-6 py-16 text-center">
          <p className="text-xl text-red-500">{error}</p>
          <Link href="/catalog" className="text-blue-500 hover:underline mt-4 inline-block">
            Вернуться к каталогу
          </Link>
        </main>
      </div>
    );
  }

  if (!book) {
    return (
      <div className="min-h-screen bg-white font-sans text-gray-700">
        <Header />
        <main className="max-w-screen-lg mx-auto px-6 py-16 text-center">
          <p className="text-xl text-gray-500">Книга не найдена.</p>
          <Link href="/catalog" className="text-blue-500 hover:underline mt-4 inline-block">
            Вернуться к каталогу
          </Link>
        </main>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-white font-sans text-gray-700">
      <Header />

      <main className="max-w-screen-xl mx-auto px-6 py-16 grid grid-cols-1 lg:grid-cols-3 gap-12">
        {/* Основной блок книги - занимает 2 колонки */}
        <section className="lg:col-span-2 space-y-8">
          <h1 className="text-5xl font-extrabold">{book.title}</h1>
          <p className="text-lg text-gray-600 font-semibold">Автор: {book.author}</p>
          <p className="text-gray-500 italic">{book.genre?.name || 'Жанр не указан'}</p>

          {/* Аннотация */}
          {book.annotation ? (
            <section>
              <h2 className="text-2xl font-bold mb-2 border-b border-gray-300 pb-1">Аннотация</h2>
              <p className="text-md leading-relaxed text-gray-700 whitespace-pre-line">{book.annotation}</p>
            </section>
          ) : (
            <p className="text-gray-400 italic">Описание отсутствует.</p>
          )}

          {/* Статистика */}
          <section className="flex gap-12 mt-8">
            <div>
              <h3 className="text-xl font-semibold">Всего копий</h3>
              <p className="text-gray-600 text-lg">{book.totalCopies ?? '–'}</p>
            </div>
            <div>
              <h3 className="text-xl font-semibold">Доступно</h3>
              <p className="text-gray-600 text-lg">{book.availableCopies ?? '–'}</p>
            </div>
          </section>

          {/* Кнопка бронирования (если доступно) */}
          {book.availableCopies > 0 ? (
            <button
              type="button"
              className="mt-8 px-6 py-3 bg-green-600 hover:bg-green-700 rounded-lg text-white font-semibold transition"
              onClick={() => alert('Функция бронирования пока не реализована')}
            >
              Забронировать книгу
            </button>
          ) : (
            <p className="mt-8 text-red-600 font-semibold">Все копии книги заняты.</p>
          )}

          {/* Отзывы */}
          <section className="mt-12">
            <h2 className="text-2xl font-bold mb-6">Отзывы</h2>
            {book.reviews && book.reviews.length > 0 ? (
              <div className="space-y-6">
                {book.reviews.map((review, idx) => (
                  <div key={idx} className="border p-4 rounded-md shadow-sm bg-gray-50">
                    <p className="text-gray-800 italic">"{review.text || 'Без текста отзыва'}"</p>
                    <p className="mt-2 text-sm font-semibold text-gray-700">— {review.authorName || 'Аноним'}</p>
                  </div>
                ))}
              </div>
            ) : (
              <p className="text-gray-500 italic">Пока нет отзывов на эту книгу.</p>
            )}
          </section>
        </section>

        {/* Боковая панель с похожими книгами */}
        <aside className="space-y-8">
            <h3 className="text-3xl font-semibold border-b pb-3 mb-6">Похожие книги</h3>
            {relatedBooks.length > 0 ? (
                relatedBooks.map(rb => (
                <div key={rb.id} className="border rounded-lg p-4 shadow hover:shadow-lg transition cursor-pointer">
                    <h4 className="font-semibold text-lg">{rb.title}</h4>
                    <p className="text-gray-600 text-sm mb-1">Автор: {rb.author}</p>
                    <p className="text-gray-500 text-sm">Жанр: {rb.genre?.name || '–'}</p>
                    <Link href={`/book/${rb.id}`} className="text-blue-500 hover:underline text-sm">
                    Подробнее
                    </Link>
                </div>
                ))
            ) : (
                <p className="text-gray-500 italic">Похожие книги недоступны.</p>
            )}
        </aside>

      </main>
    </div>
  );
}

