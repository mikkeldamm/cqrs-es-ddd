using System;
using System.Web.Http;
using Reservation.Commands.Commands;
using Reservation.Commands.Configuration;

namespace Reservation.Commands.Controllers
{
	public class ReservationController : ApiController
	{
		private readonly IProcessCommands _commandProcessor;

		public ReservationController(IProcessCommands commandProcessor)
		{
			_commandProcessor = commandProcessor;
		}

		[HttpPost]
		public void Post(string id, [FromBody]ReservationDetails details)
		{
			_commandProcessor.Process(new CreateReservationCommand(id));
			_commandProcessor.Process(new ChangeReservationOutboundDateCommand(id) { DateTime = details.OutboundDateTime });
		}

		[HttpPut]
		public void Put(string id, [FromBody]ReservationDetails details)
		{
			_commandProcessor.Process(new ChangeReservationOutboundDateCommand(id) { DateTime = details.OutboundDateTime });
		}

		[HttpDelete]
		public void Delete(string id)
		{
			_commandProcessor.Process(new DeleteReservationCommand(id));
		}

		public class ReservationDetails
		{
			public DateTime OutboundDateTime { get; set; }
		}
	}
}
