
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalentAtmDAL;

namespace TalentAtmClient.Atm.UI
{
    public partial class RunClient
    {
        public static int _choice;
        public static int _choiceAgain;


        public async Task Run()
        {


            using (ITalentAtmService talentAtmService = new TalentAtmService(new TalentAtmDBContext()))
            {
                await WelcomeMethod(talentAtmService);

            }


        }

        private static async Task WelcomeMethod(ITalentAtmService talentAtmService)
        {
            Utility.PrintColorMessage(ConsoleColor.Yellow, "***** Welcome to the Talent Bank ******");

            Screen.ShowMenuOne();

            Utility.PrintColorMessage(ConsoleColor.Cyan, "\nPlease enter an option:");

            _choice = int.Parse(Console.ReadLine());


            switch (_choice)
            {
                case 1:
                    await RunVerification(talentAtmService);
                    break;
                case 2:
                    Environment.Exit(0);

                    break;

            }
        }




       

       
    }

}

