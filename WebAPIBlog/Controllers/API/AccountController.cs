using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using WebAPIBlog.Filters;
using WebAPIBlog.Model;

namespace WebAPIBlog.Controllers.API
{
	[Authorize]
	[RoutePrefix("Account"),]
	public sealed class AccountController : ApiController
	{
		readonly ApplicationUserManager _userManager;

		public AccountController(ApplicationUserManager userManager)
		{
			_userManager = userManager;
		}

		IAuthenticationManager Authentication
		{
			get { return Request.GetOwinContext().Authentication; }
		}

		[HttpPost]
		[AllowAnonymous]
		[Route("register")]
		public async Task<IHttpActionResult> Register(RegisterModel model)
		{
			var user = new ApplicationUser() {UserName = model.UserName, Email = model.Email};

			IdentityResult result = await _userManager.CreateAsync(user, model.Password);
			if (result.Succeeded)
				return Ok();

			throw Startup.CreateHttpResponseException(HttpStatusCode.BadRequest, 
                "{\"Message\":\"The request is invalid.\", \"Error\":\"" + result.Errors.First() + "\"}");
		}

		[HttpPost]
		[AllowAnonymous]
		[Route("signin")]
		public async Task<IHttpActionResult> SignInAsync([FromBody] SignInModel model)
		{
			var user = await _userManager.FindByNameAsync(model.UserName);
			if (user != null)
			{
				if (await _userManager.CheckPasswordAsync(user, model.Password))
				{
					Authentication.SignIn(new AuthenticationProperties {IsPersistent = model.IsPersistent},
						await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie));
					return Ok();
				}
			}
			throw Startup.CreateHttpResponseException(HttpStatusCode.Forbidden, "User with this name and password doesn't exist.");
		}

		[Route]
		public async Task<ApplicationUser> Get()
		{
			return await _userManager.FindByNameAsync(User.Identity.Name);
		}

		[HttpGet]
		[AllowAnonymous]
		[Route("signout")]
		public void SignOutAsync()
		{
			Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
		}
	}
}