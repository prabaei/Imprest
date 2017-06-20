using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imprest.Data;
using Imprest.Data.Facct;
using System.Data.SqlClient;
using System.IO;

namespace testConsole
{
    class Program
    {
         public static FacctDb _facct = Singleton<FacctDb>.getInstance();
        static void Main(string[] args)
        {
            readfromtext();
            //if (_facct.IsTableExist("VOU1119"))
            //    Console.WriteLine("table exist");
            DateTime t = DateTime.Now;
            int year=t.Year;
            int searchfrom = year - 2;
            //getTableName(searchfrom);
            bool table_exist = _facct.IsTableExist(getTableName(searchfrom));
            List<Voucher> vouchlist = new List<Voucher>();
            while (table_exist)
            {
                string command = string.Format("SELECT TOP 7 COUNT(VRNO) AS Vcount,VRNO FROM {0} GROUP BY VRNO HAVING VRNO LIKE '%{1}%'", getTableName(searchfrom), "C");
                SqlDataReader allVoucher =  _facct.getFromFacct(command);
                while(allVoucher.Read())
                {
                    Voucher vouch = new Voucher();
                    vouch.voucherNo = Convert.ToString(allVoucher["VRNO"]??null);
                    vouch.count = Convert.ToInt32(allVoucher["Vcount"]);
                    vouchlist.Add(vouch);
                }
                
                table_exist = _facct.IsTableExist(getTableName(searchfrom++));
                table_exist = vouchlist.Count > 7 ? false : true;
                //vouchlist.Clear();
              // var data= vouchlist.GroupBy(m => m.voucherNo,(key,g)=>new Voucher() {count=g.Count(),voucherNo=key }).ToList();
            }
            //return vouchlist.Take(7).ToList();
        }

        public static void readfromtext()
        {
            var fileStream = new FileStream(@"C:\accountgit\Imprest\Imprest\acc.txt", FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                using (StreamWriter writer =
            new StreamWriter(@"C:\accountgit\Imprest\Imprest\insert.txt"))
                {
                    string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    string[] strarray = line.Split('\t');
                    
                        writer.WriteLine(string.Format("insert into AccountMaster values ({0},'{1}','{2}','{3}','{4}','{5}',{6})", strarray));
                    }
                    //Console.WriteLine(string.Format("insert into AccountMaster values ({0},'{1}','{2}','{3}','{4}','{5}',{6})",strarray));
                    
                    //Console.WriteLine(line.Split('\t')); 
                    //Console.WriteLine();
                }
            }
        }
        private static string getTableName(int searchfrom)
        {
            int searchshort = searchfrom % 1000;
            int nextshort = (searchfrom+1) % 1000;
            string searshortstr = "00" + Convert.ToString(searchshort);
            string nextshortstr = "00" + Convert.ToString(nextshort);
            searshortstr = searshortstr.Substring(searshortstr.Length - 2);
            nextshortstr = nextshortstr.Substring(nextshortstr.Length - 2);
            return string.Format("VOU{0}{1}", searshortstr.Trim(), nextshortstr.Trim());
        }
    }
}
