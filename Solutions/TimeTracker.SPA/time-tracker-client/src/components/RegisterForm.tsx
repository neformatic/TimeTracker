// src/components/RegisterForm.tsx
import React, { useState } from 'react';
import { Form, Input, Button, Typography, message } from 'antd';
import { UserOutlined, MailOutlined, LockOutlined } from '@ant-design/icons';
import { useAuth } from '../hooks/useAuth';
import { IRegisterRequest } from '../types/api';

const { Title } = Typography;

interface RegisterFormProps {
  onSuccess: () => void; // Колбэк, вызываемый при успешной регистрации
  onSwitchToLogin: () => void; // Колбэк для переключения на форму логина
}

/**
 * @component RegisterForm
 * @description Компонент формы регистрации нового пользователя.
 */
const RegisterForm: React.FC<RegisterFormProps> = ({ onSuccess, onSwitchToLogin }) => {
  const [loading, setLoading] = useState<boolean>(false);
  const { register } = useAuth(); // Получаем функцию register из хука useAuth

  /**
   * @function onFinish
   * @description Обработчик отправки формы регистрации.
   * @param values - Объект с данными формы (email, password, name).
   */
  const onFinish = async (values: IRegisterRequest) => {
    setLoading(true);
    const result = await register(values); // Вызываем функцию регистрации
    setLoading(false);

    if (result.success) {
      message.success('Registration successful! You can now log in.');
      onSuccess(); // Вызываем колбэк при успехе (закрыть модалку)
    } else {
      message.error(result.error || 'Registration failed. Please try again.');
    }
  };

  return (
    <>
      <Title level={3} style={{ textAlign: 'center', marginBottom: '24px' }}>
        Register
      </Title>
      <Form
        name="register"
        onFinish={onFinish}
        layout="vertical"
        scrollToFirstError
      >
        <Form.Item
          name="name"
          rules={[{ required: true, message: 'Please input your Name!' }]}
        >
          <Input prefix={<UserOutlined />} placeholder="Name" size="large" />
        </Form.Item>

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
          rules={[
            { required: true, message: 'Please input your Password!' },
            { min: 6, message: 'Password must be at least 6 characters long!' } // Пример простой клиентской валидации
          ]}
          hasFeedback // Показывает иконку обратной связи для валидации
        >
          <Input.Password prefix={<LockOutlined />} placeholder="Password" size="large" />
        </Form.Item>

        <Form.Item
          name="confirm"
          dependencies={['password']} // Зависит от поля password
          hasFeedback
          rules={[
            { required: true, message: 'Please confirm your Password!' },
            ({ getFieldValue }) => ({
              validator(_, value) {
                if (!value || getFieldValue('password') === value) {
                  return Promise.resolve();
                }
                return Promise.reject(new Error('The two passwords that you entered do not match!'));
              },
            }),
          ]}
        >
          <Input.Password prefix={<LockOutlined />} placeholder="Confirm Password" size="large" />
        </Form.Item>

        <Form.Item>
          <Button type="primary" htmlType="submit" loading={loading} block size="large">
            Register
          </Button>
        </Form.Item>
      </Form>
      <div style={{ textAlign: 'center', marginTop: '16px' }}>
        Already have an account? <Button type="link" onClick={onSwitchToLogin}>Login now!</Button>
      </div>
    </>
  );
};

export default RegisterForm;