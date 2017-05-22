using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace IdentityDDD.Data.EntityFramework
{
    internal class EmptyInitializer : CreateDatabaseIfNotExists<IdentityContext>
    {
        protected override void Seed(IdentityContext context)
        {
            DataInitializer.InitEntities(context);

            base.Seed(context);
        }
    }
}
