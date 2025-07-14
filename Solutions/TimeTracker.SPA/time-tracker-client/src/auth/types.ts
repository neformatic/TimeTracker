// src/auth/types.ts

import { IRegisterRequest } from "../types/api";

/**
 * @interface IAuthContextType
 * @description Интерфейс, описывающий структуру объекта, предоставляемого AuthContext.
 */
export interface IAuthContextType {
  isAuth: boolean; // Флаг, указывающий, аутентифицирован ли пользователь
  loading: boolean; // Флаг для отслеживания состояния загрузки (например, при инициализации контекста)
  /**
   * @function login
   * @description Функция для выполнения входа в систему.
   * @param email - Электронная почта пользователя.
   * @param password - Пароль пользователя.
   * @returns Promise<{ success: boolean; error?: string }> - Возвращает объект с результатом операции.
   */
  login: (email: string, password: string) => Promise<{ success: boolean; error?: string }>;
  /**
   * @function logout
   * @description Функция для выполнения выхода из системы.
   * @returns Promise<void>
   */
  logout: () => Promise<void>;
  // --- НОВОЕ СВОЙСТВО ДЛЯ РЕГИСТРАЦИИ ---
  register: (data: IRegisterRequest) => Promise<{ success: boolean; error?: string }>;
  // --- КОНЕЦ НОВОГО СВОЙСТВА ---
}