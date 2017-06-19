using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Imprest.Startup))]
namespace Imprest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
