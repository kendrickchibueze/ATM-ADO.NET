using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalentAtmDAL;

namespace TalentAtmClient.Atm.UI
{
    public class LoginAdminUser
    {
          


        public static async Task Login()
        {
           input: Utility.PrintColorMessage(ConsoleColor.Yellow, "\n press 1 to Create your DB/Tables or 2 to continue if you already have a DB...");
            try
            {
                

                int choicy = int.Parse(Console.ReadLine());


                switch (choicy)
                {
                    case 1:

                        await TalentAtmDB.CreateDBAndTables();

                        Thread.Sleep(2500);

                        RunClient client = new RunClient();

                        await client.Run();

                        break;
                    case 2:

                        Utility.printDotAnimation(10);

                        RunClient clientTwo = new RunClient();

                        await clientTwo.Run();


                        break;
                }

            }
            catch (FormatException)
            {
                Utility.PrintColorMessage(ConsoleColor.Red, "Please enter a valid input");

                goto input;

            }catch(Exception e)
            {
                Utility.PrintColorMessage(ConsoleColor.Red, e.Message);

                goto input;
            }
        }
    }
}
