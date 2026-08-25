using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using ApiEmociones.Data;
using ApiEmociones.Infraestructura;
using ApiEmociones.Models;
using AutoMapper;

namespace ApiEmociones.Services
{
    public class NotasService : INotasService
    {
        private emocional_SampleDBEntities db;
        private readonly IMapper _mapper;
        public NotasService()
        {
            _mapper = Mapper.Instance;
            db = new emocional_SampleDBEntities();
        }

        public bool agregarNota(NotaInputDTO x)
        {
            var tsc = db.Database.BeginTransaction();
            try
            {
                var nota = _mapper.Map<Nota>(x);
                nota.fecha=DateTime.Now;
                db.Nota.Add(nota);
                db.SaveChanges();
                tsc.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _ = ex;
                tsc.Rollback();
                return false;
            }
        }

        public bool eliminarNotaPorID(int id)
        {
            var tsc = db.Database.BeginTransaction();
            try
            {
                var objDB = db.Nota.Single(x => x.id_nota == id);
                db.Nota.Remove(objDB);
                db.SaveChanges();
                tsc.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _ = ex;
                tsc.Rollback();
                return false;
            }
        }

        public List<dynamic> notasPorUsuario(int UserID)
        {
            var result = (from nota in db.Nota
                    join usuario in db.Usuarios
                    on nota.id_usuario equals usuario.id_usuario
                    join emocion in db.Emociones
                    on nota.id_emocion equals emocion.id_emocion
                    where usuario.id_usuario == UserID
                    select new
                    {
                        nota.id_nota,
                        emocion = new {
                            emocion.id_emocion,
                            emocion.emocionNombre
                        },
                        nota.texto,
                        nota.fecha
                    }).ToList();

            return result.Select(x => new
            {
                x.id_nota,
                x.emocion,
                x.texto,
                x.fecha
            }).ToList<dynamic>();
        }

        public dynamic ObtenerNotaPorId(int id)
        {
            var resultado = (from nota in db.Nota
                             join emocion in db.Emociones
                             on nota.id_emocion equals emocion.id_emocion
                             where nota.id_nota == id
                             select new
                             {
                                 nota.id_nota,
                                 emocion = new
                                 {
                                     emocion.id_emocion,
                                     emocion.emocionNombre
                                 },
                                 nota.texto,
                                 nota.fecha
                             }).FirstOrDefault();

            return resultado;
        }


        public List<NotaDTO> notaPorFechaYUsuario(notasPorFechaDTO datos)
        {
            DateTime fechaObjetivo = DateTime.ParseExact(datos.fechaBuscada, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var notas = (from usuario in db.Usuarios
                         join nota in db.Nota
                         on usuario.id_usuario equals nota.id_usuario
                         where usuario.id_usuario == datos.idUsuario &&
                         DbFunctions.TruncateTime(nota.fecha) == fechaObjetivo
                         select nota).ToList();

            return Mapper.Map<List<NotaDTO>>(notas);
        }
    }
}