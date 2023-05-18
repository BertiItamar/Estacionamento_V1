﻿using DesafioEstacionamentoBenner.Repositories.Interfaces;
using DesafioEstacionamentoBenner.Services.Interfaces;
using Infrastructure.Entities;

namespace DesafioEstacionamentoBenner.Services;

public class PriceListService : IPriceListService
{
    public readonly IPriceListRepository _repository;

    public PriceListService(IPriceListRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<PriceList>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<PriceList> GetByIdAsync(long id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<PriceList> PostAsync(PriceList entity)
    {
        return await _repository.PostAsync(entity);
    }

    public async Task<PriceList> PutAsync(PriceList entity)
    {
        return await _repository.PutAsync(entity);
    }

    public async Task<PriceList> DeleteAsync(long id)
    {
        return await _repository.DeleteAsync(id);
    }

    /// <summary>
    /// Obtém a tabela de preços ativa para a data especificada.
    /// </summary>
    /// <param name="date">Data para a qual se deseja obter a tabela de preços.</param>
    /// <returns>A tabela de preços ativa para a data especificada, ou null se não houver nenhuma tabela ativa.</returns>
    public PriceList GetActivePriceList(DateTime date)
    {
        return _repository.GetAllAsync().Result.FirstOrDefault(pl => pl.IsActive && pl.InitialDate <= date && pl.FinalDate >= date);
    }

    /// <summary>
    /// Calcula o valor cobrado com base no tempo total de estacionamento e na tabela de preços.
    /// </summary>
    /// <param name="totalMinutes">Tempo total de estacionamento em minutos.</param>
    /// <param name="priceList">Tabela de preços utilizada para o cálculo.</param>
    /// <returns>O valor cobrado pelo tempo de estacionamento.</returns>
    public decimal CalculateAmountCharged(int totalMinutes, PriceList priceList)
    {
        decimal initialTimeValue = priceList.InitialTimeValue;
        decimal additionalHourlyValue = priceList.AdditionalHourlyValue;

        if (totalMinutes <= 10 && totalMinutes <=60)
        {
            return initialTimeValue / 2;
        }

        int hours = totalMinutes / 60;
        int remainingMinutes = totalMinutes % 60;

        decimal totalCharge = initialTimeValue;

        for (int i = 1; i < hours; i++)
        {
            totalCharge += initialTimeValue;
        }

        if (remainingMinutes > 10)
        {
            totalCharge += additionalHourlyValue;
        }

        return totalCharge;
    }
}
