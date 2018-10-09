using System.Web.Http;
using WebActivatorEx;
using WebApiTestApp;
using Swashbuckle.Application;
using System;
using System.Xml.XPath;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace WebApiTestApp
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "WebApiTestApp");

                        c.IncludeXmlComments(GetXmlCommentsPath());
                    })
                .EnableSwaggerUi(c => { });
        }

        private static string GetXmlCommentsPath()
        {
            return string.Format(@"{0}\bin\WebApiTestApp.xml", AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}
