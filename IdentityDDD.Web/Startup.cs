using IdentityDDD.Web.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Owin;
using System;

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
