using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using ApiEmociones.Infraestructura;
using AutoMapper;

namespace ApiEmociones
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Configurar AutoMapper
            Mapper.Initialize(cfg =>
            {
                // Registra el perfil de mapeo
                cfg.AddProfile<MapperProfile>();
            });
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
