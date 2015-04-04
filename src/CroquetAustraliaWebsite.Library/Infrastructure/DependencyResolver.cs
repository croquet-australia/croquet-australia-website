using System;
using Anotar.LibLog;
using Microsoft.Practices.ServiceLocation;

namespace CroquetAustraliaWebsite.Library.Infrastructure
{
    public class DependencyResolver
    {
        public static TService GetInstance<TService, TDefaultService>() where TDefaultService : TService, new()
        {
            try
            {
                return ServiceLocator.Current.GetInstance<TService>();
            }
            catch (Exception exception)
            {
                LogTo.WarnException(string.Format("ServiceLocator failed to get an instance of '{0}'. Using default service '{1}'.", typeof(TService), typeof(TDefaultService)), exception);
                return new TDefaultService();
            }
        }
    }
}