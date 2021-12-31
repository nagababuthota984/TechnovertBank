using TechnovertBank.Models;

namespace TechnovertBank.API.ApiModels
{
    public class TransferAmountModel
    {
        public string SenderAccountId { get; set; }
        public long ReceiverAccountNumber { get; set; }
        public decimal Amount { get; set; }
        public ModeOfTransferOptions Mode { get; set; }
    }
}
