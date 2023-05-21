using System;
using System.Collections.Generic;

namespace EntreNubesBack.Models;

public partial class PaymentType
{
    public int IdPaymentType { get; set; }

    public string PaymentTypeName { get; set; } = null!;

    public bool State { get; set; }

    public virtual ICollection<Expense> Expenses { get; } = new List<Expense>();

    public virtual ICollection<Payment> Payments { get; } = new List<Payment>();

    public virtual ICollection<Purchase> Purchases { get; } = new List<Purchase>();

    public virtual ICollection<Expense> IdExpenses { get; } = new List<Expense>();
}
