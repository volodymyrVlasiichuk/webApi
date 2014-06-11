using Microsoft.AspNet.Identity;
using WebAPIBlog.Model;

namespace WebAPIBlog.Domain.Services
{
	public interface IUserService : IUserStore<ApplicationUser>, IUserLoginStore<ApplicationUser>, IUserClaimStore<ApplicationUser>, IUserRoleStore<ApplicationUser>, IUserPasswordStore<ApplicationUser>,
			IUserSecurityStampStore<ApplicationUser>, IUserEmailStore<ApplicationUser>
	{
		
	}
}