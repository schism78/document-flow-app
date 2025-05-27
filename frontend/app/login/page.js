"use client";

import { useRouter } from "next/navigation";
import { useState, useEffect } from "react";

export default function LoginPage() {
  const router = useRouter();
  const [login, setLogin] = useState("");
  const [password, setPassword] = useState("");
  const [users, setUsers] = useState([]);
  const [error, setError] = useState("");

  /*
    ПОЗЖЕ ИЗМЕНИТЬ НА @tanstack-query/react
    ХЭШ-ПАРОЛЬ НЕ ИСПОЛЬЗУЕТСЯ, БУДЕТ ИСПРАВЛЕНО ПРИ СОЗДАНИИ ПАНЕЛИ АДМИНИСТРАТОРА
    В ЗАПРОСЕ ПРИХОДЯТ ВСЕ ПОЛЬЗОВАТЕЛИ, ДОБАВИТЬ DTO!!!!
  */

  useEffect(() => {
    fetch("http://localhost:5289/api/Users") 
      .then(res => res.json())
      .then(data => setUsers(data))
      .catch(err => console.error("Ошибка загрузки пользователей:", err));
  }, []);

  function handleLogin(e) {
    e.preventDefault();

    const user = users.find(
      u => u.login === login && u.passwordHash === password
    );

    if (user) {
      // Можно сохранить в localStorage или context
      console.log("Успешный вход:", user);
      router.push("/dashboard");
    } else {
      setError("Неверный логин или пароль");
    }
  }

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-50 text-gray-900">
      <div className="w-full max-w-sm p-8 bg-white rounded-xl shadow-xl border border-gray-200">
        <h1 className="text-3xl font-bold mb-6 text-center text-gray-800 tracking-tight">
          Вход в систему
        </h1>
        <form onSubmit={handleLogin} className="space-y-5">
          <div>
            <label htmlFor="login" className="block text-sm font-medium text-gray-700 mb-1">
              Логин
            </label>
            <input
              id="login"
              type="text"
              placeholder="examplelogin"
              value={login}
              onChange={e => setLogin(e.target.value)}
              required
              className="w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-black transition"
            />
          </div>

          <div>
            <label htmlFor="password" className="block text-sm font-medium text-gray-700 mb-1">
              Пароль
            </label>
            <input
              id="password"
              type="password"
              placeholder="••••••••"
              value={password}
              onChange={e => setPassword(e.target.value)}
              required
              className="w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-black transition"
            />
          </div>

          {error && (
            <p className="text-red-500 text-sm font-medium text-center">{error}</p>
          )}

          <button
            type="submit"
            className="w-full py-2 bg-black text-white font-semibold rounded-lg hover:bg-gray-800 transition"
          >
            Войти
          </button>
        </form>

        <p className="mt-6 text-xs text-center text-gray-400">
          © {new Date().getFullYear()} DocumentFlow. Все права защищены.
        </p>
      </div>
    </div>
  );
}