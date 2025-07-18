// src/components/Layout.tsx
import React, { useState } from 'react';
import { Layout as AntLayout, Menu, Button, Modal, Typography, Dropdown, Space } from 'antd';
import { UserOutlined, LogoutOutlined, DownOutlined, LoginOutlined, UserAddOutlined } from '@ant-design/icons';
import { useAuth } from '../auth/AuthContext';
import LoginForm from '../components/LoginForm';
import RegisterForm from '../components/RegisterForm';
import { useNavigate, Link } from 'react-router-dom';
import '../styles/Layout.css'; // Подключаем CSS

const { Header, Content, Footer } = AntLayout;
const { Title } = Typography;

interface AppLayoutProps {
  children: React.ReactNode;
}

const AppLayout: React.FC<AppLayoutProps> = ({ children }) => {
  const { isAuth, logout } = useAuth();
  const navigate = useNavigate();
  const [isLoginModalVisible, setIsLoginModalVisible] = useState(false);
  const [isRegisterModalVisible, setIsRegisterModalVisible] = useState(false);

  const showLoginModal = () => {
    setIsRegisterModalVisible(false);
    setIsLoginModalVisible(true);
  };

  const handleLoginSuccess = () => {
    setIsLoginModalVisible(false);
    navigate('/dashboard');
  };

  const showRegisterModal = () => {
    setIsLoginModalVisible(false);
    setIsRegisterModalVisible(true);
  };

  const handleRegisterSuccess = () => {
    setIsRegisterModalVisible(false);
    setIsLoginModalVisible(true);
  };

  const handleLogout = async () => {
    await logout();
    navigate('/');
  };

  const userMenuItems = [
    {
      key: '1',
      label: 'Profile',
      icon: <UserOutlined />,
      onClick: () => {
        console.log('Go to profile');
      },
    },
    {
      key: '2',
      label: 'Logout',
      icon: <LogoutOutlined />,
      danger: true,
      onClick: handleLogout,
    },
  ];

  const authButtons = isAuth ? (
    <Dropdown menu={{ items: userMenuItems }}>
      <Button
        type="primary"
        icon={<UserOutlined />}
        size="large"
        className="auth-button primary"
      >
        <Space>
          User
          <DownOutlined />
        </Space>
      </Button>
    </Dropdown>
  ) : (
    <Space className="auth-buttons">
      <Button
        type="primary"
        icon={<LoginOutlined />}
        onClick={showLoginModal}
        size="large"
      >
        Log In
      </Button>
      <Button
        icon={<UserAddOutlined />}
        onClick={showRegisterModal}
        size="large"
      >
        Register
      </Button>
    </Space>
  );

  return (
    <AntLayout className="layout">
      <Header className="layout-header">
        <Title level={3} className="layout-title">
          <Link to="/">TimeTracker</Link>
        </Title>
        <Menu
          theme="light"
          mode="horizontal"
          defaultSelectedKeys={['1']}
          className="layout-menu"
        >
          {/* Меню можно расширить позже */}
        </Menu>
        {authButtons}
      </Header>
      <Content className="layout-content">{children}</Content>
      <Footer className="layout-footer">TimeTracker ©2025 Created by You</Footer>

      {/* Модальное окно для логина */}
      <Modal
        title={<Title level={4} style={{ margin: 0, color: '#263238' }}>Log In</Title>}
        open={isLoginModalVisible}
        onCancel={() => setIsLoginModalVisible(false)}
        footer={null}
        width={500}
        bodyStyle={{ padding: '24px', borderRadius: 12, backgroundColor: 'rgba(255, 255, 255, 0.8)', backdropFilter: 'blur(15px)' }}
        maskStyle={{ backgroundColor: 'rgba(0, 0, 0, 0.2)' }}
        style={{ top: 100 }}
      >
        <LoginForm onSuccess={handleLoginSuccess} onSwitchToRegister={showRegisterModal} />
      </Modal>

      {/* Модальное окно для регистрации */}
      <Modal
        title={<Title level={4} style={{ margin: 0, color: '#263238' }}>Register</Title>}
        open={isRegisterModalVisible}
        onCancel={() => setIsRegisterModalVisible(false)}
        footer={null}
        width={500}
        bodyStyle={{ padding: '24px', borderRadius: 12, backgroundColor: 'rgba(255, 255, 255, 0.8)', backdropFilter: 'blur(15px)' }}
        maskStyle={{ backgroundColor: 'rgba(0, 0, 0, 0.2)' }}
        style={{ top: 100 }}
      >
        <RegisterForm onSuccess={handleRegisterSuccess} onSwitchToLogin={showLoginModal} />
      </Modal>
    </AntLayout>
  );
};

export default AppLayout;