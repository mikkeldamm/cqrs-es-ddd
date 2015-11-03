using Reservation.Commands.Configuration;

namespace Reservation.Commands.Commands
{
	public class CreateReservationCommand : Command<Domain.Models.Reservation>
	{
		public CreateReservationCommand(string id) : base(id)
		{
		}

		public override void Execute(Domain.Models.Reservation aggregate)
		{
			aggregate.Create();
		}
	}
}