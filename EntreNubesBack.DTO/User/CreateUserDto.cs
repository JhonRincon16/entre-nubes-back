namespace EntreNubesBack.DTO;

public class CreateUserDto
{
    public int? IdPerson { get; set; }
    public string? UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public int IdRol { get; set; }
}