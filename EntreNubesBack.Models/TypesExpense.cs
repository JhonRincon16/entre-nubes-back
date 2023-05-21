using System;
using System.Collections.Generic;

namespace EntreNubesBack.Models;

public partial class TypesExpense
{
    public int IdTypeExpense { get; set; }

    public string TypeExpenseName { get; set; } = null!;

    public bool State { get; set; }

    public virtual ICollection<Expense> Expenses { get; } = new List<Expense>();
}
