// src/auth/AuthContext.tsx
import React, { createContext, useState, useEffect, ReactNode } from 'react';
import apiClient from '../api/apiClient';
import { IAuthContextType } from './types'; // Импортируем интерфейс контекста
import { ILoginRequest, IRegisterRequest } from '../types/api'; // Импортируем IRegisterRequest

// Создаем контекст аутентификации. Инициализируем его как null,
// так как его реальное значение будет предоставлено AuthProvider.
export const AuthContext = createContext<IAuthContextType | null>(null);

/**
 * @interface AuthProviderProps
 * @description Интерфейс для пропсов компонента AuthProvider.
 * @property children - React-элементы, которые будут обёрнуты провайдером.
 */
interface AuthProviderProps {
  children: ReactNode;
}

/**
 * @component AuthProvider
 * @description Провайдер контекста аутентификации. Предоставляет состояние isAuth
 * и функции login/logout всем дочерним компонентам.
 */
export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
  // isAuth: булево значение, указывающее, аутентифицирован ли пользователь.
  const [isAuth, setIsAuth] = useState<boolean>(false);
  // loading: флаг для отслеживания состояния загрузки (например, при инициализации контекста
  // или при проверке токенов/сессии).
  const [loading, setLoading] = useState<boolean>(true);

  // useEffect для выполнения действий при монтировании компонента.
  // В реальном приложении здесь могла бы быть логика проверки текущей сессии
  // (например, отправка запроса к API для валидации куки или токена).
  useEffect(() => {
    // Для этого примера, мы просто считаем, что проверка завершена после монтирования.
    // В случае наличия эндпоинта проверки сессии, запрос был бы здесь.
    setLoading(false);
  }, []);

  /**
   * @function login
   * @description Функция для выполнения входа в систему, отправляющая запрос на бэкенд.
   * @param email - Электронная почта пользователя.
   * @param password - Пароль пользователя.
   * @returns Promise<{ success: boolean; error?: string }> - Результат операции.
   */
  const login = async (email: string, password: string): Promise<{ success: boolean; error?: string }> => {
    try {
      const loginData: ILoginRequest = { email, password };
      // Отправляем POST-запрос на эндпоинт Login вашего бэкенда.
      // `apiClient` настроен с `withCredentials: true`, поэтому браузер автоматически
      // отправит куки сессии и получит новые.
      await apiClient.post('/Authentication/Login', loginData);
      setIsAuth(true); // Устанавливаем статус аутентификации в true
      console.log('Login successful');
      return { success: true }; // Возвращаем успешный результат
    } catch (error: any) { // Используем `any` для ошибок Axios, чтобы получить доступ к `response`
      console.error('Login failed:', error.response?.data || error.message);
      setIsAuth(false); // Сбрасываем статус аутентификации в случае ошибки
      // Возвращаем информацию об ошибке для отображения пользователю
      return { success: false, error: error.response?.data?.message || 'An unknown error occurred' };
    }
  };

  /**
   * @function logout
   * @description Функция для выполнения выхода из системы, отправляющая запрос на бэкенд.
   * @returns Promise<void>
   */
  const logout = async (): Promise<void> => {
    try {
      // Отправляем POST-запрос на эндпоинт LogoutAsync вашего бэкенда.
      // Бэкенд очистит куки, и браузер перестанет их отправлять.
      await apiClient.post('/Authentication/Logout');
      setIsAuth(false); // Сбрасываем статус аутентификации
      console.log('Logout successful');
    } catch (error: any) {
      console.error('Logout failed:', error.response?.data || error.message);
      // В случае ошибки при логауте, можно также сбросить статус,
      // так как цель - выйти, даже если бэкенд вернул ошибку
      setIsAuth(false);
    }
  };

  // --- НОВАЯ ФУНКЦИЯ РЕГИСТРАЦИИ ---
  /**
   * @function register
   * @description Функция для выполнения регистрации нового пользователя.
   * @param data - Данные для регистрации (email, password, name).
   * @returns Promise<{ success: boolean; error?: string }> - Результат операции.
   */
  const register = async (data: IRegisterRequest): Promise<{ success: boolean; error?: string }> => {
    try {
      // Изменяем эндпоинт на '/User/CreateUser'
      await apiClient.post('/User/CreateUser', data); // Эндпоинт для регистрации
      console.log('Registration successful');
      // В зависимости от вашей логики, возможно, сразу после регистрации нужно выполнить логин
      // или перенаправить пользователя на страницу логина с сообщением.
      // Для простоты, мы не будем автоматически логинить после регистрации,
      // пользователь должен будет залогиниться сам.
      return { success: true };
    } catch (error: any) {
      console.error('Registration failed:', error.response?.data || error.message);
      return { success: false, error: error.response?.data?.message || 'An unknown error occurred' };
    }
  };
  // --- КОНЕЦ НОВОЙ ФУНКЦИИ ---

  // Объект, содержащий состояние и функции, которые будут доступны через контекст
  const value: IAuthContextType = {
    isAuth,
    loading,
    login,
    logout,
    register
  };

  return (
    <AuthContext.Provider value={value}>
      {/* Рендерим дочерние элементы только после завершения инициализации (когда loading == false) */}
      {!loading && children}
    </AuthContext.Provider>
  );
};