using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using Reservation.Domain.Events;
using Reservation.Queries.Models;
using Reservation.Queries.Views;

namespace Reservation.Queries.Hubs
{
	public class GetAllReservationsViewHub : IGetAllReservationsView
	{
		private readonly Lazy<IHubContext> _context;
		private readonly IGetAllReservationsView _getAllReservationsView;

		public GetAllReservationsViewHub(Lazy<IHubContext> context, IGetAllReservationsView getAllReservationsView)
		{
			_context = context;
			_getAllReservationsView = getAllReservationsView;
		}

		public void Handle(ReservationCreated domainEvent)
		{
			_getAllReservationsView.Handle(domainEvent);
			_context.Value.Clients.All.reservationCreated(_getAllReservationsView.Reservations);
		}

		public void Handle(ReservationOutboundDateTimeChanged domainEvent)
		{
			_getAllReservationsView.Handle(domainEvent);
			_context.Value.Clients.All.reservationOutboundDateTimeChanged(_getAllReservationsView.Reservations.SingleOrDefault(r => r.Id == domainEvent.AggregateRootId));
		}

		public void Handle(ReservationDeleted domainEvent)
		{
			_getAllReservationsView.Handle(domainEvent);
			_context.Value.Clients.All.reservationDeleted(_getAllReservationsView.Reservations);
		}

		public List<ReservationViewModel> Reservations
		{
			get { return _getAllReservationsView.Reservations; }
			set { _getAllReservationsView.Reservations = value; }
		}
	}
}