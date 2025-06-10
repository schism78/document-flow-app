'use client';

import { useState } from 'react';
import { useRouter } from 'next/navigation';

export default function AuthPage() {
  const router = useRouter();

  const [username, setUsername] = useState('');
  const [email, setEmail] = useState('');
  const [usernameOrEmail, setUsernameOrEmail] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [error, setError] = useState('');
  const [isRegister, setIsRegister] = useState(false);

  async function handleSubmit(e) {
    e.preventDefault();
    setError('');

    if (isRegister && password !== confirmPassword) {
      setError('Пароли не совпадают');
      return;
    }

    try {
      const url = isRegister
        ? 'http://localhost:5289/api/users/register'
        : 'http://localhost:5289/api/users/login';

      const body = isRegister
        ? { username, email, password, confirmPassword }
        : { usernameOrEmail, password };

      const res = await fetch(url, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(body),
      });

      if (res.ok) {
        localStorage.setItem('isAuthenticated', 'true');
        router.push('/dashboard');
      } else if (res.status === 401) {
        const data = await res.json();
        setError(data.message || 'Неверный логин или пароль');
      } else {
        setError('Ошибка сервера');
      }
    } catch {
      setError('Ошибка сети');
    }
  }

  return (
    <div className="min-h-screen bg-white flex items-center justify-center px-6">
      <div className="max-w-md w-full p-10 border border-gray-300 rounded-xl shadow-lg">
        <h2 className="text-3xl font-bold text-black mb-8 text-center">
          {isRegister ? 'Регистрация' : 'Вход'}
        </h2>

        <form onSubmit={handleSubmit} className="space-y-6">
          {isRegister ? (
            <>
              <div>
                <label
                  htmlFor="username"
                  className="block text-sm font-semibold text-gray-900 mb-2"
                >
                  Логин
                </label>
                <input
                  id="username"
                  type="text"
                  value={username}
                  onChange={(e) => setUsername(e.target.value)}
                  required
                  placeholder="Введите логин"
                  className="w-full rounded-md border border-gray-400 px-4 py-2 text-black placeholder-gray-500 focus:outline-none focus:ring-2 focus:ring-black focus:border-black transition"
                />
              </div>

              <div>
                <label
                  htmlFor="email"
                  className="block text-sm font-semibold text-gray-900 mb-2"
                >
                  Email
                </label>
                <input
                  id="email"
                  type="email"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  required
                  placeholder="Введите почту"
                  className="w-full rounded-md border border-gray-400 px-4 py-2 text-black placeholder-gray-500 focus:outline-none focus:ring-2 focus:ring-black focus:border-black transition"
                />
              </div>
            </>
          ) : (
            <div>
              <label
                htmlFor="usernameOrEmail"
                className="block text-sm font-semibold text-gray-900 mb-2"
              >
                Логин или Email
              </label>
              <input
                id="usernameOrEmail"
                type="text"
                value={usernameOrEmail}
                onChange={(e) => setUsernameOrEmail(e.target.value)}
                required
                placeholder="Введите логин или почту"
                className="w-full rounded-md border border-gray-400 px-4 py-2 text-black placeholder-gray-500 focus:outline-none focus:ring-2 focus:ring-black focus:border-black transition"
              />
            </div>
          )}

          <div>
            <label
              htmlFor="password"
              className="block text-sm font-semibold text-gray-900 mb-2"
            >
              Пароль
            </label>
            <input
              id="password"
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
              autoComplete={isRegister ? 'new-password' : 'current-password'}
              placeholder="Введите пароль"
              className="w-full rounded-md border border-gray-400 px-4 py-2 text-black placeholder-gray-500 focus:outline-none focus:ring-2 focus:ring-black focus:border-black transition"
            />
          </div>

          {isRegister && (
            <div>
              <label
                htmlFor="confirmPassword"
                className="block text-sm font-semibold text-gray-900 mb-2"
              >
                Подтвердите пароль
              </label>
              <input
                id="confirmPassword"
                type="password"
                value={confirmPassword}
                onChange={(e) => setConfirmPassword(e.target.value)}
                required
                autoComplete="new-password"
                placeholder="Повторите пароль"
                className="w-full rounded-md border border-gray-400 px-4 py-2 text-black placeholder-gray-500 focus:outline-none focus:ring-2 focus:ring-black focus:border-black transition"
              />
            </div>
          )}

          {error && (
            <p className="text-red-600 text-center text-sm font-semibold">
              {error}
            </p>
          )}

          <button
            type="submit"
            className="w-full py-3 bg-black text-white font-bold rounded-md hover:bg-gray-900 transition"
          >
            {isRegister ? 'Зарегистрироваться' : 'Войти'}
          </button>
        </form>

        <p className="mt-8 text-center text-gray-700 font-medium text-sm select-none">
          {isRegister ? 'Уже есть аккаунт?' : 'Нет аккаунта?'}{' '}
          <button
            onClick={() => {
              setIsRegister(!isRegister);
              setError('');
              setUsername('');
              setEmail('');
              setUsernameOrEmail('');
              setPassword('');
              setConfirmPassword('');
            }}
            className="underline hover:text-black focus:outline-none transition"
            type="button"
          >
            {isRegister ? 'Войти' : 'Зарегистрироваться'}
          </button>
        </p>
      </div>
    </div>
  );
}