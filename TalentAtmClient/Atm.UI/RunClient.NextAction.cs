using TalentAtmDAL;

namespace TalentAtmClient.Atm.UI
{
    public partial class RunClient
    {

        private static async Task NextActions(ITalentAtmService talentAtmService, BankAccounts bankAccount)
        {
            bool exit = false;

            do
            {
                Screen.ShowMenuTwo();

                Utility.PrintColorMessage(ConsoleColor.Yellow, "\nWhat would you like to do next?");

                Utility.PrintColorMessage(ConsoleColor.Cyan, "\nPlease enter an option:");

                int nextChoice = int.Parse(Console.ReadLine());

                _choiceAgain = nextChoice;

                try
                {
                    switch (_choiceAgain)
                    {
                        case 1:
                            await talentAtmService.CheckBalance(bankAccount);

                            await talentAtmService.InsertTransaction(bankAccount, bankAccount.AccountNumber, bankAccount.Balance, "Balance Enquiry");

                            await Task.Delay(3000);
                            break;

                        case 2:
                            await RunDepositMoney(talentAtmService, bankAccount);
                            break;

                        case 3:
                            await RunMakeWithdrawalAsync(talentAtmService, bankAccount);
                            break;

                        case 4:
                            await RunPerformTransferAsync(talentAtmService, bankAccount);
                            break;

                        case 5:
                            await talentAtmService.ViewAllTransactions(bankAccount.AccountNumber);

                            await Task.Delay(6500);

                            Console.Clear();

                            break;

                        case 6:
                            Environment.Exit(0);
                            break;

                        default:
                            break;
                    }
                }
                catch (FormatException ex)
                {
                    Utility.PrintColorMessage(ConsoleColor.Red, "Error: Invalid input. Please enter a number.");
                }
                catch (Exception ex)
                {
                    Utility.PrintColorMessage(ConsoleColor.Red, $"An error occurred: {ex.Message}");
                }

            } while (!exit);
        }



    }
}
