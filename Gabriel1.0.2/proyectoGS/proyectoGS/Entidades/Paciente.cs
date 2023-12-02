using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyectoGS.Entidades
{
    internal class Paciente
    {
        public int idPaciente { get; set; } // Identificador único del paciente
        public string nombre { get; set; }
        public string apellido { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public int telefono { get; set; }
        public string direccion { get; set; }

        // Datos médicos
        public string medicamentos_que_toma { get; set; }
        public string otras_terapias { get; set; }
        public string operaciones { get; set; }
        public string metales_en_el_cuerpo { get; set; }
        public string otros_problemas { get; set; }

        // Consultas
        public List<Consulta> Consultas { get; set; } = new List<Consulta>();

    }
}
