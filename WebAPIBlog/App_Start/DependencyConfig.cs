using System.Web.Http;
using System.Web.Http.Dependencies;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using WebAPIBlog.Domain.Repositories;
using WebAPIBlog.Domain.Services;

namespace WebAPIBlog
{
	public static class DependencyConfig
	{
		public static IDependencyResolver Resolver;

		public static void Register(HttpConfiguration config)
		{
			var container = new Container();

			container.RegisterWebApiControllers(config);
			RegisterRepositories(container);
			container.Verify();

			Resolver = new SimpleInjectorWebApiDependencyResolver(container);
			config.DependencyResolver = Resolver;
		}

		static void RegisterRepositories(Container container)
		{
			container.RegisterWebApiRequest<IPostService, MongoPostRepository>();
			container.RegisterWebApiRequest<IUserService, MongoUserRepository>();
		}
	}
}