using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IzintoCleaning.Startup))]
namespace IzintoCleaning
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
