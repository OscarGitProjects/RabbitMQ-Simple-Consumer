using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace MessageQueueConsumerConsoleApp
{
    public class SimpleQueueConsumer
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SimpleQueueConsumer() { }


        /// <summary>
        /// Method create a IModel channel to RabbitMQ
        /// </summary>
        /// <param name="strUserName">Username</param>
        /// <param name="strPassword">Password</param>
        /// <param name="strVirtualHost">Virtual host</param>
        /// <param name="strHostName">Hostname</param>
        /// <param name="iPortNumber">Port number</param>
        /// <returns>IModel channel</returns>
        /// <exception cref="Exception">Throws exception</exception>
        public IModel CreateChannel(String strUserName = "guest", String strPassword = "guest", String strVirtualHost = "/", String strHostName = "localhost", int iPortNumber = 5672)
        {
            try
            {
                var connectionFactory = new ConnectionFactory()
                {
                    HostName = strHostName,
                    Port = iPortNumber,
                    UserName = strUserName,
                    Password = strPassword,
                    VirtualHost = strVirtualHost
                };

                var connection = connectionFactory.CreateConnection();
                var channel = connection.CreateModel();
                return channel;
            }
            catch (Exception exc)
            {
                Console.WriteLine($"{nameof(SimpleQueueConsumer)}->CreateChannel() exception: " + exc.ToString());
                throw;
            }
        }


        /// <summary>
        /// Method read a message from a queue in RabbitMQ
        /// </summary>
        /// <param name="channel">IModel channel</param>
        /// <param name="strQueueName">Name of the queue</param>
        /// <exception cref="ArgumentNullException">Throws if reference to IModel channel is null</exception>
        /// <exception cref="Exception">Throws exception</exception>
        public void ReadSimpleMessage(IModel channel, String strQueueName = "simpleMessage")
        {
            if (channel == null)
                throw new ArgumentNullException($"{nameof(SimpleQueueConsumer)}->ReadSimpleMessage(). Reference to IModel channel is null");

            try
            {
                channel.QueueDeclare(queue: strQueueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var strMessage = Encoding.UTF8.GetString(body);
                    Console.WriteLine($"Recived message {strMessage} from RabbitMQ");
                };

                channel.BasicConsume(queue: strQueueName,
                                     autoAck: true,
                                     consumer: consumer);

                Console.ReadLine();
            }
            catch (Exception exc)
            {
                Console.WriteLine($"{nameof(SimpleQueueConsumer)}->ReadSimpleMessage() exception: " + exc.ToString());
                throw;
            }
        }
    }
}