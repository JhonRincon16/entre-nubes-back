namespace EntreNubesBack.DTO;

public class PersonDto
{
    public int IdPerson { get; set; }
    public string PersonName { get; set; } = null!;
    public string? DocumentNumber { get; set; }
    public string? DocumentType { get; set; }
    public string? PhoneNumber { get; set; }
}