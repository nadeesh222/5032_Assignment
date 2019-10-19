using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JHMS.Startup))]
namespace JHMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
