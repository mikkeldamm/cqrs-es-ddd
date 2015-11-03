﻿using System.Web.Http;
using System.Web.Http.Cors;

namespace Reservation.Commands
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Web API configuration and services

			// Web API routes
			var cors = new EnableCorsAttribute("*", "*", "*");
			config.EnableCors(cors);

			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);
		}
	}
}
