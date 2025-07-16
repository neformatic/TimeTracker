import React, { createContext, useState, useEffect, ReactNode, useContext } from 'react';
import apiClient from '../api/apiClient';
import { IAuthContextType } from './types';
import { ILoginRequest, IRegisterRequest } from '../types/api';

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
      await apiClient.post('/Authentication/Login', loginData);
      setIsAuth(true); // Устанавливаем статус аутентификации в true
      console.log('Login successful'); // Лог для отслеживания успеха
      return { success: true }; // Возвращаем успешный результат
    } catch (error: any) {
      console.error('Login failed:', error.response?.data || error.message); // Лог для отслеживания ошибки
      setIsAuth(false); // Сбрасываем статус аутентификации в случае ошибки
      
      let errorMessage = 'An unknown error occurred. Please try again.';

      // Пытаемся извлечь сообщение об ошибке из ответа сервера
      if (error.response?.data) {
        const responseData = error.response.data;
        if (typeof responseData === 'object' && responseData !== null && 'errorMessage' in responseData) {
          errorMessage = (responseData as { errorMessage: string }).errorMessage;
        } else if (typeof responseData === 'string' && responseData.length > 0) {
          errorMessage = responseData;
        }
      } else if (error.message) {
        errorMessage = error.message; // Если нет данных ответа, используем сообщение из объекта ошибки
      }
      
      return { success: false, error: errorMessage };
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
      console.log('Logout successful'); // Лог для отслеживания успеха
    } catch (error: any) {
      console.error('Logout failed:', error.response?.data || error.message); // Лог для отслеживания ошибки
      // В случае ошибки при логауте, можно также сбросить статус,
      // так как цель - выйти, даже если бэкенд вернул ошибку
      setIsAuth(false);
    }
  };

  /**
   * @function register
   * @description Функция для выполнения регистрации нового пользователя.
   * @param data - Данные для регистрации (email, password, name).
   * @returns Promise<{ success: boolean; error?: string }> - Результат операции.
   */
  const register = async (data: IRegisterRequest): Promise<{ success: boolean; error?: string }> => {
    try {
      // Изменяем эндпоинт на '/User/Create' для регистрации
      await apiClient.post('/User/Create', data);
      console.log('Registration successful'); // Лог для отслеживания успеха
      return { success: true };
    } catch (error: any) {
      console.error('Registration failed:', error.response?.data || error.message); // Лог для отслеживания ошибки
      
      let errorMessage = 'An unknown error occurred. Please try again.';
      
      // Пытаемся извлечь сообщение об ошибке из ответа сервера
      if (error.response?.data) {
        const responseData = error.response.data;
        if (typeof responseData === 'object' && responseData !== null && 'errorMessage' in responseData) {
          errorMessage = (responseData as { errorMessage: string }).errorMessage;
        } else if (typeof responseData === 'string' && responseData.length > 0) {
          errorMessage = responseData;
        }
      } else if (error.message) {
        errorMessage = error.message; // Если нет данных ответа, используем сообщение из объекта ошибки
      }
      
      return { success: false, error: errorMessage };
    }
  };

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

/**
 * @function useAuth
 * @description Кастомный хук для удобного доступа к контексту аутентификации.
 * Позволяет компонентам получать доступ к isAuth, loading, login, logout и register
 * без прямого использования useContext(AuthContext).
 * Выбрасывает ошибку, если используется вне AuthProvider.
 */
export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};