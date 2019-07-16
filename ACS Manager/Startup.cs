using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ACS_Manager.Startup))]
namespace ACS_Manager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
