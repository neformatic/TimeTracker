// src/api/apiClient.ts
import axios from 'axios';

// Получаем базовый URL API из переменных окружения.
// 'REACT_APP_' - это префикс, который Create React App требует для переменных окружения.
const API_BASE_URL = process.env.REACT_APP_API_BASE_URL;

// Создаём экземпляр Axios с базовой конфигурацией
const apiClient = axios.create({
  baseURL: API_BASE_URL,
  // `withCredentials: true` - это критически важный параметр!
  // Он указывает Axios'у включать HTTP-only Cookies в междоменные запросы.
  // Это необходимо, так как ваш ASP.NET Core бэкенд использует куки для аутентификации.
  withCredentials: true,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Опционально: Добавляем перехватчик ответов для централизованной обработки ошибок.
// Например, для вывода ошибок в консоль.
apiClient.interceptors.response.use(
  response => response, // Если ответ успешен, просто возвращаем его
  error => {
    // Если произошла ошибка, логируем её
    console.error('API Error:', error.response?.data || error.message);
    // Пробрасываем ошибку дальше, чтобы её могли обработать в компонентах
    return Promise.reject(error);
  }
);

export default apiClient;