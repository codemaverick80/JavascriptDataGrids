using System.IO;
using Microsoft.Extensions.Configuration;

namespace JsDataGrids.DataAccess
{
   public class EnvironmentManager
    {
        public enum Database
        {
            DatabaseConnection=0,
            AnotherDatabaseConnection=1
        }

        public static string GetConnectionString(Database database)
        {
            var configuration = GetConfiguration();
            //string connectionString =
            //    configuration.GetSection("ConnectionStrings").GetSection("DevDatabaseConnection").Value;

            //string connectionString =
            //    configuration.GetSection("ConnectionStrings:DevDatabaseConnection").Value;
        

          string connectionString =
                configuration.GetSection("ConnectionStrings").GetSection(ConnectionStringKey(database)).Value;


          return connectionString;
        }

        public static IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();

        }

        public static string ConnectionStringKey(Database database)
        {
            return database.ToString();
        }

    }
}
