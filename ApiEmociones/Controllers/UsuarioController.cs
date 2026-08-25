using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.UI.WebControls;
using ApiEmociones.Data;
using ApiEmociones.Infraestructura;
using ApiEmociones.Models;
using ApiEmociones.Services;
using ApiEmociones.Wrapper;
using AutoMapper;

namespace ApiEmociones.Controllers
{
    [RoutePrefix("Api/Usuarios")]
    public class UsuarioController : ApiController
    {
        private IUsuarioService service;
        public UsuarioController()
        {
            service = new UsuariosService();
        }

        [Route("add")]
        [HttpPost]
        public IHttpActionResult AgregarUsuario([FromBody]UsuarioDTO x)
        {
            var request = service.RegistrarUsuarios(x);
            if (request == 1)
            {
                return Ok(new ResponseWrapper<bool>
                {
                    Data = true,
                    Message = "Se registró Correctamente",
                    HttpStatusCode = HttpStatusCode.OK
                });
            }
            else if (request == -1)
            {
                return Ok(new ResponseWrapper<bool>
                {
                    Data = false,
                    Message = "El Correo ya esta en uso",
                    HttpStatusCode = HttpStatusCode.Conflict
                });
            }
            else
            {
                return BadRequest("Algo salió mal");
            }
        }

        [HttpDelete]
        [Route("remove/{idUsuario}")]
        public IHttpActionResult EliminarUsuario([FromUri]int idUsuario)
        {
            if (service.EliminarUsuario(idUsuario))
            {
                return Ok(new ResponseWrapper<bool>
                {
                    Data = true,
                    Message = "Se eliminó Correctamente",
                    HttpStatusCode = HttpStatusCode.OK
                });
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("ingresar")]
        public IHttpActionResult AutenticarUsuario([FromBody] LoginDTO login)
        {
            var usuarioId = service.Autenticacion(login.correo, login.passwd);
            if (usuarioId!=0)
            {
                return Ok(new ResponseWrapper<int>
                {
                    Data = usuarioId,
                    Message = "Autenticado Correctamente",
                    HttpStatusCode = HttpStatusCode.OK
                });
            }
            return Ok(new ResponseWrapper<int>
            {
                Data = 0,
                Message = "Correo o Contraseña Incorrectos",
                HttpStatusCode = HttpStatusCode.NotFound
            });
        }

        [HttpGet]
        [Route("ObtenerApodo")]
        public IHttpActionResult getApodo([FromUri] int UsuarioID)
        {
            var apodo = service.GetApodo(UsuarioID);
            if (apodo != null)
            {
                return Ok(new ResponseWrapper<string>
                {
                    Data = apodo,
                    Message = "Autenticado Correctamente",
                    HttpStatusCode = HttpStatusCode.OK
                });
            }
            return Ok(new ResponseWrapper<int>
            {
                Data = 0,
                Message = "No se encontro el usuario",
                HttpStatusCode = HttpStatusCode.NotFound
            });
        }
    }
}
