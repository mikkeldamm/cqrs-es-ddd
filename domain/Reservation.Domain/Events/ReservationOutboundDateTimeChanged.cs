using System;

namespace Reservation.Domain.Events
{
	public class ReservationOutboundDateTimeChanged : DomainEvent
	{
		public DateTime DateTime { get; set; }
	}
}