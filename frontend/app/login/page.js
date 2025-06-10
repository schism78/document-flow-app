'use client';

import { useState } from 'react';
import { useRouter } from 'next/navigation';

export default function LoginPage() {
  const router = useRouter();
  const [usernameOrEmail, setUsernameOrEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');

  async function handleSubmit(e) {
    e.preventDefault();
    setError('');

    try {
      const res = await fetch('http://localhost:5289/api/users/login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ usernameOrEmail, password }),
      });

      if (res.ok) {
        // Можно получить тело ответа, если нужно
        // const data = await res.json();

        localStorage.setItem('isAuthenticated', 'true');
        router.push('/dashboard');
      } else if (res.status === 401) {
        const data = await res.json();
        setError(data.message || 'Неверный логин или пароль');
      } else {
        setError('Ошибка сервера');
      }
    } catch (err) {
      setError('Ошибка сети');
    }
  }

  return (
    <div style={{ maxWidth: 400, margin: 'auto', padding: 20 }}>
      <h1>Вход</h1>
      <form onSubmit={handleSubmit}>
        <label>
          Логин или email:<br />
          <input
            type="text"
            value={usernameOrEmail}
            onChange={(e) => setUsernameOrEmail(e.target.value)}
            required
            autoComplete="username"
            style={{ width: '100%', marginBottom: 10 }}
          />
        </label>
        <label>
          Пароль:<br />
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
            autoComplete="current-password"
            style={{ width: '100%', marginBottom: 10 }}
          />
        </label>
        {error && <p style={{ color: 'red' }}>{error}</p>}
        <button type="submit" style={{ width: '100%' }}>Войти</button>
      </form>
    </div>
  );
}