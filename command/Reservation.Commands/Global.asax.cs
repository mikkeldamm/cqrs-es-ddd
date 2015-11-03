using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Reservation.Commands.Configuration;
using Reservation.Commands.Store;
using Reservation.Domain;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;

namespace Reservation.Commands
{
	public class WebApiApplication : System.Web.HttpApplication
	{
		private static Container _container;

		protected void Application_Start()
		{
			_container = new Container();

			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);

			RegisterCommands(_container);

			_container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
			_container.Verify();

			GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(_container);
		}

		private static void RegisterCommands(Container container)
		{
			var eventDelegator = new EventBus();
			var eventStore = new EventStore(eventDelegator);
			var aggregateFactory = new AggregateFactory();
			var commandProcessor = new CommandProcessor(aggregateFactory, eventStore);

			container.RegisterSingleton<IProcessCommands>(() => commandProcessor);
		}
	}
}
