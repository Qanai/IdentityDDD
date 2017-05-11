using System;
using System.Collections.Generic;

namespace IdentityDDD.Domain.Entities
{
    public class Role
    {
        #region Fields
        private ICollection<User> users;
        #endregion

        #region Scalar Properties
        public Guid RoleId { get; set; }
        public string Name { get; set; }
        #endregion

        #region Navigation Properties
        public ICollection<User> Users
        {
            get { return users ?? (users = new List<User>()); }
            set { users = value; }
        }
        #endregion
    }
}
