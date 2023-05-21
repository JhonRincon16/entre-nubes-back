using System;
using System.Collections.Generic;

namespace EntreNubesBack.Models;

public partial class Role
{
    public int IdRol { get; set; }

    public string RolName { get; set; } = null!;

    public bool State { get; set; }

    public virtual ICollection<User> Users { get; } = new List<User>();

    public virtual ICollection<Action> IdActions { get; } = new List<Action>();
}
