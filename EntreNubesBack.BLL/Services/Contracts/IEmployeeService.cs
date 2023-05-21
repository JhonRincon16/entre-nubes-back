using EntreNubesBack.DTO.Employee;

namespace EntreNubesBack.BLL.Services.Contracts;

public interface IEmployeeService
{
    Task<List<EmployeeDto>> EmployeeList();
    Task<EmployeeDto> CreateEmployee(CreateEmployeeDto employeeInfo);
    Task<bool> EditEmployee(EmployeeDto employeeDto);
    Task<bool> DeleteEmployee(int employeeId);
    Task<bool> RegisterEntrance(int employeeId);
    Task<bool> RegisterExit(int employeeId);
    Task<CalculateHoursWorkedDto> CalculateHoursWorked(int employeeId);
}