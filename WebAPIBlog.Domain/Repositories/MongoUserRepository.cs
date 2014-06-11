using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using MongoDb.ASP.Identity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using WebAPIBlog.Domain.Services;
using WebAPIBlog.Model;

namespace WebAPIBlog.Domain.Repositories
{
	// Todo: MongoDb.ASP.Identity.UserStore: Looking forward for async support in driver (https://jira.mongodb.org/browse/CSHARP-138), currently all 'async' methods are synchronous.
	public sealed class MongoUserRepository:
			MongoRepository<ApplicationUser>, IUserService
	{
		public MongoUserRepository() : base("users")
		{
		}

		bool _disposed;

		public Task AddClaimAsync(ApplicationUser user, Claim claim)
		{
			ThrowIfDisposed();
			if (user == null)
				throw new ArgumentNullException("user");

			if (!user.Claims.Any(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value))
			{
				user.Claims.Add(new IdentityUserClaim
				{
					ClaimType = claim.Type,
					ClaimValue = claim.Value
				});
			}

			return Task.FromResult(0);
		}

		public Task<IList<Claim>> GetClaimsAsync(ApplicationUser user)
		{
			ThrowIfDisposed();
			if (user == null)
				throw new ArgumentNullException("user");

			IList<Claim> result = user.Claims.Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList();
			return Task.FromResult(result);
		}

		public Task RemoveClaimAsync(ApplicationUser user, Claim claim)
		{
			ThrowIfDisposed();
			if (user == null)
				throw new ArgumentNullException("user");

			user.Claims.RemoveAll(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value);
			return Task.FromResult(0);
		}

		public Task CreateAsync(ApplicationUser user)
		{
			ThrowIfDisposed();
			if (user == null)
				throw new ArgumentNullException("user");

			Collection.Insert(user);

			return Task.FromResult(user);
		}

		public Task DeleteAsync(ApplicationUser user)
		{
			ThrowIfDisposed();
			if (user == null)
				throw new ArgumentNullException("user");

			Collection.Remove((Query.EQ("_id", ObjectId.Parse(user.Id))));
			return Task.FromResult(true);
		}

		public Task<ApplicationUser> FindByIdAsync(string userId)
		{
			ThrowIfDisposed();
			ApplicationUser user = Collection.FindOne((Query.EQ("_id", ObjectId.Parse(userId))));
			return Task.FromResult(user);
		}

		public Task<ApplicationUser> FindByNameAsync(string userName)
		{
			ThrowIfDisposed();

			ApplicationUser user = Collection.FindOne((Query.EQ("UserName", userName)));
			return Task.FromResult(user);
		}

		public Task UpdateAsync(ApplicationUser user)
		{
			ThrowIfDisposed();
			if (user == null)
				throw new ArgumentNullException("user");

			Collection.Update(Query.EQ("_id", ObjectId.Parse(user.Id)), Update.Replace(user), UpdateFlags.Upsert);

			return Task.FromResult(user);
		}

		public override void Dispose()
		{
			base.Dispose();
			_disposed = true;
		}

		public Task AddLoginAsync(ApplicationUser user, UserLoginInfo login)
		{
			ThrowIfDisposed();
			if (user == null)
				throw new ArgumentNullException("user");

			if (!user.Logins.Any(x => x.LoginProvider == login.LoginProvider && x.ProviderKey == login.ProviderKey))
			{
				user.Logins.Add(login);
			}

			return Task.FromResult(true);
		}

		public Task<ApplicationUser> FindAsync(UserLoginInfo login)
		{
			ApplicationUser user = Collection.FindOne(Query.And(Query.EQ("Logins.LoginProvider", login.LoginProvider),
				Query.EQ("Logins.ProviderKey", login.ProviderKey)));

			return Task.FromResult(user);
		}

		public Task<IList<UserLoginInfo>> GetLoginsAsync(ApplicationUser user)
		{
			ThrowIfDisposed();
			if (user == null)
				throw new ArgumentNullException("user");

			return Task.FromResult((IList<UserLoginInfo>)user.Logins);
		}

		public Task RemoveLoginAsync(ApplicationUser user, UserLoginInfo login)
		{
			ThrowIfDisposed();
			if (user == null)
				throw new ArgumentNullException("user");

			user.Logins.RemoveAll(x => x.LoginProvider == login.LoginProvider && x.ProviderKey == login.ProviderKey);

			return Task.FromResult(0);
		}

		public Task<string> GetPasswordHashAsync(ApplicationUser user)
		{
			ThrowIfDisposed();
			if (user == null)
				throw new ArgumentNullException("user");

			return Task.FromResult(user.PasswordHash);
		}

		public Task<bool> HasPasswordAsync(ApplicationUser user)
		{
			ThrowIfDisposed();
			if (user == null)
				throw new ArgumentNullException("user");

			return Task.FromResult(user.PasswordHash != null);
		}

		public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash)
		{
			ThrowIfDisposed();
			if (user == null)
				throw new ArgumentNullException("user");

			user.PasswordHash = passwordHash;
			return Task.FromResult(0);
		}

