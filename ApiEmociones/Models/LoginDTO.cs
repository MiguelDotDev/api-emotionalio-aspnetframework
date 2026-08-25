using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiEmociones.Models
{
    public class LoginDTO
    {
        public string correo {  get; set; }
        public string passwd { get; set; }
    }
}