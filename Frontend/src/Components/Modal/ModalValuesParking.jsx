import React from 'react';
import { Modal, Form, Input, DatePicker, Switch, Button } from 'antd';

const ModalCadastroValores = ({ visible, onClose, onSubmit }) => {
  const [form] = Form.useForm();

  const handleSubmit = () => {
    form.validateFields().then((values) => {
      form.resetFields();
      onSubmit(values);
    });
  };

  return (
    <Modal title="Cadastro de Valores" visible={visible} onCancel={onClose} footer={null}>
      <Form form={form} layout="vertical" onFinish={handleSubmit}>
        <Form.Item label="Data Inicial" name="initialDate" rules={[{ required: true }]}>
          <DatePicker />
        </Form.Item>
        <Form.Item label="Data Final" name="finalDate" rules={[{ required: true }]}>
          <DatePicker />
        </Form.Item>
        <Form.Item label="Valor para Tempo Inicial" name="initialTimeValue" rules={[{ required: true }]}>
          <Input type="number" step="0.01" />
        </Form.Item>
        <Form.Item label="Valor Adicional por Hora" name="additionalHourlyValue" rules={[{ required: true }]}>
          <Input type="number" step="0.01" />
        </Form.Item>
        <Form.Item label="Ativo" name="isActive" valuePropName="checked">
          <Switch />
        </Form.Item>
        <Button type="primary" htmlType="submit">
          Cadastrar
        </Button>
      </Form>
    </Modal>
  );
};

export default ModalCadastroValores;
