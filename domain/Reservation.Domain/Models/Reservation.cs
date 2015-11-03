using Reservation.Domain.Events;
using System;

namespace Reservation.Domain.Models
{
	public class Reservation : AggregateRoot,
		IEmit<ReservationCreated>,
		IEmit<ReservationOutboundDateTimeChanged>,
		IEmit<ReservationDeleted>
	{
		public DateTime OutboundDateTime { get; set; }

		public void Create()
		{
			Emit(new ReservationCreated { Id = Id });
		}

		public void Apply(ReservationCreated domainEvent)
		{
			Id = domainEvent.Id;
		}

		public void ChangeOutboundDateTime(DateTime dateTime)
		{
			Emit(new ReservationOutboundDateTimeChanged { DateTime = dateTime });
		}

		public void Apply(ReservationOutboundDateTimeChanged domainEvent)
		{
			OutboundDateTime = domainEvent.DateTime;
		}

		public void Delete()
		{
			Emit(new ReservationDeleted { Id = Id });
		}

		public void Apply(ReservationDeleted domainEvent)
		{
		}
	}
}