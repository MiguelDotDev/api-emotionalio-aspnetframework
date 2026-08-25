using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using ApiEmociones.Data;
using ApiEmociones.Models;
using AutoMapper;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace ApiEmociones.Services
{
    public class EmocionesService: IEmocionesService
    {
        private IMapper _mapper;
        private emocional_SampleDBEntities db;
        public EmocionesService()
        {
            _mapper = Mapper.Instance;
            db = new emocional_SampleDBEntities();
        }

        public List<EmocionDTO> GetAll()
        {
            return _mapper.Map<List<EmocionDTO>>(db.Emociones.ToList());
        }

        public dynamic FrecuenciaEmocionesSemanaActual(int usuarioId)
        {
            DateTime hoy = DateTime.Today;
            DateTime ultimoDomingo = hoy.AddDays(-(int)hoy.DayOfWeek);
            if (hoy.DayOfWeek == DayOfWeek.Sunday)
                ultimoDomingo = hoy;

            // Obtener las emociones desde el último domingo en adelante
            var emocionesDesdeDomingo = db.Nota
                .Where(n => n.id_usuario == usuarioId && n.fecha >= ultimoDomingo)
                .GroupBy(n => new { n.Emociones.id_emocion, n.Emociones.emocionNombre })
                .Select(g => new
                {
                    g.Key.id_emocion,
                    g.Key.emocionNombre,
                    frecuencia = g.Count()
                })
                .OrderBy(x=>x.frecuencia)
                .ToList();

            // Obtener la emoción más repetida
            var emocionMasRepetida = emocionesDesdeDomingo
                .OrderByDescending(e => e.frecuencia)
                .FirstOrDefault();

            return new
            {
                Total = (from usuario in db.Usuarios
                        join nota in db.Nota
                        on usuario.id_usuario equals nota.id_usuario
                        where nota.fecha >= ultimoDomingo && usuario.id_usuario == usuarioId
                         select nota).Count(),
                ListaFrecuencias = emocionesDesdeDomingo,
                MasFrecuente = emocionMasRepetida
            };
        }


        public dynamic FrecuenciaEmocionesEntreFechas(FrecuenciasConFechasDTO datos)
        {
            DateTime inicio = DateTime.ParseExact(datos.inicio, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime fin = DateTime.ParseExact(datos.fin, "yyyy-MM-dd", CultureInfo.InvariantCulture);


            var Notas = (from Nota in db.Nota
                            join Usuario in db.Usuarios
                            on Nota.id_usuario equals Usuario.id_usuario
                            join Emocion in db.Emociones
                            on Nota.id_emocion equals Emocion.id_emocion
                            where Usuario.id_usuario == datos.idUsuario &&
                            DbFunctions.TruncateTime(Nota.fecha) >= inicio &&
                            DbFunctions.TruncateTime(Nota.fecha) <= fin
                            select new
                            {
                                Nota.fecha,
                                Nota.texto,
                                Emocion = Emocion.emocionNombre
                            }).ToList();

            var Emociones = db.Nota
                .Where(n => n.id_usuario == datos.idUsuario &&
                DbFunctions.TruncateTime(n.fecha) >= inicio &&
                DbFunctions.TruncateTime(n.fecha) <= fin)

                .GroupBy(n => new { n.Emociones.id_emocion, n.Emociones.emocionNombre })
                .Select(g => new
                {
                    g.Key.id_emocion,
                    g.Key.emocionNombre,
                    frecuencia = g.Count()
                })
                .OrderByDescending(x => x.frecuencia)
                .ToList();

            var EmocionMasFrecuente = Emociones.FirstOrDefault();

            return new
            {
                Estadisticas = new
                {
                    Total = Notas.Count(),
                    EmocionMasFrecuente,
                    Emociones
                },
                Notas
            };
        }

        public dynamic FrecuenciaEspecificaPorHoraEntreFechas(FrecuenciasConFechasHoraDTO datos)
        {
            DateTime inicio = DateTime.ParseExact(datos.inicio, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime fin = DateTime.ParseExact(datos.fin, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            var NotasSinFiltar = (from Nota in db.Nota
                        join Usuario in db.Usuarios
                        on Nota.id_usuario equals Usuario.id_usuario
                        join Emocion in db.Emociones
                        on Nota.id_emocion equals Emocion.id_emocion
                        where Usuario.id_usuario == datos.idUsuario &&
                        DbFunctions.TruncateTime(Nota.fecha) >= inicio &&
                        DbFunctions.TruncateTime(Nota.fecha) <= fin
                        select Nota).ToList();

            var Notas = NotasSinFiltar.Where(x => x.fecha.Hour == datos.horaSeleccionada).ToList();

            var Emociones = Notas
                .Where(n => n.id_usuario == datos.idUsuario)
                .GroupBy(n => new { n.Emociones.id_emocion, n.Emociones.emocionNombre })
                .Select(g => new
                {
                    g.Key.id_emocion,
                    g.Key.emocionNombre,
                    frecuencia = g.Count()
                })
                .OrderBy(x => x.frecuencia)
                .ToList();

            var EmocionMasFrecuente = Emociones.FirstOrDefault();
            
            return new
            {
                Total = Notas.Count(),
                EmocionMasFrecuente,
                Emociones
            };
        }

        public byte[] generarReporteEmociones(FrecuenciasConFechasDTO datos)
        {
            DateTime inicio = DateTime.ParseExact(datos.inicio, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime fin = DateTime.ParseExact(datos.fin, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            var Notas = (from Nota in db.Nota
                         join Usuario in db.Usuarios on Nota.id_usuario equals Usuario.id_usuario
                         join Emocion in db.Emociones on Nota.id_emocion equals Emocion.id_emocion
                         where Usuario.id_usuario == datos.idUsuario &&
                               DbFunctions.TruncateTime(Nota.fecha) >= inicio &&
                               DbFunctions.TruncateTime(Nota.fecha) <= fin
                         select new
                         {
                             Nota.fecha,
                             Nota.texto,
                             Emocion = Emocion.emocionNombre
                         }).ToList();

            var Emociones = db.Nota
                .Where(n => n.id_usuario == datos.idUsuario &&
                            DbFunctions.TruncateTime(n.fecha) >= inicio &&
                            DbFunctions.TruncateTime(n.fecha) <= fin)
                .GroupBy(n => new { n.Emociones.id_emocion, n.Emociones.emocionNombre })
                .Select(g => new
                {
                    g.Key.emocionNombre,
                    frecuencia = g.Count()
                })
                .OrderByDescending(x => x.frecuencia)
                .ToList();

            var EmocionMasFrecuente = Emociones.FirstOrDefault();

            // --- PDF CREATION ---
            using (var ms = new MemoryStream())
            {
                var doc = new Document(PageSize.A4);
                PdfWriter.GetInstance(doc, ms);
                doc.Open();

                var titleFont = FontFactory.GetFont("Arial", 18, Font.BOLD);
                var subTitleFont = FontFactory.GetFont("Arial", 14, Font.BOLD);
                var textFont = FontFactory.GetFont("Arial", 12);

                doc.Add(new Paragraph("Reporte de emociones", titleFont));
                doc.Add(new Paragraph($"Desde: {inicio:yyyy-MM-dd} Hasta: {fin:yyyy-MM-dd}\n\n", textFont));

                // Estadísticas
                doc.Add(new Paragraph("Estadísticas generales", subTitleFont));
                doc.Add(new Paragraph($"Total de notas: {Notas.Count}", textFont));
                doc.Add(new Paragraph($"Emoción más frecuente: {EmocionMasFrecuente?.emocionNombre ?? "Ninguna"} ({EmocionMasFrecuente?.frecuencia ?? 0})\n\n", textFont));

                // Tabla de emociones
                PdfPTable table = new PdfPTable(2);
                table.WidthPercentage = 100;
                table.AddCell("Emoción");
                table.AddCell("Frecuencia");
                foreach (var e in Emociones)
                {
                    table.AddCell(e.emocionNombre);
                    table.AddCell(e.frecuencia.ToString());
                }
                doc.Add(table);
                doc.Add(new Paragraph("\n"));

                // Notas registradas
                doc.Add(new Paragraph("Notas registradas", subTitleFont));
                foreach (var nota in Notas)
                {
                    doc.Add(new Paragraph($"{nota.fecha:yyyy-MM-dd HH:mm} - [{nota.Emocion}] {nota.texto}", textFont));
                }

                doc.Add(new Paragraph("\nGráfico de emociones", subTitleFont));

                // --- Generar gráfica y agregarla ---
                /*var chartImage = GenerarGraficaEmociones(Emociones);
                iTextSharp.text.Image chartImg = iTextSharp.text.Image.GetInstance(chartImage, ImageFormat.Png);
                chartImg.ScaleToFit(400f, 300f);
                chartImg.Alignment = Element.ALIGN_CENTER;
                doc.Add(chartImg);*/

                doc.Close();
                return ms.ToArray();
            }
        }
    }
}