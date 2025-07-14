// src/pages/DashboardPage.tsx
import React from 'react';
import { Typography, Card, Space } from 'antd';

const { Title, Text } = Typography;

/**
 * @component DashboardPage
 * @description Компонент защищенной страницы дашборда.
 */
const DashboardPage: React.FC = () => {
  // Функции logout и navigate теперь обрабатываются в Layout или других компонентах.
  // Здесь мы просто отображаем содержимое страницы.
  return (
    <Card
      style={{
        width: '100%', // Карточка будет занимать всю доступную ширину в AppLayout
        textAlign: 'center',
        boxShadow: 'none', // Убираем тень, она может быть на родительском контейнере
        borderRadius: '8px',
      }}
    >
      <Title level={2}>Welcome to Your Dashboard!</Title>
      <Text type="secondary" style={{ fontSize: '1.2em' }}>
        You are successfully logged in. This is a protected page where you can manage your time entries.
      </Text>
      <Space style={{ marginTop: '24px' }}>
        {/* Кнопка Logout теперь в Header, но можно оставить здесь для быстрого доступа */}
        {/* <Button
            type="primary"
            danger
            onClick={handleLogout}
            icon={<LogoutOutlined />}
            size="large"
          >
            Logout
          </Button> */}
      </Space>
    </Card>
  );
};

export default DashboardPage;