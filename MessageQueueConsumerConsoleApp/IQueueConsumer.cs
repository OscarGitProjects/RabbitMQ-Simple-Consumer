using RabbitMQ.Client;

namespace MessageQueueConsumerConsoleApp
{
    public interface IQueueConsumer
    {
        void ReadMessage(IModel channel, String strQueueName = "default-message-queue", String strExchangeName = "default-exchange", String strRoutingKey = "acount.init");
        void Run();
    }
}