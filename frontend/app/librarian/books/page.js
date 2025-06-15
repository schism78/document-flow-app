'use client';

import Header from '../../components/header';
import { useState, useEffect } from 'react';
import { useRouter } from 'next/navigation';

export default function LibrarianPage() {
    const router = useRouter();

    const [title, setTitle] = useState('');
    const [author, setAuthor] = useState('');
    const [annotation, setAnnotation] = useState('');
    const [genreId, setGenreId] = useState('');
    const [totalCopies, setTotalCopies] = useState(1);
    const [availableCopies, setAvailableCopies] = useState(1);
    const [genres, setGenres] = useState([]);
    const [books, setBooks] = useState([]);
    const [error, setError] = useState('');
    const [newGenre, setNewGenre] = useState('');
    const [successMessage, setSuccessMessage] = useState('');
    const [searchQuery, setSearchQuery] = useState('');
    const [currentPage, setCurrentPage] = useState(1);
    const booksPerPage = 5;

    const filteredBooks = books.filter((book) => {
        const lowerQuery = searchQuery.toLowerCase();
        return (
            book.title?.toLowerCase().includes(lowerQuery) ||
            book.author?.toLowerCase().includes(lowerQuery)
        );
    });

    const indexOfLastBook = currentPage * booksPerPage;
    const indexOfFirstBook = indexOfLastBook - booksPerPage;
    const currentBooks = filteredBooks.slice(indexOfFirstBook, indexOfLastBook);
    const totalPages = Math.ceil(filteredBooks.length / booksPerPage);

    useEffect(() => {
        async function fetchBooks() {
            const res = await fetch('http://localhost:5289/api/books');
            const data = await res.json();
            setBooks(data.$values); 
        }

        async function fetchGenres() {
            const res = await fetch('http://localhost:5289/api/genres');
            const data = await res.json();
            setGenres(data.$values); 
        }

        fetchBooks();
        fetchGenres();
    }, []);

    async function handleAddBook(e) {
        e.preventDefault();
        setError('');

        try {
            const res = await fetch('http://localhost:5289/api/books', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    title,
                    author,
                    annotation,
                    genreId: genreId ? parseInt(genreId) : null,
                    totalCopies,
                    availableCopies: totalCopies,
                }),
            });

            if (!res.ok) {
                throw new Error('Ошибка при добавлении книги');
            }

            const newBook = await res.json();

            setSuccessMessage('Книга успешно добавлена!');
            setTimeout(() => setSuccessMessage(''), 3000);

            setBooks([...books, newBook]);

            // Очистка формы после успешного добавления
            setTitle('');
            setAuthor('');
            setAnnotation('');
            setGenreId('');
            setTotalCopies(1);
            setAvailableCopies(1);
        } catch (error) {
            setError(error.message);
        }
    }

    async function handleAddGenre(e) {
        e.preventDefault();
        setError('');

        if (newGenre.trim() === '') {
            setError('Название жанра не может быть пустым.');
            return;
        }

        try {
            const res = await fetch('http://localhost:5289/api/Genres/create', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ name: newGenre }),
            });

            if (!res.ok) {
                const errorData = await res.json();
                throw new Error(errorData.message || 'Ошибка при добавлении жанра');
            }

            const newGenreData = await res.json();

            setSuccessMessage('Жанр успешно добавлен в список!');
            setTimeout(() => setSuccessMessage(''), 3000);

            setGenres([...genres, newGenreData]);
            setNewGenre('');
        } catch (error) {
            setError(error.message);
        }
    }

    return (
        <div>
            <Header />
            <div className="min-h-screen bg-gray-100 p-6">
                <h1 className="text-4xl font-bold text-center mb-8">Управление библиотекой</h1>
                {successMessage && (
                    <div className="fixed bottom-6 left-6 z-50 w-fit max-w-sm flex items-start gap-3 bg-gradient-to-r from-green-500 to-emerald-500 text-white px-6 py-4 rounded-xl shadow-2xl animate-fade-in-out backdrop-blur-sm">
                        <div className="text-2xl">✅</div>
                        <div className="flex flex-col">
                            <span className="font-semibold text-md">Успешно!</span>
                            <span className="text-sm">{successMessage}</span>
                        </div>
                    </div>
                )}
                <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                    {/* Секция добавления книги */}
                    <div className="bg-white shadow-md rounded-lg p-6">
                        <h2 className="text-2xl font-bold mb-4">Добавить новую книгу</h2>
                        {error && <p className="text-red-600 mb-4">{error}</p>}
                        <form onSubmit={handleAddBook} className="space-y-4">
                            <div>
                                <label htmlFor="title" className="block text-sm font-semibold text-gray-900 mb-1">Название книги</label>
                                <input
                                    id="title"
                                    type="text"
                                    value={title}
                                    onChange={(e) => setTitle(e.target.value)}
                                    required
                                    className="w-full rounded-md border border-gray-300 px-4 py-2 focus:outline-none focus:ring-2 focus:ring-black"
                                />
                            </div>
                            <div>
                                <label htmlFor="author" className="block text-sm font-semibold text-gray-900 mb-1">Автор</label>
                                <input
                                    id="author"
                                    type="text"
                                    value={author}
                                    onChange={(e) => setAuthor(e.target.value)}
                                    className="w-full rounded-md border border-gray-300 px-4 py-2 focus:outline-none focus:ring-2 focus:ring-black"
                                />
                            </div>
                            <div>
                                <label htmlFor="annotation" className="block text-sm font-semibold text-gray-900 mb-1">Аннотация</label>
                                <textarea
                                    id="annotation"
                                    value={annotation}
                                    onChange={(e) => setAnnotation(e.target.value)}
                                    className="w-full rounded-md border border-gray-300 px-4 py-2 focus:outline-none focus:ring-2 focus:ring-black"
                                />
                            </div>
                            <div>
                                <label htmlFor="genre" className="block text-sm font-semibold text-gray-900 mb-1">Жанр</label>
                                <select
                                    id="genre"
                                    value={genreId}
                                    onChange={(e) => setGenreId(e.target.value)}
                                    className="w-full rounded-md border border-gray-300 px-4 py-2 focus:outline-none focus:ring-2 focus:ring-black"
                                >
                                    <option value="">Выберите жанр</option>
                                    {Array.isArray(genres) && genres.map((genre) => (
                                        <option key={genre.id} value={genre.id}>
                                            {genre.name}
                                        </option>
                                    ))}
                                </select>
                            </div>
                            <div>
                                <label htmlFor="totalCopies" className="block text-sm font-semibold text-gray-900 mb-1">Общее количество экземпляров</label>
                                <input
                                    id="totalCopies"
                                    type="number"
                                    value={totalCopies}
                                    onChange={(e) => setTotalCopies(e.target.value)}
                                    min="1"
                                    required
                                    className="w-full rounded-md border border-gray-300 px-4 py-2 focus:outline-none focus:ring-2 focus:ring-black"
                                />
                            </div>
                            <button
                                type="submit"
                                className="w-full py-3 bg-black text-white font-bold rounded-md hover:bg-gray-900 transition"
                            >
                                Добавить книгу
                            </button>
                        </form>
                    </div>

                    {/* Секция добавления жанра */}
                    <div className="bg-white shadow-md rounded-lg p-6">
                        <h2 className="text-2xl font-bold mb-4">Добавить новый жанр</h2>
                        <form onSubmit={handleAddGenre} className="space-y-4">
                            <div>
                                <label htmlFor="newGenre" className="block text-sm font-semibold text-gray-900 mb-1">Название жанра</label>
                                <input
                                    id="newGenre"
                                    type="text"
                                    value={newGenre}
                                    onChange={(e) => setNewGenre(e.target.value)}
                                    required
                                    className="w-full rounded-md border border-gray-300 px-4 py-2 focus:outline-none focus:ring-2 focus:ring-black"
                                />
                            </div>
                            <button
                                type="submit"
                                className="w-full py-3 bg-black text-white font-bold rounded-md hover:bg-gray-900 transition"
                            >
                                Добавить жанр
                            </button>
                        </form>
                    </div>
                </div>
                
                
                {/* Секция списка книг */}
                <div className="mt-10">
                    <h2 className="text-2xl font-bold mb-4">Список книг</h2>

                    {/* Поиск */}
                    <div className="mt-4 mb-4">
                        <input
                            type="text"
                            placeholder="Поиск по названию или автору"
                            value={searchQuery}
                            onChange={(e) => {
                                setSearchQuery(e.target.value);
                                setCurrentPage(1);
                            }}
                            className="w-full px-4 py-2 rounded-md border border-gray-300 focus:outline-none focus:ring-2 focus:ring-black"
                        />
                    </div>

                    <div className="bg-white shadow-md rounded-lg p-6">
                        {filteredBooks.length === 0 ? (
                            <p className="text-gray-600">Нет подходящих книг.</p>
                        ) : (
                            <div className="space-y-4">
                                {currentBooks.map((book) => {
                                    const isEmptyBook = !book.title && !book.author && !book.genre && !book.annotation;
                                    if (isEmptyBook) return null;

                                    return (
                                        <div key={book.id} className="border-b border-gray-300 pb-4">
                                            <div className="flex flex-col">
                                                <h3 className="font-semibold text-lg">{book.title || 'Без названия'}</h3>
                                                <p className="text-gray-600">Автор: {book.author || 'Не указан'}</p>
                                                <p className="text-gray-600">Жанр: {book.genre ? book.genre.name : 'Не указан'}</p>
                                                <p className="text-gray-600">Аннотация: {book.annotation || 'Нет аннотации'}</p>
                                            </div>
                                            <div className="mt-2">
                                                <div className="bg-gray-200 h-2 rounded">
                                                    <div
                                                        className="bg-blue-500 h-full rounded"
                                                        style={{ width: `${(book.availableCopies / book.totalCopies) * 100}%` }}
                                                    />
                                                </div>
                                                <p className="text-gray-600 text-sm mt-1">
                                                    Доступные копии: {book.availableCopies} из {book.totalCopies}
                                                </p>
                                            </div>
                                        </div>
                                    );
                                })}
                            </div>
                        )}

                        {/* Пагинация */}
                        {totalPages > 1 && (
                            <div className="flex justify-center gap-2 mt-6">
                                {Array.from({ length: totalPages }, (_, i) => (
                                    <button
                                        key={i}
                                        onClick={() => setCurrentPage(i + 1)}
                                        className={`px-4 py-2 rounded-md border ${
                                            currentPage === i + 1
                                                ? 'bg-black text-white'
                                                : 'bg-gray-100 hover:bg-gray-200'
                                        }`}
                                    >
                                        {i + 1}
                                    </button>
                                ))}
                            </div>
                        )}
                    </div>
                </div>
            </div>
        </div>
    );
}
