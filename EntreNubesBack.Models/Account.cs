using System;
using System.Collections.Generic;

namespace EntreNubesBack.Models;

public partial class Account
{
    public int IdAccount { get; set; }

    public int? IdSale { get; set; }

    public string AccountName { get; set; } = null!;

    public bool State { get; set; }

    public bool IsClosed { get; set; }

    public DateTime? CreationDate { get; set; }

    public virtual Sale? IdSaleNavigation { get; set; }

    public virtual ICollection<ProductsDetail> ProductsDetails { get; } = new List<ProductsDetail>();
}
