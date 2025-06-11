'use client';

import { useRouter } from 'next/navigation';
import { FaUserCircle } from 'react-icons/fa';
import Link from 'next/link';

export default function Header() {
  const router = useRouter();

  const handleLogout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('userData');
    router.push('/login');
  };

  const handleProfile = () => {
    // Позже сделать отслеживание по ролям
   router.push('/user/profile');
  }

  return (
    <header className="bg-white text-black shadow-md py-3 px-4 md:px-8 flex justify-between items-center rounded-b-md border-b-2 border-black transition-all duration-300">
      <Link href="/catalog" className="flex items-center">
        <h1 className="text-xl sm:text-2xl md:text-3xl font-bold tracking-wide uppercase text-black select-none">
          LibraryLife
        </h1>
      </Link>

      <div className="flex items-center space-x-4">
        <Link href="/catalog" className="font-semibold hover:text-gray-700 transition-colors duration-200">
          Каталог книг
        </Link>
        <button
          onClick={handleLogout}
          className="relative font-semibold text-lg px-4 py-1.5 rounded-full group overflow-hidden border border-black transition-all duration-300
          bg-gray-100 text-black shadow-sm
          hover:bg-gray-300 hover:text-black hover:border-gray-400
          focus:outline-none focus:ring-2 focus:ring-gray-400 focus:ring-opacity-40
        "
        >
          <span className="relative z-10 transition-all duration-300 group-hover:tracking-wider">Выйти</span>
        </button>
        <FaUserCircle onClick={handleProfile} size={32} className="text-gray-800 hover:text-gray-600 cursor-pointer transition-colors duration-200"/>
      </div>
    </header>
  );
}
