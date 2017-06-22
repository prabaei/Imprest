using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imprest.Data.Imprest;
using ModelClass = Imprest.Models.Imprest;

namespace Imprest.Service.Dbservice.Imprest
{
    public class ImprestService : IImprestService
    {
         public ImprestEF imprestdb;
        public ImprestService()
        {
            imprestdb = new ImprestEF();
        }

        public ImprestEF getInstance()
        {
            return imprestdb;
        }
        public IEnumerable<ModelClass.AccountMaster> getAccountDetail(string instid)
        {
            return imprestdb.AccountMaster.Select(m=>new ModelClass.AccountMaster() {
                AccountNo=m.AccountNo,
                Amount=m.Amount,
                CardNo=m.CardNo,
                CustId=m.CustId,
                Email=m.Email,
                InstId=m.InstId,
                Name=m.Name
            } ).Where(m => m.InstId == instid.Trim()).ToList();
        }
    }
}
