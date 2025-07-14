// src/components/Layout.tsx
import React, { useState } from 'react';
import { Layout as AntLayout, Menu, Button, Modal, Space, Flex } from 'antd';
import { LogoutOutlined, LoginOutlined, UserAddOutlined } from '@ant-design/icons';
import { useAuth } from '../hooks/useAuth';
import { useNavigate } from 'react-router-dom';
import LoginForm from './LoginForm'; // Импортируем новую форму логина
import RegisterForm from './RegisterForm'; // Импортируем новую форму регистрации

const { Header, Content, Footer } = AntLayout;

interface AppLayoutProps {
  children: React.ReactNode;
}

/**
 * @component AppLayout
 * @description Компонент макета приложения с верхним меню, футером и модальными окнами для аутентификации.
 */
const AppLayout: React.FC<AppLayoutProps> = ({ children }) => {
  const { isAuth, logout } = useAuth();
  const navigate = useNavigate();

  const [isLoginModalVisible, setIsLoginModalVisible] = useState<boolean>(false);
  const [isRegisterModalVisible, setIsRegisterModalVisible] = useState<boolean>(false);

  const handleLogout = async () => {
    await logout();
    navigate('/'); // Перенаправляем на главную страницу после выхода
  };

  const showLoginModal = () => {
    setIsRegisterModalVisible(false); // Закрываем модалку регистрации, если открыта
    setIsLoginModalVisible(true);
  };

  const hideLoginModal = () => {
    setIsLoginModalVisible(false);
  };

  const showRegisterModal = () => {
    setIsLoginModalVisible(false); // Закрываем модалку логина, если открыта
    setIsRegisterModalVisible(true);
  };

  const hideRegisterModal = () => {
    setIsRegisterModalVisible(false);
  };

  const handleLoginSuccess = () => {
    hideLoginModal();
    navigate('/dashboard'); // Перенаправляем на дашборд после успешного логина
  };

  const handleRegisterSuccess = () => {
    hideRegisterModal();
    // После успешной регистрации, можно предложить сразу залогиниться
    showLoginModal();
  };

  return (
    <AntLayout style={{ minHeight: '100vh' }}>
      <Header style={{ background: '#fff', padding: '0 20px', borderBottom: '1px solid #f0f0f0' }}>
        <Flex justify="space-between" align="center" style={{ height: '100%' }}>
          <div className="logo" style={{ color: '#001529', fontSize: '20px', fontWeight: 'bold' }}>
            TimeTracker
          </div>
          <Menu mode="horizontal" defaultSelectedKeys={['1']} style={{ flex: 1, borderBottom: 'none' }}>
            {/* Здесь можно добавить другие пункты меню, если нужно */}
            {isAuth && (
              <Menu.Item key="dashboard" onClick={() => navigate('/dashboard')}>
                Dashboard
              </Menu.Item>
            )}
          </Menu>

          <Space>
            {!isAuth ? (
              <>
                <Button icon={<LoginOutlined />} onClick={showLoginModal}>
                  Login
                </Button>
                <Button icon={<UserAddOutlined />} type="primary" onClick={showRegisterModal}>
                  Register
                </Button>
              </>
            ) : (
              <Button icon={<LogoutOutlined />} onClick={handleLogout}>
                Logout
              </Button>
            )}
          </Space>
        </Flex>
      </Header>

      <Content style={{ padding: '24px 50px', flex: 1 }}>
        <div className="site-layout-content" style={{ background: '#fff', padding: 24, minHeight: 'calc(100vh - 170px)', borderRadius: '8px' }}>
          {children} {/* Здесь будет рендериться содержимое текущей страницы (DashboardPage) */}
        </div>
      </Content>

      <Footer style={{ textAlign: 'center' }}>TimeTracker ©2024 Created by You</Footer>

      {/* Модальное окно для логина */}
      <Modal
        title="Login to Your Account"
        open={isLoginModalVisible}
        onCancel={hideLoginModal}
        footer={null} // Отключаем стандартный футер модального окна
        destroyOnClose // Разрушать компоненты внутри модалки при закрытии, чтобы сбросить форму
      >
        <LoginForm
          onSuccess={handleLoginSuccess}
          onSwitchToRegister={showRegisterModal} // Кнопка переключения
        />
      </Modal>

      {/* Модальное окно для регистрации */}
      <Modal
        title="Create New Account"
        open={isRegisterModalVisible}
        onCancel={hideRegisterModal}
        footer={null}
        destroyOnClose
      >
        <RegisterForm
          onSuccess={handleRegisterSuccess}
          onSwitchToLogin={showLoginModal} // Кнопка переключения
        />
      </Modal>
    </AntLayout>
  );
};

export default AppLayout;