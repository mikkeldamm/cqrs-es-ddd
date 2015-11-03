using System;

namespace Reservation.Domain.Events
{
	public class ReservationCreated : DomainEvent
	{
		public string Id { get; set; }
	}
}