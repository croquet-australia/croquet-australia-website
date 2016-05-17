using CroquetAustralia.Website.App.Infrastructure;
using Microsoft.Owin.Security;
using Arg = OpenMagic.Argument;

namespace CroquetAustralia.Website.App.account
{
    public class SignInViewModel : ViewModel
    {
        public SignInViewModel(string returnUrl, AuthenticationDescription[] authenticationDescriptions)
        {
            Arg.MustNotBeEmpty(authenticationDescriptions, "authenticationDescriptions");

            ReturnUrl = returnUrl;
            AuthenticationDescriptions = authenticationDescriptions;
        }

        public AuthenticationDescription[] AuthenticationDescriptions { get; private set; }
        public string ReturnUrl { get; private set; }
    }
}