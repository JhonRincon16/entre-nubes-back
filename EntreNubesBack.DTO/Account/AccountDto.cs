namespace EntreNubesBack.DTO.Account;

public class AccountDto
{
    public int IdAccount { get; set; }

    public string AccountName { get; set; } = null!;

    public bool State { get; set; }

    public bool IsClosed { get; set; }

    public DateTime CreationDate { get; set; }
}