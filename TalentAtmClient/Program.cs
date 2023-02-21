using System.Collections.Generic;
using System.Data.SqlClient;
using TalentAtmClient.Atm.UI;
using TalentAtmDAL;

namespace TalentAtmClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Utility.PrintColorMessage(ConsoleColor.Cyan, "************Welcome To My Bank Atm App*************");


            await LoginAdminUser.Login();




        }
    }
}
    




    
