using IdentityDDD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Helpers;

namespace IdentityDDD.Data.EntityFramework
{
    internal class DataInitializer
    {
        private static IdentityContext context = null;
        
        internal static void InitEntities(IdentityContext ctx)
        {
            context = ctx;
            SetSuperUser();
        }

        private static void SetSuperUser()
        {
            //if (context.Roles == null                )
            //{
            //    context.Roles = new 
            //}


            if (context.Roles == null || !context.Roles.Any(r => r.Name == "Admin"))
            {
                var role = new Role
                {
                    Name = "Admin",
                    Users = new List<User>()
                };

                string salt = Crypto.GenerateSalt();
                string pass = string.Format("{0}lar23nov", salt);

                var user = new User
                {
                    Email = "eliyahushli@gmail.com",
                    UserName = "eliyahushli@gmail.com",
                    SecurityStamp = salt,
                    PasswordHash = Crypto.HashPassword(pass)
                };

                role.Users.Add(user);

                context.Roles.Add(role);
            }
        }
    }
}
