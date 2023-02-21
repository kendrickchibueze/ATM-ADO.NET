﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalentAtmDAL;

namespace TalentAtmClient.Atm.UI
{
    public partial class RunClient
    {

        private static async Task RunVerification(ITalentAtmService talentAtmService)
        {
            try
            {
                Utility.PrintColorMessage(ConsoleColor.Cyan, "Enter your account number:");

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

                    Screen.ShowMenuTwo();

                    _choiceAgain = int.Parse(Console.ReadLine());


                    switch (_choiceAgain)
                    {
                        case 1:

                            await talentAtmService.CheckBalance(bankAccount);

                            Thread.Sleep(2500);

                            await NextAction(talentAtmService, bankAccount);

                            break;
                        case 2:

                        input: Utility.PrintColorMessage(ConsoleColor.Cyan, "Enter the amount to deposit:");

                            try
                            {

                                decimal depositAmount = decimal.Parse(Console.ReadLine());

                                await talentAtmService.DepositMoney(bankAccount, depositAmount);


                            }
                            catch (FormatException)
                            {
                                Utility.PrintColorMessage(ConsoleColor.Cyan, "Invalid input. Please enter a valid decimal number.");

                                goto input;
                            }

                            Thread.Sleep(2500);

                            Console.Clear();

                            await NextAction(talentAtmService, bankAccount);

                            break;



                        case 3:


                        inputAgain: Utility.PrintColorMessage(ConsoleColor.Cyan, "Enter the amount to withdraw:");

                            try
                            {
                                decimal withdrawalAmount = decimal.Parse(Console.ReadLine());

                                await talentAtmService.MakeWithdrawalAsync(bankAccount, withdrawalAmount);
                            }
                            catch (FormatException)
                            {
                                Utility.PrintColorMessage(ConsoleColor.Cyan, "Invalid input. Please enter a valid decimal number.");

                                goto inputAgain;
                            }
                            Thread.Sleep(2500);

                            await NextAction(talentAtmService, bankAccount);

                            break;

                        case 4:

                        inputTwo: Utility.PrintColorMessage(ConsoleColor.Cyan, "Enter the recipient's account number:");
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
                                Utility.PrintColorMessage(ConsoleColor.Red, "Invalid input. Please enter a valid number.");

                                goto inputTwo;

                            }
                            catch (Exception ex)
                            {
                                Utility.PrintColorMessage(ConsoleColor.Red, "An unexpected error occurred: " + ex.Message);

                                goto inputTwo;

                            }
                            Thread.Sleep(2500);

                            await NextAction(talentAtmService, bankAccount);

                            break;

                        case 5:
                            await talentAtmService.ViewAllTransactions(bankAccount);

                            Thread.Sleep(2500);

                            Console.Clear();

                            await NextAction(talentAtmService, bankAccount);

                            break;

                        case 6:

                            Environment.Exit(0);

                            break;

                        default:

                            await NextAction(talentAtmService, bankAccount);

                            break;


                    }

                }
                else
                {
                    Console.WriteLine("Verification failed.");
                }
            }
            catch (FormatException)
            {
                Utility.PrintColorMessage(ConsoleColor.Red, "Invalid input format. Please enter a valid number.");
            }
            catch (Exception ex)
            {
                Utility.PrintColorMessage(ConsoleColor.Red, $"An error occurred: {ex.Message}");
            }
        }
    }
}
