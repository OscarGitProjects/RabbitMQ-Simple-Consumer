using System.Text;

namespace MessageQueueConsumerConsoleApp.Menu
{
    public class MenuFactory
    {
        public enum MenuType
        {
            NA=0,
            Main_Menu=1,
        }


        /// <summary>
        /// Method return menu text as a string
        /// </summary>
        /// <param name="menuType">enum MenuType with the type of menu we want</param>
        /// <returns>string with the text for a menu</returns>
        public string GetMenu(MenuType menuType)
        {
            string strMenu = String.Empty;
            StringBuilder strBuilder = new StringBuilder();

            switch(menuType)
            {
                case MenuType.Main_Menu:
                    strBuilder.AppendLine("Main menu");
                    strBuilder.AppendLine("Select number for desired function");
                    strBuilder.AppendLine("0. Exit program");
                    strBuilder.AppendLine("1. Read simple messages");
                    strBuilder.AppendLine("2. Read Direct Exchange messages");
                    strBuilder.AppendLine("3. Read Topic Exchange messages");
                    strMenu = strBuilder.ToString();
                    break;
                default:
                    break;
            }

            return strMenu;
        }
    }
}
