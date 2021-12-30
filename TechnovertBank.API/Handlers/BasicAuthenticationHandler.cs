//using Microsoft.AspNetCore.Authentication;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
//using System;
//using System.Net.Http.Headers;
//using System.Text;
//using System.Text.Encodings.Web;
//using System.Threading.Tasks;

//namespace TechnovertBank.API.Handlers
//{
//    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
//    {
//        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
//        {
//        }

//        protected override async  Task<AuthenticateResult> HandleAuthenticateAsync()
//        {
//            if(Request.Headers.ContainsKey("Authorization"))
//            {
//                //take the authorization key value pair from the request header.
//                var authenticationToken = Convert.FromBase64String(AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]).Parameter);
//                //decode the token to utf-8
//                string decodedToken = Encoding.UTF8.GetString(authenticationToken);
//                string username = decodedToken.Split(":")[0];
//                string password = decodedToken.Split(":")[1];
//                if(ApiSecurity.ValidateUser(username, password))
//                {
//                    var claims = new[] {new Claim(ClaimTypes.Name,)}
//                }

//            }
//        }


//    }
//}
