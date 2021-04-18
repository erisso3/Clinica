using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinica.Objects
{
    public class Receta
    {
        public int id_receta { set; get; }
        public int id_cita  { set; get; }
        public DateTime fecha  { set; get; }
        public string ids_medicamentos  { set; get; }
        public string observacion  { set; get; }
        public string nombre  { set; get; }

        public Receta()
        {

        }

        public Receta(string observacion, string nombre, DateTime fecha)
        {
            this.observacion = observacion;
            this.nombre = nombre;
            this.fecha = fecha;
        }
    }
}