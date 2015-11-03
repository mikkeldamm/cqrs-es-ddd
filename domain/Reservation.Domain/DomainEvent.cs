using System;

namespace Reservation.Domain
{
	public class DomainEvent
	{
		public string AggregateRootId { get; set; }
	}



	public interface IEmit<in TDomainEvent> where TDomainEvent : DomainEvent
	{
		void Apply(TDomainEvent domainEvent);
	}

	public interface ISubscribeTo<in TDomainEvent> where TDomainEvent : DomainEvent
	{
		void Handle(TDomainEvent domainEvent);
	}



	public interface IRegisterEventHandlers
	{
		void Register<TDomainEvent>(Action<TDomainEvent> handlerForEvent) where TDomainEvent : DomainEvent;
	}

	public interface IPublishEvents
	{
		void Publish<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : DomainEvent;
	}

	public interface IReceiveEvents
	{
		void StartListener();
	}
}