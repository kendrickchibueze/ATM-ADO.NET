//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace TalentAtm
//{
//    internal class check
//    {

//        static void Main(string[] args)
//        {
//using System;
//using System.Security.Principal;
//using TalentAtmDAL;

//string connectionString = "Data Source=localhost;Initial Catalog=TalentAtmDB;Integrated Security=True";
//SqlConnection connection = new SqlConnection(connectionString);
//connection.Open();

// Deposit
//int accountNumber = 1001;
//decimal depositAmount = 100.00m;
//string depositSql = $"UPDATE BankAccounts SET Balance = Balance + {depositAmount} WHERE AccountNumber = {accountNumber}";
//SqlCommand depositCommand = new SqlCommand(depositSql, connection);
//int depositResult = depositCommand.ExecuteNonQuery();
//Console.WriteLine($"{depositResult} rows updated.");




//more deposit
//public class TalentAtmService : ITalentAtmService
//{
//    private string connectionString;

//    public TalentAtmService(string connectionString)
//    {
//        this.connectionString = connectionString;
//    }

//using System.Data.SqlClient;
//using TalentAtmDAL;

//public async Task<BankAccounts> DepositMoney(BankAccounts bankAccount, decimal depositAmount)
//{
//    using (SqlConnection connection = new SqlConnection(connectionString))
//    {
//        connection.Open();

//        int accountNumber = bankAccount.AccountNumber;
//        string depositSql = $"UPDATE BankAccounts SET Balance = Balance + {depositAmount} WHERE AccountNumber = {accountNumber}";
//        SqlCommand depositCommand = new SqlCommand(depositSql, connection);
//        int depositResult = depositCommand.ExecuteNonQuery();
//        Console.WriteLine($"{depositResult} rows updated.");

//        // Update the BankAccounts object with the new balance
//        bankAccount.Balance += depositAmount;

//        return bankAccount;
//    }
//}




//call it this way
//BankAccount account = new BankAccount
//{
//    AccountNumber = 1001,
//    FullName = "John Doe",
//    CardNumber = 12345678901234,
//    PinCode = 1234,
//    Balance = 1000.00m,
//    IsLocked = false
//};

//TalentAtmService atmService = new TalentAtmService();
//atmService.DepositMoney(account);



























// Transfer
//int senderAccountNumber = 1001;
//int recipientAccountNumber = 1002;
//decimal transferAmount = 500.00m;

//string transferSql = $@"
//                BEGIN TRANSACTION
//                    UPDATE BankAccounts SET Balance = Balance - {transferAmount} WHERE AccountNumber = {senderAccountNumber};
//                    UPDATE BankAccounts SET Balance = Balance + {transferAmount} WHERE AccountNumber = {recipientAccountNumber};
//                    INSERT INTO Transactions (BankAccountNoFrom, BankAccountNoTo, TransactionTypeId, TransactionAmount, TransactionDate)
//                    VALUES ({senderAccountNumber}, {recipientAccountNumber}, 1, {transferAmount}, GETDATE());
//                COMMIT TRANSACTION";
//SqlCommand transferCommand = new SqlCommand(transferSql, connection);
//int transferResult = transferCommand.ExecuteNonQuery();
//Console.WriteLine($"{transferResult} rows updated.");



//more transfer

//public void performTransfer(BankAccount bankAccount, VmTransfer Transfer)
//{
//    using (var connection = new SqlConnection(connectionString))
//    {
//        connection.Open();

//        int senderAccountNumber = bankAccount.AccountNumber;
//        Int64 recipientAccountNumber = Transfer.RecipientBankAccountNumber;
//        decimal transferAmount = Transfer.TransferAmount;

