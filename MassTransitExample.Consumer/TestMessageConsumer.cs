using System;
using MassTransit;
using MassTransitExample.Messages;

namespace MassTransitExample.Consumer
{
	public class TestMessageConsumer : Consumes<TestMessage>.All
	{

		private IServiceBus _bus;
		private UnsubscribeAction _unsubscribeAction;

		public TestMessageConsumer(IServiceBus bus)
		{
			_bus = bus;
		}

		public void Start()
		{
			_unsubscribeAction = _bus.SubscribeInstance(this);
		}

		public void Stop()
		{
			_unsubscribeAction();
		}


		public void Consume(TestMessage message)
		{
			Console.WriteLine(message.Text);
		}
	}
}