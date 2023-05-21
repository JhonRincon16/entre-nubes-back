using EntreNubesBack.DAL.DBContext;
using EntreNubesBack.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntreNubesBack.DTO;
using Action = EntreNubesBack.Models.Action;

namespace EntreNubesBack.Test
{
    public class SharedDatabaseFixture : IDisposable
    {
        private static readonly object _lock = new object();
        private static bool _databaseInitialized;

        public SharedDatabaseFixture()
        {
            Connection = new MySqlConnection("server=localhost; user=root; password=root; database=entrenubestest;");
            Seed();
            Connection.Open();
        }
        public DbConnection Connection
        {
            get;
        }
        public EntrenubesContext CreateContext(DbTransaction? transaction = null)
        {
            var context = new EntrenubesContext(new DbContextOptionsBuilder<EntrenubesContext>().UseMySQL(Connection).Options);
            if (transaction != null)
            {
                context.Database.UseTransaction(transaction);
            }
            return context;
        }
        private void Seed()
        {
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    using (var context = CreateContext())
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();
                        SeedData(context);
                    }
                    _databaseInitialized = true;
                }
            }
        }

        //En este metodo se insertan los datos iniciales, en este caso se agrega 2 roles y 2 usuarios como datos iniciales
        private void SeedData(EntrenubesContext context)
        {
            var action1 = new Action()
            {
                IdAction = 1,
                ActionName = "Administrar roles"
            };

            context.Add(action1);

            var rol1 = new Role()
            {
                IdRol = 1,
                RolName = "AdministradorTest",  
                State = true
            };
            rol1.IdActions.Add(action1);
            context.Add(rol1);

            var rol2 = new Role()
            {
                IdRol = 2,
                RolName = "CajeroTest",
                State = true
            };
            context.Add(rol2);

            var person = new Person()
            {
                IdPerson = 1,
                DocumentType = "CC",
                DocumentNumber = "1234567890",
                PersonName = "Jhon Test"
            };
            context.Add(person);
            
            var user = new User()
            {
                IdUser = 1,
                Email = "emailTest@gmail.com",
                Password = "sdadefasdasd",
                State = true,
                IdPerson = 1,
                IdRol = 2
            };
            context.Add(user);
            
            var thirdParty = new ThirdParty()
            {
                IdThirdParty = 1,
                BusinessName = "Pepsi",
                CompanyName = "Pepsi-Distribuidora",
                Address = "Av norte #38-48",
                Phone = "67787556",
                Category = "Proveedor",
                ProductServiceName = "Otras bebidas",
                State = true
            };
            context.Add(thirdParty);
            
            var thirdParty2 = new ThirdParty()
            {
                IdThirdParty = 2,
                BusinessName = "Licorera de boyaca",
                CompanyName = "Distribuidor norte",
                Address = "Calle 50 #45-67",
                Phone = "7876878",
                Category = "Proveedor",
                ProductServiceName = "Licores",
                State = true
            };
            context.Add(thirdParty2);
            context.SaveChanges();
        }
        public void Dispose() => Connection.Dispose();
    }
}
