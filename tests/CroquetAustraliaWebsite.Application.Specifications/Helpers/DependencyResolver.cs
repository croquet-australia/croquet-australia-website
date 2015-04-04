using BoDi;

namespace CroquetAustraliaWebsite.Application.Specifications.Helpers
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