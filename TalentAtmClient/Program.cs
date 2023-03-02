using TalentAtmClient.Atm.UI;
using TalentAtmDAL;

namespace TalentAtmClient
{
    internal class Program
    {
        static async Task Main(string[] args)

        {



            await TalentAtmDB.CreateDBAndTables();

            await Task.Delay(1000);

            RunClient client = new RunClient();

            await client.Run();




        }
    }
}






