namespace TalentAtmClient.Atm.UI
{
    public class Utility
    {




        public static async Task printDotAnimation(int timer = 10)
        {
            for (var x = 0; x < timer; x++)
            {
                PrintColorMessage(ConsoleColor.Yellow, ".");

                Task.Delay(100);
            }
            Console.WriteLine();
        }



        public static void PrintColorMessage(ConsoleColor color, string message)
        {

            Console.ForegroundColor = color;

            //tell user its not a number
            Console.WriteLine(message);

            //reset text color
            Console.ResetColor();
        }
    }
}
