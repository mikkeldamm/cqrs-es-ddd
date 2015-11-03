using System;
using Reservation.Commands.Configuration;

namespace Reservation.Commands.Commands
{
	public class ChangeReservationOutboundDateCommand : Command<Domain.Models.Reservation>
	{
		public DateTime DateTime { get; set; }

		public ChangeReservationOutboundDateCommand(string id) : base(id)
		{
		}

		public override void Execute(Domain.Models.Reservation aggregate)
		{
			aggregate.ChangeOutboundDateTime(DateTime);
		}
	}
}