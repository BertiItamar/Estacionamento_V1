import React from 'react';
import { Modal, Form, Button, Row, Col, Input, DatePicker } from 'antd';

const SaidaModal = ({ visible, onClose, onSubmit }) => {
  const [form] = Form.useForm();

  const handleSubmit = () => {
    form.validateFields().then((values) => {
      form.resetFields();
      onSubmit(values);
    });
  };

  return (
    <Modal title="Saída de Veículo" visible={visible} onCancel={onClose} footer={null}>
        <Form form={form} layout="vertical" onFinish={handleSubmit}>
            <Row gutter={24}>
                <Col span={24}>
                    <Form.Item label="Placa" name="licensePlate" rules={[{ required: true, message: 'Informe a placa do veículo' }]}>
                        <Input />
                    </Form.Item>
                </Col>
                <Col span={24}>
                <Form.Item label="Data e Hora de Saída" name="departureDate" rules={[{ required: true, }]}>
                    <DatePicker showTime format="YYYY-MM-DD HH:mm:ss" />
                </Form.Item>
            </Col>
                    <Button type="primary" htmlType="submit">Registrar Saída</Button>
            </Row>
        </Form>
    </Modal>
  );
};

export default SaidaModal;
