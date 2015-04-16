using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CroquetAustraliaWebsite.Application.App.Infrastructure
{
    public static class RazorViewEngineExtensions
    {
        public static RazorViewEngine DisableVbhtml(this RazorViewEngine engine)
        {
            engine.AreaViewLocationFormats = FilterOutVbhtml(engine.AreaViewLocationFormats);
            engine.AreaMasterLocationFormats = FilterOutVbhtml(engine.AreaMasterLocationFormats);
            engine.AreaPartialViewLocationFormats = FilterOutVbhtml(engine.AreaPartialViewLocationFormats);
            engine.ViewLocationFormats = FilterOutVbhtml(engine.ViewLocationFormats);
            engine.MasterLocationFormats = FilterOutVbhtml(engine.MasterLocationFormats);
            engine.PartialViewLocationFormats = FilterOutVbhtml(engine.PartialViewLocationFormats);
            engine.FileExtensions = FilterOutVbhtml(engine.FileExtensions);

            return engine;
        }

        private static string[] FilterOutVbhtml(string[] source)
        {
            return source.Where(s => !s.Contains("vbhtml")).ToArray();
        }
    }
}