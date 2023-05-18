namespace Infrastructure.Entities;

public class PriceList : Base
{
    public DateTime InitialDate { get; set; }
    public DateTime FinalDate { get; set; }
    public decimal InitialTimeValue { get; set; }
    public decimal AdditionalHourlyValue { get; set; }
    public bool IsActive { get; set; }   
}
