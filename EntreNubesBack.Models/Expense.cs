using System;
using System.Collections.Generic;

namespace EntreNubesBack.Models;

public partial class Expense
{
    public int IdExpense { get; set; }

    public int? IdCashClosing { get; set; }

    public int IdTypeExpense { get; set; }

    public int? IdThirdParty { get; set; }

    public int? IdEmployee { get; set; }

    public string ExpenseDescription { get; set; } = null!;

    public DateTime? Date { get; set; }

    public double? ExpenseTotal { get; set; }

    public bool State { get; set; }

    public int IdPaymentType { get; set; }

    public DateTime? CreationDate { get; set; }

    public int? LastIncomeId { get; set; }

    public virtual CashClosing? IdCashClosingNavigation { get; set; }

    public virtual Employee? IdEmployeeNavigation { get; set; }

    public virtual PaymentType IdPaymentTypeNavigation { get; set; } = null!;

    public virtual ThirdParty? IdThirdPartyNavigation { get; set; }

    public virtual TypesExpense IdTypeExpenseNavigation { get; set; } = null!;

    public virtual ICollection<PaymentType> IdPaymentTypes { get; } = new List<PaymentType>();
}
