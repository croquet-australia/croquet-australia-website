namespace CroquetAustraliaWebsite.Library.Authentication.Domain.Roles
{
    public class Developer : Role
    {
        public Developer()
            : base(typeof(Developer).Name)
        {
        }
    }
}