using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities;

public class Parking : Base
{
    public string LicensePlate { get; set; }
    public string Model { get; set; }
    public string Color { get; set; }
    public DateTime EntryDate { get; set; }
    public DateTime? DepartureDate { get; set; }
    public int? HoursDuration { get; set; }
    public int? MinutesDuration { get; set; }
    public int? ChargedTime { get; set; }
    public decimal AmountCharged { get; set; }
    public bool IsInsideParking { get; set; }
}
