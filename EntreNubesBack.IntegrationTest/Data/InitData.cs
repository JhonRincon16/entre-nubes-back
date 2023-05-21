using EntreNubesBack.DAL.DBContext;
using EntreNubesBack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Action = EntreNubesBack.Models.Action;

namespace EntreNubesBack.IntegrationTest.Data
{
    public class InitData
    {
        public static void InitDataInMemory(EntrenubesContext context)
        {
            if (context.Database.ProviderName
                      != "Microsoft.EntityFrameworkCore.InMemory")
                return;
            context.Database.EnsureCreated();
            var action1 = new Action()
            {
                IdAction = 1,
                ActionName = "Administrar roles"
            };

            context.Actions.Add(action1);

            var rol1 = new Role()
            {
                IdRol = 1,
                RolName = "AdministradorTest",
            };
            rol1.IdActions.Add(action1);
            context.Roles.Add(rol1);

            var rol2 = new Role()
            {
                IdRol = 2,
                RolName = "CajeroTest",
            };
            context.Roles.Add(rol2);
        }
    }
}
