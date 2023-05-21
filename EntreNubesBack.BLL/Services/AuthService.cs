using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using AutoMapper;
using EntreNubesBack.BLL.Services.Contracts;
using EntreNubesBack.DAL.Repositories.Contracts;
using EntreNubesBack.DTO;
using EntreNubesBack.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace EntreNubesBack.BLL.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IGenericRepository<User> _userRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper;

    public AuthService(IConfiguration configuration, 
                        IGenericRepository<User> userRepository, 
                        IMapper mapper, 
                        IEmployeeRepository employeeRepository)
    {
        _configuration = configuration;
        _userRepository = userRepository;
        _mapper = mapper;
        _employeeRepository = employeeRepository;
    }

    public async Task<SessionDto> ValidateCredentials(string email, string password)
    {
        try
        {
            SessionDto session = new SessionDto();
            var userQuery = await _userRepository.Consult(u => u.Email == email && u.Password == password && u.State);

            if (userQuery.FirstOrDefault() == null)
                throw new TaskCanceledException("Credenciales incorrectas");

            User user = userQuery.Include(rol => rol.IdRolNavigation)
                                    .ThenInclude(r => r.IdActions).First();
            var employeeAux = await _employeeRepository.Consult(e => e.State && e.IdPerson == user.IdPerson);
            var employee = employeeAux.Include(e => e.EmployeesIncomes).FirstOrDefault();
            if (employee != null)
            {
                if (employee.SalaryType == "Hora")
                {
                    session.ShowRegisterEntrance = true;
                    session.RegisterEntrance = await ValidateEntrances(employee);
                }
            }
            var token = GenerateToken(user);
            session.token = token;
            return session;
        }
        catch
        {
            throw;
        }
    }

    private string GenerateToken(User user)
    {
        var keyBytes = Encoding.ASCII.GetBytes(_configuration.GetSection("JwtKey").Value);
        var rol = user.IdRolNavigation == null ? "" : user.IdRolNavigation.RolName;
        var actions = GetActions(user.IdRolNavigation);
        var claims = new List<Claim>()
        {
            new Claim("id", user.IdUser.ToString()),
            new Claim("email", user.Email),
            new Claim("rol", rol),
            new Claim("actions", actions)
        };
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(180),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);
        string token = tokenHandler.WriteToken(tokenConfig);
        return token;
    }

    private string GetActions(Role rol)
    {
        string actions = "";
        foreach (var action in rol.IdActions)
        {
            actions += action.ActionName + ",";
        }
        return actions;
    }

    private async Task<bool> ValidateEntrances(Employee employee)
    {
        if (employee.EmployeesIncomes.Count() > 0)
        {
            var lastIncome = employee.EmployeesIncomes.LastOrDefault();
            return lastIncome.DepartureDate != null;
        }
        else
        {
            return true;
        }
    }
}