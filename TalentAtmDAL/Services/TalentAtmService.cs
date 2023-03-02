using System.Data.SqlClient;
using System.Globalization;
using TalentAtmClient.Atm.UI;

namespace TalentAtmDAL.Services
{
    public partial class TalentAtmService : ITalentAtmService
    {

        private static CultureInfo _enculture = new CultureInfo("en-US");

        private TalentAtmDBContext _dbContext;

        private bool _disposed;


        private int _maxtries = 3;

        public TalentAtmService()
        {

        }


        public TalentAtmService(TalentAtmDBContext dbContext)
        {

            _dbContext = dbContext;

        }


        public static string FormatAmount(decimal amt)
        {
            return string.Format(_enculture, "{0:C2}", amt);
        }




        public async Task<bool> performTransferAsync(BankAccounts bankAccount, VmTransfer transfer)
        {
            SqlConnection sqlConn = await _dbContext.OpenConnection();




            var transferSql = @"
                            BEGIN TRANSACTION
                                UPDATE BankAccounts SET Balance = Balance - @TransferAmount WHERE AccountNumber = @SenderAccountNumber;
                                UPDATE BankAccounts SET Balance = Balance + @TransferAmount WHERE AccountNumber = @RecipientAccountNumber;
                                INSERT INTO Transactions (BankAccountNoFrom, BankAccountNoTo, TransactionTypeId, TransactionAmount, TransactionDate)
                                VALUES (@SenderAccountNumber, @RecipientAccountNumber, 4, @TransferAmount, GETDATE());
                            COMMIT TRANSACTION";


            int senderAccountNumber = bankAccount.AccountNumber;
            long recipientAccountNumber = transfer.RecipientBankAccountNumber;
            decimal transferAmount = transfer.TransferAmount;

            using (var command = new SqlCommand(transferSql, sqlConn))
            {
                command.Parameters.AddWithValue("@TransferAmount", transferAmount);
                command.Parameters.AddWithValue("@SenderAccountNumber", senderAccountNumber);
                command.Parameters.AddWithValue("@RecipientAccountNumber", recipientAccountNumber);


                var transferResult = await command.ExecuteNonQueryAsync();



                Utility.PrintColorMessage(ConsoleColor.Green, $"Amount transferred: {transferAmount:C}");


            }


            return true;
        }







        public async Task<BankAccounts> CheckBalance(BankAccounts bankAccount)
        {
            SqlConnection sqlConn = await _dbContext.OpenConnection();

            string sql = "SELECT Balance, FullName FROM BankAccounts WHERE CardNumber = @CardNumber AND PinCode = @PinCode";

            using (SqlCommand command = new SqlCommand(sql, sqlConn))
            {
                command.Parameters.AddWithValue("@CardNumber", bankAccount.CardNumber);
                command.Parameters.AddWithValue("@PinCode", bankAccount.PinCode);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        decimal balance = reader.GetDecimal(0);
                        string fullName = reader.GetString(1);

                        bankAccount.Balance = balance;
                        bankAccount.FullName = fullName;

                        Utility.PrintColorMessage(ConsoleColor.Green, $"Welcome {bankAccount.FullName}, your Balance  is {FormatAmount(bankAccount.Balance)}");

                        return bankAccount;
                    }
                    else
                    {
                        Utility.PrintColorMessage(ConsoleColor.Red, $"Invalid account number: {bankAccount.AccountNumber}");
                        return null;
                    }
                }
            }
        }





        public async Task<BankAccounts> DepositMoney(BankAccounts bankAccount, decimal depositAmount)
        {
            SqlConnection sqlConn = await _dbContext.OpenConnection();

            using (SqlCommand depositCommand = new SqlCommand())
            {
                depositCommand.Connection = sqlConn;

                depositCommand.CommandText = $"UPDATE BankAccounts SET Balance = Balance + {depositAmount} OUTPUT INSERTED.Balance WHERE CardNumber = {bankAccount.CardNumber}";

                var result = await depositCommand.ExecuteScalarAsync();

                if (result != null)
                {
                    bankAccount.Balance = (decimal)result;

                    Utility.PrintColorMessage(ConsoleColor.Green, $"Deposited amount: {FormatAmount(depositAmount)}. New balance: {FormatAmount(bankAccount.Balance)}");
                }
            }

            return bankAccount;
        }



        protected virtual void Dispose(bool disposing)
        {

            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _dbContext.Dispose();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
