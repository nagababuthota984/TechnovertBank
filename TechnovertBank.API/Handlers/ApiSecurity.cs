using TechnovertBank.Services;

namespace TechnovertBank.API.Handlers
{
    public class ApiSecurity
    {
        private static IBankService bankService;
        public static IAccountService accountService;
        public ApiSecurity(IBankService bnkService,IAccountService accService)
        {
            bankService = bnkService;
            accountService = accService;
        }
        public static bool ValidateUser(string username, string password)
        {
            if (bankService.IsValidEmployee(username, password))
            {
                return true;
            }
            else
            {
                if (accountService.IsValidCustomer(username, password))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
