using DesafioEstacionamentoBenner.Repositories.Interfaces;
using DesafioEstacionamentoBenner.Services.Interfaces;
using Infrastructure.DTO;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

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
        // Verifica se existe alguma tabela de preço que tenha uma sobreposição de datas
        var existingPriceList = await HasPriceListInDateRangeAsync(entity.InitialDate, entity.FinalDate);

        if (existingPriceList)
        {
            // Caso exista uma tabela de preço com sobreposição de datas, você pode lançar uma exceção ou tratar de acordo com a lógica do seu aplicativo
            throw new AppException("Já existe uma tabela de preço cadastrada para o mesmo período.");
        }
        else
        {
            // Caso não exista sobreposição de datas, realiza a operação de persistência
            return await _repository.PostAsync(entity);
        }
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
        var currentDate = date.Date; // Ignorando a hora para fazer a query somente pela data no banco. 
        return _repository.GetAllAsync().Result.FirstOrDefault(pl => pl.IsActive && pl.InitialDate.Date <= currentDate && pl.FinalDate.Date >= currentDate);
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

    public async Task<bool> HasPriceListInDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var priceLists = await _repository.GetAllAsync();
        var hasPriceList = priceLists.Any(pl => pl.InitialDate <= endDate && pl.FinalDate >= startDate);

        return hasPriceList;
    }
}

