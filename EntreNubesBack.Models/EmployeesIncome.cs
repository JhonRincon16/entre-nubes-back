using System;
using System.Collections.Generic;

namespace EntreNubesBack.Models;

public partial class EmployeesIncome
{
    public int IdEmployeeIncome { get; set; }

    public int? IdEmployee { get; set; }

    public DateTime IncomeDate { get; set; }

    public DateTime? DepartureDate { get; set; }

    public virtual Employee? IdEmployeeNavigation { get; set; }
}
