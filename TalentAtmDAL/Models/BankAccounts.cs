using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentAtmDAL
{
    public class BankAccounts
    {
        public string FullName { get; set; }
        public int AccountNumber{ get; set; }
        public Int64 CardNumber{ get; set; }
        public int PinCode { get; set; }
        public decimal Balance { get; set; }

        public bool isLocked { get; set; }
    }
}
