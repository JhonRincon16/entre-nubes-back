using System;
using System.Collections.Generic;

namespace EntreNubesBack.Models;

public partial class Person
{
    public int IdPerson { get; set; }

    public string PersonName { get; set; } = null!;

    public string? DocumentNumber { get; set; }

    public string? DocumentType { get; set; }

    public string? PhoneNumber { get; set; }

    public bool State { get; set; }

    public virtual ICollection<Employee> Employees { get; } = new List<Employee>();

    public virtual ICollection<Purchase> Purchases { get; } = new List<Purchase>();

    public virtual ICollection<ThirdParty> ThirdParties { get; } = new List<ThirdParty>();

    public virtual ICollection<User> Users { get; } = new List<User>();
}
