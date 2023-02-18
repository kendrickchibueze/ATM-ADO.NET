
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalentAtmDAL;

namespace TalentAtmClient.Atm.UI
{
    public class RunClient
    {
        public static int _choice;
        public static int _choiceAgain;


        public async Task Run()
        {


            using (ITalentAtmService talentAtmService = new TalentAtmService(new TalentAtmDBContext()))
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

       
        public static  void NextAction()
        {

            Utility.PrintColorMessage(ConsoleColor.Yellow, "\nWhat would you like to do next?");

            

            Utility.PrintColorMessage(ConsoleColor.Cyan, "\nEnter 1 to go to main menu or 2 to exit...\nPlease enter an option:");

            int _choiceNext = int.Parse(Console.ReadLine());

            switch (_choiceNext)
            {
                case 1:
                    Console.Clear();
                    Screen.ShowMenuTwo();
                    break;
                case 2:
                    Environment.Exit(0);

                    break;


                


            }


        }


        private static async Task RunVerification(ITalentAtmService talentAtmService)
        {
            try
            {
                Console.WriteLine("Enter your account number:");
                int accountNumber = int.Parse(Console.ReadLine());

                Console.WriteLine("Enter your card number:");
                long cardNumber = long.Parse(Console.ReadLine());

                Console.WriteLine("Enter your PIN code:");
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
                    Console.WriteLine("Verification successful.");
                    Screen.ShowMenuTwo();

                    _choiceAgain = int.Parse(Console.ReadLine());

                    while (true)
                    {


                        switch (_choiceAgain)
                        {
                            case 1:

                                await talentAtmService.CheckBalance(bankAccount);
                                NextAction();

                                break;
                            case 2:

                            input: Console.WriteLine("Enter the amount to deposit:");

                                try
                                {

                                    decimal depositAmount = decimal.Parse(Console.ReadLine());

                                    await talentAtmService.DepositMoney(bankAccount, depositAmount);


                                }
                                catch (FormatException)
                                {
                                    Console.WriteLine("Invalid input. Please enter a valid decimal number.");
                                    goto input;
                                }

                                NextAction();

                                break;



                            case 3:


                            inputAgain: Console.WriteLine("Enter the amount to withdraw:");

                                try
                                {
                                    decimal withdrawalAmount = decimal.Parse(Console.ReadLine());

                                    await talentAtmService.MakeWithdrawalAsync(bankAccount, withdrawalAmount);
                                }
                                catch (FormatException)
                                {
                                    Console.WriteLine("Invalid input. Please enter a valid decimal number.");
                                    goto inputAgain;
                                }

                                NextAction();
                                break;

                            case 4:



                            inputTwo: Console.WriteLine("Enter the recipient's account number:");
                                try
                                {

                                    int recipientAccountNumber = int.Parse(Console.ReadLine());

                                    Console.WriteLine("Enter the transfer amount:");
                                    decimal transferAmount = decimal.Parse(Console.ReadLine());

                                    VmTransfer transfer = new VmTransfer
                                    {
                                        RecipientBankAccountNumber = recipientAccountNumber,
                                        TransferAmount = transferAmount
                                    };

                                    await talentAtmService.performTransferAsync(bankAccount, transfer);


                                }
                                catch (FormatException ex)
                                {
                                    Console.WriteLine("Invalid input. Please enter a valid number.");
                                    goto inputTwo;

                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("An unexpected error occurred: " + ex.Message);
                                    goto inputTwo;

                                }
                                NextAction();
                                break;

                            case 5:
                                await talentAtmService.ViewAllTransactions(bankAccount);

                                break;

                            case 6:
                                Environment.Exit(0);

                                break;
                            default:
                                NextAction();
                                break;


                        }

                    }

                }
                else
                {
                    Console.WriteLine("Verification failed.");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input format. Please enter a valid number.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }

}

