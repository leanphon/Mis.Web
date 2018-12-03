using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MIS.Web.Startup))]
namespace MIS.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
