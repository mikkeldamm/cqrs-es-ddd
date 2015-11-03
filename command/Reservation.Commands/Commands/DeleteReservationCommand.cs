using Reservation.Commands.Configuration;

namespace Reservation.Commands.Commands
{
	public class DeleteReservationCommand : Command<Domain.Models.Reservation>
	{
		public DeleteReservationCommand(string id) : base(id)
		{
		}

		public override void Execute(Domain.Models.Reservation aggregate)
		{
			aggregate.Delete();
		}
	}
}