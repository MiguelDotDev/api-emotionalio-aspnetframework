using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiEmociones.Models
{
    public class FrecuenciasConFechasDTO
    {
        public int idUsuario {  get; set; }
        public string inicio { get; set; }
        public string fin {  get; set; }
    }
}