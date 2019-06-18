using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ScraperFullStackMVC.Startup))]
namespace ScraperFullStackMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
