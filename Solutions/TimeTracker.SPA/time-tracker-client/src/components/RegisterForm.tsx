// src/components/RegisterForm.tsx
import React, { useState } from 'react';
import { Button, Form, Input, App, Typography, Flex } from 'antd';
import { UserOutlined, LockOutlined, MailOutlined } from '@ant-design/icons';
import { useAuth } from '../auth/AuthContext';
import { IRegisterRequest } from '../types/api';
import '../styles/RegisterForm.css'; // Подключаем CSS

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
      className="register-form"
    >
      {/* Name Field */}
      <Flex justify="center">
        <Form.Item
          label={<Text strong style={{ color: '#263238' }}>Name</Text>}
          name="name"
          rules={[{ required: true, message: 'Please input your name!' }]}
        >
          <Input
            prefix={<UserOutlined />}
            placeholder="Your Name"
            size="large"
          />
        </Form.Item>
      </Flex>

      {/* Email Field */}
      <Flex justify="center">
        <Form.Item
          label={<Text strong>Email</Text>}
          name="email"
          rules={[
            { required: true, message: 'Please input your email!' },
            { type: 'email', message: 'The input is not valid E-mail!' }
          ]}
        >
          <Input
            prefix={<MailOutlined />}
            placeholder="email@example.com"
            size="large"
          />
        </Form.Item>
      </Flex>

      {/* Password Field */}
      <Flex justify="center">
        <Form.Item
          label={<Text strong>Password</Text>}
          name="password"
          rules={[
            { required: true, message: 'Please input your password!' },
            { min: 6, message: 'Password must be at least 6 characters!' }
          ]}
        >
          <Input.Password
            prefix={<LockOutlined />}
            placeholder="Password"
            size="large"
          />
        </Form.Item>
      </Flex>

      {/* Confirm Password Field */}
      <Flex justify="center">
        <Form.Item
          label={<Text strong>Confirm Password</Text>}
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
        >
          <Input.Password
            prefix={<LockOutlined />}
            placeholder="Confirm Password"
            size="large"
          />
        </Form.Item>
      </Flex>

      {/* Submit Button */}
      <Form.Item style={{ marginBottom: 16 }}>
        <Flex justify="center">
          <Button
            type="primary"
            htmlType="submit"
            loading={loading}
            block
            size="large"
            className="register-button"
          >
            Register
          </Button>
        </Flex>
      </Form.Item>

      {/* Login Link */}
      <Flex justify="center" style={{ maxWidth: 300, margin: '0 auto' }}>
        <Text style={{ color: '#455A64' }}>
          Already have an account?{' '}
          <Button
            type="link"
            onClick={onSwitchToLogin}
            className="login-link"
          >
            Log In!
          </Button>
        </Text>
      </Flex>
    </Form>
  );
};

export default RegisterForm;