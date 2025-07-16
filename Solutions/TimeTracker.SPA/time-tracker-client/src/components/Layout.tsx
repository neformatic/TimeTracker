// src/components/Layout.tsx
import React, { useState } from 'react';
import { Layout as AntLayout, Menu, Button, Modal, Typography, Dropdown, Space } from 'antd';
import { UserOutlined, LogoutOutlined, DownOutlined, LoginOutlined, UserAddOutlined } from '@ant-design/icons';
import { useAuth } from '../auth/AuthContext';
import LoginForm from '../components/LoginForm';
import RegisterForm from '../components/RegisterForm';
import { useNavigate, Link } from 'react-router-dom';

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
        style={{
          borderRadius: 8,
          borderColor: '#4A90E2',
          background: '#4A90E2',
          color: '#fff',
          fontWeight: 600,
          boxShadow: '0 4px 12px rgba(0,0,0,0.15)'
        }}
      >
        <Space>
          User
          <DownOutlined />
        </Space>
      </Button>
    </Dropdown>
  ) : (
    <Space>
      <Button
        type="primary"
        icon={<LoginOutlined />}
        onClick={showLoginModal}
        size="large"
        style={{
          borderRadius: 8,
          borderColor: '#4A90E2',
          background: '#4A90E2',
          color: '#fff',
          fontWeight: 600,
          boxShadow: '0 4px 12px rgba(0,0,0,0.15)'
        }}
      >
        Log In
      </Button>
      <Button
        icon={<UserAddOutlined />}
        onClick={showRegisterModal}
        size="large"
        style={{
          borderRadius: 8,
          borderColor: '#4A90E2',
          background: 'transparent',
          color: '#4A90E2',
          fontWeight: 600,
          boxShadow: '0 4px 12px rgba(0,0,0,0.08)'
        }}
      >
        Register
      </Button>
    </Space>
  );

  return (
    <AntLayout style={{ minHeight: '100vh' }}>
      <Header
        style={{
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'space-between',
          padding: '0 48px',
          background: 'rgba(255, 255, 255, 0.7)',
          backdropFilter: 'blur(10px)',
          boxShadow: '0 2px 8px rgba(0,0,0,0.1)',
          height: 80,
        }}
      >
        <Title level={3} style={{ color: '#263238', margin: 0, fontWeight: 700 }}>
          <Link to="/" style={{ color: '#263238', textDecoration: 'none' }}>TimeTracker</Link>
        </Title>
        <Menu
          theme="light"
          mode="horizontal"
          defaultSelectedKeys={['1']}
          style={{
            flex: 1,
            minWidth: 0,
            justifyContent: 'flex-end',
            background: 'transparent',
            borderBottom: 'none',
          }}
        >
        </Menu>
        {authButtons}
      </Header>

      <Content
        style={{
          padding: '0',
          flex: 1,
          background: 'linear-gradient(to bottom, #e0f7fa 0%, #a7d9f7 100%)',
          display: 'flex',
          justifyContent: 'center',
          // align-items: 'flex-start' уже установлен, это то, что нужно для избежания растяжения по высоте
          alignItems: 'flex-start',
          paddingTop: 24,
          paddingBottom: 24,
        }}
      >
        {children}
      </Content>

      <Footer
        style={{
          textAlign: 'center',
          background: 'rgba(255, 255, 255, 0.7)',
          backdropFilter: 'blur(10px)',
          color: '#455A64',
          padding: '20px 0',
        }}
      >
        TimeTracker ©2024 Created by You
      </Footer>

      {/* Модальное окно для логина */}
      <Modal
        title={<Title level={4} style={{ margin: 0, color: '#263238' }}>Log In</Title>}
        open={isLoginModalVisible}
        onCancel={() => setIsLoginModalVisible(false)}
        footer={null}
        width={500} // Уменьшено с 750px до 500px
        bodyStyle={{
          padding: '24px',
          borderRadius: 12,
          backgroundColor: 'rgba(255, 255, 255, 0.8)',
          backdropFilter: 'blur(15px)',
        }}
        maskStyle={{ backgroundColor: 'rgba(0, 0, 0, 0.2)' }}
        // top задаёт позицию модалки сверху. Это помогает ей не растягиваться по высоте
        style={{ top: 100 }}
      >
        <LoginForm
          onSuccess={handleLoginSuccess}
          onSwitchToRegister={showRegisterModal}
        />
      </Modal>

      {/* Модальное окно для регистрации */}
      <Modal
        title={<Title level={4} style={{ margin: 0, color: '#263238' }}>Register</Title>}
        open={isRegisterModalVisible}
        onCancel={() => setIsRegisterModalVisible(false)}
        footer={null}
        width={500} // Уменьшено с 750px до 500px
        bodyStyle={{
          padding: '24px',
          borderRadius: 12,
          backgroundColor: 'rgba(255, 255, 255, 0.8)',
          backdropFilter: 'blur(15px)',
        }}
        maskStyle={{ backgroundColor: 'rgba(0, 0, 0, 0.2)' }}
        // top задаёт позицию модалки сверху. Это помогает ей не растягиваться по высоте
        style={{ top: 100 }}
      >
        <RegisterForm
          onSuccess={handleRegisterSuccess}
          onSwitchToLogin={showLoginModal}
        />
      </Modal>
    </AntLayout>
  );
};

export default AppLayout;