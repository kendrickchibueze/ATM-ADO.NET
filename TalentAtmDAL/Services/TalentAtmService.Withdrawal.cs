using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentAtmDAL.Services
{
    partial class TalentAtmService
    {

        public async Task<bool> MakeWithdrawalAsync(BankAccounts bankAccount, decimal withdrawalAmount)
        {
            SqlConnection sqlConn = await _dbContext.OpenConnection();

            var withdrawalSql = @"UPDATE BankAccounts 
                                  SET Balance = Balance - @WithdrawalAmount 
                                  WHERE CardNumber = @CardNumber 
                                  AND PinCode = @PinCode 
                                  AND Balance >= @WithdrawalAmount";

            using (SqlCommand command = new SqlCommand(withdrawalSql, sqlConn))
            {
                command.Parameters.AddWithValue("@WithdrawalAmount", withdrawalAmount);
                command.Parameters.AddWithValue("@CardNumber", bankAccount.CardNumber);
                command.Parameters.AddWithValue("@PinCode", bankAccount.PinCode);

                var withdrawalResult = await command.ExecuteNonQueryAsync();

                if (withdrawalResult == 1)
                {
                    var remainingSql = @"SELECT Balance FROM BankAccounts WHERE CardNumber = @CardNumber";

                    using (SqlCommand remainingCommand = new SqlCommand(remainingSql, sqlConn))
                    {
                        remainingCommand.Parameters.AddWithValue("@CardNumber", bankAccount.CardNumber);

                        var remainingBalance = await remainingCommand.ExecuteScalarAsync();

                        Console.WriteLine($"Withdrawal successful. Amount withdrawn: {FormatAmount(withdrawalAmount)}. Remaining balance: {FormatAmount((decimal)remainingBalance)}");
                    }

                    return true;
                }
                else
                {
                    Console.WriteLine("Withdrawal failed.");

                    return false;
                }
            }
        }
    }
}
