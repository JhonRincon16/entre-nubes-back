using System;
using System.Collections.Generic;

namespace EntreNubesBack.Models;

public partial class Product
{
    public int IdProduct { get; set; }

    public string ProductName { get; set; } = null!;

    public string ProductCategory { get; set; } = null!;

    public double? ProductPrice { get; set; }

    public int ProductStock { get; set; }

    public bool State { get; set; }

    public virtual ICollection<Payment> Payments { get; } = new List<Payment>();

    public virtual ICollection<ProductsDetail> ProductsDetails { get; } = new List<ProductsDetail>();

    public virtual ICollection<PurchaseDetail> PurchaseDetails { get; } = new List<PurchaseDetail>();
}
