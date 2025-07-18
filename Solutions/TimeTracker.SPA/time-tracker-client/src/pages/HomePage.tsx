// src/pages/HomePage.tsx
import React from 'react';
import { Button, Flex, Typography } from 'antd';
import { useNavigate } from 'react-router-dom';
import '../styles/HomePage.css'; // Подключаем CSS

const { Title, Text } = Typography;

const HomePage: React.FC = () => {
  const navigate = useNavigate();

  const handleLoginClick = () => {
    navigate('/login');
  };

  const handleRegisterClick = () => {
    navigate('/login?mode=register');
  };

  return (
    <Flex vertical justify="flex-start" align="center" className="home-page">
      {/* Кнопки авторизации */}
      <Flex gap="middle" className="auth-buttons">
        <Button type="primary" size="large" onClick={handleLoginClick}>
          Log In
        </Button>
        <Button type="default" size="large" onClick={handleRegisterClick}>
          Register
        </Button>
      </Flex>

      {/* Основной контент */}
      <Flex vertical align="center" justify="center" className="content-wrapper">
        <Title level={1} className="title">
          TimeTracker: Productivity Meets Calm
        </Title>
        <Text className="description">
          Streamline your work, track your progress, and find your focus. TimeTracker helps you manage tasks efficiently,
          so you can achieve more without the stress, leaving you time to relax.
        </Text>
      </Flex>
    </Flex>
  );
};

export default HomePage;