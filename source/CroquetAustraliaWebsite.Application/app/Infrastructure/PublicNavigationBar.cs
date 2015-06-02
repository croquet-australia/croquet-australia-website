using System.Collections.Generic;

namespace CroquetAustraliaWebsite.Application.App.Infrastructure
{
    public class PublicNavigationBar
    {
        public static IEnumerable<NavigationItem> GetNavigationItems()
        {
            return new[]
            {
                new NavigationItem("Contact Us", "~/governance/contact-us"),
                new NavigationItem("Governance", 
                    new NavigationItem("State Associations", "~/governance/state-associations")),
                new NavigationItem("Disciplines",
                    new NavigationItem("Golf Croquet",
                        new NavigationItem("Refereeing", "~/disciplines/golf-croquet/refereeing")))
            };
        }
    }
}
