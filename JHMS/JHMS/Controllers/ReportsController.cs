using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JHMS.Controllers
{
    [Authorize(Roles = "manager")]
    public class ReportsController : Controller
    {
        // GET: Reports
        public ActionResult BookingReport()
        {
            return View();
        }
    }
}