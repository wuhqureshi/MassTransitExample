using System;
using MassTransit;
using MassTransit.Distributor;
using MassTransit.Log4NetIntegration;
using MassTransit.Transports.Msmq;
using MassTransitExample.Messages;

namespace MassTransitExample.Consumer
{
	public class TestMessageConsumer
	{
		private readonly IServiceBus _bus;
		private readonly IEndpointManagement _management;
		private readonly IEndpointManagement _errorManageMent;

		public TestMessageConsumer(string queueAddress)
		{
			_bus = ServiceBusFactory.New(sbc =>
			{
				sbc.UseMulticastSubscriptionClient();
				sbc.SetNetwork("localhost");

				sbc.UseMsmq();
				sbc.VerifyMsmqConfiguration();
				sbc.ReceiveFrom(queueAddress);

				sbc.UseControlBus();

				sbc.ImplementDistributorWorker<TestMessage>(message => Consume, 1, 1);

				sbc.UseLog4Net();
			});

			_management = MsmqEndpointManagement.New(_bus.Endpoint.Address);
			_errorManageMent = MsmqEndpointManagement.New(_bus.Endpoint.ErrorTransport.Address);
		}

		public void Consume(TestMessage message)
		{
			Console.WriteLine(message.Text + " -- RemainingMessages: " + _management.Count() + " -- Error Count: " + _errorManageMent.Count());
		}
	}
}