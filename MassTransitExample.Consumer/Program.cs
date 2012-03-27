using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MassTransit;
using MassTransit.Distributor;
using MassTransitExample.Messages;
using MassTransit.Log4NetIntegration;
using log4net.Config;

namespace MassTransitExample.Consumer
{
	class Program
	{
		static void Main(string[] args)
		{
			XmlConfigurator.Configure();
			Console.WriteLine("Press a key to start consumer...");
			Console.ReadKey();

			var bus = ServiceBusFactory.New(sbc =>
			{
				sbc.UseMulticastSubscriptionClient();

				sbc.UseMsmq();
				sbc.VerifyMsmqConfiguration();
				sbc.ReceiveFrom("msmq://localhost/test_consumer");

				sbc.UseControlBus();
				sbc.ImplementDistributorWorker<TestMessage>(null, 1, 1);
				//sbc.Subscribe(s => s.Consumer<TestMessageConsumer>());
				sbc.UseLog4Net();
			});

			var consumer = new TestMessageConsumer(bus);

			consumer.Start();

			Console.WriteLine("Press a key to end consumer...");
			Console.ReadKey();
		}
	}
}
