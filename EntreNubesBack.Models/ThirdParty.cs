using System;
using System.Collections.Generic;

namespace EntreNubesBack.Models;

public partial class ThirdParty
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

    public bool State { get; set; }

    public virtual ICollection<Expense> Expenses { get; } = new List<Expense>();

    public virtual Person? IdPersonNavigation { get; set; }
}
