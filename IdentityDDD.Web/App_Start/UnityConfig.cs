using System.Web.Mvc;
using Microsoft.Practices.Unity;
using IdentityDDD.Domain;
using Unity.Mvc5;
using IdentityDDD.Data.EntityFramework;
using IdentityDDD.Web.Identity;
using Microsoft.AspNet.Identity;
using System;

namespace IdentityDDD.Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<IUnitOfWork, UnitOfWork>(new HierarchicalLifetimeManager(),
                new InjectionConstructor("IdentityDDD"));
            container.RegisterType<IUserStore<IdentityUser, Guid>, UserStore>(new TransientLifetimeManager());
            container.RegisterType<RoleStore>(new TransientLifetimeManager());
   
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}