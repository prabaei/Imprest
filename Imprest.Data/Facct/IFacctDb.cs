using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imprest.Data.Facct
{
    public interface IFacctDb
    {
        void openConnection();
        bool IsTableExist(string tablename);

        SqlDataReader getFromFacct(string command);
    }
}
