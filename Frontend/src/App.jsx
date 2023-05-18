import './App.css';
import React from 'react';
import ptBR from 'antd/es/locale/pt_BR';
import { ConfigProvider, theme } from 'antd';
import Home from './Pages/Home';

function App() {
  return (
    <ConfigProvider
      locale={ptBR}
      theme={{
        algorithm: theme.darkAlgorithm,
        token: {
          colorPrimary: '#ff1e16',
          borderRadius: 16,
        },
      }}
    >
      <Home />
    </ConfigProvider>
  );
}

export default App;
