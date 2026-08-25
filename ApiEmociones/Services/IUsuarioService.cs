using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ApiEmociones.Data;
using ApiEmociones.Models;

namespace ApiEmociones.Services
{
    public interface IUsuarioService
    {
        int Autenticacion(string correo, string password);
        bool EliminarUsuario(int id);
        string GetApodo(int id);
        int RegistrarUsuarios(UsuarioDTO X);
    }
}