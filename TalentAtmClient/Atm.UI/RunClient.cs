using TalentAtmDAL;
using TalentAtmDAL.Services;

namespace TalentAtmClient.Atm.UI
{
    public partial class RunClient
    {
        public static int _choice;
        public static int _choiceAgain;


        public async Task Run()
        {
            Console.Clear();

            using (ITalentAtmService talentAtmService = new TalentAtmService(new TalentAtmDBContext()))
            {
                await WelcomeMethod(talentAtmService);

            }


        }

        private static async Task WelcomeMethod(ITalentAtmService talentAtmService)
        {

            Utility.PrintColorMessage(ConsoleColor.Cyan, "************Welcome To My Bank Atm App*************");

            Thread.Sleep(1000);

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

