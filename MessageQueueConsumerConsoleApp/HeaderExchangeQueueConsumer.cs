using MessageQueueConsumerConsoleApp.UI;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace MessageQueueConsumerConsoleApp
{
    /// <summary>
    /// Header exchange routes messages based on header values and are very similar to Topic exchange.
    /// Topic exchange uses routing key, but it does not do an exact match on the routing key. 
    /// Instead it does a patterna match based on the pattern
    /// </summary>
    public class HeaderExchangeQueueConsumer : BaseQueueConsumer, IQueueConsumer
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ui">Reference to user interface</param>
        public HeaderExchangeQueueConsumer(IUI ui) : base(ui)
        { }


        /// <summary>
        /// Method read a message from a queue in RabbitMQ
        /// </summary>
        /// <param name="channel">IModel channel</param>
        /// <param name="strQueueName">Name of the queue</param>
        /// <param name="strExchangeName">Name of the exchange</param>
        /// <param name="strRoutingKey">Routing key</param>
        /// <exception cref="ArgumentNullException">Throws if reference to IModel channel is null</exception>
        /// <exception cref="Exception">Throws exception</exception>
        public void ReadMessage(IModel channel, string strQueueName = "default-message-queue", string strExchangeName = "default-exchange", string strRoutingKey = "acount.*")
        {
            if (channel == null)
                throw new ArgumentNullException($"{nameof(HeaderExchangeQueueConsumer)}->ReadMessage(). Reference to IModel channel is null");

            try
            {
                channel.ExchangeDeclare(
                    exchange: strExchangeName,
                    type: ExchangeType.Headers,
                    durable: false,
                    autoDelete: false,
                    arguments: null);

                channel.QueueDeclare(
                    queue: strQueueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var header = new Dictionary<string, Object> {{ "account", "new" }};

                channel.QueueBind(
                    queue: strQueueName,
                    exchange: strExchangeName,
                    routingKey: String.Empty,
                    arguments: header);

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
                Console.WriteLine($"{nameof(HeaderExchangeQueueConsumer)}->ReadMessage() exception: " + exc.ToString());
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
                string strExchangeName = "header-exchange";
                string strQueueName = "header-exchange-message-queue";
                string strRoutingKey = String.Empty;

                this.m_Ui.WriteLine("Running HeaderExchangeQueueConsumer...");
                this.m_Ui.WriteLine("Press a key to stop running");

                // Read messages. Press a key to stop running
                this.ReadMessage(channel, strQueueName, strExchangeName, strRoutingKey);
            }
            catch (Exception exc)
            {
                this.m_Ui.WriteLine($"{nameof(HeaderExchangeQueueConsumer)}->Run() exception: " + exc.ToString());
                throw;
            }
            finally
            {
                channel?.Close();
            }
        }
    }
}
