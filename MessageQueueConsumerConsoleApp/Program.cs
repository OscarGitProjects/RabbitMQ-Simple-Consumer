using MessageQueueConsumerConsoleApp.Menu;
using MessageQueueConsumerConsoleApp.UI;

namespace MessageQueueConsumerConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            Program program = new Program();
            program.RunProgram(new ConsoleUI());
            //Console.WriteLine("Press a key to close application");
            //Console.ReadLine();
        }


        /// <summary>
        /// Method show main menu where a user selects functions
        /// </summary>
        /// <param name="ui">Reference to user interface</param>
        public void RunProgram(IUI ui)
        {
            try
            {
                MenuFactory menuFactory = new MenuFactory();
                IQueueConsumer? consumer = null;
                String? strInput = String.Empty;

                do
                {
                    strInput = String.Empty;

                    ui.Clear();

                    // Show menu
                    ui.WriteLine(menuFactory.GetMenu(MenuFactory.MenuType.Main_Menu));

                    // Read input from console
                    strInput = ui.ReadLine();

                    if (!String.IsNullOrWhiteSpace(strInput))
                    {
                        strInput = strInput.Trim();

                        if (strInput.Equals("0"))
                        {// Exit program
                            return;
                        }
                        else if (strInput.Equals("1"))
                        {
                            consumer = new SimpleQueueConsumer(ui);
                            consumer.Run();                           

                            ui.WriteLine("Press a key to continue");
                            ui.ReadLine();
                        }
                        else if (strInput.Equals("2"))
                        {
                            consumer = new ExchangeQueueConsumer(ui);
                            consumer.Run();

                            ui.WriteLine("Press a key to continue");
                            ui.ReadLine();
                        }
                    }


                }
                while (true);
            }
            catch (Exception exc)
            {
                ui.WriteLine("Program->RunProgram() exception: " + exc.ToString());
            }
        }
    }
}
