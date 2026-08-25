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
    [RoutePrefix("Api/Notas")]
    public class NotasController : ApiController
    {
        private INotasService service;
        public NotasController()
        {
            service = new NotasService();
        }

        [HttpPost]
        [Route("agregarNota")]
        public IHttpActionResult agregarNota(NotaInputDTO x)
        {
            if (service.agregarNota(x))
            {
                return Ok(new ResponseWrapper<bool>
                {
                    Data = true,
                    Message = "Se agregó correctamente",
                    HttpStatusCode = HttpStatusCode.OK
                });
            } else
            {
                return BadRequest("Hubo un error");
            }

        }

        [HttpDelete]
        [Route("eliminarNotaPorID/{id}")]
        public IHttpActionResult eliminarNotaPorID(int id)
        {
            if (service.eliminarNotaPorID(id))
            {
                return Ok(new ResponseWrapper<bool>
                {
                    Data = true,
                    Message = "Se elimino correctamente",
                    HttpStatusCode = HttpStatusCode.OK
                });
            }
            else
            {
                return BadRequest("Hubo un error");
            }
        }

        [HttpGet]
        [Route("notaPorId/{id}")]
        public IHttpActionResult notaPorId(int id)
        {
            var nota = service.ObtenerNotaPorId(id);
            if (nota != null)
            {
                return Ok(new ResponseWrapper<dynamic>
                {
                    Data = nota,
                    Message = "Consultado correctamente",
                    HttpStatusCode = HttpStatusCode.OK
                });
            }
            else
            {
                return BadRequest("Hubo un error");
            }
        }

        [HttpGet]
        [Route("notasPorUsuario/{userId}")]
        public IHttpActionResult notasPorUsuario(int userId)
        {
            var notas = service.notasPorUsuario(userId);
            if (notas != null)
            {
                return Ok(new ResponseWrapper<List<dynamic>>
                {
                    Data = notas,
                    Message = "Se agregó correctamente",
                    HttpStatusCode = HttpStatusCode.OK
                });
            }
            else
            {
                return BadRequest("Hubo un error");
            }
        }

        [HttpGet]
        [Route("notasPorFecha")]
        public IHttpActionResult notasPorFecha([FromUri]notasPorFechaDTO datos)
        {
            var notas = service.notaPorFechaYUsuario(datos);
            if (notas != null)
            {
                return Ok(new ResponseWrapper<List<NotaDTO>>
                {
                    Data = notas,
                    Message = "Se agregó correctamente",
                    HttpStatusCode = HttpStatusCode.OK
                });
            }
            else
            {
                return BadRequest("Hubo un error");
            }
        }
    }
}
