using System;
using System.Collections.Generic;

namespace EntreNubesBack.Models;

public partial class PaymentTypeExpense
{
    public int IdExpense { get; set; }

    public int IdPaymentType { get; set; }

    public virtual PaymentType IdPaymentTypeNavigation { get; set; } = null!;
}
