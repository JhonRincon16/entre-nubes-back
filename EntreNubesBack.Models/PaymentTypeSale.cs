using System;
using System.Collections.Generic;

namespace EntreNubesBack.Models;

public partial class PaymentTypeSale
{
    public int IdSale { get; set; }

    public int IdPaymentType { get; set; }

    public double? AmountToPay { get; set; }

    public virtual PaymentType IdPaymentTypeNavigation { get; set; } = null!;

    public virtual Sale IdSaleNavigation { get; set; } = null!;
}
