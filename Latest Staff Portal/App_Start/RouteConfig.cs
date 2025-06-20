﻿using System.Web.Mvc;
using System.Web.Routing;

namespace Latest_Staff_Portal
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Login", action = "Login", id = UrlParameter.Optional }
            );
        }
    }
}