namespace CroquetAustralia.Library.Authentication.Domain.Roles
{
    public abstract class Role
    {
        protected Role(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}