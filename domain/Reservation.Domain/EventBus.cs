using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;

namespace Reservation.Domain
{
	public class EventBus : IRegisterEventHandlers, IPublishEvents, IReceiveEvents
	{
		public const string BusConnectionString = "connstringhere";
		public const string BusTopicName = "ReservationTopic";
		
		private readonly TopicClient _client;
		private readonly Dictionary<string, List<Action<BrokeredMessage>>> _handlers = new Dictionary<string, List<Action<BrokeredMessage>>>();

		public EventBus()
		{
			var namespaceManager = NamespaceManager.CreateFromConnectionString(BusConnectionString);
			if (!namespaceManager.TopicExists(BusTopicName))
			{
				namespaceManager.CreateTopic(BusTopicName);
			}

			if (!namespaceManager.SubscriptionExists(BusTopicName, "AllMessages"))
			{
				namespaceManager.CreateSubscription(BusTopicName, "AllMessages");
			}

			_client = TopicClient.CreateFromConnectionString(BusConnectionString, BusTopicName);
		}

		public void Register<TDomainEvent>(Action<TDomainEvent> handler) where TDomainEvent : DomainEvent
		{
			List<Action<BrokeredMessage>> handlers;

			if (!_handlers.TryGetValue(typeof(TDomainEvent).AssemblyQualifiedName, out handlers))
			{
				handlers = new List<Action<BrokeredMessage>>();
				_handlers.Add(typeof(TDomainEvent).AssemblyQualifiedName, handlers);
			}

			handlers.Add((message) => { handler(message.GetBody<TDomainEvent>()); });
		}

		public void Publish<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : DomainEvent
		{
			var message = new BrokeredMessage(domainEvent);
			
            message.Properties["DomainEventType"] = domainEvent.GetType().AssemblyQualifiedName;

			_client.Send(message);
		}

		public void StartListener()
		{
			var client = SubscriptionClient.CreateFromConnectionString(BusConnectionString, BusTopicName, "AllMessages");
			var options = new OnMessageOptions
			{
				AutoComplete = false,
				AutoRenewTimeout = TimeSpan.FromMinutes(1)
			};

			client.OnMessage((message) =>
			{
				try
				{
					var type = message.Properties["DomainEventType"] as string;

					List<Action<BrokeredMessage>> handlers;

					if (!_handlers.TryGetValue(type, out handlers))
					{
						return;
					}

					foreach (var handler in handlers)
					{
						handler(message);
					}

					// Remove message from subscription.
					message.Complete();
				}
				catch (Exception e)
				{
					// Indicates a problem, unlock message in subscription.
					message.Abandon();
				}

			}, options);
		}
	}
}
