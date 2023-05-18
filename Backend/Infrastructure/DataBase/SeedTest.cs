using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataBase;

internal class SeedTest
{
    public static void OnModelCreating(ModelBuilder builder)
    {
        foreach (Microsoft.EntityFrameworkCore.Metadata.IMutableForeignKey? relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }

        builder.Entity<Parking>().HasData(
        new Parking
        {
            Id = 1,
            LicensePlate = "TESTE",
            Model = "Volvo",
            Color = "Vermelho",
            EntryDate = DateTime.Now,
            DepartureDate = DateTime.Now.AddHours(4),
            HoursDuration = 4,
            MinutesDuration = 11,
            ChargedTime = 5,
            AmountCharged = 25,
            IsInsideParking = true,
            CreateDate = DateTime.Now,
            UpdateDate = null,
            DeleteDate = null
        },
        new Parking
        {
            Id = 2,
            LicensePlate = "TESTEDELETE",
            Model = "Volvo",
            Color = "Vermelho",
            EntryDate = DateTime.Now,
            DepartureDate = DateTime.Now.AddHours(4),
            HoursDuration = 4,
            MinutesDuration = 11,
            ChargedTime = 5,
            AmountCharged = 25,
            IsInsideParking = true,
            CreateDate = DateTime.Now,
            UpdateDate = null,
            DeleteDate = null
        });

        builder.Entity<PriceList>().HasData(
        new PriceList
        {
            Id = 1,
            InitialDate = new DateTime(2023, 1, 1),
            FinalDate = new DateTime(2023, 1, 12),
            InitialTimeValue = 5,
            AdditionalHourlyValue = 1,
            IsActive = true,
            CreateDate = DateTime.Now,
            UpdateDate = null,
            DeleteDate = null
        },
        new PriceList
        {
            Id = 2,
            InitialDate = new DateTime(2023, 1, 1),
            FinalDate = new DateTime(2023, 1, 12),
            InitialTimeValue = 5,
            AdditionalHourlyValue = 1,
            IsActive = true,
            CreateDate = DateTime.Now,
            UpdateDate = null,
            DeleteDate = null
        });
    }
}
