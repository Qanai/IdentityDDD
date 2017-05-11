using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IdentityDDD.Web.Startup))]
namespace IdentityDDD.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
