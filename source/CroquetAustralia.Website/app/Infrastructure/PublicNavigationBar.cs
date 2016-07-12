using System.Collections.Generic;

namespace CroquetAustralia.Website.App.Infrastructure
{
    public static class PublicNavigationBar
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
                    new NavigationItem("Constitution, Regulations & Policies", "~/governance/constitution-regulations-and-policies"),
                    new NavigationItem("Members", "~/governance/members"),
                    new NavigationItem("Board Meeting Minutes", "~/governance/minutes/board-meeting-minutes"),
                    new NavigationItem("Insurance", "~/governance/insurance")),
                new NavigationItem("Tournaments", "~/tournaments"),
                new NavigationItem("Disciplines",
                    new NavigationItem("Association Croquet",
                        new NavigationItem("Coaching", "~/disciplines/association-croquet/coaching"),
                        new NavigationItem("Refereeing", "~/disciplines/association-croquet/refereeing"),
                        new NavigationItem("Resources", "~/disciplines/association-croquet/resources")),
                    new NavigationItem("Gateball", "http://gateball.com.au"),
                    new NavigationItem("Golf Croquet",
                        new NavigationItem("Coaching", "~/disciplines/golf-croquet/coaching"),
                        new NavigationItem("Refereeing", "~/disciplines/golf-croquet/refereeing"),
                        new NavigationItem("Resources", "~/disciplines/golf-croquet/resources"))),
                new NavigationItem("WCF", "http://www.worldcroquet.org.uk/")
            };
        }
    }
}