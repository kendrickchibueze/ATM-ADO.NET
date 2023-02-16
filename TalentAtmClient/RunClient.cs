
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalentAtmDAL;

namespace TalentAtmClient
{
    public  class RunClient
    {



        public async Task Run()
        {


            using (ITalentAtmService talentAtmService = new TalentAtmService(new TalentAtmDBContext()))
            {


                //verification

                //BankAccounts bankAccount = new BankAccounts
                //{
                //    AccountNumber = 1002,
                //    CardNumber = 23456789012345,
                //    PinCode = 2345
                //};

                //bool isVerified = await talentAtmService.verifyCardNumberPassword(bankAccount);

                //if (isVerified)
                //{
                //    Console.WriteLine("Verification successful.");
                //}
                //else
                //{
                //    Console.WriteLine("Verification failed.");
                //}


                //check balance

                //BankAccounts bankAccount = new BankAccounts
                //{

                //    CardNumber = 12345678901234,
                //    PinCode = 1234
                //};

                // await talentAtmService.CheckBalance(bankAccount);



                //deposit money
                //BankAccounts bankAccount = new BankAccounts
                //{

                //    CardNumber = 12345678901234,
                //    PinCode = 1234,

                //};

                //decimal depositAmount = 500;

                //await talentAtmService.DepositMoney(bankAccount, depositAmount);





                //withdrawal
                //BankAccounts bankAccount = new BankAccounts
                //{
                //    CardNumber = 23456789012345,
                //    PinCode = 2345,

                //};

                //decimal withdrawalAmount = 1000;

                // await talentAtmService.MakeWithdrawalAsync(bankAccount, withdrawalAmount);



                //transfer money

                //var senderAccount = new BankAccounts {

                //    //CardNumber = 23456789012345,
                //    //PinCode = 2345,
                //    AccountNumber = 1002

                //};
                //var transfer = new VmTransfer { 
                //    RecipientBankAccountNumber = 1003,
                //    TransferAmount = 1000
                //};

                //bool success = await talentAtmService.performTransferAsync(senderAccount, transfer);
                //if (success)
                //{
                //    Console.WriteLine($"Transfer successful.");
                //}
                //else
                //{
                //    Console.WriteLine($"Transfer failed.");
                //}



                BankAccounts bankAccount = new BankAccounts
                {
                    CardNumber = 34567890123456,
                    PinCode = 3456
                    
                };

                VmTransfer transfer = new VmTransfer
                {
                    RecipientBankAccountNumber = 1003,
                    TransferAmount = 100.00m
                };

                 await talentAtmService.performTransferAsync(bankAccount, transfer);




















            }









        }


    }

}

