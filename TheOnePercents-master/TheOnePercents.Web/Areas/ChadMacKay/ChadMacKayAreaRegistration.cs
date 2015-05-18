using System.Web.Mvc;

namespace TheOnePercents.Web.Areas.ChadMacKay
{
    public class ChadMacKayAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ChadMacKay";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ChadMacKay_default",
                "ChadMacKay/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "TheOnePercents.Web.Areas.ChadMacKay.Controllers" }
            );
        }
    }
}
