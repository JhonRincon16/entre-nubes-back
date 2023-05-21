using System;
using System.Collections.Generic;

namespace EntreNubesBack.Models;

public partial class Payment
{
    public int IdPayment { get; set; }

    public int IdSale { get; set; }

    public int IdPaymentType { get; set; }

    public int IdProduct { get; set; }

    public int IdProductDetail { get; set; }

    public int Quantity { get; set; }

    public double AmountToPay { get; set; }

    public bool State { get; set; }

    public DateTime Date { get; set; }

    public virtual PaymentType IdPaymentTypeNavigation { get; set; } = null!;

    public virtual ProductsDetail IdProductDetailNavigation { get; set; } = null!;

    public virtual Product IdProductNavigation { get; set; } = null!;

    public virtual Sale IdSaleNavigation { get; set; } = null!;
}
