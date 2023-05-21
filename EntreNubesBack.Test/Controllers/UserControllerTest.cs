using AutoMapper;
using EntreNubesBack.API.Controllers;
using EntreNubesBack.API.Util;
using EntreNubesBack.BLL.Services;
using EntreNubesBack.DAL.Repositories;
using EntreNubesBack.DTO;
using EntreNubesBack.Models;
using EntreNubesBack.Utility;
using Microsoft.AspNetCore.Mvc;
using Action = System.Action;

namespace EntreNubesBack.Test.Controllers;

[Collection("Sequential")]
public class UserControllerTest : IClassFixture<SharedDatabaseFixture>
{
    private readonly IMapper _mapper;
    private SharedDatabaseFixture Fixture
    {
        get;
    }

    public UserControllerTest(SharedDatabaseFixture fixture)
    {
        Fixture = fixture;
        var configuration = new MapperConfiguration(config =>
        {
            config.AddProfile<AutoMapperProfile>();
        });

        _mapper = configuration.CreateMapper();
    }
    
    [Fact]
    public void CreateUser_SavesCorrectData()
    {
        Thread.Sleep(1000);
        using (var context = Fixture.CreateContext())
        {
            var request = new CreateUserDto()
            {
                UserName = "UserTest",
                Email = "userTest@gmail.com",
                Password = "eded82fead402293e0ad6f774aa4cb1d77245c2bc62f3b4c4f3dfcaacb336fe2", //passwordTest
                IdRol = 1,
                IdPerson = 0
            };
            var userRepository = new GenericRepository<User>(context);
            var personRepository = new GenericRepository<Person>(context);
            var userService = new UserService(userRepository, personRepository, _mapper);
            var userController = new UserController(userService);
            var result = userController.CreateUser(request).Result;
            OkObjectResult okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var response = ((Response<UserDto>)(okResult.Value)).Value;
            Assert.Equal("UserTest", response.Person.PersonName);
        }
    }
    
    [Fact]
    public void CreateUser_RepeatEmailError()
    {
        Thread.Sleep(1000);
        using (var context = Fixture.CreateContext())
        {
            var request = new CreateUserDto()
            {
                UserName = "UserTest",
                Email = "emailTest@gmail.com",
                Password = "eded82fead402293e0ad6f774aa4cb1d77245c2bc62f3b4c4f3dfcaacb336fe2", //passwordTest
                IdRol = 1,
                IdPerson = 0
            };
            var userRepository = new GenericRepository<User>(context);
            var personRepository = new GenericRepository<Person>(context);
            var userService = new UserService(userRepository, personRepository, _mapper);
            var userController = new UserController(userService);
            var result = userController.CreateUser(request).Result;
            OkObjectResult okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var response = ((Response<UserDto>)(okResult.Value));
            Assert.False(response.Status);
        }
    }
    
    [Fact]
    public void EditUser_EditCorrectData()
    {
        Thread.Sleep(1000);
        using (var transaction = Fixture.Connection.BeginTransaction())
        {
            using (var context = Fixture.CreateContext(transaction))
            {
                var request = new EditUserDto()
                {
                    IdUser = 1,
                    UserName = "UserTest",
                    Email = "emailTest@gmail.com",
                    Password = "eded82fead402293e0ad6f774aa4cb1d77245c2bc62f3b4c4f3dfcaacb336fe2", //passwordTest
                    IdRol = 1,
                    IdPerson = 0
                };
                var userRepository = new GenericRepository<User>(context);
                var personRepository = new GenericRepository<Person>(context);
                var userService = new UserService(userRepository, personRepository, _mapper);
                var userController = new UserController(userService);
                var result = userController.EditUser(request).Result;
                OkObjectResult okResult = result as OkObjectResult;
                Assert.NotNull(okResult);
                bool resultEdit = ((Response<bool>)(okResult.Value)).Value;
                Assert.True(resultEdit);
            }
        }
    }
    
    [Fact]
    public void DeleteUser_CorrectDelete()
    {
        Thread.Sleep(1000);
        using (var transaction = Fixture.Connection.BeginTransaction())
        {
            using (var context = Fixture.CreateContext(transaction))
            {
                var userRepository = new GenericRepository<User>(context);
                var personRepository = new GenericRepository<Person>(context);
                var userService = new UserService(userRepository, personRepository, _mapper);
                var userController = new UserController(userService);
                var result = userController.DeactivateUser(1).Result;
                OkObjectResult okResult = result as OkObjectResult;
                Assert.NotNull(okResult);
                bool resultDelete = ((Response<bool>)(okResult.Value)).Value;
                Assert.True(resultDelete);
            }
        }
    }
}