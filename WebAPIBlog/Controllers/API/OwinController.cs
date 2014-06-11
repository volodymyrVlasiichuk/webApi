using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace WebAPIBlog.Controllers.API
{
	public class OwinApiController : ApiController
	{
		public IOwinContext OwinContext
		{
			get { return Request == null ? null : Request.GetOwinContext(); }
		}

		public IOwinRequest OwinRequest
		{
			get { return OwinContext == null ? null : OwinContext.Request; }
		}

		public IOwinResponse OwinResponse
		{
			get { return OwinContext == null ? null : OwinContext.Response; }
		}

		public IAuthenticationManager OwinAuthentication
		{
			get { return OwinContext == null ? null : OwinContext.Authentication; }
		}

		public IDictionary<string, object> OwinEnvironment
		{
			get { return OwinContext == null ? null : OwinContext.Environment; }
		}

		/// <summary>
		/// Simple wrapper over <see cref="OwinContextExtensions.GetUserManager{TManager}"/> that prevent NullPointerException if <see cref="ApiController.Request"/> is null.
		/// </summary>
		/// <typeparam name="TManager"></typeparam>
		/// <returns>User manager from owin context or null if context is not initialized.</returns>
		public TManager GetUserManager<TManager>()
			where TManager : class
		{
			return OwinContext == null ? null : OwinContext.GetUserManager<TManager>();
		}
	}
}