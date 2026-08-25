using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiEmociones.Models
{
    public class UsuarioDTO
    {
        public string correo { get; set; }
        public string password { get; set; }
        public string apodo { get; set; }
    }
}