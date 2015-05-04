namespace CroquetAustraliaWebsite.Library.Authentication.Domain.Roles
{
    public class Editor : Role
    {
        public Editor()
            : base(typeof(Editor).Name)
        {
        }
    }
}