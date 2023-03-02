using System.Data.SqlClient;
using TalentAtmClient.Atm.UI;

namespace TalentAtmDAL.Services
{
    partial class TalentAtmService
    {

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
                            Utility.PrintColorMessage(ConsoleColor.Red, "The account is locked due to too many failed attempts.");
                            return false;
                        }
                        else
                        {
                            string verifyQuery = "SELECT * FROM BankAccounts WHERE AccountNumber = @AccountNumber AND " +
                                                 "CardNumber = @CardNumber AND PinCode = @PinCode";


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
                                        string updateQuery = "UPDATE BankAccounts SET FailedAttempts = FailedAttempts + 1 WHERE " +
                                                              "AccountNumber = @AccountNumber";

                                        using (SqlCommand updateCommand = new SqlCommand(updateQuery, sqlConn))
                                        {
                                            updateCommand.Parameters.AddWithValue("@AccountNumber", bankAccount.AccountNumber);
                                            updateCommand.ExecuteNonQuery();
                                        }

                                        return BlockAccount(bankAccount, sqlConn, failedAttempts);
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
                        Utility.PrintColorMessage(ConsoleColor.Red, "Account not found.");
                        return false;
                    }
                }
            }
        }

        private bool BlockAccount(BankAccounts bankAccount, SqlConnection sqlConn, int failedAttempts)
        {
            // Check if the account should be locked
            if (failedAttempts + 1 >= _maxtries)
            {
                // Lock the account
                string blockQuery = "UPDATE BankAccounts SET isLocked = 1, FailedAttempts = 3 WHERE " +
                                      "AccountNumber = @AccountNumber";

                using (SqlCommand blockCommand = new SqlCommand(blockQuery, sqlConn))
                {
                    blockCommand.Parameters.AddWithValue("@AccountNumber", bankAccount.AccountNumber);

                    blockCommand.ExecuteNonQuery();
                }

                Utility.PrintColorMessage(ConsoleColor.Red, "The account has been locked due to too many failed attempts.");

                return false;
            }
            else
            {
                return false;
            }
        }



    }
}
