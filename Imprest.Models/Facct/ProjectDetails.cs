using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imprest.Models.Facct
{
    public class ProjectDetails
    {
        public string InstituteId { get; set; }
        public string coorname { get; set; }

        public string title { get; set; }

        public int projectType { get; set; }
        public string projectNo { get; set; }

        public string voucherNo { get; set; }
        public decimal Amount { get; set; }

        public string chequno { get; set; }

        public string comno { get; set; }
    }
}
