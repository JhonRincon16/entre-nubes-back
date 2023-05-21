using EntreNubesBack.DAL.DBContext;
using EntreNubesBack.IntegrationTest.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntreNubesBack.IntegrationTest.Context
{
    public class EntreNubesContextMemory
    {

        public EntrenubesContext GetContext()
        {
            var options = new DbContextOptionsBuilder<EntrenubesContext>()
                .ConfigureWarnings
                (x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .UseInMemoryDatabase(databaseName: "entrenubes")
                .Options;

            var context = new EntrenubesContext(options);
            InitData.InitDataInMemory(context);
            return context;
        }
    }
}
