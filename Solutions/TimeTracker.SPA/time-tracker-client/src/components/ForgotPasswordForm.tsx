// src/components/ForgotPasswordForm.tsx
import React, { useState } from 'react';
import { Form, Input, Button, App, Flex, Typography } from 'antd';
import { MailOutlined } from '@ant-design/icons';
import apiClient from '../api/apiClient';
import '../styles/ForgotPasswordForm.css'; // Подключаем CSS

const { Text } = Typography;

interface ForgotPasswordFormProps {
  onSuccess: () => void;
  onCancel: () => void;
}

const ForgotPasswordForm: React.FC<ForgotPasswordFormProps> = ({ onSuccess, onCancel }) => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const { message } = App.useApp();

  const onFinish = async (values: { email: string }) => {
    setLoading(true);
    try {
      await apiClient.post('/Password/RequestResetPassword', { email: values.email });
      onSuccess();
      message.success('If an account with that email exists, a password reset link has been sent!');
    } catch (error: any) {
      console.error('Password reset request failed:', error.response?.data || error.message);
      let errorMessage = 'Failed to send password reset link. Please try again.';
      if (error.response?.data?.errorMessage) {
        errorMessage = error.response.data.errorMessage;
      } else if (error.response?.data) {
        errorMessage = typeof error.response.data === 'string' ? error.response.data : errorMessage;
      } else if (error.message) {
        errorMessage = error.message;
      }
      message.error(errorMessage);
    } finally {
      setLoading(false);
    }
  };

  return (
    <Form form={form} name="forgot_password" layout="vertical" onFinish={onFinish} autoComplete="off" colon={false} className="forgot-password-form">
      <Text className="instruction-text">
        Enter your email address and we'll send you a link to reset your password.
      </Text>

      <Flex justify="center">
        <Form.Item label={<Text strong className="email-label">Email</Text>} name="email"
          rules={[
            { required: true, message: 'Please input your email!' },
            { type: 'email', message: 'The input is not a valid E-mail!' }
          ]}
        >
          <Input prefix={<MailOutlined className="site-form-item-icon" style={{ color: 'rgba(0,0,0,.65)' }} />} placeholder="email@example.com" size="large" />
        </Form.Item>
      </Flex>

        <Flex justify="center">
          <Form.Item style={{ marginBottom: 0 }}>
          <Button
            type="primary"
            htmlType="submit"
            loading={loading}
            block
            size="large"
            className="ant-btn-primary"
          >
            Send Reset Link
          </Button>
          </Form.Item>
        </Flex>

        <Flex justify="center">
          <Form.Item style={{ marginTop: 10 }}>
          <Button
            type="default"
            onClick={onCancel}
            block
            size="large"
            className="ant-btn-default"
          >
            Cancel
          </Button>
          </Form.Item>
        </Flex>
    </Form>
  );
};

export default ForgotPasswordForm;