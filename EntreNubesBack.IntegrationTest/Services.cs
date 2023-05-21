using EntreNubesBack.DAL.DBContext;
using EntreNubesBack.IntegrationTest.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntreNubesBack.IntegrationTest
{
    public class Services
    {

        private static HttpClient _httpClient;
        public static EntrenubesContext entrenubesContext;

        public static void Init()
        {
            var builder = new WebHostBuilder()
                .UseEnvironment("IntegrationTesting")
                .ConfigureAppConfiguration((c, config) =>
                {
                    config.SetBasePath(Path.Combine(
                        Directory.GetCurrentDirectory(), "..", "..", "..", "..", "EntreNubesBack.API"));
                    config.AddJsonFile("appsetings.json");
                });

            var server = new TestServer(builder);
            entrenubesContext = server.Host.Services.GetService(typeof(EntrenubesContext)) as EntrenubesContext;
            InitData.InitDataInMemory(entrenubesContext);
            _httpClient = server.CreateClient();

        }
    }
}
