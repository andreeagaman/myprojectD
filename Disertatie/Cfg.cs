using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Disertatie
{
    public static class Cfg
    {
        private static readonly string _dbConnectionString;
             
        static Cfg() {
            _dbConnectionString = WebConfigurationManager.ConnectionStrings["Disertatie"].ToString();
        }

        public static string DbConnectionString
        {
            get { return _dbConnectionString; }
        }
      
    }
}