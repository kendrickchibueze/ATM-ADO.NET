using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentAtmDAL
{
    public  interface ITalentAtmService:IDisposable
    {
        Task<BankAccounts> CheckBalance(BankAccounts bankAccount);

        Task<BankAccounts> DepositMoney(BankAccounts bankAccount, decimal depositAmount);

        Task<bool> performTransferAsync(BankAccounts bankAccount, VmTransfer Transfer);

        Task<bool> MakeWithdrawalAsync(BankAccounts bankAccount, decimal withdrawalAmount);


         Task<bool> verifyCardNumberPassword(BankAccounts bankAccount);

        Task<bool>ViewAllTransactions(BankAccounts bankAccount, long loggedInAccountNumber);    

        
    }
}
