using System;
using MassTransit;
using MassTransit.Distributor.Messages;
using MassTransit.Transports.Msmq;
using MassTransitExample.Messages;
using MassTransit.Context;

namespace MassTransitExample.Consumer
{
	public class TestMessageConsumer
	{
		private IServiceBus _bus;
		private IEndpointManagement _management;
		private IEndpointManagement _errorManageMent;

		public IServiceBus ServiceBus { get { return _bus; } set { _bus = value; } }

		public void Start()
		{
			_management = MsmqEndpointManagement.New(_bus.Endpoint.Address);
			_errorManageMent = MsmqEndpointManagement.New(_bus.Endpoint.ErrorTransport.Address);
		}

		public void Consume(TestMessage message)
		{
			var context = _bus.MessageContext<Distributed<TestMessage>>();
			var retryCount = context.RetryCount;

			if (retryCount == 0)
			{
				context.RetryLater();
			}
			else
			{
				Console.WriteLine(message.Text + " -- Retry Count: " + retryCount + " -- RemainingMessages: " + _management.Count() + " -- Error Count: " + _errorManageMent.Count());
			}
		}
	}
}