//        string transferSql = $@"
//            BEGIN TRANSACTION
//                UPDATE BankAccounts SET Balance = Balance - {transferAmount} WHERE AccountNumber = {senderAccountNumber};
//                UPDATE BankAccounts SET Balance = Balance + {transferAmount} WHERE AccountNumber = {recipientAccountNumber};
//                INSERT INTO Transaction (BankAccountNoFrom, BankAccountNoTo, TransactionTypeId, TransactionAmount, TransactionDate)
//                VALUES ({senderAccountNumber}, {recipientAccountNumber}, 1, {transferAmount}, GETDATE());
//            COMMIT TRANSACTION";
//        SqlCommand transferCommand = new SqlCommand(transferSql, connection);
//        int transferResult = transferCommand.ExecuteNonQuery();
//        Console.WriteLine($"{transferResult} rows updated.");
//    }
//}


//call it this way
//var talentAtmService = new TalentAtmService();
//var bankAccount = new BankAccount { AccountNumber = 1001 };
//var transfer = new VmTransfer
//{
//    TransferAmount = 500.00m,
//    RecipientBankAccountNumber = 1002,
//    RecipientBankAccountName = "Jane Smith"
//};

//talentAtmService.performTransfer(bankAccount, transfer);
















// Withdrawal
//int withdrawAccountNumber = 1002;
//decimal withdrawAmount = 500.00m;
//string withdrawSql = $"UPDATE BankAccounts SET Balance = Balance - {withdrawAmount} WHERE AccountNumber = {withdrawAccountNumber}";
//SqlCommand withdrawCommand = new SqlCommand(withdrawSql, connection);
//int withdrawResult = withdrawCommand.ExecuteNonQuery();
//Console.WriteLine($"{withdrawResult} rows updated.");

//connection.Close();



//more withdrawal
//public class TalentAtmService : BankingService
//{
//    private readonly string _connectionString;

//    public TalentAtmService(string connectionString)
//    {
//        _connectionString = connectionString;
//    }

//public void MakeWithdrawal(BankAccount bankAccount)
//{
//    using (var connection = new SqlConnection(_connectionString))
//    {
//        connection.Open();

//        var withdrawalSql = $"UPDATE BankAccounts SET Balance = Balance - {bankAccount.WithdrawalAmount} WHERE AccountNumber = {bankAccount.AccountNumber}";
//        var withdrawalCommand = new SqlCommand(withdrawalSql, connection);
//        var withdrawalResult = withdrawalCommand.ExecuteNonQuery();
//        Console.WriteLine($"{withdrawalResult} rows updated.");

//        connection.Close();
//    }
//}
//}



//call like this
//var connectionString = "Data Source=localhost;Initial Catalog=TalentAtmDB;Integrated Security=True";
//var service = new TalentAtmService(connectionString);
//var bankAccount = new BankAccount
//{
//    AccountNumber = 1002,
//    WithdrawalAmount = 500.00m
//};
//service.MakeWithdrawal(bankAccount);






















//            // Account number of the bank account to check the balance of
//int accountNumber = 1001;

//// SQL query to get the balance of the bank account
//string sql = "SELECT Balance FROM BankAccounts WHERE AccountNumber = @accountNumber";

//// Create a new SqlConnection object
//using (SqlConnection connection = new SqlConnection(connectionString))
//{
//    // Open the connection to the database
//    connection.Open();

//    // Create a new SqlCommand object with the SQL query and connection
//    using (SqlCommand command = new SqlCommand(sql, connection))
//    {
//        // Add the account number parameter to the command
//        command.Parameters.AddWithValue("@accountNumber", accountNumber);

//        // Execute the command and get the result
//        using (SqlDataReader reader = command.ExecuteReader())
//        {
//            if (reader.Read())
//            {
//                // Get the balance value from the result and display it
//                decimal balance = reader.GetDecimal(0);
//                Console.WriteLine($"The balance of account number {accountNumber} is {balance:C}");
//            }
//            else
//            {
//                // No rows were returned, so the account number is invalid
//                Console.WriteLine($"Invalid account number: {accountNumber}");
//            }
//        }
//    }
//}



//check balance more 
//public void CheckBalance(BankAccount bankAccount)
//{
//    string sql = "SELECT Balance FROM BankAccounts WHERE AccountNumber = @accountNumber";

