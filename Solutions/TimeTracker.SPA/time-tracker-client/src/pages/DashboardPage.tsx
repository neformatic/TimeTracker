// src/pages/DashboardPage.tsx
import React from 'react';
import { Typography, Card, Flex, Space, Button } from 'antd';
import '../styles/DashboardPage.css'; // Подключаем CSS

const { Title, Text } = Typography;

const DashboardPage: React.FC = () => {
  return (
    <Flex
      justify="center"
      align="middle"
      className="dashboard-container"
    >
      <Card className="dashboard-card">
        <Title level={2} className="dashboard-title">
          Welcome to Your TimeTracker Dashboard!
        </Title>
        <Text className="dashboard-description">
          You are successfully logged in. This is your personal space where you can effectively manage your tasks,
          track your productivity, and find moments of calm in your work day.
        </Text>
        { 
        <Space style={{ marginTop: '30px' }} direction="vertical" size="large">
          <Button type="primary" size="large" style={{ borderRadius: 8 }}>
            View Your Tasks
          </Button>
          <Button type="default" size="large" style={{ borderRadius: 8 }}>
            Start New Entry
          </Button>
        </Space>
        }
      </Card>
    </Flex>
  );
};

export default DashboardPage;