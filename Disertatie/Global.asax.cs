using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Disertatie.Mvc;
using Disertatie.Core;
using AutoMapper;
using OpenForum.Core;

namespace Disertatie
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new NotificationAttribute("~/Views/Shared/_Layout.cshtml", "~/Views/Shared/_Layout2Columns.cshtml"));
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("elmah.axd");
            routes.IgnoreRoute("{*htc}", new { htc = @".*\.htc(/.*)?" });
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //if (HttpRuntime.UsingIntegratedPipeline)
            //{

                routes.MapRoute("NotFound", "NotFound",
                    new { controller = "Error", action = "NotFound" });
                routes.MapRoute("Login", "login",
                    new { controller = "Account", action = "LogOn" });
                routes.MapRoute("Membru_Home", "Membru",
                 new { controller = "Home", action = "Membru" });
                //routes.MapRoute("Service_Home", "Home/Service/{name}",
                // new { controller = "Home", action = "Service", name = UrlParameter.Optional });

                routes.MapRoute("Images", "Images/{action}/{imageUid}",
                    new { controller = "Images", id = UrlParameter.Optional });

                routes.MapRoute(
                    "Default", // Route name
                    "{controller}/{action}/{id}", // URL with parameters
                    new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
                );
            //}
            //else
            //{

            //    routes.MapRoute("NotFound", "NotFound.aspx",
            //        new { controller = "Error", action = "NotFound" });
            //    routes.MapRoute("Login", "login.aspx",
            //        new { controller = "Account", action = "LogOn" });
            //    //routes.MapRoute("Operator_Home", "UltimeleDosare.aspx",
            //    // new { controller = "Home", action = "Operator" });
            //    //routes.MapRoute("Service_Home", "Home.aspx/Service/{name}",
            //    //new { controller = "Home", action = "Service", name = UrlParameter.Optional });

            //    routes.MapRoute("Images", "Images.aspx/{action}/{imageUid}",
            //        new { controller = "Images", id = UrlParameter.Optional });

            //    routes.MapRoute(
            //        "Default", // Route name
            //        "{controller}.aspx/{action}/{id}", // URL with parameters
            //        new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            //    );
            //}
        }

        protected void Application_Start()
        {
            
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

#if DEBUG
            HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();
#endif
            var factory = SessionManager.ConfigureFromFile(Server.MapPath("~/hibernate.config"));

           ConfigureAutoMapper();
           
           //OpenForumManager.SimpleInitialize();
            //IncarcaMarciAuto();
        }
        private void ConfigureAutoMapper()
        {

            Mapper.CreateMap<Disertatie.Areas.Admin.Models.UserModel, Core.Models.Utilizator>();
            Mapper.CreateMap<Core.Models.Utilizator, Disertatie.Areas.Admin.Models.UserModel>()
                .AfterMap((source, dest) => { dest.ConfirmareParola = source.Parola; });
        }
    }
}