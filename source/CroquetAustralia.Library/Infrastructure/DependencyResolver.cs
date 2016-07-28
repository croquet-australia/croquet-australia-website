using System;
using Anotar.LibLog;
using Microsoft.Practices.ServiceLocation;

namespace CroquetAustralia.Library.Infrastructure
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
                LogTo.WarnException($"ServiceLocator failed to get an instance of '{typeof(TService)}'. Using default service '{typeof(TDefaultService)}'.", exception);
                return new TDefaultService();
            }
        }
    }
}