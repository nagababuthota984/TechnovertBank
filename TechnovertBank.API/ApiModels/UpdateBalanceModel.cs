namespace TechnovertBank.API.ApiModels
{
    public class UpdateBalanceModel
    {
        public string AccountId { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyName { get; set; }

    }
}
