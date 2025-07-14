// src/index.tsx
import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';
import 'antd/dist/reset.css'; // Импортируем сброс стилей Ant Design для кроссбраузерности

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement // Используем type assertion для TypeScript
);
root.render(
  <React.StrictMode>
    <App />
  </React.StrictMode>
);