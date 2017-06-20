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
        [HttpPost]
        public JsonResult getVoucherList(string voucherNO)
        {
           var voucherLIst= getvoucherFacct(voucherNO);
            return Json( voucherLIst , JsonRequestBehavior.AllowGet);
            
        }
        [HttpPost]
        public JsonResult getVoucherDetails(string voucherNO)
        {
            var voucherList = getvoucherDetailList(voucherNO);
            return null;
        }

        private object getvoucherDetailList(string voucherNO)
        {
            DateTime t = DateTime.Now;
            //int top = 7;
            int year = t.Year;
            int searchfrom = year - 2;
            //getTableName(searchfrom);
            bool table_exist = _facct.IsTableExist(getTableName(searchfrom));
            List<Voucher> vouchlist = new List<Voucher>();
            while (table_exist)
            {
            string command = string.Format("SELECT [VRNO],[AMOUNT],[NPRNO],[ICCNO],[COMNO],[CQNO] FROM {0}",tablename);


                SqlDataReader allVoucher = _facct.getFromFacct(command);
                while (allVoucher.Read())
                {
                    Voucher vouch = new Voucher();
                    vouch.voucherNo = Convert.ToString(allVoucher["VRNO"] ?? null);
                    vouch.count = Convert.ToInt32(allVoucher["Vcount"]);
                    vouchlist.Add(vouch);
                }
                searchfrom++;
                vouchlist = vouchlist.GroupBy(m => m.voucherNo, (key, g) => new Voucher() { count = g.Sum(m => m.count), voucherNo = key }).ToList();
                table_exist = _facct.IsTableExist(getTableName(searchfrom));
                if (table_exist)
                {
                    table_exist = vouchlist.Count < 7 ? true : false;
                    if (table_exist)
                    {
                        if (vouchlist.Count < 7)
                            top = 7 - vouchlist.Count;

                    }
                }
                //vouchlist.Clear();
                // var data= vouchlist.GroupBy(m => m.voucherNo,(key,g)=>new Voucher() {count=g.Count(),voucherNo=key }).ToList();
            }
            return vouchlist.Take(7).ToList();
        }

        #endregion

        public List<Voucher> getvoucherFacct(string voucherNO)
        {
            DateTime t = DateTime.Now;
            int top = 7;
            int year = t.Year;
            int searchfrom = year - 2;
            //getTableName(searchfrom);
            bool table_exist = _facct.IsTableExist(getTableName(searchfrom));
            List<Voucher> vouchlist = new List<Voucher>();
            while (table_exist)
            {
                string command = string.Format("SELECT TOP {2} COUNT(VRNO) AS Vcount,VRNO FROM {0} GROUP BY VRNO HAVING VRNO LIKE '%{1}%'", getTableName(searchfrom), voucherNO,top);

                SqlDataReader allVoucher = _facct.getFromFacct(command);
                while (allVoucher.Read())
                {
                    Voucher vouch = new Voucher();
                    vouch.voucherNo = Convert.ToString(allVoucher["VRNO"] ?? null);
                    vouch.count = Convert.ToInt32(allVoucher["Vcount"]);
                    vouchlist.Add(vouch);
                }
                searchfrom++;
                vouchlist=vouchlist.GroupBy(m => m.voucherNo, (key, g) => new Voucher() { count = g.Sum(m=>m.count), voucherNo = key }).ToList();
                table_exist = _facct.IsTableExist(getTableName(searchfrom));
                if (table_exist)
                {
                    table_exist = vouchlist.Count < 7 ? true : false;
                    if (table_exist)
                    {
                        if(vouchlist.Count<7)
                        top = 7 - vouchlist.Count;

                    }
                }
                //vouchlist.Clear();
                // var data= vouchlist.GroupBy(m => m.voucherNo,(key,g)=>new Voucher() {count=g.Count(),voucherNo=key }).ToList();
            }
            return vouchlist.Take(7).ToList();
            
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