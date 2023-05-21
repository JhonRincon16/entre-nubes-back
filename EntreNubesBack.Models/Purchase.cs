using System;
using System.Collections.Generic;

namespace EntreNubesBack.Models;

public partial class Purchase
{
    public int IdPurchase { get; set; }

    public int? IdPerson { get; set; }

    public int? IdCashClosing { get; set; }

    public string? PurchaseDescription { get; set; }

    public bool State { get; set; }

    public DateTime? CreationDate { get; set; }

    public int IdPaymentType { get; set; }

    public virtual CashClosing? IdCashClosingNavigation { get; set; }

    public virtual PaymentType IdPaymentTypeNavigation { get; set; } = null!;

    public virtual Person? IdPersonNavigation { get; set; }

    public virtual ICollection<PurchaseDetail> PurchaseDetails { get; } = new List<PurchaseDetail>();
}
