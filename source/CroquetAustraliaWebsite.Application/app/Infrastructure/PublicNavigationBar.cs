using System.Collections.Generic;

namespace CroquetAustraliaWebsite.Application.App.Infrastructure
{
    public class PublicNavigationBar
    {
        public static IEnumerable<NavigationItem> GetNavigationItems()
        {
            return new[]
            {
                new NavigationItem("Contact Us",
                    new NavigationItem("Office & Board", "~/governance/contact-us"),
                    new NavigationItem("Committees", "~/governance/contact-us#committees"),
                    new NavigationItem("Appointed Officers", "~/governance/contact-us#appointed-officers"),
                    new NavigationItem("State Associations", "~/governance/state-associations")),
                new NavigationItem("Governance",
                    new NavigationItem("Background", "~/governance/background"),
                    new NavigationItem("Constitution, Regulations & Policies", "~/governance/constitution-regulations-and-policies")),
                new NavigationItem("Tournaments", "~/tournaments"),
                new NavigationItem("Disciplines",
                    new NavigationItem("Golf Croquet",
                        new NavigationItem("Refereeing", "~/disciplines/golf-croquet/refereeing"),
                        new NavigationItem("Resources", "~/disciplines-golf-croquet-resources")))
            };
        }
    }
}
