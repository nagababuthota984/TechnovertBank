using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TechnovertBank.API.ApiModels
{
    public static class Constant
    {
        public const string AccountWithIdNotFound = "Account with matching id not found";
        public const string InvalidAccountIdFormat = "Invalid Account Id Format";
        public const string AccountWithNumNotFound = "Account with matching Account Number not found";
        public const string InvalidAccountNumFormat = "Invalid account number format.";
        public const string BankWithIdNotFound = "Bank with matching id not found.";
        public const string CustomerDetailsUpdated = "Customer details updated successfully";
        public const string AccountDeleted = "Account has been deleted";
        public const string InvalidBankIdFormat = "Invalid bank Id format.";
        public const string CurrencyAdded = "Currency added successfully";
        public const string CurrencyAlreadyExists = "Currency already exists!";
        public const string EmployeeCreationFailed = "Employee not created. Please try again!";
        public const string CustomerWithIdNotFound = "Customer with matching id not found";
        public const string InvalidCustomerIdFormat = "Please provide a valid customer id";
        public const string TransWithMatchingIdNotFound = "Transaction with matching id not found";
        public const string TransWithMatchingDateNotFound = "Transactions with matching date not found";
        public const string TransWithMatchingBankIdNotFound = "Transactions with matching bankid not found";
        public const string BankCreationFailed = "Couldn't create bank. Please provide valid details and try again";
        public const string AmountTransferSuccess = "Amount transferred successfully";
        public const string WithdrawlSuccess = "Withdrawn successfully!";
        public const string InsufficientFunds = "Insufficient funds.";
        public const string AmountMustAboveZero = "Amount should be greater than 0.";
        public const string CurrencyWithMatchingNameNotFound = "Currency with matching name not found.";
        public const string DepositSuccess = "Deposited Successfully!";
    }
}
