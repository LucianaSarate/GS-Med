using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyectoGS.Entidades
{
    internal class Consulta
    {
        public int idConsulta { get; set; } // Identificador único de la consulta
        public DateTime fecha { get; set; }
        public string motivo { get; set; }
        public string observaciones { get; set; }
    }
}
