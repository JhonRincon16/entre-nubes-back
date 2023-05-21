using System;
using System.Collections.Generic;

namespace EntreNubesBack.Models;

public partial class HistoryChange
{
    public int IdHistoryChanges { get; set; }

    public int? IdUser { get; set; }

    public string TableName { get; set; } = null!;

    public string Actions { get; set; } = null!;

    public DateTime Date { get; set; }

    public virtual User? IdUserNavigation { get; set; }
}
