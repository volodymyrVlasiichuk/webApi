using Microsoft.AspNet.Identity;
using WebAPIBlog.Domain.Services;
using WebAPIBlog.Model;

namespace WebAPIBlog
{
	public sealed class ApplicationUserManager : UserManager<ApplicationUser>
	{
		public ApplicationUserManager(IUserService store)
			: base(store)
		{
			UserValidator = new UserValidator<ApplicationUser>(this)
			{
				AllowOnlyAlphanumericUserNames = false,
				RequireUniqueEmail = true
			};

			PasswordValidator = new PasswordValidator
			{
				RequiredLength = 6,
				RequireNonLetterOrDigit = false,
				RequireDigit = false,
				RequireLowercase = false,
				RequireUppercase = false,
			};
		}

		//public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) 
		//{
		//	var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(new MongoUrl("mongodb://127.0.0.1/b5dhBlog")));
		//	// Configure validation logic for usernames
		//	manager.UserValidator = new UserValidator<ApplicationUser>(manager)
		//	{
		//		AllowOnlyAlphanumericUserNames = false,
		//		RequireUniqueEmail = true
		//	};
		//	// Configure validation logic for passwords
		//	manager.PasswordValidator = new PasswordValidator
		//	{
		//		RequiredLength = 6,
		//		RequireNonLetterOrDigit = true,
		//		RequireDigit = true,
		//		RequireLowercase = true,
		//		RequireUppercase = true,
		//	};
		//	var dataProtectionProvider = options.DataProtectionProvider;
		//	if (dataProtectionProvider != null)
		//	{
		//		manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
		//	}
		//	return manager;
		//}
	}
}
