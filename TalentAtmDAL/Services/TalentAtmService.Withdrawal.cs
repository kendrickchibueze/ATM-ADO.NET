using System.Data.SqlClient;
using TalentAtmClient.Atm.UI;

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

                        Utility.PrintColorMessage(ConsoleColor.Green, $"Withdrawal successful. Amount withdrawn: {FormatAmount(withdrawalAmount)}. Remaining balance: {FormatAmount((decimal)remainingBalance)}");
                    }

                    return true;
                }
                else
                {
                    Utility.PrintColorMessage(ConsoleColor.Red, "Withdrawal failed.");

                    return false;
                }
            }
        }
    }
}
