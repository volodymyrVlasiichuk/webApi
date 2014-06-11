using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Html;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Owin;
using WebAPIBlog;
using WebAPIBlog.Filters;

[assembly: OwinStartup(typeof (Startup))]

namespace WebAPIBlog
{
	public sealed partial class Startup
	{
		static Startup()
		{
			var factory = new ClaimsIdentityFactory<IUser, string>();
			DefaultRoleClaimType = factory.RoleClaimType;
			DefaultUserIdClaimType = factory.UserIdClaimType;
			DefaultUserNameClaimType = factory.UserNameClaimType;
		}

		public static readonly HtmlSanitizer Sanitizer = new HtmlSanitizer()
		{
			AllowedAttributes = HtmlSanitizer.DefaultAllowedAttributes.Union(new[] {"style"})
		};

		public static HttpResponseException CreateHttpResponseException(HttpStatusCode code, string message)
		{
			return new HttpResponseException(new HttpResponseMessage(code) {Content = new StringContent(message)});
		}

		public static readonly string DefaultRoleClaimType;
		public static readonly string DefaultUserIdClaimType;
		public static readonly string DefaultUserNameClaimType;

		public void Configuration(IAppBuilder app)
		{
			var config = new HttpConfiguration();
			DependencyConfig.Register(config);

			WebApiConfig.Register(config);

			ConfigureAuth(app);

			// Todo: WebApiBlog.API.Startup: remove inappropriate configs
			config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
			config.Filters.Add(new NullModelCheckAttribute());
			config.Filters.Add(new ValidationActionFilter());

			app.UseWebApi(config);
		}
	}
}