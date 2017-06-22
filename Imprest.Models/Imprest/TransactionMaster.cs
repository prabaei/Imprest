using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imprest.Models.Imprest
{
    public class TransactionMaster
    {
        public string name { get; set; }
        public decimal accountno { get; set; }
        public string institute {get;set;}
        public string projectNo { get; set; }
        public string commitno { get; set; }
        public string date { get; set; }
        public string supplier { get; set; }
        public decimal amount { get; set; }
        public string bankdate { get; set; }
        public int vouchertype { get; set; }
        public string chequedate { get; set; }
        public string narration { get; set; }
        public string remarks { get; set; }

    }
}
