using System;
using System.Collections.Generic;

namespace EntreNubesBack.Models;

public partial class Action
{
    public int IdAction { get; set; }

    public string ActionName { get; set; } = null!;

    public bool State { get; set; }

    public virtual ICollection<Role> IdRols { get; } = new List<Role>();
}
