using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using ApiEmociones.Data;
using ApiEmociones.Infraestructura;
using ApiEmociones.Models;
using AutoMapper;

namespace ApiEmociones.Services
{
    public class UsuariosService: IUsuarioService
    {
        private readonly IMapper _mapper;
        private emocional_SampleDBEntities db;
        public UsuariosService()
        {
            _mapper = Mapper.Instance;
            db = new emocional_SampleDBEntities();
        }

        public int RegistrarUsuarios(UsuarioDTO X)
        {
            var transaccion = db.Database.BeginTransaction();
            try
            {
                var UsuarioPrevio = db.Usuarios.FirstOrDefault(x => x.correo == X.correo);

                if (UsuarioPrevio == null)
                {
                    db.Usuarios.Add(_mapper.Map<Usuarios>(X));
                    db.SaveChanges();
                    transaccion.Commit();
                    return 1;
                }
                transaccion.Rollback();
                return -1;
            }
            catch (Exception ex)
            {
                _ = ex;
                transaccion.Rollback();
                return 0;
            }
        }

        public bool EliminarUsuario(int id)
        {
            var transaccion = db.Database.BeginTransaction();
            try
            {
                var user = db.Usuarios.FirstOrDefault(x=>x.id_usuario == id);
                if (user != null)
                {
                    db.Usuarios.Remove(user);
                    db.SaveChanges();
                    transaccion.Commit();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _ = ex;
                transaccion.Rollback();
                return false;
            }
        }

        public string GetApodo(int id)
        {
            try
            {
                var Usuario = db.Usuarios.FirstOrDefault(x=>x.id_usuario == id);
                return Usuario.apodo;
            }
            catch (Exception ex)
            {
                _= ex;
                return null;
            }
        }
        public int Autenticacion(string correo, string password)
        {
            var usuario = db.Usuarios.FirstOrDefault(x => x.correo == correo && x.password == password);
            if (usuario != null)
            {
                return usuario.id_usuario;
            }
            return 0;
        }
    }
}