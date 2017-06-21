using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imprest.Data.Models.Facct
{
    public class VoucherDetails
    {
        public string voucherNo { get; set; }
        public decimal Amount { get; set; }
        public string NPRNO { get; set; }
        public string ICCNO { get; set; }
        public string chequno { get; set; }
    
        public string comno { get; set; }
        
        public string InstituteId { get; set; }
        public string coorname { get; set; }

        public string title { get; set; }

        public int projectType { get; set; }
    }
}
