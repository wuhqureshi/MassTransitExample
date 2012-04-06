using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MassTransit;
using MassTransit.Distributor;
using MassTransit.Log4NetIntegration;
using MassTransitExample.Messages;
using StructureMap;

namespace MassTransitExample.Consumer.IoC
{
	public static class StructureMap
	{
		public static IContainer Init()
		{
			ObjectFactory.Initialize(x =>
			                         	{
			                         		var consumer = new TestMessageConsumer();

											var serviceBus = ServiceBusFactory.New(sbc =>
											{
												sbc.UseMulticastSubscriptionClient();
												sbc.SetNetwork("localhost");

												sbc.UseMsmq();
												sbc.VerifyMsmqConfiguration();
												sbc.ReceiveFrom("msmq://localhost/test_consumer");

												sbc.UseControlBus();

												sbc.ImplementDistributorWorker<TestMessage>(msg => consumer.Consume, 1, 1);

												sbc.UseLog4Net();
											});

			                         		consumer.ServiceBus = serviceBus;

			                         		x.For<TestMessageConsumer>()
			                         			.Singleton()
			                         			.Use(consumer);
			                         	});

			return ObjectFactory.Container;
		}
	}
}
