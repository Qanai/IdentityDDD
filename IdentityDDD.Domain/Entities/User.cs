using System;
using System.Collections.Generic;

namespace IdentityDDD.Domain.Entities
{
    public class User
    {
        #region Fields
        private ICollection<Claim> claims;
        private ICollection<ExternalLogin> externalLogins;
        private ICollection<Role> roles;
        #endregion

        #region Scalar Properties
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual string SecurityStamp { get; set; }
        #endregion

        #region Navigation Properties
        public virtual ICollection<Claim> Claims
        {
            get { return claims ?? (claims = new List<Claim>()); }
            set { claims = value; }
        }

        public virtual ICollection<ExternalLogin> Logins
        {
            get { return externalLogins ?? (externalLogins = new List<ExternalLogin>()); }
            set { externalLogins = value; }
        }

        public virtual ICollection<Role> Roles
        {
            get { return roles ?? (roles = new List<Role>()); }
            set { roles = value; }
        }
        #endregion
    }
}
