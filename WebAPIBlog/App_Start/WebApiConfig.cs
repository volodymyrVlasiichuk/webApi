using System.Web.Http;

namespace WebAPIBlog
{
	public static class WebApiConfig
	{
		public static void	Register(HttpConfiguration config)
		{
			// Web API configuration and services

			// Web API routes
			config.MapHttpAttributeRoutes();

			//config.Routes.MapHttpRoute(
			//	name: "DefaultApi",
			//	routeTemplate: "blog/{controller}",
			//	defaults: new { id = RouteParameter.Optional }
			//);
		}
	}
}
