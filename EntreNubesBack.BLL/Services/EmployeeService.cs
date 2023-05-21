using AutoMapper;
using EntreNubesBack.BLL.Services.Contracts;
using EntreNubesBack.DAL.Repositories.Contracts;
using EntreNubesBack.DTO.Employee;
using EntreNubesBack.Models;
using Microsoft.EntityFrameworkCore;

namespace EntreNubesBack.BLL.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IGenericRepository<Person> _personRepository;
    private readonly IGenericRepository<User> _userRepository;
    private readonly IGenericRepository<EmployeesIncome> _employeeIncomeRepository;
    private readonly IExpenseRepository _expenseRepository;
    private readonly IMapper _mapper;

    public EmployeeService(IEmployeeRepository employeeRepository, 
                           IMapper mapper, 
                           IGenericRepository<Person> personRepository, 
                           IGenericRepository<User> userRepository, 
                           IGenericRepository<EmployeesIncome> employeeIncomeRepository, 
                           IExpenseRepository expenseRepository)
    {
        _employeeRepository = employeeRepository;
        _personRepository = personRepository;
        _userRepository = userRepository;
        _employeeIncomeRepository = employeeIncomeRepository;
        _expenseRepository = expenseRepository;
        _mapper = mapper;
    }

    public async Task<List<EmployeeDto>> EmployeeList()
    {
        var employees = await _employeeRepository.Consult(e => e.State);
        var listWithData = employees
            .Include(e => e.IdPersonNavigation)
            .ThenInclude(p => p.Users)
            .ThenInclude(u => u.IdRolNavigation)
            .Include(u => u.EmployeesIncomes).ToList();
        List<EmployeeDto> list = new List<EmployeeDto>();
        foreach (var employee in listWithData)
        {
            var employeeDto = _mapper.Map<EmployeeDto>(employee);
            var lastIncome = employee.EmployeesIncomes.LastOrDefault();
            if (lastIncome != null)
            {
                employeeDto.ShowRegisterExit = lastIncome.DepartureDate == null;
            }
            list.Add(employeeDto);
        }
        return list;
    }

    public async Task<EmployeeDto> CreateEmployee(CreateEmployeeDto employeeInfo)
    {
        try
        {
            var person = await _personRepository.Get(p => p.DocumentType.ToUpper() == employeeInfo.DocumentType.ToUpper()
                                                          && p.DocumentNumber.ToUpper() == employeeInfo.DocumentNumber.ToUpper());
            if (person != null)
                throw new TaskCanceledException("El documento ingresado ya existe");

            var user = await _userRepository.Get(u => u.Email == employeeInfo.Email);
            if (user != null)
                throw new TaskCanceledException("Correo ingresado no disponible");

            var employee = await _employeeRepository.CreateEmployee(employeeInfo);
            if (employee == null)
                throw new TaskCanceledException("Error al crear al empleado");
            return _mapper.Map<EmployeeDto>(employee); 
        }
        catch
        {
            throw;
        }
    }

    public async Task<bool> EditEmployee(EmployeeDto employeeDto)
    {
        try
        {
            var employee = await _employeeRepository.Get(e => e.IdEmployee == employeeDto.IdEmployee);
            if (employee == null)
                throw new TaskCanceledException("El empleado no existe");
        
            var person = await _personRepository.Get(p => p.IdPerson != employeeDto.IdPerson 
                                                          && p.DocumentType == employeeDto.UserDto.Person.DocumentType
                                                          && p.DocumentNumber == employeeDto.UserDto.Person.DocumentNumber);
            if (person != null)
                throw new TaskCanceledException("El documento ingresado ya pertenece a otra persona");

            var user = await _userRepository.Get(u => u.IdUser != employeeDto.UserDto.IdUser
                                                      && u.Email == employeeDto.UserDto.Email);
            if (user != null)
                throw new TaskCanceledException("El email ingresado no esta disponible");

            var result = await _employeeRepository.EditEmployee(employeeDto);
            if (!result)
                throw new TaskCanceledException("Error al editar al empleado");
            return result;
        }
        catch
        {
            throw;
        }
    }

    public async Task<bool> DeleteEmployee(int employeeId)
    {
        var employee = await _employeeRepository.Consult(e => e.IdEmployee == employeeId);
        var aux = employee.Include(e => e.IdPersonNavigation)
                                                        .ThenInclude(p => p.Users).First();
        if (employee == null)
            throw new TaskCanceledException("El empleado no existe");
        aux.State = false;
        aux.IdPersonNavigation.Users.First().State = false;
        
        bool result = await _employeeRepository.Edit(aux);
        if (!result)
            throw new TaskCanceledException("Error al editar los datos del empleado");
        return result;
    }

    public async Task<bool> RegisterEntrance(int userId)
    {
        try
        {
            var user = await _userRepository.Get(u => u.IdUser == userId);
            if (user == null)
                throw new TaskCanceledException("El usuario no existe");
            var employee = await _employeeRepository.Consult(e => e.State && e.IdPerson == user.IdPerson);
            if (employee == null)
                throw new TaskCanceledException("El empleado no existe");
            var employeeDetail = employee.Include(e => e.EmployeesIncomes).FirstOrDefault();
            if(employeeDetail == null)
                throw new TaskCanceledException("El empleado no existe");
            if (employeeDetail.EmployeesIncomes.Any())
            {
                var lastIncome = employeeDetail.EmployeesIncomes.LastOrDefault();
                if (lastIncome != null)
                {
                    if (lastIncome.DepartureDate != null)
                    {
                        employeeDetail.EmployeesIncomes.Add(new EmployeesIncome()
                        {
                            IdEmployee = employeeDetail.IdEmployee,
                            IncomeDate = DateTime.Now
                        });
                        bool result = await _employeeRepository.Edit(employeeDetail);
                        return result;
                    }
                    else
                    {
                        throw new TaskCanceledException("No se puede registrar una entrada debido ha que hay un turno activo.");
                    }
                }
            }
            employeeDetail.EmployeesIncomes.Add(new EmployeesIncome()
            {
                IdEmployee = employeeDetail.IdEmployee,
                IncomeDate = DateTime.Now
            });
            return await _employeeRepository.Edit(employeeDetail);
        }
        catch
        {
            throw;
        }
    }

    public async Task<bool> RegisterExit(int userId)
    {
        var user = await _userRepository.Get(u => u.IdUser == userId);
        if (user == null)
            throw new TaskCanceledException("El usuario no existe");
        var employee = await _employeeRepository.Consult(e => e.State && e.IdPerson == user.IdPerson);
        if (employee == null)
            throw new TaskCanceledException("El empleado no existe");
        var employeeDetail = employee.Include(e => e.EmployeesIncomes).FirstOrDefault();
        if(employeeDetail == null)
            throw new TaskCanceledException("El empleado no existe");
        if (employeeDetail.EmployeesIncomes.Any())
        {
            var lastIncome = employeeDetail.EmployeesIncomes.LastOrDefault();
            if (lastIncome != null)
            {
                if (lastIncome!.DepartureDate == null)
                {
                    lastIncome.DepartureDate = DateTime.Now;
                }
                else
                {
                    throw new TaskCanceledException("No hay ingresos registrados");
                }
                bool result = await _employeeRepository.Edit(employeeDetail);
                return result;
            }
            return false;
        }
        else
        {
            throw new TaskCanceledException("No hay ingresos registrados");
        }
    }

    public async Task<CalculateHoursWorkedDto> CalculateHoursWorked(int employeeId)
    {
        CalculateHoursWorkedDto info = new CalculateHoursWorkedDto();
        int lastPaymentIncomeId = 0;
        var employee = await _employeeRepository.Consult(e => e.State && e.IdEmployee == employeeId);
        if (employee.FirstOrDefault() == null)
            throw new TaskCanceledException("El empleado no existe");
        var employeeDetail = employee.Include(e => e.EmployeesIncomes).FirstOrDefault();
        var lastPayroll = _expenseRepository.Consult(e => e.State && e.IdEmployee == employeeId)
                                                            .Result.Include(e => e.IdTypeExpenseNavigation)
                                                            .OrderBy(e => e.IdEmployee)
                                                            .LastOrDefault();
        if (lastPayroll != null)
        {
            lastPaymentIncomeId = lastPayroll.LastIncomeId.Value;
        }
        if (employeeDetail.EmployeesIncomes.Any())
        {
            var hours = employeeDetail.EmployeesIncomes.Where(ei => ei.IdEmployeeIncome > lastPaymentIncomeId && ei.DepartureDate != null)
                                                             .Select(ei => (ei.IncomeDate - ei.DepartureDate).Value.TotalHours).Sum();
            info.HoursWorked = Math.Abs(hours);
            var lastIncome = employeeDetail.EmployeesIncomes.LastOrDefault(ei => ei.DepartureDate != null);
            if (lastIncome != null)
            {
                info.LastIncomeId = lastIncome.IdEmployeeIncome;
            }
        }
        return info;
    }
}