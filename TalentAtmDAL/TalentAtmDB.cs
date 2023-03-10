using System.Configuration;
using System.Data.SqlClient;
using TalentAtmClient.Atm.UI;

namespace TalentAtmDAL
{
    public class TalentAtmDB
    {

        public static async Task CreateDBAndTables()
        {


            string connectionString = ConfigurationManager.ConnectionStrings["DATA"].ConnectionString;


            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("Connection string not found or invalid.");
            }

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
            string dataSource = builder.DataSource;
            string integratedSecurity = "Integrated security=SSPI;";
            string databaseName = "database=master;";
            string serverName = "Server=" + dataSource + ";";
            string newConnectionString = serverName + integratedSecurity + databaseName;



            string createDatabaseQuery = @"
                                        IF NOT EXISTS(SELECT name FROM sys.databases WHERE name = 'PTalentAtmDB')
                                        BEGIN
                                            CREATE DATABASE PTalentAtmDB;
                                        END";



            string useDatabaseQuery = "USE PTalentAtmDB";


            string createBankAccountsTableQuery = @"
                                        IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'BankAccounts')
                                        BEGIN
                                            CREATE TABLE BankAccounts (
                                                AccountNumber BIGINT PRIMARY KEY,
                                                FullName VARCHAR(50) NOT NULL,
                                                CardNumber BIGINT NOT NULL,
                                                PinCode INT NOT NULL,
                                                Balance DECIMAL(18,2) NOT NULL,
                                                isLocked BIT NOT NULL,
                                                FailedAttempts INT NOT NULL DEFAULT 0
                                            )
                                        END";




            string createTransactionTypeTableQuery = @"
                                        IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'TransactionType')
                                        BEGIN
                                            CREATE TABLE TransactionType (
                                                TransactionTypeId INT PRIMARY KEY IDENTITY(1,1),
                                                TransactionTypeName VARCHAR(50) NOT NULL
                                            )
                                        END";


            string createTransactionsTableQuery = @"
                                            IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Transactions')
                                            BEGIN
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
                                                )
                                            END";




            // SQL query to create the VmTransfers table
            string createVmTransfersTableQuery = @"
                       IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'VmTransfers')
                       BEGIN
                        CREATE TABLE VmTransfers (
                            TransferId INT PRIMARY KEY IDENTITY(1,1),
                            TransferAmount DECIMAL(18,2) NOT NULL,
                            RecipientBankAccountNumber BIGINT NOT NULL,
                            RecipientBankAccountName VARCHAR(50) NOT NULL,
                            FOREIGN KEY (RecipientBankAccountNumber) REFERENCES BankAccounts(AccountNumber)
                        )
                      END";



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
                using (SqlConnection connection = new SqlConnection(newConnectionString))
                {
                    connection.Open();

                    
                    using (SqlCommand command = new SqlCommand(createDatabaseQuery, connection))
                    {
                        command.ExecuteNonQuery();
                        
                    }

                    
                    using (SqlCommand command = new SqlCommand(useDatabaseQuery, connection))
                    {
                        command.ExecuteNonQuery();
                       
                    }

                   
                    using (SqlCommand command = new SqlCommand(createBankAccountsTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                       
                    }

                  
                    using (SqlCommand command = new SqlCommand(createTransactionTypeTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                      
                    }

                    
                    using (SqlCommand command = new SqlCommand(createTransactionsTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                        
                    }

                    
                    using (SqlCommand command = new SqlCommand(createVmTransfersTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                      
                    }




                    // SQL query to check if TransactionType table is empty
                    string checkTransactionTypeTableQuery = @"SELECT COUNT(*) FROM TransactionType";

                    int TransactioTypeCount = 0;

                    using (SqlCommand command = new SqlCommand(checkTransactionTypeTableQuery, connection))
                    {
                        TransactioTypeCount = (int)await command.ExecuteScalarAsync();
                    }


                    if (TransactioTypeCount == 0)
                    {


                        // Populate the TransactionType table
                        using (SqlCommand command = new SqlCommand(populateTransactionTypeTableQuery, connection))
                        {
                            command.ExecuteNonQuery();
                           
                        }
                    }




                    // SQL query to check if BankAccounts table is empty
                    string checkBankAccountsTableQuery = @"SELECT COUNT(*) FROM BankAccounts";

                    int bankAccountsCount = 0;

                    using (SqlCommand command = new SqlCommand(checkBankAccountsTableQuery, connection))
                    {
                        bankAccountsCount = (int)await command.ExecuteScalarAsync();
                    }


                    if (bankAccountsCount == 0)
                    {

                        // Insert data into the BankAccounts table
                        using (SqlCommand command = new SqlCommand(insertBankAccountsTableQuery, connection))
                        {
                            command.ExecuteNonQuery();
                            
                        }

                    }



                    // SQL query to check if Transactions table is empty
                    string checkTransactionsTableQuery = @"SELECT COUNT(*) FROM Transactions";

                    int TransactionsCount = 0;

                    using (SqlCommand command = new SqlCommand(checkTransactionsTableQuery, connection))
                    {
                        TransactionsCount = (int)await command.ExecuteScalarAsync();
                    }



                    // SQL query to check if VmTransfers table is empty
                    string checkVmTransferTableQuery = @"SELECT COUNT(*) FROM VmTransfers";

                    int VmTransfersCount = 0;

                    using (SqlCommand command = new SqlCommand(checkVmTransferTableQuery, connection))
                    {
                        VmTransfersCount = (int)await command.ExecuteScalarAsync();
                    }


                    if (VmTransfersCount == 0)
                    {


                        // Insert data into the VmTransfers table
                        using (SqlCommand command = new SqlCommand(insertVmTransfersTableQuery, connection))
                        {
                            command.ExecuteNonQuery();

                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }


        }





    }
}
