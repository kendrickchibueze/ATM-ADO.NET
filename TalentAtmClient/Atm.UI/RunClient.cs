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

       


        public static async Task WelcomeMethod(ITalentAtmService talentAtmService)
        {

        inputTry: Utility.PrintColorMessage(ConsoleColor.Cyan, "\nPlease enter an option:");
            try
            {
                Screen.ShowMenuOne();



                _choice = int.Parse(Console.ReadLine());


                switch (_choice)
                {
                    case 1:
                        await RunVerification(talentAtmService);
                        break;
                    case 2:
                        Environment.Exit(0);
                        break;
                    default:
                        Utility.PrintColorMessage(ConsoleColor.Red, "\nInvalid input. Please enter a valid option.");
                        goto inputTry;
                        break;
                }
            }
            catch (FormatException)
            {
                Utility.PrintColorMessage(ConsoleColor.Red, "\nInvalid input format. Please enter a valid integer option.");
                goto inputTry;
            }
            catch (Exception ex)
            {
                Utility.PrintColorMessage(ConsoleColor.Red, $"\nAn error occurred: {ex.Message}");
                goto inputTry;
            }
        }







    }

}

