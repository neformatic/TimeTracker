// src/components/RegisterForm.tsx
import { Button, Form, Input, App, Typography, Flex } from 'antd';
import React, { useState } from 'react';
import { useAuth } from '../auth/AuthContext';
import { IRegisterRequest } from '../types/api';
import { UserOutlined, LockOutlined, MailOutlined } from '@ant-design/icons';

const { Text } = Typography;

interface RegisterFormProps {
  onSuccess: () => void;
  onSwitchToLogin: () => void;
}

const RegisterForm: React.FC<RegisterFormProps> = ({ onSuccess, onSwitchToLogin }) => {
  const { register } = useAuth();
  const [loading, setLoading] = useState(false);
  const { message } = App.useApp();

  const onFinish = async (values: IRegisterRequest) => {
    setLoading(true);
    const result = await register(values);
    setLoading(false);

    if (result.success) {
      message.success('Registration successful! You can now log in.');
      onSuccess();
    } else {
      message.error(result.error || 'Registration failed. Please try again.');
    }
  };

  return (
    <Form
      name="register"
      layout="vertical"
      onFinish={onFinish}
      autoComplete="off"
      colon={false}
    >
      {/* Изменение: Устанавливаем ширину для Form.Item и центрируем его */}
      <Flex justify="center">
        <Form.Item
          label={<Text strong style={{ color: '#263238' }}>Name</Text>}
          name="name"
          rules={[{ required: true, message: 'Please input your name!' }]}
          style={{ marginBottom: 18, width: '100%', maxWidth: 500 }}
        >
          <Input
            prefix={<UserOutlined style={{ color: 'rgba(0,0,0,.65)' }} />}
            placeholder="Your Name"
            size="large"
            style={{ borderRadius: 8, borderColor: '#d9d9d9', width: 350 }}
          />
        </Form.Item>
      </Flex>

      {/* Изменение: Устанавливаем ширину для Form.Item и центрируем его */}
      <Flex justify="center">
        <Form.Item
          label={<Text strong style={{ color: '#263238' }}>Email</Text>}
          name="email"
          rules={[{ required: true, message: 'Please input your email!' }, { type: 'email', message: 'The input is not valid E-mail!' }]}
          style={{ marginBottom: 18, width: '100%', maxWidth: 500 }}
        >
          <Input
            prefix={<MailOutlined style={{ color: 'rgba(0,0,0,.65)' }} />}
            placeholder="email@example.com"
            size="large"
            style={{ borderRadius: 8, borderColor: '#d9d9d9' }}
          />
        </Form.Item>
      </Flex>

      {/* Изменение: Устанавливаем ширину для Form.Item и центрируем его */}
      <Flex justify="center">
        <Form.Item
          label={<Text strong style={{ color: '#263238' }}>Password</Text>}
          name="password"
          rules={[{ required: true, message: 'Please input your password!' }, { min: 6, message: 'Password must be at least 6 characters!' }]}
          style={{ marginBottom: 18, width: '100%', maxWidth: 500 }}
        >
          <Input.Password
            prefix={<LockOutlined style={{ color: 'rgba(0,0,0,.65)' }} />}
            placeholder="Password"
            size="large"
            style={{ borderRadius: 8, borderColor: '#d9d9d9' }}
          />
        </Form.Item>
      </Flex>

      {/* Изменение: Устанавливаем ширину для Form.Item и центрируем его */}
      <Flex justify="center">
        <Form.Item
          label={<Text strong style={{ color: '#263238' }}>Confirm Password</Text>}
          name="confirm"
          dependencies={['password']}
          hasFeedback
          rules={[
            {
              required: true,
              message: 'Please confirm your password!',
            },
            ({ getFieldValue }) => ({
              validator(_, value) {
                if (!value || getFieldValue('password') === value) {
                  return Promise.resolve();
                }
                return Promise.reject(new Error('The two passwords that you entered do not match!'));
              },
            }),
          ]}
          style={{ marginBottom: 30, width: '100%', maxWidth: 500 }}
        >
          <Input.Password
            prefix={<LockOutlined style={{ color: 'rgba(0,0,0,.65)' }} />}
            placeholder="Confirm Password"
            size="large"
            style={{ borderRadius: 8, borderColor: '#d9d9d9' }}
          />
        </Form.Item>
      </Flex>

      {/* Изменение: Оборачиваем кнопку в Flex для центрирования, возвращаем `block` и убираем `width` из стиля кнопки */}
      <Form.Item style={{ marginBottom: 16 }}>
        <Flex justify="center">
          <Button
            type="primary"
            htmlType="submit"
            loading={loading}
            block // Возвращаем block
            size="large"
            style={{
              height: 48,
              borderRadius: 8,
              fontSize: 16,
              fontWeight: 600,
              background: '#4A90E2',
              borderColor: '#4A90E2',
              maxWidth: 500, // Устанавливаем ту же максимальную ширину, что и для полей
            }}
          >
            Register
          </Button>
        </Flex>
      </Form.Item>
      <Flex justify="center" style={{ maxWidth: 300, margin: '0 auto' }}> {/* Центрируем "Already have an account?" */}
        <Text style={{ color: '#455A64' }}>
          Already have an account?{' '}
          <Button
            type="link"
            onClick={onSwitchToLogin}
            style={{
              padding: 0,
              height: 'auto',
              verticalAlign: 'baseline',
              color: '#4A90E2',
              fontWeight: 600
            }}
          >
            Log In!
          </Button>
        </Text>
      </Flex>
    </Form>
  );
};

export default RegisterForm;