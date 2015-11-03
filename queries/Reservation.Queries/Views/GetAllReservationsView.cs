using System.Collections.Generic;
using System.Linq;
using Reservation.Domain;
using Reservation.Domain.Events;
using Reservation.Queries.Models;

namespace Reservation.Queries.Views
{
	public interface IGetAllReservationsView :
        ISubscribeTo<ReservationCreated>,
		ISubscribeTo<ReservationOutboundDateTimeChanged>,
		ISubscribeTo<ReservationDeleted>
	{
		List<ReservationViewModel> Reservations { get; set; }
	}

	public class GetAllReservationsView : IGetAllReservationsView
	{
		public List<ReservationViewModel> Reservations { get; set; }

		public GetAllReservationsView()
		{
			Reservations = new List<ReservationViewModel>();
		}

		public void Handle(ReservationCreated domainEvent)
		{
			var reservation = Reservations.SingleOrDefault(r => r.Id == domainEvent.Id);
			if (reservation == null)
			{
				reservation = new ReservationViewModel();
				Reservations.Add(reservation);
			}

			reservation.Id = domainEvent.Id;
		}

		public void Handle(ReservationOutboundDateTimeChanged domainEvent)
		{
			var reservation = Reservations.SingleOrDefault(r => r.Id == domainEvent.AggregateRootId);
			if (reservation != null)
			{
				reservation.OutboundDateTime = domainEvent.DateTime;
			}
		}

		public void Handle(ReservationDeleted domainEvent)
		{
			Reservations.RemoveAll(r => r.Id == domainEvent.Id);
		}
	}
}