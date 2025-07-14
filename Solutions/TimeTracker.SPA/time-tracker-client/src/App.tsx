// src/App.tsx
import React from 'react';
import { BrowserRouter as Router, Routes, Route, useNavigate } from 'react-router-dom';
import { AuthProvider } from './auth/AuthContext';
import DashboardPage from './pages/DashboardPage'; // DashboardPage остаётся
import PrivateRoute from './components/PrivateRoute';
import AppLayout from './components/Layout'; // Импортируем новый Layout компонент
import { Flex, Spin } from 'antd'; // Импорт для спиннера
import { useAuth } from './hooks/useAuth';

/**
 * @component App
 * @description Главный компонент приложения, который настраивает маршрутизацию
 * и предоставляет контекст аутентификации всему приложению.
 */
const App: React.FC = () => {
  return (
    <Router>
      <AuthProvider>
        {/* Оборачиваем все маршруты в AppLayout */}
        <AppLayout>
          <Routes>
            {/* Главная страница, когда пользователь не залогинен, будет пустой или с редиректом.
                Пользователь будет использовать модальные окна для логина/регистрации.
                Мы можем просто перенаправлять на дашборд, если пользователь залогинен.
            */}
            <Route
                path="/"
                element={
                    <AuthRedirector />
                }
            />

            {/* Защищенный маршрут для страницы дашборда */}
            <Route
              path="/dashboard"
              element={
                <PrivateRoute>
                  <DashboardPage />
                </PrivateRoute>
              }
            />
          </Routes>
        </AppLayout>
      </AuthProvider>
    </Router>
  );
};

// Вспомогательный компонент для перенаправления с главной страницы
const AuthRedirector: React.FC = () => {
    const { isAuth, loading } = useAuth();
    const navigate = useNavigate();

    React.useEffect(() => {
        if (!loading) {
            if (isAuth) {
                navigate('/dashboard', { replace: true });
            }
        }
    }, [isAuth, loading, navigate]);

    if (loading) {
        return (
            <Flex justify="center" align="center" style={{ minHeight: 'inherit' }}>
                <Spin size="large" />
            </Flex>
        );
    }

    // Если не залогинен, просто ничего не показываем на главной странице,
    // пользователь будет использовать кнопки в меню.
    return null;
};


export default App;