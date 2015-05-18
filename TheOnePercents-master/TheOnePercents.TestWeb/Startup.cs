using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TheOnePercents.TestWeb.Startup))]
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Web.config", Watch = true)]
namespace TheOnePercents.TestWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
