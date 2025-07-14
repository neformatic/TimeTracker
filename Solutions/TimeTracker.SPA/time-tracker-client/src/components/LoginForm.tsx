// src/components/LoginForm.tsx
import React, { useState } from 'react';
import { Form, Input, Button, Typography, message } from 'antd';
import { MailOutlined, LockOutlined } from '@ant-design/icons';
import { useAuth } from '../hooks/useAuth';
import { ILoginRequest } from '../types/api';

const { Title } = Typography;

interface LoginFormProps {
  onSuccess: () => void; // Колбэк, вызываемый при успешном логине
  onSwitchToRegister: () => void; // Колбэк для переключения на форму регистрации
}

/**
 * @component LoginForm
 * @description Компонент формы входа в систему.
 */
const LoginForm: React.FC<LoginFormProps> = ({ onSuccess, onSwitchToRegister }) => {
  const [loading, setLoading] = useState<boolean>(false);
  const { login } = useAuth();

  /**
   * @function onFinish
   * @description Обработчик отправки формы логина.
   * @param values - Объект с данными формы (email, password).
   */
  const onFinish = async (values: ILoginRequest) => {
    setLoading(true);
    const result = await login(values.email, values.password);
    setLoading(false);

    if (result.success) {
      message.success('Logged in successfully!');
      onSuccess(); // Вызываем колбэк при успехе (закрыть модалку)
    } else {
      message.error(result.error || 'Login failed. Please check your credentials.');
    }
  };

  return (
    <>
      <Title level={3} style={{ textAlign: 'center', marginBottom: '24px' }}>
        Login
      </Title>
      <Form
        name="login"
        initialValues={{ remember: true }}
        onFinish={onFinish}
        layout="vertical"
      >
        <Form.Item
          name="email"
          rules={[
            { required: true, message: 'Please input your Email!' },
            { type: 'email', message: 'Please enter a valid Email!' }
          ]}
        >
          <Input prefix={<MailOutlined />} placeholder="Email" size="large" />
        </Form.Item>

        <Form.Item
          name="password"
          rules={[{ required: true, message: 'Please input your Password!' }]}
        >
          <Input.Password prefix={<LockOutlined />} placeholder="Password" size="large" />
        </Form.Item>

        <Form.Item>
          <Button type="primary" htmlType="submit" loading={loading} block size="large">
            Login
          </Button>
        </Form.Item>
      </Form>
      <div style={{ textAlign: 'center', marginTop: '16px' }}>
        Don't have an account? <Button type="link" onClick={onSwitchToRegister}>Register now!</Button>
      </div>
    </>
  );
};

export default LoginForm;