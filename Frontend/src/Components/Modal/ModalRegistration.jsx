import React from 'react';
import { Modal, Form, Input, Button, DatePicker, Row, Col } from 'antd';

const CadastroModal = ({ visible, onClose, onSubmit }) => {
  const [form] = Form.useForm();

  const handleSubmit = () => {
    form.validateFields().then((values) => {
      form.resetFields();
      onSubmit(values);
    });
  };

  return (
    <Modal title="Cadastro de Veículo" visible={visible} onCancel={onClose} footer={null}>
      <Form form={form} layout="vertical" onFinish={handleSubmit}>
        <Row gutter={24}>
            <Col span={24}>
                <Form.Item label="Placa" name="licensePlate" rules={[{ required: true, message: 'Informe a placa do veículo' }]}>
                    <Input />
                </Form.Item>
            </Col>
            <Col span={24}>
                <Form.Item label="Model" name="model" rules={[{ required: true, message: 'Informe o modelo do veículo' }]}>
                    <Input />
                </Form.Item>
            </Col>
            <Col span={24}>
                <Form.Item label="Cor" name="color" rules={[{ required: true, message: 'Informe a cor do veículo' }]}>
                    <Input />
            </Form.Item>
            </Col>
            <Col span={24}>
                <Form.Item label="Data de Entrada" name="entryDate" rules={[{ required: true, }]}>
                    <DatePicker showTime format="YYYY-MM-DD HH:mm:ss" />
                </Form.Item>
            </Col>
            {/* Adicione aqui os demais campos do formulário */}
            <Button type="primary" htmlType="submit">Cadastrar</Button>
        </Row>
      </Form>
    </Modal>
  );
};

export default CadastroModal;
