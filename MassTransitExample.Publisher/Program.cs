using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
			Console.WriteLine("Press a key to start publisher...");
			Console.ReadKey();

			var bus = ServiceBusFactory.New(sbc =>
			{
				sbc.UseMulticastSubscriptionClient();

				sbc.UseMsmq();
				sbc.VerifyMsmqConfiguration();
				sbc.ReceiveFrom("msmq://localhost/test_publisher");

				sbc.UseControlBus();
				sbc.UseDistributorFor<TestMessage>();
				sbc.UseLog4Net();
			});

			for (int i = 1; i <= 20; i++)
			{
				Thread.Sleep(500);
				bus.Publish(new TestMessage { Text = "This is my test message " + i });
			}
			
			Console.WriteLine("Press a key to end publisher...");
			Console.ReadKey();
		}
	}
}
