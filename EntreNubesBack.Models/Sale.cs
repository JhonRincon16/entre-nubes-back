using System;
using System.Collections.Generic;

namespace EntreNubesBack.Models;

public partial class Sale
{
    public int IdSale { get; set; }

    public int? IdCashClosing { get; set; }

    public DateTime? SaleDate { get; set; }

    public double? TotalSale { get; set; }

    public bool State { get; set; }

    public virtual ICollection<Account> Accounts { get; } = new List<Account>();

    public virtual CashClosing? IdCashClosingNavigation { get; set; }

    public virtual ICollection<Payment> Payments { get; } = new List<Payment>();
}
