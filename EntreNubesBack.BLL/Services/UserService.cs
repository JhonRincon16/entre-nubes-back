using AutoMapper;
using EntreNubesBack.BLL.Services.Contracts;
using EntreNubesBack.DAL.Repositories.Contracts;
using EntreNubesBack.DTO;
using EntreNubesBack.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace EntreNubesBack.BLL.Services;

public class UserService : IUserService
{

    private readonly IGenericRepository<User> _userRepository;
    private readonly IGenericRepository<Person> _personRepository;
    private readonly IMapper _mapper;

    public UserService(IGenericRepository<User> userRepository, IGenericRepository<Person> personRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _personRepository = personRepository;
        _mapper = mapper;
    }

    public async Task<List<UserDto>> List()
    {
        try
        {
            var userQuery = await _userRepository.ConsultAsNoTacking();
            var usersList = userQuery.Include(r => r.IdRolNavigation)
                                                               .Include(p => p.IdPersonNavigation);
            return _mapper.Map<List<UserDto>>(usersList);
        }
        catch
        {
            throw;
        }
    }

    public async Task<UserDto> Create(CreateUserDto user)
    {
        try
        {
            User userToCreate = new User()
            {
                Email = user.Email,
                IdRol = user.IdRol,
                Password = user.Password,
                State = true
            };
            
            var person = await _personRepository.Get(p => p.PersonName == user.UserName);
            if (person == null && user.IdPerson == 0)
            {
                person = await _personRepository.Create(new Person(){PersonName = user.UserName});
                userToCreate.IdPerson = person.IdPerson;
            }
            else
            {
                person.PersonName = user.UserName;
                await _personRepository.Edit(person);
                userToCreate.IdPerson = user.IdPerson;
            }

            var actualUser = await _userRepository.Get(u => u.Email == user.Email);
            if (actualUser != null)
                throw new TaskCanceledException("Ya existe un usuario con el email ingresado.");
            
            var newUser = await _userRepository.Create(userToCreate);

            if (newUser.IdUser == 0) 
                throw new TaskCanceledException("El usuario no se pudo crear");

            var query = await _userRepository.Consult(u => u.IdUser == newUser.IdUser);
            newUser = query.Include(r => r.IdRolNavigation)
                            .Include(p => p.IdPersonNavigation).First();
            return _mapper.Map<UserDto>(newUser);
        }
        catch
        {
            throw;
        }
    }

    public async Task<bool> Edit(EditUserDto user)
    {
        try
        {
            var actualUser = await _userRepository.Get(u => u.IdUser == user.IdUser);

            if (actualUser == null)
                throw new TaskCanceledException("El usuario no existe");

            Person person = null;
            if (user.IdPerson == 0)
            {
                person = await _personRepository.Create(new Person() { PersonName = user.UserName });
            }
            else
            {
                person = await _personRepository.Get(p => p.IdPerson == user.IdPerson);
                person.PersonName = user.UserName;
                bool status = await _personRepository.Edit(person);
                if (!status)
                    throw new TaskCanceledException("No se pudo editar el nombre de usuario");
            }

            actualUser.IdPerson = person.IdPerson;
            actualUser.Email = user.Email;
            actualUser.IdRol = user.IdRol;
            actualUser.Password = user.Password;

            bool response = await _userRepository.Edit(actualUser);

            if (!response)
                throw new TaskCanceledException("No se pudo editar al usuario");

            return response;
        }
        catch
        {
            throw;
        }
    }

    public async Task<bool> ChangeStatus(int userId)
    {
        try
        {
            User user = await _userRepository.Get(u => u.IdUser == userId);
            if (user == null)
                throw new TaskCanceledException("No existe el usuario");
            user.State = !user.State;
            bool result = await _userRepository.Edit(user);
            return result;
        }
        catch
        {
            throw;
        }
    }
}