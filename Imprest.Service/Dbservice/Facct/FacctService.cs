using Imprest.Data.Facct;
using Imprest.Models.Facct;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imprest.Service.Dbservice.Facct
{
    public class FacctService : IFacctService
    {
        IFacctDb _facct;
        public FacctService() : this(new FacctDb())
        {

        }
        public FacctService(IFacctDb db)
        {
            _facct = db;
            _facct.openConnection();
        }
        public IEnumerable<Voucher> getvoucherFacct(string voucherNO)
        {
            //_facct.openConnection();
            DateTime t = DateTime.Now;
            int top = 7;
            int year = t.Year;
            int searchfrom = year - 2;
            //getTableName(searchfrom);
            bool table_exist = _facct.IsTableExist(getTableName(searchfrom));
            List<Voucher> vouchlist = new List<Voucher>();
            while (table_exist)
            {
                string command = string.Format("SELECT TOP {2} COUNT(VRNO) AS Vcount,VRNO FROM {0} GROUP BY VRNO HAVING VRNO LIKE '%{1}%'", getTableName(searchfrom), voucherNO, top);

                SqlDataReader allVoucher = _facct.getFromFacct(command);
                while (allVoucher.Read())
                {
                    Voucher vouch = new Voucher();
                    vouch.voucherNo = Convert.ToString(allVoucher["VRNO"] ?? null);
                    vouch.count = Convert.ToInt32(allVoucher["Vcount"]);
                    vouchlist.Add(vouch);
                }
                searchfrom++;
                //vouchlist=vouchlist.GroupBy(m => m.voucherNo).Select(m=>new Voucher{ count=m.Sum(m=>m.count)}).ToList();
                vouchlist = (from list in vouchlist
                             group list by list.voucherNo into vg
                             select new Voucher
                             {
                                 voucherNo = vg.Key,
                                 count = vg.Sum(x => x.count)
                             }).ToList();
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
            return vouchlist.ToList();

        }
        public IEnumerable<VoucherDetails> getvoucherDetailList(string voucherNO)
        {
            //_facct.openConnection();
            DateTime t = DateTime.Now;
            //int top = 7;
            int year = t.Year;
            int searchfrom = year - 2;
            //getTableName(searchfrom);
            bool table_exist = _facct.IsTableExist(getTableName(searchfrom));

            List<string> tablenames = new List<string>();
            string command = string.Empty;
            while (table_exist)
            {
                command += string.Format("SELECT [VRNO],[AMOUNT],[NPRNO],[ICCNO],[COMNO],[CQNO] FROM {0} WHERE VRNO='{1}'", getTableName(searchfrom), voucherNO.Trim());

                //tablenames.Add(getTableName(searchfrom));
                searchfrom++;
                table_exist = _facct.IsTableExist(getTableName(searchfrom));
                if (table_exist)
                    command += " UNION ALL ";
            }

            List<VoucherDetails> vouchlist = new List<VoucherDetails>();

            SqlDataReader allVoucher = _facct.getFromFacct(command);

            while (allVoucher.Read())
            {
                VoucherDetails vouch = new VoucherDetails();
                vouch.voucherNo = Convert.ToString(allVoucher["VRNO"] ?? null);
                vouch.Amount = Convert.ToDecimal(allVoucher["AMOUNT"] ?? 0);
                vouch.chequno = Convert.ToString(allVoucher["CQNO"]);
                vouch.NPRNO = String.Equals(Convert.ToString(allVoucher["NPRNO"]), "ICC") == true ? " " : Convert.ToString(allVoucher["NPRNO"]);
                vouch.ICCNO = Convert.ToString(allVoucher["ICCNO"]);
                vouchlist.Add(vouch);
            }

            return vouchlist.ToList();
        }

        public IEnumerable<ProjectDetails> getProjectDetails(IEnumerable<VoucherDetails> vouchDetails)
        {
            string nprno = string.Empty;
            string cprno = string.Empty;
            string mstlstCmd = string.Empty;
            string cmstlstCmd = string.Empty;

            List<ProjectDetails> ProjDetailslist = new List<ProjectDetails>();
            foreach (var single in vouchDetails)
            {
                if (!string.IsNullOrEmpty(single.NPRNO))
                {

                    mstlstCmd = string.Format("SELECT [INSTID],[COOR_NAME],[NPRNO],[TITLE],1 AS PT  FROM [FACCT].[dbo].[MSTLST] where NPRNO IN ('{0}')", single.NPRNO);
                }
                if (!string.IsNullOrEmpty(single.ICCNO))
                {

                    cmstlstCmd = string.Format("SELECT [INSTID] ,[CPRNO] ,[C_TITLE] ,[COOR_NAME1],2 AS PT  FROM [FACCT].[dbo].[CMSTLST] where CPRNO IN ('{0}')", single.ICCNO);
                }
                if (!string.IsNullOrEmpty(mstlstCmd))
                {
                    SqlDataReader dr = _facct.getFromFacct(mstlstCmd);
                    while (dr.Read())
                    {
                        ProjectDetails ProjDetails = new ProjectDetails();
                        ProjDetails.InstituteId = Convert.ToString(dr["INSTID"] ?? "");
                        ProjDetails.coorname = Convert.ToString(dr["COOR_NAME"] ?? "");
                        ProjDetails.projectNo = Convert.ToString(dr["NPRNO"]) ?? "";
                        ProjDetails.title = Convert.ToString(dr["TITLE"]) ?? "";
                        ProjDetails.projectType = Convert.ToInt32(dr["PT"]);
                        ProjDetails.Amount = single.Amount;
                        ProjDetails.chequno = single.chequno;
                        ProjDetails.comno = single.comno;
                        ProjDetails.voucherNo = single.voucherNo;
                        ProjDetailslist.Add(ProjDetails);
                    }
                }
                if (!string.IsNullOrEmpty(cmstlstCmd))
                {
                    SqlDataReader dr = _facct.getFromFacct(cmstlstCmd);
                    while (dr.Read())
                    {
                        ProjectDetails ProjDetails = new ProjectDetails();
                        ProjDetails.InstituteId = Convert.ToString(dr["INSTID"] ?? "");
                        ProjDetails.coorname = Convert.ToString(dr["COOR_NAME1"] ?? "");
                        ProjDetails.projectNo = Convert.ToString(dr["CPRNO"]) ?? "";
                        ProjDetails.title = Convert.ToString(dr["C_TITLE"]) ?? "";
                        ProjDetails.projectType = Convert.ToInt32(dr["PT"]);
                        ProjDetails.Amount = single.Amount;
                        ProjDetails.chequno = single.chequno;
                        ProjDetails.comno = single.comno;
                        ProjDetails.voucherNo = single.voucherNo;
                        ProjDetailslist.Add(ProjDetails);
                    }
                }
            }



            return ProjDetailslist;
        }
        public string getTableName(int searchfrom)
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