//    using (SqlConnection connection = new SqlConnection(_connectionString))
//    {
//        connection.Open();

//        using (SqlCommand command = new SqlCommand(sql, connection))
//        {
//            command.Parameters.AddWithValue("@accountNumber", bankAccount.AccountNumber);

//            using (SqlDataReader reader = command.ExecuteReader())
//            {
//                if (reader.Read())
//                {
//                    decimal balance = reader.GetDecimal(0);
//                    Console.WriteLine($"The balance of account number {bankAccount.AccountNumber} is {balance:C}");
//                }
//                else
//                {
//                    Console.WriteLine($"Invalid account number: {bankAccount.AccountNumber}");
//                }
//            }
//        }
//    }
//}

//call it
//string connectionString = "Data Source=localhost;Initial Catalog=TalentAtmDB;Integrated Security=True";
//TalentAtmService atmService = new TalentAtmService(connectionString);

//// Create a bank account to check the balance of
//BankAccount bankAccount = new BankAccount { AccountNumber = 1001 };

//// Call the CheckBalance method
//atmService.CheckBalance(bankAccount);



























//            //check all transactions
//using (SqlConnection connection = new SqlConnection(connectionString))
//{
//    connection.Open();

//    string query = "SELECT t.TransactionId, t.TransactionDate, t.TransactionAmount, t.TransactionTypeId, a.FullName, b.FullName " +
//                   "FROM Transactions t " +
//                   "INNER JOIN BankAccounts a ON t.BankAccountNoFrom = a.AccountNumber " +
//                   "INNER JOIN BankAccounts b ON t.BankAccountNoTo = b.AccountNumber";

//    using (SqlCommand command = new SqlCommand(query, connection))
//    {
//        using (SqlDataReader reader = command.ExecuteReader())
//        {
//            while (reader.Read())
//            {
//                int transactionId = (int)reader["TransactionId"];
//                DateTime transactionDate = (DateTime)reader["TransactionDate"];
//                decimal transactionAmount = (decimal)reader["TransactionAmount"];
//                int transactionTypeId = (int)reader["TransactionTypeId"];
//                string fromAccountName = (string)reader[4];
//                string toAccountName = (string)reader[5];

//                Console.WriteLine("Transaction Id: {0}, Date: {1}, Amount: {2}, Type Id: {3}, From: {4}, To: {5}", transactionId, transactionDate, transactionAmount, transactionTypeId, fromAccountName, toAccountName);
//            }
//        }
//    }
//}






//public void Transactions(BankAccount bankAccount, Transaction transaction)
//{
//    using (SqlConnection connection = new SqlConnection(connectionString))
//    {
//        connection.Open();

//        // Insert transaction
//        string insertSql = $@"INSERT INTO Transactions (BankAccountNoFrom, BankAccountNoTo, TransactionTypeId, TransactionAmount, TransactionDate)
//                              VALUES ({transaction.SenderAccountNumber}, {transaction.RecipientAccountNumber}, 1, {transaction.Amount}, GETDATE())";
//        using (SqlCommand insertCommand = new SqlCommand(insertSql, connection))
//        {
//            int insertResult = insertCommand.ExecuteNonQuery();
//            Console.WriteLine($"{insertResult} rows inserted.");
//        }

//        // View transactions
//        string viewSql = "SELECT t.TransactionId, t.TransactionDate, t.TransactionAmount, t.TransactionTypeId, a.FullName, b.FullName " +
//                         "FROM Transactions t " +
//                         "INNER JOIN BankAccounts a ON t.BankAccountNoFrom = a.AccountNumber " +
//                         "INNER JOIN BankAccounts b ON t.BankAccountNoTo = b.AccountNumber";
//        using (SqlCommand viewCommand = new SqlCommand(viewSql, connection))
//        {
//            using (SqlDataReader reader = viewCommand.ExecuteReader())
//            {
//                while (reader.Read())
//                {
//                    int transactionId = (int)reader["TransactionId"];
//                    DateTime transactionDate = (DateTime)reader["TransactionDate"];
//                    decimal transactionAmount = (decimal)reader["TransactionAmount"];
//                    int transactionTypeId = (int)reader["TransactionTypeId"];
//                    string fromAccountName = (string)reader[4];
//                    string toAccountName = (string)reader[5];

