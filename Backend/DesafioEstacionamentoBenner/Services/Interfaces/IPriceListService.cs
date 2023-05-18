using Infrastructure.Entities;

namespace DesafioEstacionamentoBenner.Services.Interfaces;

public interface IPriceListService
{
    Task<List<PriceList>> GetAllAsync();

    Task<PriceList> GetByIdAsync(long id);

    Task<PriceList> PostAsync(PriceList entity);

    Task<PriceList> PutAsync(PriceList entity);

    Task<PriceList> DeleteAsync(long id);

    PriceList GetActivePriceList(DateTime date);

    decimal CalculateAmountCharged(int totalMinutes, PriceList priceList);
}
