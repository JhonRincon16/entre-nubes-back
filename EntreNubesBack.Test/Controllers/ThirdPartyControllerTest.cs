using AutoMapper;
using EntreNubesBack.API.Controllers;
using EntreNubesBack.API.Util;
using EntreNubesBack.BLL.Services;
using EntreNubesBack.DAL.Repositories;
using EntreNubesBack.DTO;
using EntreNubesBack.Models;
using EntreNubesBack.Utility;
using Microsoft.AspNetCore.Mvc;

namespace EntreNubesBack.Test.Controllers;

public class ThirdPartyControllerTest : IClassFixture<SharedDatabaseFixture>
{
    private readonly IMapper _mapper;
    private SharedDatabaseFixture Fixture
    {
        get;
    }

    public ThirdPartyControllerTest(SharedDatabaseFixture fixture)
    {
        Fixture = fixture;
        var configuration = new MapperConfiguration(config =>
        {
            config.AddProfile<AutoMapperProfile>();
        });

        _mapper = configuration.CreateMapper();
    }
    
    [Fact]
    public void CreateThirdParty_RepeatedData()
    {
        Thread.Sleep(1000);
        using (var context = Fixture.CreateContext())
        {
            var request = new ThirdPartyDto()
            {
                IdThirdParty = 0,
                BusinessName = "CocaCola",
                CompanyName = "Pepsi-Distribuidora",
                Address = "Av oriental # 29-60",
                Phone = "13123",
                Category = "Proveedor",
                ProductServiceName = "Licor"
            };

            var personRepository = new GenericRepository<Person>(context);
            var thirdPartyRepository = new ThirdPartyRepository(context);
            var thirdPartyService = new ThirdPartyService(thirdPartyRepository, _mapper, personRepository);
            var thirdPartyController = new ThirdPartyController(thirdPartyService); ;
            var result = thirdPartyController.CreateThirdParty(request).Result;
            OkObjectResult okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var response = ((Response<ThirdPartyDto>)(okResult.Value));
            Assert.False(response.Status);
        }
    }
    
    [Fact]
    public void CreateThirdParty_SavesCorrectData()
    {
        Thread.Sleep(1000);
        using (var context = Fixture.CreateContext())
        {
            var request = new ThirdPartyDto()
            {
                IdThirdParty = 0,
                BusinessName = "CocaCola",
                CompanyName = "Femsa-Distribuidora",
                Address = "Av oriental # 29-60",
                Phone = "13123",
                Category = "Proveedor",
                ProductServiceName = "Licor"
            };

            var personRepository = new GenericRepository<Person>(context);
            var thirdPartyRepository = new ThirdPartyRepository(context);
            var thirdPartyService = new ThirdPartyService(thirdPartyRepository, _mapper, personRepository);
            var thirdPartyController = new ThirdPartyController(thirdPartyService); ;
            var result = thirdPartyController.CreateThirdParty(request).Result;
            OkObjectResult okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var response = ((Response<ThirdPartyDto>)(okResult.Value)).Value;
            Assert.Equal("CocaCola", response.BusinessName);
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
                var request = new ThirdPartyDto()
                {
                    IdThirdParty = 1,
                    BusinessName = "Pepsi Edited",
                    Nit = "12324d",
                    CompanyName = "Distribuidor edited",
                    Address = "Av oriental # 29-60",
                    Phone = "13123",
                    Category = "Proveedor",
                    ProductServiceName = "Licor"
                };
                var personRepository = new GenericRepository<Person>(context);
                var thirdPartyRepository = new ThirdPartyRepository(context);
                var thirdPartyService = new ThirdPartyService(thirdPartyRepository, _mapper, personRepository);
                var thirdPartyController = new ThirdPartyController(thirdPartyService); ;
                var result = thirdPartyController.EditThirdParty(request).Result;
                OkObjectResult okResult = result as OkObjectResult;
                Assert.NotNull(okResult);
                bool resultEdit = ((Response<bool>)(okResult.Value)).Value;
                Assert.True(resultEdit);
            }
        }
    }
}