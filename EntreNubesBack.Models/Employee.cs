using System;
using System.Collections.Generic;

namespace EntreNubesBack.Models;

public partial class Employee
{
    public int IdEmployee { get; set; }

    public int? IdPerson { get; set; }

    public bool State { get; set; }

    public string EmployeeType { get; set; } = null!;

    public string SalaryType { get; set; } = null!;

    public double Salary { get; set; }

    public virtual ICollection<EmployeesIncome> EmployeesIncomes { get; } = new List<EmployeesIncome>();

    public virtual ICollection<Expense> Expenses { get; } = new List<Expense>();

    public virtual Person? IdPersonNavigation { get; set; }
}
