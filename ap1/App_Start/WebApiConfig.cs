using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;

namespace ap1.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(name: "AllEvents",
                            routeTemplate: "api/{controller}",
                            defaults: new
                            {
                                controller = "webapi"
                            }

    );

            config.Routes.MapHttpRoute(name: "EventDetailsAPI",
                                        routeTemplate: "api/{controller}/{id}",
                                        defaults: new
                                        {
                                        }

                );

            config.Routes.MapHttpRoute(name: "EnrollUserToEventByQR",
                            routeTemplate: "api/{controller}/{UserId}/{EventId}/{TicketAmount}",
                            defaults: new
                            {
                            }

                );

            config.Routes.MapHttpRoute(name: "StartDate",
                routeTemplate: "api/{controller}/{localization}/{StartDate}",
                defaults: new
                {
                }
              );

            config.Routes.MapHttpRoute( name: "StartDateAndEndDate",
                                        routeTemplate: "api/{controller}/{localization}/{StartDate}/{EndDate}",
                                        defaults: new
                                        {
                                        }
                                      );

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.MediaTypeMappings.Add
                              ( new System.Net.Http.Formatting.RequestHeaderMapping("Accept",
                                "text/html",
                                StringComparison.InvariantCultureIgnoreCase,
                                true,
                                "application/json")
                              );
        }
    }
}