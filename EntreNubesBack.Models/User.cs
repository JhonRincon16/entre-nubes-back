using System;
using System.Collections.Generic;

namespace EntreNubesBack.Models;

public partial class User
{
    public int IdUser { get; set; }

    public int? IdPerson { get; set; }

    public int? IdRol { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool State { get; set; }

    public virtual ICollection<CashClosing> CashClosings { get; } = new List<CashClosing>();

    public virtual ICollection<HistoryChange> HistoryChanges { get; } = new List<HistoryChange>();

    public virtual Person? IdPersonNavigation { get; set; }

    public virtual Role? IdRolNavigation { get; set; }
}
