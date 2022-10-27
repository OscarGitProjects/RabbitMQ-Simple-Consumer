using MessageQueueConsumerConsoleApp.UI;
using RabbitMQ.Client;

namespace MessageQueueConsumerConsoleApp
{
    public class BaseQueueConsumer
    {
        /// <summary>
        /// Reference to a userinterface
        /// </summary>
        protected readonly IUI m_Ui;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ui">Reference to user interface</param>
        public BaseQueueConsumer(IUI ui)
        {
            this.m_Ui = ui;
        }

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
        protected IModel CreateChannel(String strUserName = "guest", String strPassword = "guest", String strVirtualHost = "/", String strHostName = "localhost", int iPortNumber = 5672)
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
                Console.WriteLine($"{nameof(BaseQueueConsumer)}->CreateChannel() exception: " + exc.ToString());
                throw;
            }
        }
    }
}
