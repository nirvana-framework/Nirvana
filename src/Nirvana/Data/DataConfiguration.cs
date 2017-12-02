using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Nirvana.Data
{
    public class DataConfiguration
    {
        private IConfigurationRoot _configurationRoot;

        public DataConfiguration(string directory,string fileName)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(directory)
                .AddJsonFile(fileName, optional: false, reloadOnChange: true);

            _configurationRoot = builder.Build();
        }


        public DataConfiguration():this( Directory.GetCurrentDirectory(),"nirvana.datasettings.json")
        {
            
        }

        public string GetConnectionString(string key)
        {
           return  _configurationRoot.GetChildren().First(x => x.Key == key).Value;
        }

    }
}
