using AutoMapper;
using EntreNubesBack.API.Controllers;
using EntreNubesBack.API.Util;
using EntreNubesBack.BLL.Services;
using EntreNubesBack.BLL.Services.Contracts;
using EntreNubesBack.DAL.Repositories;
using EntreNubesBack.DAL.Repositories.Contracts;
using EntreNubesBack.DTO;
using EntreNubesBack.Models;
using EntreNubesBack.Utility;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Action = EntreNubesBack.Models.Action;

namespace EntreNubesBack.Test.Controllers
{
    [Collection("Sequential")]
    public class RolControllerTest : IClassFixture<SharedDatabaseFixture>
    {
        private readonly IMapper _mapper;
        private SharedDatabaseFixture Fixture
        {
            get;
        }

        public RolControllerTest(SharedDatabaseFixture fixture)
        {
            Fixture = fixture;
            var configuration = new MapperConfiguration(config =>
            {
                config.AddProfile<AutoMapperProfile>();
            });

            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public void CreateRol_SavesCorrectData()
        {
            Thread.Sleep(5);
            using (var context = Fixture.CreateContext())
            {
                var request = new CreateRolDto()
                {
                    Name = "RolTest",
                    Actions = new List<int>()
                };
                var actionRepository = new GenericRepository<Action>(context);
                var rolRepository = new RolRepository(context, actionRepository);
                var service = new RolService(rolRepository, _mapper, actionRepository);
                var rolController = new RolController(service);
                var result = rolController.CreateRol(request).Result;
                OkObjectResult okResult = result as OkObjectResult;
                Assert.NotNull(okResult);
                var newRolName = ((Response<RolDto>)(okResult.Value)).Value.RolName;
                Assert.Equal("RolTest", newRolName);
            }
        }

        [Fact]
        public void EditRol_EditCorrectData()
        {
            using (var transaction = Fixture.Connection.BeginTransaction())
            {
                using (var context = Fixture.CreateContext(transaction))
                {
                    var request = new EditRolDto()
                    {
                        Name = "TestEdited",
                        Actions = new List<int>(),
                        IdRol = 1
                    };
                    var actionRepository = new GenericRepository<Action>(context);
                    var rolRepository = new RolRepository(context, actionRepository);
                    var service = new RolService(rolRepository, _mapper, actionRepository);
                    var rolController = new RolController(service);
                    var result = rolController.EditRol(request).Result;
                    OkObjectResult okResult = result as OkObjectResult;
                    Assert.NotNull(okResult);
                    bool resultEdit = ((Response<bool>)(okResult.Value)).Value;
                    Assert.True(resultEdit);
                }
            }
        }
        
        [Fact]
        public void EditRol_NonExistentRol()
        {
            Thread.Sleep(1000);
            using (var transaction = Fixture.Connection.BeginTransaction())
            {
                using (var context = Fixture.CreateContext(transaction))
                {
                    var request = new EditRolDto()
                    {
                        Name = "CajeroTestEdited",
                        Actions = new List<int>(),
                        IdRol = 4
                    };
                    var actionRepository = new GenericRepository<Action>(context);
                    var rolRepository = new RolRepository(context, actionRepository);
                    var service = new RolService(rolRepository, _mapper, actionRepository);
                    var rolController = new RolController(service);
                    var result = rolController.EditRol(request).Result;
                    BadRequestObjectResult badResult = result as BadRequestObjectResult;
                    Assert.NotNull(badResult);
                    bool resultEdit = ((Response<bool>)(badResult.Value)).Value;
                    Assert.False(resultEdit);
                }
            }
        }
        
        [Fact]
        public void GetRoles_ReturnsAllRoles()
        {
            using (var context = Fixture.CreateContext())
            {
                var actionRepository = new GenericRepository<Action>(context);
                var rolRepository = new RolRepository(context, actionRepository);
                var service = new RolService(rolRepository, _mapper, actionRepository);
                var rolController = new RolController(service);
                var result = rolController.GetRoles().Result;
                OkObjectResult okResult = result as OkObjectResult;
                Assert.NotNull(okResult);
                var rolesCount = ((Response<List<RolDto>>)(okResult.Value)).Value.Count();
                Assert.Equal(2,rolesCount);
            }
        }
        
        [Fact]
        public void DeleteRol_SuccessStateChange()
        {
            Thread.Sleep(1000);
            using (var transaction = Fixture.Connection.BeginTransaction())
            {
                using (var context = Fixture.CreateContext(transaction))
                {
                    var actionRepository = new GenericRepository<Action>(context);
                    var rolRepository = new RolRepository(context, actionRepository);
                    var service = new RolService(rolRepository, _mapper, actionRepository);
                    var rolController = new RolController(service);
                    var result = rolController.DeleteRol(1).Result;
                    OkObjectResult okResult = result as OkObjectResult;
                    Assert.NotNull(okResult);
                    bool resultEdit = ((Response<bool>)(okResult.Value)).Value;
                    Assert.True(resultEdit);
                }
            }
        }
    }
}
