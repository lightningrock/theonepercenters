using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheOnePercents.Infrastructure;
using TheOnePercents.Model;
using TheOnePercents.Repository;

namespace TheOnePercents.Web.Areas.ChadMacKay.Controllers

{
    public class CookieSettingsController : Controller
    {
        private readonly IRepositoryBase<theonepercentersEntities1> _repositoryBase;

        public CookieSettingsController() {

            _repositoryBase = RepositoryDI.Resolve<IRepositoryBase<theonepercentersEntities1>>();
        }

        /// <summary>
        /// Get the setting for the company and save them to a cookie.
        /// </summary>
        /// <param name="companyID"></param>
        public void CreateSettingsCookie(Guid companyID)
        {
            var settingID = _repositoryBase.Get<Company>(x => x.CompanyID == companyID).SettingID;

            var settings = _repositoryBase.Get<Setting>(x => x.SettingID == settingID);

            HttpCookie cookie = new HttpCookie("SettingsCookie");
            cookie.Values["CookieCompanyID"] = companyID.ToString();
            cookie.Values["CookieCompanyEmail"] = settings.CompanyEmail;
            cookie.Values["CookieRegionalSettings"] = settings.RegionalSettingCode;
            cookie.Values["CookieTimezoneCode"] = settings.TimeZoneCode;
  
            cookie.Expires = DateTime.Now.AddDays(2);
            Response.Cookies.Add(cookie);
        }

        public void RemoveSettingsCookie()
        {
            if (Request.Cookies.AllKeys.Contains("SettingsCookie"))
            {
                HttpCookie cookie = Request.Cookies["SettingsCookie"];
                cookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(cookie);
            }
        }

        public void RemoveAllCookie()
        {
            if (Request.Cookies.AllKeys.Contains("SettingsCookie"))
            {
                HttpCookie cookie = Request.Cookies["SettingsCookie"];
                cookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(cookie);
            }

            if (Request.Cookies.AllKeys.Contains("ClientControl")) {
                HttpCookie cookie = Request.Cookies["ClientControl"];
                cookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(cookie);
            }
            if (Request.Cookies.AllKeys.Contains("ClientInControl"))
            {
                HttpCookie cookie = Request.Cookies["ClientInControl"];
                cookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(cookie);
            }


            if (Request.Cookies.AllKeys.Contains("ClientControlFeatures"))
            {
                HttpCookie cookie = Request.Cookies["ClientControlFeatures"];
                cookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(cookie);
            }
        }
    }
}
