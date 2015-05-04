namespace CroquetAustraliaWebsite.Library.Authentication.Domain.Roles
{
    public abstract class Role
    {
        public string Name { get; private set; }

        protected Role(string name)
        {
            Name = name;
        }
    }
}