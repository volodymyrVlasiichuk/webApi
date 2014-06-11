using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace WebAPIBlog.Filters
{
	public class RequireHttpsAttribute : AuthorizationFilterAttribute
	{
		public override void OnAuthorization(HttpActionContext actionContext)
		{
			var request = actionContext.Request;
			var requestUri = actionContext.Request.RequestUri;

			if (requestUri.Scheme == Uri.UriSchemeHttps)
				return;

			if (request.Method == HttpMethod.Get)
				actionContext.Response = CreateHttpsRedirect(requestUri, HttpStatusCode.Found);
			else if (request.Method == HttpMethod.Post)
				actionContext.Response = CreateHttpsRedirect(requestUri, HttpStatusCode.TemporaryRedirect);
			else
				throw new InvalidOperationException("Attribute can be applied to HTTP GET and POST methods only.");
		}

		static HttpResponseMessage CreateHttpsRedirect(Uri uri, HttpStatusCode code)
		{
			var response = new HttpResponseMessage(code);
			response.Headers.Add("Location", Uri.UriSchemeHttps + Uri.SchemeDelimiter + uri.Host + uri.PathAndQuery);
			response.ReasonPhrase = "Https required.";
			return response;
		}
	}
}