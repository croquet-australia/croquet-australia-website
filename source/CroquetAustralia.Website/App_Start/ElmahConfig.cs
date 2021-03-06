﻿using System;
using System.ComponentModel.Design;
using Anotar.NLog;
using CroquetAustralia.Library.Settings;
using CroquetAustralia.Website;
using Elmah;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(ElmahConfig), "Start")]

namespace CroquetAustralia.Website
{
    /// <summary>
    ///     Configures <see cref="Elmah.Io.ErrorLog" />.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         ElmahConfig uses AppSetings to configure <see cref="Elmah.ErrorLog" />. Typical configuration is in production
    ///         use Elmah.Io, otherwise use MemoryErrorLog.
    ///     </para>
    ///     <para>
    ///         This source code was originally inspired by blog post http://blog.elmah.io/configuring-elmah-io-from-code/ to
    ///         keep Elmah.Io LogId out of source control. I extended it to use ErrorLogType in AppSettings.
    ///     </para>
    /// </remarks>
    public static class ElmahConfig
    {
        public static void Start()
        {
            LogTo.Trace("Start");
            ServiceCenter.Current = CreateServiceProviderQueryHandler(ServiceCenter.Current);
        }

        private static ServiceProviderQueryHandler CreateServiceProviderQueryHandler(ServiceProviderQueryHandler sp)
        {
            LogTo.Trace("CreateServiceProviderQueryHandler(ServiceProviderQueryHandler sp)");

            return context =>
            {
                try
                {
                    var container = new ServiceContainer(sp(context));
                    var elmahSettings = new ElmahSettings();

                    container.AddService(typeof(ErrorLog), GetErrorLog(elmahSettings));

                    return container;
                }
                catch (Exception exception)
                {
                    LogTo.ErrorException("Could not configure Elmah.ErrorLog.", exception);
                    throw;
                }
            };
        }

        // todo: why is this method called twice during application startup when MemoryErrorLog is used.
        private static ErrorLog GetErrorLog(ElmahSettings elmahSettings)
        {
            var errorLogType = elmahSettings.ErrorLogType;

            LogTo.Info("Using '{0}' for error logging.", errorLogType);

            switch (errorLogType)
            {
                case "MemoryErrorLog":
                    return new MemoryErrorLog();

                default:
                    throw new NotSupportedException(string.Format("ErrorLogType '{0}' is not supported.", errorLogType));
            }
        }
    }
}