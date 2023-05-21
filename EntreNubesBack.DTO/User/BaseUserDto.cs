namespace EntreNubesBack.DTO;

public class BaseUserDto
{
    public int IdUser { get; set; }

    public PersonDto? Person { get; set; } = null!;
    
    public bool State { get; set; }
    
    public bool IsWorker { get; set; }
}