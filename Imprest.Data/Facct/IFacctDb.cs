using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imprest.Data.Facct
{
    interface IFacctDb
    {
        
        bool IsTableExist(string tablename);

        SqlDataReader getVoucherNo(string command);
    }
}
