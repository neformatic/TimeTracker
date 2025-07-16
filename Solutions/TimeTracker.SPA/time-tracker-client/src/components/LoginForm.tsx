// src/components/LoginForm.tsx
import { Button, Form, Input, App, Typography, Flex, Alert, Modal } from 'antd';
import { UserOutlined, LockOutlined } from '@ant-design/icons';
import React, { useState } from 'react';
import { useAuth } from '../auth/AuthContext';
import { ILoginRequest } from '../types/api';
import ForgotPasswordForm from './ForgotPasswordForm';

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
    >
      {generalError && (
        <Alert
          description={generalError}
          type="error"
          onClose={() => setGeneralError(null)}
          style={{ marginBottom: 24, borderRadius: 8 }}
        />
      )}

      {/* Изменение: Устанавливаем ширину для Form.Item и центрируем его */}
      <Flex justify="center">
        <Form.Item
          label={<Text strong style={{ color: '#263238' }}>Email</Text>}
          name="email"
          rules={[
            { required: true, message: 'Please input your email!' },
            { type: 'email', message: 'The input is not a valid E-mail!' }
          ]}
          style={{ marginBottom: 18, width: '100%', maxWidth: 500 }}
        >
          <Input
            prefix={<UserOutlined className="site-form-item-icon" style={{ color: 'rgba(0,0,0,.65)' }} />}
            placeholder="email@example.com"
            size="large"
            style={{ borderRadius: 8, borderColor: '#d9d9d9', width: 350 }}
          />
        </Form.Item>
      </Flex>

      {/* Изменение: Устанавливаем ширину для Form.Item и центрируем его */}
      <Flex justify="center">
        <Form.Item
          label={<Text strong style={{ color: '#263238' }}>Password</Text>}
          name="password"
          rules={[{ required: true, message: 'Please input your password!' }]}
          style={{ marginBottom: 30, width: '100%', maxWidth: 500 }}
        >
          <Input.Password
            prefix={<LockOutlined className="site-form-item-icon" style={{ color: 'rgba(0,0,0,.65)' }} />}
            placeholder="Password"
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
            block // Возвращаем block, чтобы кнопка занимала ширину родителя
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
            Log In
          </Button>
        </Flex>
      </Form.Item>

      <Flex justify="center" style={{ marginBottom: 20, maxWidth: 300, margin: '0 auto 20px auto' }}> {/* Центрируем Forgot password */}
        <div />
        <Button
          type="link"
          onClick={() => setIsForgotPasswordModalVisible(true)}
          style={{ padding: 0, height: 'auto', color: '#4A90E2', fontWeight: 500 }}
        >
          Forgot your password?
        </Button>
      </Flex>

      <Flex justify="center" style={{ maxWidth: 300, margin: '0 auto' }}> {/* Центрируем Register now */}
        <Text style={{ color: '#455A64' }}>
          Don't have an account?{' '}
          <Button
            type="link"
            onClick={onSwitchToRegister}
            style={{
              padding: 0,
              height: 'auto',
              verticalAlign: 'baseline',
              color: '#4A90E2',
              fontWeight: 600
            }}
          >
            Register now!
          </Button>
        </Text>
      </Flex>

      <Modal
        title={<Title level={4} style={{ margin: 0, color: '#263238' }}>Forgot Password</Title>}
        open={isForgotPasswordModalVisible}
        onCancel={() => setIsForgotPasswordModalVisible(false)}
        footer={null}
        width={500}
        bodyStyle={{
          padding: '24px',
          borderRadius: 12,
          backgroundColor: 'rgba(255, 255, 255, 0.8)',
          backdropFilter: 'blur(15px)',
        }}
        maskStyle={{ backgroundColor: 'rgba(0, 0, 0, 0.2)' }}
        style={{ top: 100 }}
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