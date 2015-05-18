using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheOnePercents.Infrastructure;
using TheOnePercents.Model;
using TheOnePercents.Repository;
using TheOnePercents.Web.Areas.ChadMacKay.Models;

namespace TheOnePercents.Web.Areas.ChadMacKay.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepositoryBase<theonepercentersEntities1> _repositoryBase;
        private string companyNamy { get; set; }

        public HomeController()
        {   
            _repositoryBase = RepositoryDI.Resolve<IRepositoryBase<theonepercentersEntities1>>();
            companyNamy = "Chad MacKay"; 
        }

        public ActionResult Index()
        {
            Company company = _repositoryBase.FindNonTracking<Company>(x => x.CompanyName == companyNamy);
            CompayViewModel compayViewModel = new CompayViewModel();

            compayViewModel.CompanyID = company.CompanyID;
            compayViewModel.CompanyName = company.CompanyName;

            return View(compayViewModel);
        }

    }
}