//                    Console.WriteLine("Transaction Id: {0}, Date: {1}, Amount: {2}, Type Id: {3}, From: {4}, To: {5}", transactionId, transactionDate, transactionAmount, transactionTypeId, fromAccountName, toAccountName);
//                }
//            }
//        }

//        connection.Close();
//    }
//}




//NOTE INSERT AND VIEW TRANSACTIONS 
//void InsertTransaction(BankAccount bankAccount, Transaction transaction)
//{
//    using (SqlConnection connection = new SqlConnection(connectionString))
//    {
//        connection.Open();

//        string query = "INSERT INTO Transactions (BankAccountNoFrom, BankAccountNoTo, TransactionTypeId, TransactionAmount, TransactionDate) " +
//                       "VALUES (@fromAccount, @toAccount, @transactionType, @transactionAmount, @transactionDate)";

//        using (SqlCommand command = new SqlCommand(query, connection))
//        {
//            command.Parameters.AddWithValue("@fromAccount", transaction.FromAccount.AccountNumber);
//            command.Parameters.AddWithValue("@toAccount", transaction.ToAccount.AccountNumber);
//            command.Parameters.AddWithValue("@transactionType", transaction.TransactionType);
//            command.Parameters.AddWithValue("@transactionAmount", transaction.Amount);
//            command.Parameters.AddWithValue("@transactionDate", transaction.Date);

//            int result = command.ExecuteNonQuery();

//            if (result > 0)
//            {
//                Console.WriteLine("Transaction inserted successfully");
//            }
//            else
//            {
//                Console.WriteLine("Transaction insertion failed");
//            }
//        }
//    }
//}

//void ViewTransaction(BankAccount bankAccount)
//{
//    using (SqlConnection connection = new SqlConnection(connectionString))
//    {
//        connection.Open();

//        string query = "SELECT t.TransactionId, t.TransactionDate, t.TransactionAmount, t.TransactionTypeId, a.FullName, b.FullName " +
//                       "FROM Transactions t " +
//                       "INNER JOIN BankAccounts a ON t.BankAccountNoFrom = a.AccountNumber " +
//                       "INNER JOIN BankAccounts b ON t.BankAccountNoTo = b.AccountNumber " +
//                       "WHERE t.BankAccountNoFrom = @accountNumber OR t.BankAccountNoTo = @accountNumber";

//        using (SqlCommand command = new SqlCommand(query, connection))
//        {
//            command.Parameters.AddWithValue("@accountNumber", bankAccount.AccountNumber);

//            using (SqlDataReader reader = command.ExecuteReader())
//            {
//                while (reader.Read())
//                {
//                    int transactionId = (int)reader["TransactionId"];
//                    DateTime transactionDate = (DateTime)reader["TransactionDate"];
//                    decimal transactionAmount = (decimal)reader["TransactionAmount"];
//                    int transactionTypeId = (int)reader["TransactionTypeId"];
//                    string fromAccountName = (string)reader[4];
//                    string toAccountName = (string)reader[5];

//                    Console.WriteLine("Transaction Id: {0}, Date: {1}, Amount: {2}, Type Id: {3}, From: {4}, To: {5}", transactionId, transactionDate, transactionAmount, transactionTypeId, fromAccountName, toAccountName);
//                }
//            }
//        }
//    }
//}






















//            public void BlockAccount(int accountNumber, int cardNumber, int pinCode)
//            {
//                string connectionString = "Data Source=myServerAddress;Initial Catalog=myDataBase;User ID=myUsername;Password=myPassword";
//                using (SqlConnection connection = new SqlConnection(connectionString))
//                {
//                    connection.Open();

