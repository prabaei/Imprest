using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imprest.Data
{
    public  class Dbbase : IDataConnection
    {
        public static SqlConnection sqlobject= new SqlConnection() ;
        //private connectionString;
         public Dbbase(string dbconnection)
        {
            try
            {
                sqlobject = new SqlConnection(ConfigurationManager.ConnectionStrings[dbconnection].ConnectionString);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public  void closeConnection()
        {
            if (sqlobject.State == System.Data.ConnectionState.Open)
                sqlobject.Close();
        }

        public  void openConnection()
        {
            if (sqlobject.State == System.Data.ConnectionState.Closed)
            sqlobject.Open();
        }
     
       
    }
}
