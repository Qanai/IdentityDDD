using IdentityDDD.Domain;
using IdentityDDD.Domain.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace IdentityDDD.Web.Identity
{
    public class UserStore : IUserLoginStore<IdentityUser, Guid>, IUserClaimStore<IdentityUser, Guid>,
        IUserRoleStore<IdentityUser, Guid>, IUserPasswordStore<IdentityUser, Guid>,
        IUserSecurityStampStore<IdentityUser, Guid>, IUserStore<IdentityUser, Guid>, IDisposable
    {
        private readonly IUnitOfWork unitOfWork;

        public UserStore(IUnitOfWork uow)
        {
            unitOfWork = uow;
        }

        #region IUserStore Members
        public Task CreateAsync(IdentityUser user)
        {
            var u = SetUser(user);

            unitOfWork.UserRepository.Add(u);
            return unitOfWork.CommitAsync();
        }

        public Task DeleteAsync(IdentityUser user)
        {
            var u = SetUser(user);

            unitOfWork.UserRepository.Remove(u);
            return unitOfWork.CommitAsync();
        }

        public Task<IdentityUser> FindByIdAsync(Guid userId)
        {
            var user = unitOfWork.UserRepository.GetSingle(u => u.UserId == userId);
            return Task.FromResult<IdentityUser>(GetIdentityUser(user));
        }

        public Task<IdentityUser> FindByNameAsync(string userName)
        {
            var user = unitOfWork.UserRepository.FindByUserName(userName);
            return Task.FromResult<IdentityUser>(GetIdentityUser(user));
        }

        public Task UpdateAsync(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var usr = unitOfWork.UserRepository.GetSingle(u => u.UserId == user.Id);
            if (usr == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");

            PopulateUser(usr, user);

            unitOfWork.UserRepository.Update(usr);
            return unitOfWork.CommitAsync();
        }

        public void Dispose()
        {

        }
        #endregion

        #region IUserClaimStore Members
        public Task AddClaimAsync(IdentityUser user, System.Security.Claims.Claim claim)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (claim == null)
                throw new ArgumentNullException("claim");

            var usr = unitOfWork.UserRepository.GetSingle(u => u.UserId == user.Id, u => u.Claims);
            if (usr == null)
            {
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");
            }

            var clm = new Claim
            {
                ClaimType = claim.Type,
                ClaimValue = claim.Value,
                User = usr
            };

            usr.Claims.Add(clm);
            unitOfWork.UserRepository.Update(usr);
            return unitOfWork.CommitAsync();
        }

        public Task<IList<System.Security.Claims.Claim>> GetClaimsAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var usr = unitOfWork.UserRepository.GetSingle(u => u.UserId == user.Id);
            if (usr == null)
            {
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");
            }

            return Task.FromResult<IList<System.Security.Claims.Claim>>(usr.Claims.Select(c => new System.Security.Claims.Claim(c.ClaimType, c.ClaimValue)).ToList());
        }

        public Task RemoveClaimAsync(IdentityUser user, System.Security.Claims.Claim claim)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (claim == null)
                throw new ArgumentNullException("claim");

            var usr = unitOfWork.UserRepository.GetSingle(u => u.UserId == user.Id, u => u.Claims);
            if (usr == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");

            var c = usr.Claims.FirstOrDefault(x => x.ClaimValue == claim.Value && x.ClaimType == claim.Type);
            usr.Claims.Remove(c);

            unitOfWork.UserRepository.Update(usr);
            return unitOfWork.CommitAsync();
        }
        #endregion

        #region IUserLoginStore Members
        public Task AddLoginAsync(IdentityUser user, UserLoginInfo login)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (login == null)
                throw new ArgumentNullException("login");

            var usr = unitOfWork.UserRepository.GetSingle(u => u.UserId == user.Id, u => u.Logins);
            if (usr == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");

            var l = new ExternalLogin
            {
                LoginProvider = login.LoginProvider,
                ProviderKey = login.ProviderKey,
                User = usr
            };

            usr.Logins.Add(l);

            unitOfWork.UserRepository.Update(usr);
            return unitOfWork.CommitAsync();
        }

        public Task<IdentityUser> FindAsync(UserLoginInfo login)
        {
            if (login == null)
                throw new ArgumentNullException("login");

            var identityUser = default(IdentityUser);

            var l = unitOfWork.ExternalLoginRepository.GetByProviderAndKey(login.LoginProvider, login.ProviderKey);
            if (l != null)
                identityUser = GetIdentityUser(l.User);

            return Task.FromResult<IdentityUser>(identityUser);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var usr = unitOfWork.UserRepository.GetSingle(u => u.UserId == user.Id, u => u.Logins);
            if (usr == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");

            return Task.FromResult<IList<UserLoginInfo>>(usr.Logins.Select(x => new UserLoginInfo(x.LoginProvider, x.ProviderKey)).ToList());
        }

        public Task RemoveLoginAsync(IdentityUser user, UserLoginInfo login)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (login == null)
                throw new ArgumentNullException("login");

            var usr = unitOfWork.UserRepository.GetSingle(u => u.UserId == user.Id, u => u.Logins);
            if (usr == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");

            var l = usr.Logins.FirstOrDefault(x => x.LoginProvider == login.LoginProvider && x.ProviderKey == login.ProviderKey);
            usr.Logins.Remove(l);

            unitOfWork.UserRepository.Update(usr);
            return unitOfWork.CommitAsync();
        }
        #endregion

        #region IUserRoleStore Members
        public Task AddToRoleAsync(IdentityUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Argument cannot be null, empty, or whitespace: roleName.");

            var usr = unitOfWork.UserRepository.GetSingle(u => u.UserId == user.Id, u => u.Roles);
            if (usr == null)
            {
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");
            }

            var rl = unitOfWork.RoleRepository.FindByName(roleName);
            if (rl == null)
            {
                throw new ArgumentException("roleName does not correspond to a Role entity.", "roleName");
            }

            usr.Roles.Add(rl);
            unitOfWork.UserRepository.Update(usr);
            return unitOfWork.CommitAsync();
        }

        public Task<IList<string>> GetRolesAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var usr = unitOfWork.UserRepository.GetSingle(u => u.UserId == user.Id, u => u.Roles);
            if (usr == null)
            {
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");
            }

            return Task.FromResult<IList<string>>(usr.Roles.Select(r => r.Name).ToList());
        }

        public Task<bool> IsInRoleAsync(IdentityUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Argument cannot be null, empty, or whitespace: role.");

            var usr = unitOfWork.UserRepository.GetSingle(u => u.UserId == user.Id, u => u.Roles);
            if (usr == null)
            {
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");
            }

            return Task.FromResult<bool>(usr.Roles.Any(r => r.Name == roleName));
        }

        public Task RemoveFromRoleAsync(IdentityUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Argument cannot be null, empty, or whitespace: role.");

            var usr = unitOfWork.UserRepository.GetSingle(u => u.UserId == user.Id, u => u.Roles);
            if (usr == null)
            {
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");
            }

            var rl = usr.Roles.FirstOrDefault(r => r.Name == roleName);
            usr.Roles.Remove(rl);
            unitOfWork.UserRepository.Update(usr);
            return unitOfWork.CommitAsync();
        }
        #endregion

        #region IUserPasswordStore Members
        public Task<string> GetPasswordHashAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            return Task.FromResult<string>(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            return Task.FromResult<bool>(!string.IsNullOrWhiteSpace(user.PasswordHash));
        }

        public Task SetPasswordHashAsync(IdentityUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }
        #endregion

        #region IUserSecurityStampStore<IdentityUser, Guid> Members
        public Task<string> GetSecurityStampAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            return Task.FromResult<string>(user.SecurityStamp);
        }

        public Task SetSecurityStampAsync(IdentityUser user, string stamp)
        {
            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }
        #endregion

        #region Private Methods
        private User GetUser(IdentityUser identityUser)
        {
            if (identityUser == null)
                return null;

            var user = new IdentityDDD.Domain.Entities.User();
            PopulateUser(user, identityUser);

            return user;
        }

        private IdentityUser GetIdentityUser(User user)
        {
            if (user == null)
                return null;

            var identityUser = new IdentityUser();
            PopulateIdentityUser(identityUser, user);

            return identityUser;
        }

        private void PopulateIdentityUser(IdentityUser identityUser, User user)
        {
            identityUser.Id = user.UserId;
            identityUser.UserName = user.UserName;
            identityUser.PasswordHash = user.PasswordHash;
            identityUser.SecurityStamp = user.SecurityStamp;
        }

        private void PopulateUser(User user, IdentityUser identityUser)
        {
            user.UserId = identityUser.Id;
            user.UserName = identityUser.UserName;
            user.PasswordHash = identityUser.PasswordHash;
            user.SecurityStamp = identityUser.SecurityStamp;
        }

        private User SetUser(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return GetUser(user);
        }
        #endregion
    }
}