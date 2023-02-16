
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


                //BankAccounts bankAccount = new BankAccounts
                //{
                //    CardNumber = 23456789012346,
                //    PinCode = 3456,
                //    AccountNumber = 1003

                //};

                //VmTransfer transfer = new VmTransfer
                //{
                //    RecipientBankAccountNumber = 1001,
                //    TransferAmount = 1000.00m
                //};

                //await talentAtmService.performTransferAsync(bankAccount, transfer);



                BankAccounts bankAccount = new BankAccounts(); // create a BankAccounts object as needed
                bool success = await talentAtmService.ViewAllTransactions(bankAccount);





















            }









        }


    }

}

