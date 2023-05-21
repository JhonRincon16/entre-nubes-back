using EntreNubesBack.DTO.Employee;
using EntreNubesBack.Models;

namespace EntreNubesBack.DAL.Repositories.Contracts;

public interface IEmployeeRepository : IGenericRepository<Employee>
{
    Task<Employee> CreateEmployee(CreateEmployeeDto employeeInfo);
    Task<bool> EditEmployee(EmployeeDto employeeDto);
}