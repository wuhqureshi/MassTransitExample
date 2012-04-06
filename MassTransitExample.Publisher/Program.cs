using System;
using MassTransit;
using MassTransit.Distributor;
using MassTransitExample.Messages;
using MassTransit.Log4NetIntegration;
using log4net.Config;

namespace MassTransitExample.Publisher
{
	class Program
	{
		static void Main(string[] args)
		{
			XmlConfigurator.Configure();
			Console.Write("Starting publisher...");

			var bus = ServiceBusFactory.New(sbc =>
			{
				sbc.UseMulticastSubscriptionClient();
				sbc.SetNetwork("localhost");

				sbc.UseMsmq();
				sbc.VerifyMsmqConfiguration();
				sbc.ReceiveFrom("msmq://localhost/test_publisher");

				sbc.UseControlBus();

				sbc.UseDistributorFor<TestMessage>();

				sbc.UseLog4Net();
			});

			Console.WriteLine("Started");

			var messagePublisher = new TestMessagePublisher(bus);
			messagePublisher.SendMessages(500);
			
			Console.WriteLine("Press a key to end publisher...");
			Console.ReadKey();
		}
	}
}
