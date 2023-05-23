using DesafioEstacionamentoBenner.DTO;
using Infrastructure.Entities;

namespace DesafioEstacionamentoBenner.Services.Interfaces;

public interface IParkingService
{
    Task<List<Parking>> GetAllAsync();

    Task<Parking> GetByIdAsync(long id);

    Task<Parking> DeleteAsync(long id);

    Task<Parking> RegisterEntry(RegisterEntryRequestDTO entity);

    Task<Parking> RegisterDeparture(RegisterDepartureRequestDTO entity);
}
