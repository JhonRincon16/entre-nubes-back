using System;
using System.Collections.Generic;

namespace EntreNubesBack.Models;

public partial class PurchaseDetail
{
    public int IdPurchase { get; set; }

    public int IdProduct { get; set; }

    public int Quantity { get; set; }

    public double? TotalPrice { get; set; }

    public virtual Product IdProductNavigation { get; set; } = null!;

    public virtual Purchase IdPurchaseNavigation { get; set; } = null!;
}
