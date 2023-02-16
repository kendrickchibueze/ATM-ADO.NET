using System.Collections.Generic;
using System.Data.SqlClient;
using TalentAtmDAL;

namespace TalentAtmClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("************Welcome To My Bank Atm App*************");



            RunClient client = new RunClient();

            await client.Run();









            //TalentAtmDB talentDB = new();

            //await talentDB.CreateDBAndTables();





        }
    }
}
    




    
