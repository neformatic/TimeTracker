// src/pages/DashboardPage.tsx
import React from 'react';
import { Typography, Card, Space, Flex } from 'antd'; // Добавляем Flex для центрирования

const { Title, Text } = Typography;

/**
 * @component DashboardPage
 * @description Компонент защищенной страницы дашборда, адаптированный под новый дизайн.
 */
const DashboardPage: React.FC = () => {
  return (
    // Используем Flex для центрирования карточки на странице
    <Flex
      justify="center"
      align="middle"
      style={{
        minHeight: 'calc(100vh - 64px)', // Вычитаем высоту хедера, если он есть
        padding: 24, // Отступы вокруг карточки
        width: '100%',
        // Фон для самой DashboardPage не задаём, предполагая, что его задаст AppLayout.
        // Если DashboardPage рендерится без общего лейаута,
        // то сюда можно добавить background: 'linear-gradient(to bottom, #e0f7fa 0%, #a7d9f7 100%)',
      }}
    >
      <Card
        style={{
          width: '80%', // Пусть карточка занимает 80% ширины от контейнера Flex
          maxWidth: 900, // Максимальная ширина для больших экранов
          textAlign: 'center',
          borderRadius: 12, // Скругленные углы
          boxShadow: '0 10px 30px rgba(0,0,0,0.15)', // Красивая тень
          border: 'none', // Убираем стандартную границу
          backgroundColor: 'rgba(255, 255, 255, 0.8)', // Полупрозрачный белый фон
          backdropFilter: 'blur(15px)', // Эффект размытия
          padding: '40px 30px', // Увеличиваем внутренние отступы для более просторного вида
        }}
        bodyStyle={{ padding: 0 }} // Убираем внутренний padding body Ant Design Card
      >
        <Title level={2} style={{ color: '#263238', marginBottom: 20, fontWeight: 600 }}>
          Welcome to Your TimeTracker Dashboard!
        </Title>
        <Text style={{ fontSize: '1.2em', color: '#455A64', lineHeight: 1.6 }}>
          You are successfully logged in. This is your personal space where you can effectively manage your tasks,
          track your productivity, and find moments of calm in your work day.
        </Text>
        <Space style={{ marginTop: '30px' }} direction="vertical" size="large">
          {/* Здесь могут быть кнопки или ссылки на другие разделы дашборда */}
          {/* Например: */}
          {/* <Button type="primary" size="large" style={{ borderRadius: 8 }}>
            View Your Tasks
          </Button>
          <Button type="default" size="large" style={{ borderRadius: 8 }}>
            Start New Entry
          </Button> */}
        </Space>
      </Card>
    </Flex>
  );
};

export default DashboardPage;