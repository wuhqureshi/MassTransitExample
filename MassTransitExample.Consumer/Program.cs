using System;
using log4net.Config;

namespace MassTransitExample.Consumer
{
	class Program
	{
		static void Main(string[] args)
		{
			XmlConfigurator.Configure();
			Console.Write("Starting consumer...");

			var cons = new TestMessageConsumer("msmq://localhost/test_consumer");
			
			Console.WriteLine("Started");


			Console.WriteLine("Press a key to end consumer...");
			Console.ReadKey();
		}
	}
}
