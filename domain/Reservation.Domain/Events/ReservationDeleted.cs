using System;

namespace Reservation.Domain.Events
{
	public class ReservationDeleted : DomainEvent
	{
		public string Id { get; set; }
	}
}