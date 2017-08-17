using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IbStats.Web.Startup))]
namespace IbStats.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