//                    // Check if the account is already blocked
//                    string selectQuery = "SELECT isLocked FROM BankAccounts WHERE AccountNumber = @AccountNumber";
//                    using (SqlCommand selectCommand = new SqlCommand(selectQuery, connection))
//                    {
//                        selectCommand.Parameters.AddWithValue("@AccountNumber", accountNumber);
//                        bool isLocked = (bool)selectCommand.ExecuteScalar();

//                        if (isLocked)
//                        {
//                            Console.WriteLine("The account is already locked");
//                            return;
//                        }
//                    }

//                    // Check the pin code
//string verifyQuery = "SELECT * FROM BankAccounts WHERE AccountNumber = @AccountNumber AND CardNumber = @CardNumber AND PinCode = @PinCode";
//using (SqlCommand verifyCommand = new SqlCommand(verifyQuery, connection))
//{
//    verifyCommand.Parameters.AddWithValue("@AccountNumber", accountNumber);
//    verifyCommand.Parameters.AddWithValue("@CardNumber", cardNumber);
//    verifyCommand.Parameters.AddWithValue("@PinCode", pinCode);

//    using (SqlDataReader reader = verifyCommand.ExecuteReader())
//    {
//        if (!reader.HasRows)
//        {
//            // Increment the number of failed attempts
//            string updateQuery = "UPDATE BankAccounts SET FailedAttempts = FailedAttempts + 1 WHERE AccountNumber = @AccountNumber";
//            using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
//            {
//                updateCommand.Parameters.AddWithValue("@AccountNumber", accountNumber);
//                updateCommand.ExecuteNonQuery();
//            }

//            // Check if the account should be locked
//            selectQuery = "SELECT FailedAttempts FROM BankAccounts WHERE AccountNumber = @AccountNumber";
//            using (SqlCommand selectCommand = new SqlCommand(selectQuery, connection))
//            {
//                selectCommand.Parameters.AddWithValue("@AccountNumber", accountNumber);
//                int failedAttempts = (int)selectCommand.ExecuteScalar();

//                if (failedAttempts >= 3)
//                {
//                    // Block the account
//                    string blockQuery = "UPDATE BankAccounts SET isLocked = 1 WHERE AccountNumber = @AccountNumber";
//                    using (SqlCommand blockCommand = new SqlCommand(blockQuery, connection))
//                    {
//                        blockCommand.Parameters.AddWithValue("@AccountNumber", accountNumber);
//                        blockCommand.ExecuteNonQuery();
//                    }

//                    Console.WriteLine("The account has been locked due to too many failed attempts.");
//                }
//                else
//                {
//                    Console.WriteLine("Invalid card number or pin code.");
//                }
//            }
//        }
//        else
//        {
//            Console.WriteLine("Verification successful.");
//        }
//    }
//}






//            public bool VerifyCardNumberAndPin(string cardNumber, string pin)
//            {
//                // Define the connection string and SQL query
//                string connectionString = "Data Source=myServerAddress;Initial Catalog=myDataBase;User Id=myUsername;Password=myPassword;";
//                string sql = "SELECT COUNT(*) FROM BankAccounts WHERE CardNumber = @cardNumber AND PinCode = @pin";

//                // Open a connection to the database
//                using (SqlConnection connection = new SqlConnection(connectionString))
//                {
//                    connection.Open();

//                    // Create a command object with the SQL query and parameters
//                    using (SqlCommand command = new SqlCommand(sql, connection))
//                    {
//                        command.Parameters.AddWithValue("@cardNumber", cardNumber);
//                        command.Parameters.AddWithValue("@pin", pin);

//                        // Execute the query and get the number of matching rows
//                        int rowCount = (int)command.ExecuteScalar();

//                        // Return true if there is exactly one matching row, indicating a valid card number and pin
//                        return (rowCount == 1);
//                    }
//                }
//            }
//        }
//    }

//}
//}
