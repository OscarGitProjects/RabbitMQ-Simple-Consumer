using RabbitMQ.Client;

namespace MessageQueueConsumerConsoleApp
{
    public class Program : SimpleQueueConsumer
    {
        static void Main(string[] args)
        {
            Program program = new Program();
            IModel? channel = null;

            try
            {
                SimpleQueueConsumer consumer = new SimpleQueueConsumer();

                // Create a IModel channel to RabbitMQ
                channel = consumer.CreateChannel("guest", "guest", "/", "localhost", 5672);

                Console.WriteLine("Running SimpleQueueConsumer...");
                Console.WriteLine("Press a key to stop running");

                // Read messages. Press a key to stop running
                consumer.ReadSimpleMessage(channel, "simpleMessage");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Program->Main() exception: " + ex.ToString());
            }
            finally
            {
                if (channel != null)
                    channel.Close();
            }


            Console.WriteLine("Press a key to close application");
            Console.ReadLine();
        }
    }
}
