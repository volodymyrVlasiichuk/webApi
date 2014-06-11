using System;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace WebAPIBlog
{
	public partial class Startup
	{
		public void ConfigureAuth(IAppBuilder app)
		{
			// Todo:It's dependency resolving actually. WTF???
			// app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

			// Todo: Add authentication through tokens in json.
			app.UseCookieAuthentication(new CookieAuthenticationOptions()
			{
				CookieName = DefaultAuthenticationTypes.ApplicationCookie,
				AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                CookieDomain = ".wab.local"
			});
			
			//app.UseOAuthBearerTokens(new OAuthAuthorizationServerOptions()
			//{
			//	Provider = new ApplicationOAuthProvider("self"),
			//	AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
			//	AllowInsecureHttp = true
			//});
		}
	}
}