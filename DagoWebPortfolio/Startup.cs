using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DagoWebPortfolio.Startup))]
namespace DagoWebPortfolio
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
