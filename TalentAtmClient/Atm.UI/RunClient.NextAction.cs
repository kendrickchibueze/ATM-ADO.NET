using TalentAtmDAL;

namespace TalentAtmClient.Atm.UI
{
    public partial class RunClient
    {


        public static async Task NextAction(ITalentAtmService talentAtmService, BankAccounts bankAccount)
        {

            Console.Clear();

            Screen.ShowMenuTwo();

        next: Utility.PrintColorMessage(ConsoleColor.Yellow, "\nWhat would you like to do next?");

        tryAction: Utility.PrintColorMessage(ConsoleColor.Cyan, "\nPlease enter an option:");

            while (true)
            {
                int _choiceNext = int.Parse(Console.ReadLine());

                switch (_choiceNext)
                {
                    case 1:
                        await talentAtmService.CheckBalance(bankAccount);

                        goto next;

                        goto tryAction;

                        break;
                    case 2:
                        Utility.PrintColorMessage(ConsoleColor.Cyan, "please enter the deposit amount..");

                        decimal depositAmount = decimal.Parse(Console.ReadLine());

                        await talentAtmService.DepositMoney(bankAccount, depositAmount);

                        goto next;

                        goto tryAction;


                        break;
                    case 3:
                        Utility.PrintColorMessage(ConsoleColor.Cyan, "please enter the withdrawal amount..");

                        decimal withdrawalAmount = decimal.Parse(Console.ReadLine());

                        await talentAtmService.MakeWithdrawalAsync(bankAccount, withdrawalAmount);

                        goto next;

                        goto tryAction;

                        break;
                    case 4:

                        Utility.PrintColorMessage(ConsoleColor.Cyan, "enter the recipient account number...");

                        int recipientAccountNumber = int.Parse(Console.ReadLine());

                        Console.WriteLine("Enter the transfer amount:");

                        decimal transferAmount = decimal.Parse(Console.ReadLine());

                        VmTransfer transfer = new VmTransfer
                        {
                            RecipientBankAccountNumber = recipientAccountNumber,

                            TransferAmount = transferAmount
                        };

                        await talentAtmService.performTransferAsync(bankAccount, transfer);

                        goto next;

                        goto tryAction;

                        break;
                    case 5:
                        await talentAtmService.ViewAllTransactions(bankAccount, bankAccount.AccountNumber);

                        goto next;

                        goto tryAction;

                        break;
                    case 6:

                        Environment.Exit(0);

                        break;





                }
            }

        }
    }
}
