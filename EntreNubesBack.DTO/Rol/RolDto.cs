namespace EntreNubesBack.DTO;
public class RolDto
{
    public int IdRol { get; set; }

    public string RolName { get; set; } = null!;

    public List<ActionDto> IdActions { get; set; } = null!;
}