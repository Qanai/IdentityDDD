using System;

namespace IdentityDDD.Domain.Entities
{
    public class Claim
    {
        private User user;

        #region Scalar Properties
        public virtual int ClaimId { get; set; }
        public virtual Guid UserId { get; set; }
        public virtual string ClaimType { get; set; }
        public virtual string ClaimValue { get; set; }
        #endregion

        #region Navigation Properties
        public virtual User User
        {
            get { return user; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                user = value;
                UserId = value.UserId;
            }
        }
        #endregion
    }
}
