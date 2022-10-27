namespace MessageQueueConsumerConsoleApp.UI
{
    public class ConsoleUI : IUI
    {
        public void Clear()
        {
            Console.Clear();

            //Console.CursorVisible = false;
            //Console.SetCursorPosition(0, 0);
        }

        public void WriteLine(string strText)
        {
            Console.WriteLine(strText);
        }

        public void Write(string strText)
        {
            Console.Write(strText);
        }

        public string? ReadLine()
        {
            return Console.ReadLine();
        }
    }
}
