using System;
using System.Collections.Generic;

namespace EntreNubesBack.Models;

public partial class ProductsDetail
{
    public int IdDetail { get; set; }

    public int IdAccount { get; set; }

    public int IdProduct { get; set; }

    public int ProductQuantity { get; set; }

    public double? TotalPrice { get; set; }

    public bool State { get; set; }

    public double ProductPrice { get; set; }

    public virtual ICollection<AddProductUnitsDetail> AddProductUnitsDetails { get; } = new List<AddProductUnitsDetail>();

    public virtual Account IdAccountNavigation { get; set; } = null!;

    public virtual Product IdProductNavigation { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; } = new List<Payment>();
}
