using System;
using System.Collections.Generic;
using ApiEmociones.Models;

namespace ApiEmociones.Services
{
    internal interface INotasService
    {
        bool agregarNota(NotaInputDTO x);
        bool eliminarNotaPorID(int id);
        List<NotaDTO> notaPorFechaYUsuario(notasPorFechaDTO datos);
        List<dynamic> notasPorUsuario(int UserID);
        dynamic ObtenerNotaPorId(int id);
    }
}
