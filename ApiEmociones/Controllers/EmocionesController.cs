using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ApiEmociones.Data;
using ApiEmociones.Models;
using ApiEmociones.Services;
using ApiEmociones.Wrapper;

namespace ApiEmociones.Controllers
{
    [RoutePrefix("Api/Emociones")]
    public class EmocionesController : ApiController
    {
        private IEmocionesService service;
        public EmocionesController()
        {
            service = new EmocionesService();
        }

        [HttpGet]
        [Route("getAll")]
        public IHttpActionResult GetAll()
        {
            var data = service.GetAll();
            if (data.Count > 0)
            {
                return Ok(new ResponseWrapper<List<EmocionDTO>>
                {
                    Data = data,
                    HttpStatusCode = HttpStatusCode.OK,
                    Message = "Consulta Correcta"
                });
            }
            else
            {
                return BadRequest("Error");
            }
        }

        [HttpGet]
        [Route("FrecuenciasSemanales/{idUsuario}")]
        public IHttpActionResult notaPorId([FromUri] int idUsuario)
        {
            var query = service.FrecuenciaEmocionesSemanaActual(idUsuario);

            if (query.Total < 1)
            {
                return Ok(new ResponseWrapper<dynamic>
                {
                    Data = 0,
                    Message = "Aún no se registra una nota esta semana",
                    HttpStatusCode = HttpStatusCode.NotFound
                });
            }

            return Ok(new ResponseWrapper<dynamic>
            {
                Data = query,
                Message = "Frecuencia de emociones obtenida correctamente",
                HttpStatusCode = HttpStatusCode.OK
            });
        }



        [HttpGet]
        [Route("FrecuenciasConFechas")]
        public IHttpActionResult FrecuenciasConFechas([FromUri] FrecuenciasConFechasDTO datos)
        {
            var query = service.FrecuenciaEmocionesEntreFechas(datos);

            if (query.Estadisticas.Total < 1)
            {
                return Ok(new ResponseWrapper<dynamic>
                {
                    Data = 0,
                    Message = "No se encontró notas en esas fechas",
                    HttpStatusCode = HttpStatusCode.NotFound
                });
            }

            return Ok(new ResponseWrapper<dynamic>
            {
                Data = query,
                Message = "Frecuencia de emociones obtenida correctamente",
                HttpStatusCode = HttpStatusCode.OK
            });
        }

        [HttpGet]
        [Route("FrecuenciasConFechasPorHora")]
        public IHttpActionResult FrecuenciasConFechasPorHora([FromUri] FrecuenciasConFechasHoraDTO datos)
        {
            var query = service.FrecuenciaEspecificaPorHoraEntreFechas(datos);

            if (query.Total < 1)
            {
                return Ok(new ResponseWrapper<dynamic>
                {
                    Data = 0,
                    Message = "No se encontró notas en esas fechas",
                    HttpStatusCode = HttpStatusCode.NotFound
                });
            }

            return Ok(new ResponseWrapper<dynamic>
            {
                Data = query,
                Message = "Frecuencia de emociones obtenida correctamente",
                HttpStatusCode = HttpStatusCode.OK
            });
        }
    }
}
