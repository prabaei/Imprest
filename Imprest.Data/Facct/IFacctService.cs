using Imprest.Data.Models.Facct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imprest.Data.Facct
{
    public interface IFacctService
    {

        IEnumerable<Voucher> getvoucherFacct(string voucherNO);

        IEnumerable<VoucherDetails> getvoucherDetailList(string voucherNO);

        IEnumerable<ProjectDetails> getProjectDetails(IEnumerable<VoucherDetails> vouchDetails);
        string getTableName(int searchfrom);
    }
}
