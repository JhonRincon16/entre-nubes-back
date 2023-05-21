namespace EntreNubesBack.DTO.Employee;

public class EmployeeDto
{
    public int IdEmployee { get; set; }
    public int? IdPerson { get; set; }
    public string EmployeeType { get; set; } = null!;
    public string SalaryType { get; set; } = null!;
    public double Salary { get; set; }
    public bool State { get; set; }
    public UserDto  UserDto { get; set; }
    public bool ShowRegisterExit { get; set; }
}