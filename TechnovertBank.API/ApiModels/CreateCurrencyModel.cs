namespace TechnovertBank.API.ApiModels
{
    public class CreateCurrencyModel
    {
        public string CurrencyName { get; set; }
        public decimal ExchangeRate { get; set; }
        public string BankId { get; set; }
    }
}
