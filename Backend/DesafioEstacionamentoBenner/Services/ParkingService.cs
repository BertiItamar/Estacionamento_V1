using DesafioEstacionamentoBenner.DTO_s;
using DesafioEstacionamentoBenner.Repositories.Interfaces;
using DesafioEstacionamentoBenner.Services.Interfaces;
using Infrastructure.DTO;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace DesafioEstacionamentoBenner.Services;

public class ParkingService : IParkingService
{
    public readonly IParkingRepository _repository;
    private readonly IPriceListService _priceListService;

    public ParkingService(IParkingRepository repository, IPriceListService priceListService)
    {
        _repository = repository;
        _priceListService = priceListService;
    }

    public async Task<List<Parking>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Parking> GetByIdAsync(long id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Parking> DeleteAsync(long id)
    {
        return await _repository.DeleteAsync(id);
    }

    /// <summary>
    /// Registra a entrada de um veículo no estacionamento.
    /// Verifica se há uma tabela de preços ativa para a data de entrada e cria um novo registro de estacionamento.
    /// </summary>
    /// <param name="entity">Objeto contendo os dados da entrada do veículo, incluindo a placa, a data de entrada, o modelo e a cor.</param>
    /// <returns>O objeto Parking recém-criado após a entrada do veículo.</returns>
    public async Task<Parking> RegisterEntry(RegisterEntryRequestDTO entity)
    {
        PriceList activePriceList = _priceListService.GetActivePriceList(entity.EntryDate);

        var existingVehicle = await _repository.GetDbSet()
       .FirstOrDefaultAsync(vehicle => vehicle.LicensePlate == entity.LicensePlate && vehicle.IsInsideParking);

        if (existingVehicle != null)
        {
            throw new AppException("O veículo já está dentro do estacionamento.");
        }

        if (activePriceList == null)
        {
            throw new AppException("Nenhuma tabela de preços ativa encontrada para a data de entrada.");
        }

        Parking newParking = new Parking
        {
            EntryDate = entity.EntryDate,
            LicensePlate = entity.LicensePlate,
            Model = entity.Model,
            Color = entity.Color,
            IsInsideParking = true
        };

        return await _repository.PostAsync(newParking);
    }

    /// <summary>
    /// Registra a saída de um veículo do estacionamento.
    /// Verifica se há uma tabela de preços ativa para a data de saída e atualiza os dados do veículo.
    /// </summary>
    /// <param name="entity">Objeto contendo os dados da saída do veículo, incluindo a placa e a data de saída.</param>
    /// <returns>O objeto Parking atualizado após a saída do veículo.</returns>
    public async Task<Parking> RegisterDeparture(RegisterDepartureRequestDTO entity)
    {
        PriceList activePriceList = _priceListService.GetActivePriceList(entity.DepartureDate);

        if (activePriceList == null)
        {
            throw new AppException("Nenhuma tabela de preços ativa encontrada para a data de saída.");
        }

        var vehicle = await GetVehicleByLicensePlate(entity.LicensePlate);

        if (vehicle == null)
        {
            throw new AppException("Veículo não encontrado.");
        }

        if (!vehicle.IsInsideParking)
        {
            throw new AppException("Veículo não está dentro do estacionamento.");
        }

        vehicle.DepartureDate = entity.DepartureDate;
        vehicle.IsInsideParking = false;

        (int duration, int chargedTime) = CalculateDurationAndChargedTime(vehicle.EntryDate, entity.DepartureDate);

        int totalMinutes = duration;
        decimal amountCharged = _priceListService.CalculateAmountCharged(totalMinutes, activePriceList);

        int durationHours = duration / 60; 
        int durationMinutes = duration % 60; 
        int chargedTimeHours = chargedTime;

        vehicle.HoursDuration = durationHours;
        vehicle.MinutesDuration = durationMinutes;
        vehicle.ChargedTime = chargedTimeHours;
        vehicle.AmountCharged = amountCharged;

        await _repository.PutAsync(vehicle);

        return vehicle;
    }

    /// <summary>
    /// Recupera um veículo pelo número da placa no banco de dados.
    /// </summary>
    /// <param name="licensePlate">Número da placa do veículo.</param>
    /// <returns>O veículo correspondente à placa fornecida, se encontrado;.</returns>
    private async Task<Parking> GetVehicleByLicensePlate(string licensePlate)
    {
        return await _repository.GetDbSet().FirstOrDefaultAsync(vehicle => vehicle.LicensePlate == licensePlate && vehicle.IsInsideParking && vehicle.DeleteDate == null);
    }

    /// <summary>
    /// Calcula a duração e o tempo cobrado com base na data de entrada e saída do veículo.
    /// </summary>
    /// <param name="entryDate">Data de entrada do veículo.</param>
    /// <param name="departureDate">Data de saída do veículo.</param>
    /// <returns>Tupla contendo a duração e o tempo cobrado.</returns>
    private (int DurationMinutes, int ChargedHours) CalculateDurationAndChargedTime(DateTime entryDate, DateTime departureDate)
    {
        if(departureDate < entryDate)
        {
            throw new AppException("A data de saída precisa ser maior que a data de entrada!");
        }

        TimeSpan duration = departureDate - entryDate;

        int durationMinutes = (int)duration.TotalMinutes;
        int totalHours = durationMinutes / 60;
        int minutos = durationMinutes % 60;

        int chargedHours;

        if (durationMinutes <= 10 && totalHours < 1)
        {
            // Se a duração for menor ou igual a 10 minutos, cobra-se 30 minutos
            chargedHours = 30;
        }
        if(totalHours >= 1 && minutos >= 10)
        {
            chargedHours = (int)Math.Ceiling((double)durationMinutes / 60);
        }
        else
        {
            // Caso contrário, arredonda-se para baixo a quantidade de horas
            chargedHours = totalHours;
        }

        return (durationMinutes, chargedHours);
    }
}
