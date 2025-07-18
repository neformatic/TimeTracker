// src/pages/LoginPage.tsx
import React, { useState, useEffect } from 'react';
import { Flex, Card, Typography } from 'antd';
import LoginForm from '../components/LoginForm';
import RegisterForm from '../components/RegisterForm';
import { useNavigate, useLocation } from 'react-router-dom';
import '../styles/LoginPage.css'; // Подключаем CSS

const { Title } = Typography;

const LoginPage: React.FC = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const [isRegisterMode, setIsRegisterMode] = useState(false);

  useEffect(() => {
    const params = new URLSearchParams(location.search);
    if (params.get('mode') === 'register') {
      setIsRegisterMode(true);
    } else {
      setIsRegisterMode(false);
    }
  }, [location.search]);

  const handleAuthSuccess = () => {
    navigate('/dashboard', { replace: true });
  };

  return (
    <Flex justify="center" align="flex-start" className="login-page">
      <Card className="login-card">
        <Title level={2} className="login-title">
          {isRegisterMode ? 'Create Account' : 'Welcome Back!'}
        </Title>
        {isRegisterMode ? (
          <RegisterForm
            onSuccess={handleAuthSuccess}
            onSwitchToLogin={() => {
              setIsRegisterMode(false);
              navigate('/login', { replace: true });
            }}
          />
        ) : (
          <LoginForm
            onSuccess={handleAuthSuccess}
            onSwitchToRegister={() => {
              setIsRegisterMode(true);
              navigate('/login?mode=register', { replace: true });
            }}
          />
        )}
      </Card>
    </Flex>
  );
};

export default LoginPage;