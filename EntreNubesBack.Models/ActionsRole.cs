using System;
using System.Collections.Generic;

namespace EntreNubesBack.Models;

public partial class ActionsRole
{
    public int IdRol { get; set; }

    public int IdAction { get; set; }

    public virtual Action IdActionNavigation { get; set; } = null!;

    public virtual Role IdRolNavigation { get; set; } = null!;
}
