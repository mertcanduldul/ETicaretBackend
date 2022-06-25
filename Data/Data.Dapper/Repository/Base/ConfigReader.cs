using System;
using System.Configuration;
using Microsoft.Extensions.Configuration;
namespace Dapper.Repository.Base
{
    public class ConfigReader
    {
        public static string GetAppSettingsValue(string key)
        {
            return new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")[key];
        }

        public static string GetConnectionStringsValue(string key)
        {
            return new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")[key];
        }
    }
}

