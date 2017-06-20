using Imprest.Data.Facct;
using Imprest.Models.Facct;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Imprest.Controllers
{
    public class VoucherEntryController : Controller
    {
        readonly IFacctDb _facct;
        public VoucherEntryController(IFacctDb db)
        {
            _facct = db;
            
        }
        // GET: VoucherEntry
        public ActionResult Index()
        {
           
            return View();
        }

        #region AutocompleteRequest
        public JsonResult getVoucherList(string voucherNO)
        {
           var voucherLIst= getvoucherFacct(voucherNO);
            return Json( voucherLIst , JsonRequestBehavior.AllowGet);
            
        }


        #endregion

        public List<Voucher> getvoucherFacct(string voucherNO)
        {
            DateTime t = DateTime.Now;
            int year = t.Year;
            int searchfrom = year - 2;
            //getTableName(searchfrom);
            bool table_exist = _facct.IsTableExist(getTableName(searchfrom));
            List<Voucher> vouchlist = new List<Voucher>();
            while (table_exist)
            {
                string command = string.Format("SELECT TOP 7 COUNT(VRNO) AS Vcount,VRNO FROM {0} GROUP BY VRNO HAVING VRNO LIKE '%{1}%'", getTableName(searchfrom), voucherNO);

                SqlDataReader allVoucher = _facct.getVoucherNo(command);
                while (allVoucher.Read())
                {
                    Voucher vouch = new Voucher();
                    vouch.voucherNo = Convert.ToString(allVoucher["VRNO"] ?? null);
                    vouch.count = Convert.ToInt32(allVoucher["Vcount"]);
                    vouchlist.Add(vouch);
                }

                table_exist = _facct.IsTableExist(getTableName(searchfrom+1));
                
                //vouchlist.Clear();
                // var data= vouchlist.GroupBy(m => m.voucherNo,(key,g)=>new Voucher() {count=g.Count(),voucherNo=key }).ToList();
            }
            return vouchlist.ToList();
            
        }
        private static string getTableName(int searchfrom)
        {
            int searchshort = searchfrom % 1000;
            int nextshort = (searchfrom + 1) % 1000;
            string searshortstr = "00" + Convert.ToString(searchshort);
            string nextshortstr = "00" + Convert.ToString(nextshort);
            searshortstr = searshortstr.Substring(searshortstr.Length - 2);
            nextshortstr = nextshortstr.Substring(nextshortstr.Length - 2);
            return string.Format("VOU{0}{1}", searshortstr.Trim(), nextshortstr.Trim());
        }
    }
}