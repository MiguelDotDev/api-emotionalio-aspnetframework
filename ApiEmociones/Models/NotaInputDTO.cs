using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiEmociones.Models
{
    public class NotaInputDTO
    {
        public int id_usuario { get; set; }
        public int id_emocion { get; set; }
        public string texto { get; set; }
    }
}