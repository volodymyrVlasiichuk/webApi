using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using MongoDb.ASP.Identity;

namespace WebAPIBlog.Model.Identity
{
	public class IdentityUser : IUser
	{
		/// <summary>
		/// Unique key for the user.
		/// </summary>
		/// <value>The identifier.</value>
		/// <returns>The unique key for the user.</returns>
		public virtual string Id { get; set; }

		/// <summary>
		/// Gets or sets the name of the user.
		/// </summary>
		/// <value>The name of the user.</value>
		public virtual string UserName { get; set; }
		
		/// <summary>
		/// Gets or sets the email of the user.
		/// </summary>
		public virtual string Email { get; set; }

		public virtual bool EmailConfirmed { get; set; }

		/// <summary>
		/// Gets or sets the password hash.
		/// </summary>
		/// <value>The password hash.</value>
		public virtual string PasswordHash { get; set; }

		/// <summary>
		/// Gets or sets the security stamp.
		/// </summary>
		/// <value>The security stamp.</value>
		public virtual string SecurityStamp { get; set; }

		/// <summary>
		/// Gets the roles.
		/// </summary>
		/// <value>The roles.</value>
		public virtual List<string> Roles { get; private set; }

		/// <summary>
		/// Gets the claims.
		/// </summary>
		/// <value>The claims.</value>
		public virtual List<IdentityUserClaim> Claims { get; private set; }

		/// <summary>
		/// Gets the logins.
		/// </summary>
		/// <value>The logins.</value>
		public virtual List<UserLoginInfo> Logins { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="IdentityUser"/> class.
		/// </summary>
		public IdentityUser()
		{
			Claims = new List<IdentityUserClaim>();
			Roles = new List<string>();
			Logins = new List<UserLoginInfo>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="IdentityUser"/> class.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		public IdentityUser(string userName)
			: this()
		{
			UserName = userName;
		}
	}
}