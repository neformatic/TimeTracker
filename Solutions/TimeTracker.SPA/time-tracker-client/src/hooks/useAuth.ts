// src/hooks/useAuth.ts
import { useContext } from 'react';
import { IAuthContextType } from '../auth/types'; // Импортируем интерфейс контекста
import { AuthContext } from '../auth/AuthContext'; // Импортируем сам контекст

/**
 * @function useAuth
 * @description Пользовательский хук для удобного доступа к контексту аутентификации.
 * @returns IAuthContextType - Возвращает объект, предоставляемый AuthContext.
 * @throws Error - Выбрасывает ошибку, если хук используется вне AuthProvider.
 */
export const useAuth = (): IAuthContextType => {
  const context = useContext(AuthContext);
  // Проверяем, что хук используется внутри AuthProvider, чтобы избежать ошибок.
  if (context === null) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};