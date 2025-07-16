// src/App.tsx
import React from 'react';
import { BrowserRouter as Router, Routes, Route, useNavigate, Navigate } from 'react-router-dom';
import { AuthProvider, useAuth } from './auth/AuthContext';
import DashboardPage from './pages/DashboardPage';
import ResetPasswordPage from './pages/ResetPasswordPage';
import LoginPage from './pages/LoginPage';
import HomePage from './pages/HomePage';
import PrivateRoute from './components/PrivateRoute';
import AppLayout from './components/Layout';
import { Flex, Spin, ConfigProvider, App as AntdApp } from 'antd';

// Главный компонент приложения, который настраивает маршрутизацию
// и предоставляет контекст аутентификации всему приложению.
const App: React.FC = () => {
  return (
    // ConfigProvider для глобальной настройки темы Ant Design
    <ConfigProvider
      theme={{
        token: {
          colorPrimary: '#4A90E2', // Основной цвет для кнопок, ссылок и т.д.
          borderRadius: 8, // Глобальное скругление углов для большинства компонентов
          fontFamily: 'Arial, sans-serif', // Пример другого шрифта (убедитесь, что он доступен)
        },
      }}
    >
      {/* Обертка для использования контекстных функций Ant Design (message, notification, modal) */}
      <AntdApp>
        {/* Обертка для всех маршрутов приложения */}
        <Router>
          {/* Предоставляет контекст аутентификации всем дочерним компонентам */}
          <AuthProvider>
            {/* Контейнер для всех маршрутов */}
            <Routes>
              {/* Маршрут для страницы сброса пароля.
                  Он находится на верхнем уровне, так как не требует аутентификации
                  и не использует стандартный AppLayout. */}
              <Route path="/reset-password/:token" element={<ResetPasswordPage />} />

              {/* Маршрут для страницы логина/регистрации.
                  Также находится на верхнем уровне, так как она самодостаточна
                  и не использует AppLayout. */}
              <Route path="/login" element={<LoginPage />} />

              {/* Маршрут для главной страницы ('/').
                  Он использует AuthRedirector, который решит,
                  показать HomePage (для неавторизованных) или Dashboard (для авторизованных). */}
              <Route
                path="/"
                element={<AuthRedirector />}
              />

              {/* Защищенные маршруты. Они используют PrivateRoute для проверки аутентификации
                  и AppLayout для отображения общего макета приложения (хедер, сайдбар и т.д.). */}
              <Route
                path="/dashboard"
                element={
                  <PrivateRoute> {/* Защищает маршрут, перенаправляя неавторизованных */}
                    <AppLayout> {/* Предоставляет общий макет */}
                      <DashboardPage /> {/* Компонент самой страницы */}
                    </AppLayout>
                  </PrivateRoute>
                }
              />
            </Routes>
          </AuthProvider>
        </Router>
      </AntdApp>
    </ConfigProvider>
  );
};

// Вспомогательный компонент, который управляет перенаправлением
// или отображением контента в зависимости от статуса аутентификации.
// Показывает спиннер во время загрузки.
const AuthRedirector: React.FC = () => {
    const { isAuth, loading } = useAuth(); // Получаем статус аутентификации и загрузки из контекста

    if (loading) {
        // Если данные аутентификации загружаются, показываем большой спиннер по центру экрана.
        return (
            <Flex justify="center" align="center" style={{ minHeight: '100vh' }}>
                <Spin size="large" />
            </Flex>
        );
    }

    if (isAuth) {
        // Если пользователь аутентифицирован, используем <Navigate /> для изменения URL
        // и перенаправления на дашборд.
        return <Navigate to="/dashboard" replace />;
    } else {
        // Если пользователь НЕ аутентифицирован, отображаем HomePage.
        // HomePage теперь является "лендинговой" страницей.
        return <HomePage />;
    }
};

export default App;