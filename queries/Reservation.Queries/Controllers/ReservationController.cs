using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Reservation.Queries.Models;
using Reservation.Queries.Views;

namespace Reservation.Queries.Controllers
{
	public class ReservationController : ApiController
	{
		private readonly IGetAllReservationsView _getAllReservationsView;

		public ReservationController(IGetAllReservationsView getAllReservationsView)
		{
			_getAllReservationsView = getAllReservationsView;
		}

		[HttpGet]
		public IEnumerable<ReservationViewModel> Get()
		{
			return _getAllReservationsView.Reservations;
		}

		[HttpGet]
		public ReservationViewModel Get(string id)
		{
			var reservations = _getAllReservationsView.Reservations;

			return reservations.SingleOrDefault(r => r.Id == id);
		}
	}
}
