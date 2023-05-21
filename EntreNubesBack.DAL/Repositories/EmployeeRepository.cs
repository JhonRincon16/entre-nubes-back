using EntreNubesBack.DAL.DBContext;
using EntreNubesBack.DAL.Repositories.Contracts;
using EntreNubesBack.DTO.Employee;
using EntreNubesBack.Models;

namespace EntreNubesBack.DAL.Repositories;

public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
{
    private readonly IGenericRepository<Person> _personRepository;
    private readonly IGenericRepository<User> _userRepository;
    private readonly EntrenubesContext _dbContext;

    public EmployeeRepository(EntrenubesContext dbContext, 
                              IGenericRepository<Person> personRepository, 
                              IGenericRepository<User> userRepository) : base(dbContext)
    {
        _dbContext = dbContext;
        _personRepository = personRepository;
        _userRepository = userRepository;
    }

    public async Task<Employee> CreateEmployee(CreateEmployeeDto employeeInfo)
    {
        using (var transaction = _dbContext.Database.BeginTransaction())
        {
            try
            {
                var newPerson = await _personRepository.Create(new Person()
                {
                    DocumentType = employeeInfo.DocumentType,
                    DocumentNumber = employeeInfo.DocumentNumber,
                    PersonName = employeeInfo.Name,
                    PhoneNumber = employeeInfo.PhoneNumber,
                });
                if (newPerson == null)
                    throw new TaskCanceledException("Eror al crear la informacion personal");
                
                var newUser = await _userRepository.Create(new User()
                {
                    Email = employeeInfo.Email,
                    Password = employeeInfo.Password,
                    IdPerson = newPerson.IdPerson,
                    IdRol = employeeInfo.RolId,
                    State = true
                });
                if(newUser == null)
                    throw new TaskCanceledException("Eror al crear la informacion de acceso");

                var newEmployee = await base.Create(new Employee()
                {
                    State = true,
                    SalaryType = employeeInfo.SalaryType,
                    EmployeeType = employeeInfo.EmployeeType,
                    IdPerson = newPerson.IdPerson,
                    Salary = employeeInfo.Salary
                });
                if (newEmployee == null)
                    throw new TaskCanceledException("Error al crear el trabajador");
                transaction.Commit();
                return newEmployee;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }

    public async Task<bool> EditEmployee(EmployeeDto employeeDto)
    {
        using (var transaction = _dbContext.Database.BeginTransaction())
        {
            try
            {
                var personToEdit = await _personRepository.Get(p => p.IdPerson == employeeDto.IdPerson);
                personToEdit.DocumentType = employeeDto.UserDto.Person.DocumentType;
                personToEdit.DocumentNumber = employeeDto.UserDto.Person.DocumentNumber;
                personToEdit.PersonName = employeeDto.UserDto.Person.PersonName;
                personToEdit.PhoneNumber = employeeDto.UserDto.Person.PhoneNumber;
                var result = await _personRepository.Edit(personToEdit);
                if (!result)
                    throw new TaskCanceledException("Error al editar los datos personales");
                
                var userToEdit = await _userRepository.Get(u => u.IdUser == employeeDto.UserDto.IdUser);
                userToEdit.IdRol = employeeDto.UserDto.Rol.IdRol;
                userToEdit.Email = employeeDto.UserDto.Email;
                userToEdit.Password = employeeDto.UserDto.Password;
                var resultUser = await _userRepository.Edit(userToEdit);
                if (!resultUser)
                    throw new TaskCanceledException("Error al editar los datos de acceso");

                var employeeToEdit = await base.Get(e => e.IdEmployee == employeeDto.IdEmployee);
                employeeToEdit.SalaryType = employeeDto.SalaryType;
                employeeToEdit.EmployeeType = employeeDto.EmployeeType;
                employeeToEdit.Salary = employeeDto.Salary;
                var employeeResult = await base.Edit(employeeToEdit);
                if (!employeeResult)
                    throw new TaskCanceledException("Error al editar los datos del empleado");
                transaction.Commit();
                return employeeResult;
            }
            catch 
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}