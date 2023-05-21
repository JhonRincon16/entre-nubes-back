namespace EntreNubesBack.DTO.Employee;

public class CreateEmployeeDto
{
    public string DocumentType { get; set; }
    public string DocumentNumber { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string EmployeeType { get; set; }
    public string SalaryType { get; set; }
    public double Salary { get; set; }
    public int RolId { get; set; }
}