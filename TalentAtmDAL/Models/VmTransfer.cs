namespace TalentAtmDAL
{
    public class VmTransfer
    {
        public decimal TransferAmount { get; set; }
        public Int64 RecipientBankAccountNumber { get; set; }

        public string RecipientBankAccountName { get; set; }
    }
}
