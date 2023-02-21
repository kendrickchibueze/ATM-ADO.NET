using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentAtmDAL
{
    public  class TalentAtmDB
    {

        public  static async Task CreateDBAndTables()
        {
            // Connection string for the local SQL Server instance
            string connectionString = "Server=DESKTOP-HTUFPR1\\SQLEXPRESS;Integrated security=SSPI;database=master";

            // SQL query to create the database
            string createDatabaseQuery = "CREATE DATABASE PTalentAtmDB";

            // SQL query to use the TalentAtmDB database
            string useDatabaseQuery = "USE PTalentAtmDB";

            // SQL query to create the BankAccounts table
            string createBankAccountsTableQuery = @"
                CREATE TABLE BankAccounts (
                    AccountNumber BIGINT PRIMARY KEY,
                    FullName VARCHAR(50) NOT NULL,
                    CardNumber BIGINT NOT NULL,
                    PinCode INT NOT NULL,
                    Balance DECIMAL(18,2) NOT NULL,
                    isLocked BIT NOT NULL,
                    FailedAttempts INT NOT NULL DEFAULT 0
                )";

            // SQL query to create the TransactionType table
            string createTransactionTypeTableQuery = @"
                CREATE TABLE TransactionType (
                    TransactionTypeId INT PRIMARY KEY IDENTITY(1,1),
                    TransactionTypeName VARCHAR(50) NOT NULL
                )";

            // SQL query to create the Transactions table
            string createTransactionsTableQuery = @"
                CREATE TABLE Transactions (
                    TransactionId INT PRIMARY KEY IDENTITY(1,1),
                    BankAccountNoFrom BIGINT NOT NULL,
                    BankAccountNoTo BIGINT NOT NULL,
                    TransactionTypeId INT NOT NULL,
                    TransactionAmount DECIMAL(18,2) NOT NULL,
                    TransactionDate DATETIME NOT NULL,
                    FOREIGN KEY (BankAccountNoFrom) REFERENCES BankAccounts(AccountNumber),
                    FOREIGN KEY (BankAccountNoTo) REFERENCES BankAccounts(AccountNumber),
                    FOREIGN KEY (TransactionTypeId) REFERENCES TransactionType(TransactionTypeId)
                )";

            // SQL query to create the VmTransfers table
            string createVmTransfersTableQuery = @"
                CREATE TABLE VmTransfers (
                    TransferId INT PRIMARY KEY IDENTITY(1,1),
                    TransferAmount DECIMAL(18,2) NOT NULL,
                    RecipientBankAccountNumber BIGINT NOT NULL,
                    RecipientBankAccountName VARCHAR(50) NOT NULL,
                    FOREIGN KEY (RecipientBankAccountNumber) REFERENCES BankAccounts(AccountNumber)
                )";



            // SQL query to populate the TransactionType table
            string populateTransactionTypeTableQuery = @"
                SET IDENTITY_INSERT TransactionType ON;

                INSERT INTO TransactionType (TransactionTypeId, TransactionTypeName)
                VALUES
                    (1, 'Balance Enquiry'),
                    (2, 'Cash Deposit'),
                    (3, 'Withdrawal'),
                    (4, 'Transfer'),
                    (5, 'Transactions');

                SET IDENTITY_INSERT TransactionType OFF;";

            // SQL query to insert data into the Transactions table
            string insertTransactionsTableQuery = @"
                INSERT INTO Transactions (BankAccountNoFrom, BankAccountNoTo, TransactionTypeId, TransactionAmount, TransactionDate)
                VALUES 
                    (1001, 1002, 4, 500.00, CURRENT_TIMESTAMP),
                    (1003, 1001, 2, 1000.00, CURRENT_TIMESTAMP),
                    (1002, 1003, 3, 1000.00, CURRENT_TIMESTAMP);";



            string insertBankAccountsTableQuery = @"
                INSERT INTO BankAccounts(AccountNumber, FullName, CardNumber, PinCode, Balance, isLocked)
                VALUES
                (1001, 'John Doe', 12345678901234, 1234, 1000.00, 0),
                (1002, 'Jane Smith', 23456789012345, 2345, 5000.00, 0),
                (1003, 'Dave Gray', 23456789012346, 3456, 8000.00, 0),
                (1004, 'Frank Johnson', 23456789012347,3457, 6000.00, 0),
                (1005, 'Steve Crown', 23456789012348, 3458, 9000.00, 0),
                (1006, 'Allen Skott', 23456789012349, 3459, 4000.00, 0),
                (1008, 'Terry John', 23456789012340, 4567, 2000.00, 0),
                (1009, 'Smith Curry', 23456789012341, 5643, 3000.00, 0),
                (1011, 'Robert Johnson', 34567890123456, 3456, 7500.00, 1); ";

            // SQL query to insert data into the VmTransfers table
            string insertVmTransfersTableQuery = @"
               INSERT INTO VmTransfers(TransferAmount, RecipientBankAccountNumber, RecipientBankAccountName)
                VALUES
                (500.00, 1002, 'Jane Smith'),
                (1000.00, 1001, 'John Doe'),
                (750.00, 1003, 'Robert Johnson')";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Create the TalentAtmDB database
                    using (SqlCommand command = new SqlCommand(createDatabaseQuery, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Created PTalentAtmDB database.");
                    }

                    // Use the TalentAtmDB database
                    using (SqlCommand command = new SqlCommand(useDatabaseQuery, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Using PTalentAtmDB database.");
                    }

                    // Create the BankAccounts table
                    using (SqlCommand command = new SqlCommand(createBankAccountsTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Created BankAccounts table.");
                    }

                    // Create the TransactionType table
                    using (SqlCommand command = new SqlCommand(createTransactionTypeTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Created TransactionType table.");
                    }

                    // Create the Transactions table
                    using (SqlCommand command = new SqlCommand(createTransactionsTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Created Transactions table.");
                    }

                    // Create the VmTransfers table
                    using (SqlCommand command = new SqlCommand(createVmTransfersTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Created VmTransfers table.");
                    }

                    // Populate the TransactionType table
                    using (SqlCommand command = new SqlCommand(populateTransactionTypeTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Populated TransactionType table.");
                    }



                    // Insert data into the BankAccounts table
                    using (SqlCommand command = new SqlCommand(insertBankAccountsTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Inserted data into BankAccounts table.");
                    }


                    // Insert data into the Transactions table
                    using (SqlCommand command = new SqlCommand(insertTransactionsTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Inserted data into Transactions table.");
                    }

                    // Insert data into the VmTransfers table
                    using (SqlCommand command = new SqlCommand(insertVmTransfersTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Inserted data into VmTransfers table.");
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            Console.ReadLine(); // wait for user to press enter before closing the console window.
        }
    }
}
