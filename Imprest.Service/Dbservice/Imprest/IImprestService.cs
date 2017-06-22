using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imprest.Data.Imprest;
using ModelClass = Imprest.Models.Imprest;

namespace Imprest.Service.Dbservice.Imprest
{
    public interface IImprestService 
    {
        IEnumerable<ModelClass.AccountMaster> getAccountDetail(string Instid);
        ImprestEF getInstance();
    }
}
