using System;

namespace IdentityDDD.Domain.Entities
{
    public class ExternalLogin
    {
        private User user;

        #region Scalar Properties
        public virtual string LoginProvider { get; set; }
        public virtual string ProviderKey { get; set; }
        public virtual Guid UserId { get; set; }
        #endregion

        #region Navigation Properties
        public virtual User User
        {
            get { return user; }
            set
            {
                user = value;
                UserId = value.UserId;
            }
        }

        #endregion
    }
}
