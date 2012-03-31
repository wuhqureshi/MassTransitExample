using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MassTransit;
using MassTransitExample.Messages;

namespace MassTransitExample.Publisher
{
	public class TestMessagePublisher
	{
		private readonly IServiceBus _bus;

		public TestMessagePublisher(IServiceBus bus)
		{
			_bus = bus;
		}

		/// <summary>
		/// Sends a batch of messages to subscribed consumers
		/// </summary>
		/// <param name="messageCount">Number of messages to send</param>
		/// <param name="interval">interval between each message in milliseconds</param>
		public void SendMessages(int messageCount, int interval)
		{
			for (int i = 1; i <= messageCount; i++)
			{
				Thread.Sleep(interval);
				_bus.Publish(new TestMessage { Text = "This is my test message " + i });
			}
		}
	}
}
