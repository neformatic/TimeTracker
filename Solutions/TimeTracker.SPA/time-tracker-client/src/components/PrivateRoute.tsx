// src/components/PrivateRoute.tsx
import React, { ReactNode } from 'react';
import { Navigate } from 'react-router-dom';
import { useAuth } from '../hooks/useAuth';
import { Spin, Flex } from 'antd';

interface PrivateRouteProps {
  children: ReactNode;
}

const PrivateRoute: React.FC<PrivateRouteProps> = ({ children }) => {
  const { isAuth, loading } = useAuth();

  if (loading) {
    return (
      <Flex justify="center" align="center" style={{ minHeight: '100vh' }}>
        <Spin size="large" />
      </Flex>
    );
  }

  // --- ИСПРАВЛЕНИЕ ЗДЕСЬ ---
  // Если пользователь аутентифицирован, возвращаем дочерние элементы.
  // TypeScript может быть очень строгим с ReactNode, который может быть undefined.
  // Обычно children, переданные в качестве JSX, являются ReactElement.
  // Однако, чтобы успокоить TS, мы явно проверяем isAuth и возвращаем ReactElement.
  if (isAuth) {
    // Убедимся, что children всегда является чем-то, что React может рендерить.
    // В большинстве случаев children, переданные в PrivateRoute, уже будут JSX.
    // Если есть сомнения, можно обернуть: return <>{children}</>;
    // Но для типичных случаев, прямой возврат children должен работать,
    // если он действительно является ReactElement.
    return children as React.ReactElement; // Явно приводим к React.ReactElement
  }

  // Если не аутентифицирован, перенаправляем.
  return <Navigate to="/" replace />;
  // --- КОНЕЦ ИСПРАВЛЕНИЯ ---
};

export default PrivateRoute;