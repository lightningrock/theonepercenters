using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheOnePercents.Web.Areas.ChadMacKay.BootstrapSupport;

namespace TheOnePercents.Web.Areas.ChadMacKay.Controllers
{
    public class BootstrapBaseController : CookieSettingsController
    {

        public void Attention(string message)
        {
            TempData.Remove(Alerts.ATTENTION);
            TempData.Add(Alerts.ATTENTION, message);
        }

        public void Success(string message) {
            TempData.Remove(Alerts.SUCCESS);
            TempData.Add(Alerts.SUCCESS, message);
        }

        public void Information(string message)
        {
            TempData.Remove(Alerts.INFORMATION);
            TempData.Add(Alerts.INFORMATION, message);
        }

        public void Error(string message)
        {
            TempData.Remove(Alerts.ERROR);
            TempData.Add(Alerts.ERROR, message);
        }

        public void Clear()
        {
            TempData.Remove(Alerts.ATTENTION);
            TempData.Remove(Alerts.SUCCESS);
            TempData.Remove(Alerts.INFORMATION);
            TempData.Remove(Alerts.ERROR);
        }
    }
}
