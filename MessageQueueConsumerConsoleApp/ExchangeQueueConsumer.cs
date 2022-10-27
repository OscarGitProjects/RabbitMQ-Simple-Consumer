using MessageQueueConsumerConsoleApp.UI;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace MessageQueueConsumerConsoleApp
{
    public class ExchangeQueueConsumer : BaseQueueConsumer, IQueueConsumer
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ui">Reference to user interface</param>
        public ExchangeQueueConsumer(IUI ui) : base(ui)
        { }


        /// <summary>
        /// Method read a message from a queue in RabbitMQ
        /// </summary>
        /// <param name="channel">IModel channel</param>
        /// <param name="strQueueName">Name of the queue</param>
        /// <param name="strExchangeName">Name of the exchange</param>
        /// <exception cref="ArgumentNullException">Throws if reference to IModel channel is null</exception>
        /// <exception cref="Exception">Throws exception</exception>
        public void ReadMessage(IModel channel, String strQueueName = "default-message-queue", String strExchangeName = "default-exchange")
        {
            if (channel == null)
                throw new ArgumentNullException($"{nameof(ExchangeQueueConsumer)}->ReadMessage(). Reference to IModel channel is null");

            try
            {
                channel.ExchangeDeclare(
                    exchange: strExchangeName,
                    type: ExchangeType.Direct,
                    durable: false,
                    autoDelete: false,
                    arguments: null);

                channel.QueueDeclare(
                    queue: strQueueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                channel.QueueBind(
                    queue: strQueueName,
                    exchange: strExchangeName,
                    routingKey: strQueueName,
                    arguments: null);

                // Fetch 10 messages at the time
                //channel.BasicQos(0, 10, false);

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

                this.m_Ui.ReadLine();
            }
            catch (Exception exc)
            {
                Console.WriteLine($"{nameof(ExchangeQueueConsumer)}->ReadMessage() exception: " + exc.ToString());
                throw;
            }
        }


        /// <summary>
        /// Method is reading messages from RabbitMQ queue
        /// </summary>
        /// <exception cref="Exception">Throws exception</exception>
        public void Run()
        {
            IModel? channel = null;

            try
            {
                // Create a IModel channel to RabbitMQ
                channel = this.CreateChannel("guest", "guest", "/", "localhost", 5672);

                // Names for exchange and queue
                string strExchangeName = "direct-exchange";
                string strQueueName = "direct-exchange-message-queue";

                this.m_Ui.WriteLine("Running ExchangeQueueConsumer...");
                this.m_Ui.WriteLine("Press a key to stop running");

                // Read messages. Press a key to stop running
                this.ReadMessage(channel, strQueueName, strExchangeName);
            }
            catch (Exception exc)
            {
                this.m_Ui.WriteLine($"{nameof(ExchangeQueueConsumer)}->Run() exception: " + exc.ToString());
                throw;
            }
            finally
            {
                if (channel != null)
                    channel.Close();
            }
        }
    }
}
