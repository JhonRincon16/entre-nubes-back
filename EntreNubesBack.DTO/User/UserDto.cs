namespace EntreNubesBack.DTO;

public class UserDto : BaseUserDto
{
    public RolDto Rol { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}