// src/types/api.ts

/**
 * @interface ILoginRequest
 * @description Интерфейс для данных запроса логина, отправляемых на бэкенд.
 */
export interface ILoginRequest {
  email: string;
  password: string;
}
// --- НОВЫЙ ИНТЕРФЕЙС ДЛЯ РЕГИСТРАЦИИ ---
/**
 * @interface IRegisterRequest
 * @description Интерфейс для данных запроса регистрации, отправляемых на бэкенд.
 */
export interface IRegisterRequest {
  email: string;
  password: string;
  name: string; // Добавляем поле для имени пользователя
}
// --- КОНЕЦ НОВОГО ИНТЕРФЕЙСА ---