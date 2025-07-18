// src/components/LoginForm.tsx
import React, { useState } from 'react';
import { Button, Form, Input, App, Typography, Flex, Alert, Modal } from 'antd';
import { UserOutlined, LockOutlined } from '@ant-design/icons';
import ForgotPasswordForm from './ForgotPasswordForm';
import { useAuth } from '../auth/AuthContext';
import { ILoginRequest } from '../types/api';
import '../styles/LoginForm.css'; // Подключаем CSS

const { Text, Title } = Typography;

interface LoginFormProps {
  onSuccess: () => void;
  onSwitchToRegister: () => void;
}

const LoginForm: React.FC<LoginFormProps> = ({ onSuccess, onSwitchToRegister }) => {
  const { login } = useAuth();
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const { message } = App.useApp();
  const [generalError, setGeneralError] = useState<string | null>(null);
  const [isForgotPasswordModalVisible, setIsForgotPasswordModalVisible] = useState(false);

  const onFinish = async (values: ILoginRequest) => {
    setLoading(true);
    setGeneralError(null);
    form.setFields([{ name: 'email', errors: [] }, { name: 'password', errors: [] }]);
    const result = await login(values.email, values.password);
    setLoading(false);
    if (result.success) {
      message.success('Login successful!');
      onSuccess();
    } else {
      setGeneralError(result.error || 'An unexpected error occurred. Please try again.');
    }
  };

  return (
    <Form
      form={form}
      name="login"
      layout="vertical"
      onFinish={onFinish}
      autoComplete="off"
      colon={false}
      className="login-form"
    >
      {generalError && (
        <Alert
          description={generalError}
          type="error"
          onClose={() => setGeneralError(null)}
          className="ant-alert"
        />
      )}

      <Flex justify="center">
        <Form.Item
          label={<Text strong>Email</Text>}
          name="email"
          rules={[
            { required: true, message: 'Please input your email!' },
            { type: 'email', message: 'The input is not a valid E-mail!' }
          ]}
        >
          <Input
            prefix={<UserOutlined className="site-form-item-icon" />}
            placeholder="email@example.com"
            size="large"
          />
        </Form.Item>
      </Flex>

      <Flex justify="center">
        <Form.Item
          label={<Text strong>Password</Text>}
          name="password"
          rules={[{ required: true, message: 'Please input your password!' }]}
        >
          <Input.Password
            prefix={<LockOutlined className="site-form-item-icon" />}
            placeholder="Password"
            size="large"
          />
        </Form.Item>
      </Flex>

      <Form.Item style={{ marginBottom: 16 }}>
        <Flex justify="center">
          <Button
            type="primary"
            htmlType="submit"
            loading={loading}
            block
            size="large"
            className="login-button"
          >
            Log In
          </Button>
        </Flex>
      </Form.Item>

      <Flex justify="center" className="forgot-password-link-container">
        <Button
          type="link"
          onClick={() => setIsForgotPasswordModalVisible(true)}
          className="forgot-password-link"
        >
          Forgot your password?
        </Button>
      </Flex>

      <Flex justify="center" className="register-link-container">
        <Text>
          Don't have an account?{' '}
          <Button
            type="link"
            onClick={onSwitchToRegister}
            className="register-link"
          >
            Register now!
          </Button>
        </Text>
      </Flex>

      <Modal
        title={<Title level={4} style={{ margin: 0 }}>Forgot Password</Title>}
        open={isForgotPasswordModalVisible}
        onCancel={() => setIsForgotPasswordModalVisible(false)}
        footer={null}
        width={500}
      >
        <ForgotPasswordForm
          onSuccess={() => {
            setIsForgotPasswordModalVisible(false);
          }}
          onCancel={() => setIsForgotPasswordModalVisible(false)}
        />
      </Modal>
    </Form>
  );
};

export default LoginForm;