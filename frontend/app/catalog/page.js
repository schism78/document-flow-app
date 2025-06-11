'use client';

import { useState, useEffect } from 'react';
import Header from '../components/header';
import Link from 'next/link';

const mockBooks = [
  { id: 1, title: '1984', author: 'Джордж Оруэлл', genre: 'Фантастика' },
  { id: 2, title: 'Мастер и Маргарита', author: 'Михаил Булгаков', genre: 'Роман' },
  { id: 3, title: 'Война и мир', author: 'Лев Толстой', genre: 'Исторический роман' },
  // Добавьте больше книг по мере необходимости
];

export default function BookCatalog() {
  const [books, setBooks] = useState([]);
  const [searchTerm, setSearchTerm] = useState('');

  useEffect(() => {
    // Здесь можно заменить mockBooks на реальный API-запрос
    setBooks(mockBooks);
  }, []);

  const filteredBooks = books.filter(book =>
    book.title.toLowerCase().includes(searchTerm.toLowerCase()) ||
    book.author.toLowerCase().includes(searchTerm.toLowerCase())
  );

  return (
    <div className="min-h-screen bg-white font-sans text-gray-700">
      <Header />

      <main className="max-w-screen-lg mx-auto px-6 py-16">
        <h1 className="text-4xl font-bold mb-8 text-center">Каталог книг</h1>

        {/* Поисковая строка */}
        <div className="mb-8">
          <input
            type="text"
            placeholder="Поиск по названию или автору"
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            className="w-full p-2 border border-gray-300 rounded"
          />
        </div>

        {/* Список книг */}
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {filteredBooks.length > 0 ? (
            filteredBooks.map(book => (
              <div key={book.id} className="border rounded-lg p-4 shadow-md">
                <h2 className="text-xl font-semibold">{book.title}</h2>
                <p className="text-gray-600">Автор: {book.author}</p>
                <p className="text-gray-500">Жанр: {book.genre}</p>
                <Link href={`/book/${book.id}`} className="mt-4 inline-block text-blue-500 hover:underline">
                  Подробнее
                </Link>
              </div>
            ))
          ) : (
            <p className="text-center text-gray-500">Книги не найдены.</p>
          )}
        </div>
      </main>
    </div>
  );
}
