﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace TalentAtmDAL
{
    public  class TalentAtmService : ITalentAtmService
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
            return String.Format(_enculture, "{0:C2}", amt);
        }



        public async Task<bool> performTransferAsync(BankAccounts bankAccount, VmTransfer transfer)
        {
            SqlConnection sqlConn = await _dbContext.OpenConnection();

            var transferSql = @"
                            BEGIN TRANSACTION
                                UPDATE BankAccounts SET Balance = Balance - @TransferAmount WHERE AccountNumber = @SenderAccountNumber;
                                UPDATE BankAccounts SET Balance = Balance + @TransferAmount WHERE AccountNumber = @RecipientAccountNumber;
                                INSERT INTO Transactions (BankAccountNoFrom, BankAccountNoTo, TransactionTypeId, TransactionAmount, TransactionDate)
                                VALUES (@SenderAccountNumber, @RecipientAccountNumber, 1, @TransferAmount, GETDATE());
                            COMMIT TRANSACTION";

            int senderAccountNumber = bankAccount.AccountNumber;
            Int64 recipientAccountNumber = transfer.RecipientBankAccountNumber;
            decimal transferAmount = transfer.TransferAmount;

            using (var command = new SqlCommand(transferSql, sqlConn))
            {
                command.Parameters.AddWithValue("@TransferAmount", transferAmount);
                command.Parameters.AddWithValue("@SenderAccountNumber", senderAccountNumber);
                command.Parameters.AddWithValue("@RecipientAccountNumber", recipientAccountNumber);

                var transferResult = await command.ExecuteNonQueryAsync();
                Console.WriteLine($"{transferResult} rows updated.");
            }

            return true;
        }

        //public async Task<decimal> performTransferAsync(BankAccounts bankAccount, VmTransfer transfer)
        //{
        //    SqlConnection sqlConn = await _dbContext.OpenConnection();

        //    var transferSql = @"
        //            BEGIN TRANSACTION
        //                UPDATE BankAccounts SET Balance = Balance - @TransferAmount WHERE CardNumber = @CardNumber AND PinCode = @PinCode;
        //                UPDATE BankAccounts SET Balance = Balance + @TransferAmount WHERE AccountNumber = @RecipientAccountNumber;
        //                INSERT INTO Transactions (BankAccountNoFrom, BankAccountNoTo, TransactionTypeId, TransactionAmount, TransactionDate)
        //                VALUES (@SenderAccountNumber, @RecipientAccountNumber, 1, @TransferAmount, GETDATE());
        //            COMMIT TRANSACTION";

        //    decimal senderBalanceBefore = bankAccount.Balance;

        //    using (var command = new SqlCommand(transferSql, sqlConn))
        //    {
        //        command.Parameters.AddWithValue("@TransferAmount", transfer.TransferAmount);
        //        command.Parameters.AddWithValue("@CardNumber", bankAccount.CardNumber);
        //        command.Parameters.AddWithValue("@PinCode", bankAccount.PinCode);
        //        command.Parameters.AddWithValue("@SenderAccountNumber", bankAccount.AccountNumber);
        //        command.Parameters.AddWithValue("@RecipientAccountNumber", transfer.RecipientBankAccountNumber);

        //        var transferResult = await command.ExecuteNonQueryAsync();
        //        Console.WriteLine($"{transferResult} rows updated.");

        //        decimal senderBalanceAfter = senderBalanceBefore - transfer.TransferAmount;
        //        decimal recipientBalance = await GetBalanceAsync(transfer.RecipientBankAccountNumber);

        //        Console.WriteLine($"Amount transferred: {transfer.TransferAmount:C}");
        //        Console.WriteLine($"Sender balance: {senderBalanceAfter:C}");
        //        Console.WriteLine($"Recipient balance: {recipientBalance:C}");

        //        return transfer.TransferAmount;
        //    }
        //}

        //public async Task<decimal> GetBalanceAsync(long accountNumber)
        //{
        //    SqlConnection sqlConn = await _dbContext.OpenConnection();

        //    var balanceSql = @"SELECT Balance FROM BankAccounts WHERE AccountNumber = @AccountNumber";
        //    using (var command = new SqlCommand(balanceSql, sqlConn))
        //    {
        //        command.Parameters.AddWithValue("@AccountNumber", accountNumber);
        //        var balance = (decimal)await command.ExecuteScalarAsync();
        //        return balance;
        //    }
        //}








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
                        
                        Console.WriteLine($"Welcome {bankAccount.FullName}, your Balance  is {FormatAmount(bankAccount.Balance)}");

                        return bankAccount;
                    }
                    else
                    {
                        Console.WriteLine($"Invalid account number: {bankAccount.AccountNumber}");
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

                    Console.WriteLine($"Deposited amount: {FormatAmount(depositAmount)}. New balance: {FormatAmount(bankAccount.Balance)}");
                }
            }

            return bankAccount;
        }








       

        public async Task<bool> verifyCardNumberPassword(BankAccounts bankAccount)
        {
            SqlConnection sqlConn = await _dbContext.OpenConnection();

           string selectQuery = "SELECT isLocked, FailedAttempts FROM BankAccounts WHERE AccountNumber = @AccountNumber";

        using (SqlCommand selectCommand = new SqlCommand(selectQuery, sqlConn))
        {
            selectCommand.Parameters.AddWithValue("@AccountNumber", bankAccount.AccountNumber);
            using (SqlDataReader reader = selectCommand.ExecuteReader())
            {
                if (reader.Read())
                {
                    bool isLocked = (bool)reader["isLocked"];

                    int failedAttempts = (int)reader["FailedAttempts"];

                    if (isLocked)
                    {
                        Console.WriteLine("The account is locked due to too many failed attempts.");
                        return false;
                    }
                    else
                    {
                        string verifyQuery = "SELECT * FROM BankAccounts WHERE AccountNumber = @AccountNumber AND CardNumber = @CardNumber AND PinCode = @PinCode";
                        using (SqlCommand verifyCommand = new SqlCommand(verifyQuery, sqlConn))
                        {
                            verifyCommand.Parameters.AddWithValue("@AccountNumber", bankAccount.AccountNumber);

                            verifyCommand.Parameters.AddWithValue("@CardNumber", bankAccount.CardNumber);

                            verifyCommand.Parameters.AddWithValue("@PinCode", bankAccount.PinCode);

                            using (SqlDataReader verifyReader = verifyCommand.ExecuteReader())
                            {
                                if (!verifyReader.HasRows)
                                {
                                    // Increment the number of failed attempts
                                    string updateQuery = "UPDATE BankAccounts SET FailedAttempts = FailedAttempts + 1 WHERE AccountNumber = @AccountNumber";
                                    using (SqlCommand updateCommand = new SqlCommand(updateQuery, sqlConn))
                                    {
                                        updateCommand.Parameters.AddWithValue("@AccountNumber", bankAccount.AccountNumber);
                                        updateCommand.ExecuteNonQuery();
                                    }

                                    // Check if the account should be locked
                                    if (failedAttempts + 1 >= _maxtries)
                                    {
                                        // Lock the account
                                        string blockQuery = "UPDATE BankAccounts SET isLocked = 1, FailedAttempts = 3 WHERE AccountNumber = @AccountNumber";
                                        using (SqlCommand blockCommand = new SqlCommand(blockQuery, sqlConn))
                                        {
                                            blockCommand.Parameters.AddWithValue("@AccountNumber", bankAccount.AccountNumber);
                                            blockCommand.ExecuteNonQuery();
                                        }

                                        Console.WriteLine("The account has been locked due to too many failed attempts.");

                                        return false;
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Account not found.");
                    return false;
                }
            }
        }
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
