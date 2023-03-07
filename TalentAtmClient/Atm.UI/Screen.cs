namespace TalentAtmClient.Atm.UI
{
    public class Screen
    {


      


        public static void ShowMenuOne()
        {
            Console.Clear();
            Utility.PrintColorMessage(ConsoleColor.Cyan, "************Welcome To My Bank Atm App*************");
            Utility.PrintColorMessage(ConsoleColor.Yellow, " ------------------------");
            Utility.PrintColorMessage(ConsoleColor.Yellow, "| TalentBank ATM Menu    |");
            Utility.PrintColorMessage(ConsoleColor.Yellow, "|                        |");
            Utility.PrintColorMessage(ConsoleColor.Yellow, "| 1. Insert ATM card     |");
            Utility.PrintColorMessage(ConsoleColor.Yellow, "| 2. Exit                |");
            Utility.PrintColorMessage(ConsoleColor.Yellow, "|                        |");
            Utility.PrintColorMessage(ConsoleColor.Yellow, " ------------------------");
        }

        public static void ShowMenuTwo()
        {
            Console.Clear();
            Utility.PrintColorMessage(ConsoleColor.Yellow, " ---------------------------");
            Utility.PrintColorMessage(ConsoleColor.Yellow, "| TalentBank ATM Secure Menu |");
            Utility.PrintColorMessage(ConsoleColor.Yellow, "|                            |");
            Utility.PrintColorMessage(ConsoleColor.Yellow, "| 1. Balance Enquiry         |");
            Utility.PrintColorMessage(ConsoleColor.Yellow, "| 2. Cash Deposit            |");
            Utility.PrintColorMessage(ConsoleColor.Yellow, "| 3. Withdrawal              |");
            Utility.PrintColorMessage(ConsoleColor.Yellow, "| 4. Transfer                |");
            Utility.PrintColorMessage(ConsoleColor.Yellow, "| 5. Transactions            |");
            Utility.PrintColorMessage(ConsoleColor.Yellow, "| 6. Logout                  |");
            Utility.PrintColorMessage(ConsoleColor.Yellow, "|                            |");
            Utility.PrintColorMessage(ConsoleColor.Yellow, " ---------------------------");








        }



    }
}
