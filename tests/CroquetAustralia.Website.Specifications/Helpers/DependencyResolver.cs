using BoDi;

namespace CroquetAustralia.Website.Specifications.Helpers
{
    public static class DependencyResolver
    {
        public static IObjectContainer Container { get; set; }

        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }
    }
}