using System;
using System.Collections.Generic;

namespace EntreNubesBack.Models;

public partial class AddProductUnitsDetail
{
    public int IdAddUnits { get; set; }

    public int IdDetail { get; set; }

    public string? Description { get; set; }

    public int ProductQuantity { get; set; }

    public DateTime CreationDate { get; set; }

    public virtual ProductsDetail IdDetailNavigation { get; set; } = null!;
}
