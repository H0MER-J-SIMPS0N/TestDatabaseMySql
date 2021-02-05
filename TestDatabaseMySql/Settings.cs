using Microsoft.Extensions.Configuration;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TestDatabaseMySql
{
    public class Settings
    {
        private readonly Logger _logger;
        public Settings(Logger logger)
        {
            _logger = logger;
        }

        public string GetConnectionString()
        {
            try
            {
                var builder = new ConfigurationBuilder();
                builder.SetBasePath(Directory.GetCurrentDirectory());
                builder.AddJsonFile("appsettings.json");
                var config = builder.Build();
                return config.GetConnectionString("DbConnection");
            }
            catch (Exception ex)
            {
                _logger.Fatal($"Не удалось получить ConnectionString из файла appsettings.json по причине:{Environment.NewLine}{ex}");
                throw new Exception();
            }
        }
    }
}
