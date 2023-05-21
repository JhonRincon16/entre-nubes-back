namespace EntreNubesBack.DTO;

public class ThirdPartyDto
{
    public int IdThirdParty { get; set; }

    public int? IdPerson { get; set; }

    public string BusinessName { get; set; } = null!;

    public string? CompanyName { get; set; }

    public string? Nit { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string Category { get; set; } = null!;

    public string? ProductServiceName { get; set; }
    
    public PersonDto? AdvisorDto { get; set; }
}