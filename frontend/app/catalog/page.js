'use client';

import { useState, useEffect } from 'react';
import Header from '../components/header';
import Link from 'next/link';

export default function BookCatalog() {
  const [books, setBooks] = useState([]);
  const [searchTerm, setSearchTerm] = useState('');

  useEffect(() => {
    const fetchBooks = async () => {
      const response = await fetch('http://localhost:5289/api/books');
      const data = await response.json();
      console.log(data); // Проверьте структуру данных
      setBooks(data.$values || []); // Убедитесь, что устанавливаете массив
    };

    fetchBooks();
  }, []);

  const filteredBooks = books.filter(book =>
    book.title && book.author && (
      book.title.toLowerCase().includes(searchTerm.toLowerCase()) ||
      book.author.toLowerCase().includes(searchTerm.toLowerCase())
    )
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
                <p className="text-gray-500">Жанр: {book.genre?.name}</p>
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
