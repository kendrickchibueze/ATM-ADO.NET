using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using TalentAtmClient.Atm.UI;

namespace TalentAtmDAL.Services
{
    partial class TalentAtmService
    {


        public async Task<bool> InsertTransaction(BankAccounts bankAccount, long loggedInAccountNumber, decimal amount, string transactionTypeName)
        {
            try
            {
                using (SqlConnection sqlConn = await _dbContext.OpenConnection())
                {
                    
                    string transactionTypeQuery = "SELECT TransactionTypeId FROM TransactionType WHERE TransactionTypeName = @transactionTypeName";
                    
                    int transactionTypeId;

                    using (SqlCommand transactionTypeCommand = new SqlCommand(transactionTypeQuery, sqlConn))
                    {
                        transactionTypeCommand.Parameters.AddWithValue("@transactionTypeName", transactionTypeName);

                        object result = await transactionTypeCommand.ExecuteScalarAsync();

                        if (result == null || result == DBNull.Value)
                        {
                            Utility.PrintColorMessage(ConsoleColor.Red, "Invalid transaction type: " + transactionTypeName);

                            return false;
                        }
                        transactionTypeId = (int)result;
                    }

                    string insertTransactionQuery = @"INSERT INTO Transactions (TransactionDate, TransactionAmount, TransactionTypeId, BankAccountNoFrom, BankAccountNoTo)
                                                      VALUES (@transactionDate, @transactionAmount, @transactionTypeId, @loggedInBankAccountNumber, @bankAccountNumber)";

                    using (SqlCommand command = new SqlCommand(insertTransactionQuery, sqlConn))
                    {
                        command.Parameters.AddWithValue("@transactionDate", DateTime.Now);
                        command.Parameters.AddWithValue("@transactionAmount", amount);
                        command.Parameters.AddWithValue("@transactionTypeId", transactionTypeId);
                        command.Parameters.AddWithValue("@bankAccountNumber", bankAccount.AccountNumber);
                        command.Parameters.AddWithValue("@loggedInBankAccountNumber", loggedInAccountNumber);

                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            Utility.PrintColorMessage(ConsoleColor.Green, "Transaction successful!");
                            return true;
                        }
                        else
                        {
                            Utility.PrintColorMessage(ConsoleColor.Red, "Transaction failed!");
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.PrintColorMessage(ConsoleColor.Red, "Error inserting transaction: " + ex.Message);
                return false;
            }
        }


        public async Task ViewAllTransactions(long loggedInAccountNumber)
        {
            try
            {
                using (SqlConnection sqlConn = await _dbContext.OpenConnection())
                {
                    string query = @"SELECT t.TransactionId, t.TransactionDate, t.TransactionAmount, tt.TransactionTypeName, 
                                     a.AccountNumber, a.FullName FROM Transactions t 
                                     JOIN TransactionType tt ON t.TransactionTypeId = tt.TransactionTypeId 
                                     JOIN BankAccounts a ON t.BankAccountNoFrom = a.AccountNumber OR t.BankAccountNoTo = a.AccountNumber 
                                     WHERE t.BankAccountNoFrom = @accountNumber OR t.BankAccountNoTo = @accountNumber
                                     ORDER BY t.TransactionDate DESC"
                    ;




                    using (SqlCommand command = new SqlCommand(query, sqlConn))
                    {
                        command.Parameters.Add("@accountNumber", SqlDbType.BigInt).Value = loggedInAccountNumber;

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {



                            Utility.PrintColorMessage(ConsoleColor.Cyan, "------------------------------------------------------------------------------------------------------------");
                            Utility.PrintColorMessage(ConsoleColor.Cyan, "| Transaction ID | Transaction Date| Transaction Amount | Transaction Type | Account Number| Account Name   |");
                            Utility.PrintColorMessage(ConsoleColor.Cyan, "-------------------------------------------------------------------------------------------------------------");

                            while (await reader.ReadAsync())
                            {
                                int transactionId = reader.GetInt32(0);
                                DateTime transactionDate = reader.GetDateTime(1);
                                decimal transactionAmount = reader.GetDecimal(2);
                                string transactionTypeName = reader.GetString(3);
                                long accountNumber = reader.GetInt64(4);
                                string fullname = reader.GetString(5);

                                Utility.PrintColorMessage(ConsoleColor.Cyan, $"| {transactionId,15} | {transactionDate,18} | {transactionAmount,19:C} | {transactionTypeName,17} | {accountNumber,14} | {fullname,12} |");
                            }

                            Utility.PrintColorMessage(ConsoleColor.Cyan, "--------------------------------------------------------------------------------------------------------------------------------------------");


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.PrintColorMessage(ConsoleColor.Red, "Error retrieving transactions: " + ex.Message);
            }
        }






    }
}
