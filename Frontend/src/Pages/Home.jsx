/* eslint-disable no-unused-vars */
/* eslint-disable import/no-unresolved */
import React, { useEffect, useState } from 'react';
import { Table, Button, message } from 'antd';
import api from '../Services/api';
import CadastroModal from '../Components/Modal/ModalRegistration';
import SaidaModal from '../Components/Modal/ModalExit';
import ModalCadastroValores from '../Components/Modal/ModalValuesParking';
import useMessage from 'antd/es/message/useMessage';

const dayjs = require('dayjs');

export default function Home() {
  const [loading, setLoading] = useState(false);
  const [dataSource, setDataSource] = useState([]);
  const [dataSourcePrice, setDataSourcePrice] = useState([]);
  const [isCadastroModalVisible, setIsCadastroModalVisible] = useState(false);
  const [isSaidaModalVisible, setIsSaidaModalVisible] = useState(false);
  const [isModalVisible, setIsModalVisible] = useState(false);

  const closeCadastroModal = () => {
    setIsCadastroModalVisible(false);
  };

  const handleOpenModal = () => {
    setIsModalVisible(true);
  };

  const handleCloseModal = () => {
    setIsModalVisible(false);
  };

  const handleCadastroSubmit = async (values) => {
    const objectCreateParking = {
      entryDate: dayjs(values.entryDate).subtract(3, 'hours'),
      licensePlate: values.licensePlate,
      model: values.model,
      brand: values.brand,
      color: values.color,
    };
    await api
      .post('api/Parking/RegisterEntry', objectCreateParking)
      .then(({ data }) => {
        console.log(data);
        setLoading(false);
        message.success('Veículo cadastrado com sucesso!');
      })
      .catch((e) => message.error('Erro ao tentar cadastrar veículo'));
    closeCadastroModal();
    GetParking();
  };

  // Função para fechar o modal de saída
  const closeSaidaModal = () => {
    setIsSaidaModalVisible(false);
  };

  // Função para processar o formulário de saída
  const handleExitSubmit = async (values) => {
    const objectCreateParking = {
      licensePlate: values.licensePlate,
      departureDate: dayjs(values.departureDate).subtract(3, 'hours'),
    };
    await api
      .put('api/Parking/RegisterDeparture', objectCreateParking)
      .then(({ data }) => {
        console.log(data);
        setLoading(false);
        message.success('Veículo finalizado com sucesso!');
      })
      .catch((e) => message.error('Erro ao tentar finalizar veículo'));
    closeSaidaModal();
    GetParking();
  };

  const handleFormSubmit = async (values) => {
    const objectCreateParking = {
      initialDate: values.initialDate,
      finalDate: values.finalDate,
      initialTimeValue: values.initialTimeValue,
      additionalHourlyValue: values.additionalHourlyValue,
      isActive: true,
    };
    await api
      .post('api/PriceList', objectCreateParking)
      .then(({ data }) => {
        console.log(data);
        setLoading(false);
        message.success('Preço do Estacionamento cadastrado com sucesso!');
      })
      .catch((e) => {});
    setIsModalVisible(false);
    GetPriceList();
  };

  async function GetPriceList() {
    await api
      .get('api/PriceList')
      .then(({ data }) => {
        const listObjects = [];
        data?.forEach((element, index) => {
          const formattedInitialDate = dayjs(element?.initialDate).format(
            'DD/MM/YYYY'
          );
          const formattedFinalDate = element?.finalDate
            ? dayjs(element?.finalDate).format('DD/MM/YYYY')
            : '';
          const formattedInitialTime = dayjs(element?.initialDate).format(
            'HH:mm:ss'
          );
          const formattedFinalTime = element?.finalDate
            ? dayjs(element?.finalDate).format('HH:mm:ss')
            : '';

          listObjects.push({
            key: element.id,
            initialDate: formattedInitialDate,
            finalDate: formattedFinalDate,
            initialTime: formattedInitialTime,
            finalTime: formattedFinalTime,
            initialTimeValue: element.initialTimeValue,
            additionalHourlyValue: element.additionalHourlyValue,
            isActive: true,
          });
        });
        setDataSourcePrice(listObjects);
      })
      .catch((e) => {});
  }

  async function GetParking() {
    await api
      .get('api/Parking')
      .then(({ data }) => {
        const listObjects = [];
        data?.forEach((element, index) => {
          const formattedEntryDate = dayjs(element?.entryDate).format(
            'DD/MM/YYYY'
          );
          const formattedEntryTime = dayjs(element?.entryDate).format('HH:mm');
          const formattedDepartureDate = element?.departureDate
            ? dayjs(element?.departureDate).format('DD/MM/YYYY')
            : '';
          const formattedDepartureTime = element?.departureDate
            ? dayjs(element?.departureDate).format('HH:mm')
            : '';

          listObjects.push({
            key: element.id,
            licensePlate: element?.licensePlate,
            entryDate: formattedEntryDate,
            entryTime: formattedEntryTime,
            departureDate: formattedDepartureDate,
            departureTime: formattedDepartureTime,
            chargedTime: element?.chargedTime,
            hoursDuration: element?.hoursDuration,
            minutesDuration: element?.minutesDuration,
            amountCharged: element?.amountCharged
              ? element?.amountCharged
              : null,
          });
        });
        setDataSource(listObjects);
      })
      .catch((e) => {
        message.error('Erro ao tentar trazer lista de preços');
      });
  }

  const formatMoney = (value) => {
    return value?.toLocaleString('pt-BR', {
      style: 'currency',
      currency: 'BRL',
    });
  };

  const formatDuration = (hours, minutes) => {
    return `${hours || 0} horas ${minutes || 0} minutos`;
  };

  const formatDurationPrice = (hours, minutes) => {
    return hours === 0 ? `${minutes || 0} minutos` : `${hours || 0} horas `;
  };

  const columnsPrice = [
    {
      title: 'Data Inicial',
      dataIndex: 'initialDate',
      key: 'initialDate',
      render: (text) => <span>{text}</span>,
    },
    {
      title: 'Data Final',
      dataIndex: 'finalDate',
      key: 'finalDate',
    },
    {
      title: 'Valor Inicial',
      dataIndex: 'initialTimeValue',
      key: 'initialTimeValue',
      render: (text, record) => formatMoney(record.initialTimeValue),
    },
    {
      title: 'Valor Adicional',
      dataIndex: 'additionalHourlyValue',
      key: 'additionalHourlyValue',
      render: (text, record) => formatMoney(record.additionalHourlyValue),
    },
  ];

  const columns = [
    {
      title: 'Placa',
      dataIndex: 'licensePlate',
      key: 'licensePlate',
      render: (text) => <span>{text}</span>,
    },
    {
      title: 'Horário de Chegada',
      dataIndex: 'entryDateTime',
      key: 'entryDateTime',
      render: (_, record) => (
        <span>
          {record.entryDate} {record.entryTime}
        </span>
      ),
    },
    {
      title: 'Horário de Saída',
      dataIndex: 'departureDateTime',
      key: 'departureDateTime',
      render: (_, record) => (
        <span>
          {record.departureDate} {record.departureTime}
        </span>
      ),
    },
    {
      title: 'Duração',
      dataIndex: 'hoursDuration',
      key: 'hoursDuration',
      render: (text, record) =>
        formatDuration(record.hoursDuration, record.minutesDuration),
    },
    {
      title: 'Tempo Cobrado',
      dataIndex: 'chargedTime',
      key: 'chargedTime',
      render: (text, record) =>
        formatDurationPrice(record.chargedTime, record.minutesDuration),
    },
    {
      title: 'Preço',
      dataIndex: 'amountCharged',
      key: 'amountCharged',
      render: (text, record) => formatMoney(record.amountCharged),
    },
  ];

  useEffect(() => {
    GetParking();
    GetPriceList();
  }, []);
  return (
    <>
      <h1
        style={{
          textAlign: 'center',
          marginTop: 60,
          marginBottom: -110,
          fontSize: 50,
          color: '#333333',
        }}
      >
        Estacionamento Benner
      </h1>
      <div
        style={{
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
          marginTop: '  200px',
        }}
      >
        <div style={{ position: 'absolute', top: 0, right: 0 }}>
          <Table
            columns={columnsPrice}
            dataSource={dataSourcePrice}
            loading={loading}
            style={{ width: '100%', maxWidth: 'px', maxHeight: 30 }}
            size="small"
            pagination={{ pageSize: 7, hideOnSinglePage: true }}
          />
        </div>

        <div>
          <Button
            style={{ marginRight: '10px', marginBottom: '10px' }}
            onClick={() => setIsCadastroModalVisible(true)}
          >
            Abrir Cadastro de Veículo
          </Button>
          <Button
            style={{ marginRight: '10px', marginBottom: '10px' }}
            onClick={() => setIsSaidaModalVisible(true)}
          >
            Saída de veículo
          </Button>
          <Button onClick={handleOpenModal}>
            Cadastro de Preços do Estacionamento
          </Button>
        </div>

        {/* Tabela centralizada */}
        <Table
          columns={columns}
          dataSource={dataSource}
          pagination={{ pageSize: 10 }}
          loading={loading}
          style={{ width: '95%', maxWidth: '1000px', margin: 'auto' }}
        />
        <div>
          <CadastroModal
            visible={isCadastroModalVisible}
            onClose={closeCadastroModal}
            onSubmit={handleCadastroSubmit}
          />
          <SaidaModal
            visible={isSaidaModalVisible}
            onClose={closeSaidaModal}
            onSubmit={handleExitSubmit}
          />
          <ModalCadastroValores
            visible={isModalVisible}
            onClose={handleCloseModal}
            onSubmit={handleFormSubmit}
          />
        </div>
      </div>
    </>
  );
}
