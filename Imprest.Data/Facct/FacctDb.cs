using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imprest.Data.Facct
{
    public class FacctDb : Dbbase,IFacctDb
    {
        public FacctDb() : base("FACCT")
        {
            openConnection();
        }

        public SqlDataReader getVoucherNo(string command)
        {
            using (SqlCommand _cmd = new SqlCommand(command, sqlobject))
            {
                try
                {
                    SqlDataReader reader = _cmd.ExecuteReader();
                    return reader;
                }
                catch (Exception ex)
                {
                    throw ex;
                }


            }
        }

        public bool IsTableExist(string tablename)
        {
            string command = string.Format("select case when exists((select * from information_schema.tables where table_name = '{0}'))then 1 else 0 end", tablename);
            using (SqlCommand _cmd = new SqlCommand(command,sqlobject))
            {
                try
                {
                   return (int)_cmd.ExecuteScalar()==1? true:false;

                }
                catch (Exception ex) { throw ex; }
            }
        }
    }
}
