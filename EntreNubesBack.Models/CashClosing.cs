using System;
using System.Collections.Generic;

namespace EntreNubesBack.Models;

public partial class CashClosing
{
    public int IdCashClosing { get; set; }

    public int? IdUser { get; set; }

    public DateTime? DateCashClosing { get; set; }

    public DateTime StartDate { get; set; }

    public double BaseCash { get; set; }

    public virtual ICollection<Expense> Expenses { get; } = new List<Expense>();

    public virtual User? IdUserNavigation { get; set; }

    public virtual ICollection<Purchase> Purchases { get; } = new List<Purchase>();

    public virtual ICollection<Sale> Sales { get; } = new List<Sale>();
}
