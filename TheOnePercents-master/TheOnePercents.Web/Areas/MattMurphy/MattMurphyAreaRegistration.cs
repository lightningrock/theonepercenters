using System.Web.Mvc;

namespace TheOnePercents.Web.Areas.MattMurphy
{
    public class ChadMacKayAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "MattMurphy";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "MattMurphy_default",
                "MattMurphy/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "TheOnePercents.Web.Areas.MattMurphy.Controllers" }
            );
        }
    }
}
