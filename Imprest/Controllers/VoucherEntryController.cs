
using Imprest.Service.Dbservice.Facct;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Imprest.Models.Facct;
using Imprest.Service.Dbservice.Imprest;

namespace Imprest.Controllers
{
    public class VoucherEntryController : Controller
    {
        readonly IImprestService _imprestservice;
        readonly IFacctService _facctservice;
        public VoucherEntryController(IFacctService facctservice, IImprestService ImprestService)
        {
            _facctservice = facctservice;
            _imprestservice = ImprestService;
        }
        // GET: VoucherEntry
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult setDetails(string voucherNO,string amount,string comno,string projNO,string vouchTy,string cheqno,string inst)
        {
            var accountDetailsList=_imprestservice.getAccountDetail(inst);
            return null;
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
            return PartialView(projectDetails as IEnumerable<ProjectDetails>);
        }
        #endregion
        
    }
}