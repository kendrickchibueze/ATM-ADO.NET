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

        public async Task<bool> ViewAllTransactions(BankAccounts bankAccount, long loggedInAccountNumber)
        {
            try
            {
                using (SqlConnection sqlConn = await _dbContext.OpenConnection())
                {
                    string transactQuery = @"SELECT TOP 3 t.TransactionId, t.TransactionDate, t.TransactionAmount, 
                                            t.TransactionTypeId, tt.TransactionTypeName 
                                            FROM Transactions t 
                                            INNER JOIN TransactionType tt ON t.TransactionTypeId = tt.TransactionTypeId 
                                            WHERE (t.BankAccountNoFrom = @loggedInBankAccountNumber OR t.BankAccountNoTo = @loggedInBankAccountNumber) 
                                            AND (t.BankAccountNoFrom = @bankAccountNumber OR t.BankAccountNoTo = @bankAccountNumber) 
                                            AND tt.TransactionTypeName != 'Balance Enquiry'
                                            ORDER BY t.TransactionDate DESC";

                    using (SqlCommand command = new SqlCommand(transactQuery, sqlConn))
                    {
                        command.Parameters.AddWithValue("@bankAccountNumber", bankAccount.AccountNumber);
                        command.Parameters.AddWithValue("@loggedInBankAccountNumber", loggedInAccountNumber);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (!reader.HasRows)
                            {
                                Console.WriteLine("You have no transactions yet.");
                                return true;
                            }
                            while (await reader.ReadAsync())
                            {
                                int transactionId = (int)reader["TransactionId"];
                                DateTime transactionDate = (DateTime)reader["TransactionDate"];
                                decimal transactionAmount = (decimal)reader["TransactionAmount"];
                                string transactionTypeName = (string)reader["TransactionTypeName"];

                                Console.WriteLine("Transaction Id: {0}, Date: {1}, Amount: {2}, Type: {3}",
                                    transactionId, transactionDate, transactionAmount, transactionTypeName);
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error viewing recent transaction: " + ex.Message);
                return false;
            }
        }
    }
}
