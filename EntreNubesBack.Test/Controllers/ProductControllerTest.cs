using AutoMapper;
using EntreNubesBack.Utility;

namespace EntreNubesBack.Test.Controllers;

public class ProductControllerTest : IClassFixture<SharedDatabaseFixture>
{
    private readonly IMapper _mapper;
    private SharedDatabaseFixture Fixture
    {
        get;
    }

    public ProductControllerTest(SharedDatabaseFixture fixture)
    {
        Fixture = fixture;
        var configuration = new MapperConfiguration(config =>
        {
            config.AddProfile<AutoMapperProfile>();
        });

        _mapper = configuration.CreateMapper();
    }

    
}