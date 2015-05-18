using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TheOnePercents.Web.Areas.ChadMacKay.Controllers
{
    public class CircleStatsController : Controller
    {
        //
        // GET: /ChadMacKay/CircleStats/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadCircleStats() {
            return PartialView("../Dashboard/Partial/_circleStatsPartial"); 
        }

    }
}
