using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheOnePercents.TestWeb.Logging;

namespace TheOnePercents.Web.Controllers
{
    public class HomeController : Controller
    {
        //public HomeController(IRepositoryBase<theonepercentersEntities> repositoryBase) {
        //    _repositoryBase = repositoryBase;
        //}

        Log4NetLogger logger = new Log4NetLogger();

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            logger.Info("Index page log from " + Request["REMOTE_ADDR"]);

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
