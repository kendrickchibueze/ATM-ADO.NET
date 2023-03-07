
using TalentAtmDAL;

namespace TalentAtmClient.Atm.UI
{
    public partial class RunClient
    {

        private static async Task RunVerification(ITalentAtmService talentAtmService)
        {

            bool exit = false;
            do
            {
            checkInput: Utility.PrintColorMessage(ConsoleColor.Cyan, "Enter your account number:");
                try
                {
                    int accountNumber = int.Parse(Console.ReadLine());

                    Utility.PrintColorMessage(ConsoleColor.Cyan, "Enter your card number:");

                    long cardNumber = long.Parse(Console.ReadLine());

                    Utility.PrintColorMessage(ConsoleColor.Cyan, "Enter your PIN code:");

                    int pinCode = int.Parse(Console.ReadLine());

                    BankAccounts bankAccount = new BankAccounts
                    {
                        AccountNumber = accountNumber,
                        CardNumber = cardNumber,
                        PinCode = pinCode
                    };

                    bool isVerified = await talentAtmService.verifyCardNumberPassword(bankAccount);

                    if (isVerified)
                    {
                        Utility.PrintColorMessage(ConsoleColor.Green, "Verification successful.");

                        await Task.Delay(1000);

                        Screen.ShowMenuTwo();

                        _choiceAgain = int.Parse(Console.ReadLine());


                        switch (_choiceAgain)
                        {
                            case 1:

                                await talentAtmService.CheckBalance(bankAccount);

                                await talentAtmService.InsertTransaction(bankAccount, bankAccount.AccountNumber, bankAccount.Balance, "Balance Enquiry");

                                await Task.Delay(3000);


                                await NextActions(talentAtmService, bankAccount);



                                break;

                            case 2:




                                await RunDepositMoney(talentAtmService, bankAccount);

                                await NextActions(talentAtmService, bankAccount);



                                break;



                            case 3:




                                await RunMakeWithdrawalAsync(talentAtmService, bankAccount);

                                await NextActions(talentAtmService, bankAccount);



                                break;

                            case 4:



                                await RunPerformTransferAsync(talentAtmService, bankAccount);

                                await NextActions(talentAtmService, bankAccount);



                                break;

                            case 5:

                                await talentAtmService.ViewAllTransactions(bankAccount.AccountNumber);

                                await Task.Delay(6500);

                                Console.Clear();

                                await NextActions(talentAtmService, bankAccount);



                                break;

                            case 6:

                                Environment.Exit(0);

                                break;

                            default:

                                await NextActions(talentAtmService, bankAccount);



                                break;


                        }

                    }
                    else
                    {
                        Utility.PrintColorMessage(ConsoleColor.Red, "Verification failed.");

                        goto checkInput;
                    }
                }
                catch (FormatException)
                {
                    Utility.PrintColorMessage(ConsoleColor.Red, "Invalid input format. Please enter a valid number.");

                    goto checkInput;

                }
                catch (Exception ex)
                {
                    Utility.PrintColorMessage(ConsoleColor.Red, $"An error occurred: {ex.Message}");

                    goto checkInput;
                }


                Console.Clear();


            } while (!exit);

        }





        private static async Task RunDepositMoney(ITalentAtmService talentAtmService, BankAccounts bankAccount)
        {

        input: Utility.PrintColorMessage(ConsoleColor.Cyan, "Enter the amount to deposit:");

            try
            {

                decimal depositAmount = decimal.Parse(Console.ReadLine());

                if (depositAmount < 0)
                {
                    Utility.PrintColorMessage(ConsoleColor.Red, "Error: Cannot deposit a negative amount.");
                    goto input;

                }

                await talentAtmService.DepositMoney(bankAccount, depositAmount);

                await talentAtmService.InsertTransaction(bankAccount, bankAccount.AccountNumber, depositAmount, "Cash Deposit");


            }
            catch (FormatException)
            {
                Utility.PrintColorMessage(ConsoleColor.Cyan, "Invalid input. Please enter a valid decimal number.");

                goto input;
            }

            await Task.Delay(3000);

            Console.Clear();

        }

        private static async Task RunPerformTransferAsync(ITalentAtmService talentAtmService, BankAccounts bankAccount)
        {

        inputTwo: Utility.PrintColorMessage(ConsoleColor.Cyan, "Enter the recipient's account number:");
            try
            {

                int recipientAccountNumber = int.Parse(Console.ReadLine());

                Utility.PrintColorMessage(ConsoleColor.Cyan, "Enter the transfer amount:");

                decimal transferAmount = decimal.Parse(Console.ReadLine());
                if (transferAmount < 0)
                {
                    Utility.PrintColorMessage(ConsoleColor.Red, "Error: Cannot transfer a negative amount.");
                    goto inputTwo;
                    
                }

                VmTransfer transfer = new VmTransfer
                {
                    RecipientBankAccountNumber = recipientAccountNumber,

                    TransferAmount = transferAmount
                };

                await talentAtmService.performTransferAsync(bankAccount, transfer);

                await talentAtmService.InsertTransaction(bankAccount, bankAccount.AccountNumber, transferAmount, "Transfer");


            }
            catch (FormatException ex)
            {
                Utility.PrintColorMessage(ConsoleColor.Red, "Invalid input. Please enter a valid number.");

                goto inputTwo;

            }
            catch (Exception ex)
            {
                Utility.PrintColorMessage(ConsoleColor.Red, "An unexpected error occurred: " + ex.Message);

                goto inputTwo;

            }

            await Task.Delay(3000);
        }



        private static async Task RunMakeWithdrawalAsync(ITalentAtmService talentAtmService, BankAccounts bankAccount)
        {


        inputAgain: Utility.PrintColorMessage(ConsoleColor.Cyan, "Enter the amount to withdraw:");

            try
            {
                decimal withdrawalAmount = decimal.Parse(Console.ReadLine());

                if(withdrawalAmount <0)
                {
                    Utility.PrintColorMessage(ConsoleColor.Red, "Error: Cannot withdraw a negative amount.");
                    goto inputAgain;
                }

                await talentAtmService.MakeWithdrawalAsync(bankAccount, withdrawalAmount);

                await talentAtmService.InsertTransaction(bankAccount, bankAccount.AccountNumber, withdrawalAmount, "Withdrawal");
            }
            catch (FormatException)
            {
                Utility.PrintColorMessage(ConsoleColor.Cyan, "Invalid input. Please enter a valid decimal number.");

                goto inputAgain;
            }
            await Task.Delay(3000);
        }




    }
}