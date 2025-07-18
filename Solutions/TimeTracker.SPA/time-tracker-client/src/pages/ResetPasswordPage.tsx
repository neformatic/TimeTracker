// src/pages/ResetPasswordPage.tsx
import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { Form, Input, Button, App, Typography, Card, Flex, Alert } from 'antd';
import { LockOutlined } from '@ant-design/icons';
import apiClient from '../api/apiClient';
import '../styles/ResetPasswordPage.css'; // Подключаем CSS

const { Title, Text } = Typography;

interface ResetPasswordFormValues {
  password: string;
  confirmPassword: string;
}

const ResetPasswordPage: React.FC = () => {
  const { token } = useParams<{ token: string }>();
  const navigate = useNavigate();
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const [resetSuccess, setResetSuccess] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const { message } = App.useApp();

  useEffect(() => {
    if (!token) {
      setError('Password reset token is missing from the URL. Please ensure you clicked the full link.');
    }
  }, [token]);

  const onFinish = async (values: ResetPasswordFormValues) => {
    if (values.password !== values.confirmPassword) {
      setError('Passwords do not match!');
      return;
    }
    if (!token) {
      setError('Password reset token is missing.');
      return;
    }

    setLoading(true);
    setError(null);

    try {
      await apiClient.post('/Password/ResetPassword', {
        token: token,
        password: values.password,
      });
      setResetSuccess(true);
      message.success('Your password has been successfully reset!');
      setTimeout(() => {
        navigate('/login');
      }, 3000);
    } catch (err: any) {
      console.error('Password reset failed:', err.response?.data || err.message);
      let errorMessage = 'Failed to reset password. Please try again.';
      if (err.response?.data?.errorMessage) {
        errorMessage = err.response.data.errorMessage;
      } else if (err.response?.data) {
        errorMessage = typeof err.response.data === 'string' ? err.response.data : errorMessage;
      } else if (err.message) {
        errorMessage = err.message;
      }
      setError(errorMessage);
    } finally {
      setLoading(false);
    }
  };

  if (resetSuccess) {
    return (
      <Flex justify="center" align="flex-start" className="reset-password-container">
        <Card className="reset-password-card">
          <Title level={4} className="reset-password-title">
            Password Reset Successful!
          </Title>
          <Text className="reset-password-description">
            Your password has been changed.
          </Text>
          <Text className="reset-password-description">
            You will be redirected to the login page shortly.
          </Text>
          <Text strong style={{ marginTop: 20, display: 'block' }}>
            If you are not redirected,{' '}
            <Button type="link" onClick={() => navigate('/login')} className="reset-password-link">
              click here
            </Button>
            .
          </Text>
        </Card>
      </Flex>
    );
  }

  return (
    <Flex justify="center" align="flex-start" className="reset-password-container">
      <Card className="reset-password-card">
        <Flex justify="center" style={{ marginBottom: 10 }}>
          <Title level={2} className="reset-password-title">
            Reset Your Password
          </Title>
        </Flex>

        {error && (
          <Alert
            description={error}
            type="error"
            onClose={() => setError(null)}
            className="reset-password-alert"
          />
        )}

        {!token && (
          <Alert
            message="Invalid Link"
            description="The password reset link is invalid or missing. Please request a new one."
            type="warning"
            showIcon
            className="reset-password-alert"
          />
        )}

        <Form
          form={form}
          name="reset_password"
          layout="vertical"
          onFinish={onFinish}
          autoComplete="off"
          colon={false}
          disabled={!token}
        >
            <Form.Item
              label={<Text strong style={{ color: '#263238' }}>New Password</Text>}
              name="password"
              rules={[
                { required: true, message: 'Please input your new password!' },
                { min: 6, message: 'Password must be at least 6 characters long!' }
              ]}
              
              hasFeedback
              className="reset-password-form-item"
            >
              <Flex justify="center">
              <Input.Password
                prefix={<LockOutlined className="site-form-item-icon" style={{ color: 'rgba(0,0,0,.45)' }} />}
                placeholder="Enter new password"
                size="large"
              />
              </Flex>
            </Form.Item>

            <Form.Item
              label={<Text strong style={{ color: '#263238' }}>Confirm New Password</Text>}
              name="confirmPassword"
              dependencies={['password']}
              hasFeedback
              rules={[
                { required: true, message: 'Please confirm your new password!' },
                ({ getFieldValue }) => ({
                  validator(_, value) {
                    if (!value || getFieldValue('password') === value) {
                      return Promise.resolve();
                    }
                    return Promise.reject(new Error('The two passwords that you entered do not match!'));
                  },
                }),
              ]}
              className="reset-password-form-item"
            >
              <Flex justify="center">
              <Input.Password
                prefix={<LockOutlined className="site-form-item-icon" style={{ color: 'rgba(0,0,0,.45)' }} />}
                placeholder="Confirm new password"
                size="large"
              />
              </Flex>
            </Form.Item>

          <Form.Item>
            <Flex justify="center">
              <Button
                type="primary"
                htmlType="submit"
                loading={loading}
                block
                size="large"
                className="reset-password-button"
              >
                Reset Password
              </Button>
            </Flex>
          </Form.Item>
        </Form>
      </Card>
    </Flex>
  );
};

export default ResetPasswordPage;