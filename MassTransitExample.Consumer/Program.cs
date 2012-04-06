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

			//var cons = new TestMessageConsumer();
			var container = IoC.StructureMap.Init();
			var consumer = container.GetInstance<TestMessageConsumer>();
			consumer.Start();
			Console.WriteLine("Started");


			Console.WriteLine("Press a key to end consumer...");
			Console.ReadKey();
		}
	}
}
