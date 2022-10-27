namespace MessageQueueConsumerConsoleApp.UI
{
    public interface IUI
    {
        void Clear();
        string? ReadLine();
        void Write(string strText);
        void WriteLine(string strText);
    }
}
