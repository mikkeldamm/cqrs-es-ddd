using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Reservation.Domain;
using Reservation.Domain.Events;
using Reservation.Queries.Views;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using Reservation.Queries.Hubs;
using Microsoft.AspNet.SignalR;
using System;

namespace Reservation.Queries
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
			
			RegisterQueries(_container);

			_container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
			_container.Verify();

			GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(_container);
		}

		private static void RegisterQueries(Container container)
		{
			var eventDelegator = new EventBus();

			var reservationHub = new Lazy<IHubContext>(() => GlobalHost.ConnectionManager.GetHubContext<ReservationHub>());

			var getAllReservationsView = new GetAllReservationsView();
			var getAllReservationsViewDecorator = new GetAllReservationsViewHub(reservationHub, getAllReservationsView);
			
			container.RegisterSingleton<IGetAllReservationsView>(() => { return getAllReservationsViewDecorator; });
			
			eventDelegator.Register<ReservationCreated>(getAllReservationsViewDecorator.Handle);
			eventDelegator.Register<ReservationOutboundDateTimeChanged>(getAllReservationsViewDecorator.Handle);
			eventDelegator.Register<ReservationDeleted>(getAllReservationsViewDecorator.Handle);

			eventDelegator.StartListener();
		}
	}
}
