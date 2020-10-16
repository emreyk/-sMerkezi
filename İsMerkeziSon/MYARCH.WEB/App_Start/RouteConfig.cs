using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MYARCH.WEB
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();


            routes.MapRoute(
            name: "TopluBorclandirDuzenle",
            url: "TopluBorclandir/Duzenle/{refno}",
            defaults: new { controller = "TopluBorclandir", action = "Duzenle", refno = UrlParameter.Optional }
            );

          routes.MapRoute(
          name: "TahsilatGuncelle",
          url: "Tahsilat/Guncelle/{refno}",
          defaults: new { controller = "HesapHaraket", action = "FinansalTahsilatDuzenle", refno = UrlParameter.Optional }
          );

            routes.MapRoute(
               name: "Login",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
           );

          



            routes.MapRoute(
            name: "TopluBorclandirSil",
            url: "TopluBorclandir/Sil/{refno}",
            defaults: new { controller = "TopluBorclandir", action = "Sil", refno = UrlParameter.Optional }
            );

   

            routes.MapRoute(
            name: "Kisiler",
            url: "Kisiler/KisiDuzenle/{ id}",
              defaults: new { controller = "Kisiler", action = "KisiDuzenle" }
          );

        }
    }
}
