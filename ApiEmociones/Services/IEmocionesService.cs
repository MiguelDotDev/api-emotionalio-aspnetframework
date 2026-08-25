using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiEmociones.Data;
using ApiEmociones.Models;

namespace ApiEmociones.Services
{
    public interface IEmocionesService
    {
        dynamic FrecuenciaEmocionesEntreFechas(FrecuenciasConFechasDTO Datos);
        dynamic FrecuenciaEmocionesSemanaActual(int usuaioId);
        dynamic FrecuenciaEspecificaPorHoraEntreFechas(FrecuenciasConFechasHoraDTO datos);
        List<EmocionDTO> GetAll();
    }
}