		public Task AddToRoleAsync(ApplicationUser user, string role)
		{
			ThrowIfDisposed();
			if (user == null)
				throw new ArgumentNullException("user");

			if (!user.Roles.Contains(role, StringComparer.InvariantCultureIgnoreCase))
				user.Roles.Add(role);

			return Task.FromResult(true);
		}

		public Task<IList<string>> GetRolesAsync(ApplicationUser user)
		{
			ThrowIfDisposed();
			if (user == null)
				throw new ArgumentNullException("user");

			return Task.FromResult<IList<string>>(user.Roles);
		}

		public Task<bool> IsInRoleAsync(ApplicationUser user, string role)
		{
			ThrowIfDisposed();
			if (user == null)
				throw new ArgumentNullException("user");

			return Task.FromResult(user.Roles.Contains(role, StringComparer.InvariantCultureIgnoreCase));
		}

		public Task RemoveFromRoleAsync(ApplicationUser user, string role)
		{
			ThrowIfDisposed();
			if (user == null)
				throw new ArgumentNullException("user");

			user.Roles.RemoveAll(r => String.Equals(r, role, StringComparison.InvariantCultureIgnoreCase));

			return Task.FromResult(0);
		}

		public Task<string> GetSecurityStampAsync(ApplicationUser user)
		{
			ThrowIfDisposed();
			if (user == null)
				throw new ArgumentNullException("user");

			return Task.FromResult(user.SecurityStamp);
		}

		public Task SetSecurityStampAsync(ApplicationUser user, string stamp)
		{
			ThrowIfDisposed();
			if (user == null)
				throw new ArgumentNullException("user");

			user.SecurityStamp = stamp;
			return Task.FromResult(0);
		}

		void ThrowIfDisposed()
		{
			if (_disposed)
				throw new ObjectDisposedException(GetType().Name);
		}

		public Task SetEmailAsync(ApplicationUser user, string email)
		{
			ThrowIfDisposed();
			if (user == null)
				throw new ArgumentNullException("user");

			user.Email = email;
			return Task.FromResult(0);
		}

		public Task<string> GetEmailAsync(ApplicationUser user)
		{
			ThrowIfDisposed();
			if (user == null)
				throw new ArgumentNullException("user");

			return Task.FromResult(user.Email);
		}

		public Task<bool> GetEmailConfirmedAsync(ApplicationUser user)
		{
			ThrowIfDisposed();
			if (user == null)
				throw new ArgumentNullException("user");

			return Task.FromResult(user.EmailConfirmed);
		}
		
		public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed)
		{
			ThrowIfDisposed();
			if (user == null)
				throw new ArgumentNullException("user");

			user.EmailConfirmed = true;
			return Task.FromResult(0);
		}

		public Task<ApplicationUser> FindByEmailAsync(string email)
		{
			ThrowIfDisposed();
			if (email == null)
				throw new ArgumentNullException("email");

			ApplicationUser user = Collection.FindOne((Query.EQ("email", email)));
			return Task.FromResult(user);
		}
	}
}