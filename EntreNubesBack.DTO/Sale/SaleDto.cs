using EntreNubesBack.DTO.Account;
using EntreNubesBack.DTO.Payment;

namespace EntreNubesBack.DTO.Sale;

public class SaleDto
{
    public int IdSale { get; set; }
    public int? IdCashClosing { get; set; }
    public DateTime? SaleDate { get; set; }
    public double? TotalSale { get; set; }
    public bool State { get; set; }
    public List<AccountDto> Accounts { get; } = new List<AccountDto>();
    public ICollection<PaymentDto> Payments { get; } = new List<PaymentDto>();
}