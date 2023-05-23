namespace DesafioEstacionamentoBenner.DTO;

public class RegisterEntryRequestDTO
{
    public string LicensePlate { get; set; }
    public string Model { get; set; }
    public string Color { get; set; }
    public DateTime EntryDate { get; set; }
}
