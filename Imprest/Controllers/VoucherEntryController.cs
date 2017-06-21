using Imprest.Data.Facct;
using Imprest.Data.Models.Facct;
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
        
        readonly IFacctService _facctservice;
        public VoucherEntryController(IFacctService facctservice)
        {
            _facctservice = facctservice;
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
           var voucherLIst= _facctservice.getvoucherFacct(voucherNO);
            return Json( voucherLIst , JsonRequestBehavior.AllowGet);
            
        }
        [HttpGet]
        public PartialViewResult getVoucherDetails(string voucherNO)
        {
            var voucherList = _facctservice.getvoucherDetailList(voucherNO);
           var projectDetails= _facctservice.getProjectDetails(voucherList);
            return PartialView(voucherList as IEnumerable<VoucherDetails>);
        }
        #endregion
        
    }
}