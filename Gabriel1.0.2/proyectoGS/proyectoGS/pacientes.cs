using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace proyectoGS
{
    public partial class pacientes : Form
    {
        OleDbConnection conexion = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=pacientes.accdb");
        OleDbCommand command = new OleDbCommand();
        public pacientes()
        {
            InitializeComponent();
        }
        private void pacientes_Load(object sender, EventArgs e)
        {
         //   conexion.Open();
        }
        private int CalcularEdad(DateTime fechaNacimiento, DateTime fechaActual)
        {
            int edad = fechaActual.Year - fechaNacimiento.Year;
            if (fechaActual.Month < fechaNacimiento.Month || (fechaActual.Month == fechaNacimiento.Month && fechaActual.Day < fechaNacimiento.Day))
            {
                edad--;
            }
            return edad;
        }
        public string tomMedic()
        {
            string solucion = "";

            if (radioButton1.Checked == true)
            {
                solucion = txtMedicamentos.Text;
            }
            else if (radioButton2.Checked == true)
            {
                solucion = "-";
            }
            return solucion;
        }
        public string operaciones()
        {
            string solucion = "";

            if (radioButton1.Checked == true)
            {
                solucion = txtOperaciones.Text;
            }
            else if (radioButton2.Checked == true)
            {
                solucion = "-";
            }
            return solucion;
        }
        public string metales()
        {
            string solucion = "";

            if (radioButton1.Checked == true)
            {
                solucion = txtMetales.Text;
            }
            else if (radioButton2.Checked == true)
            {
                solucion = "-";
            }
            return solucion;
        }

        private void dtpFecha_ValueChanged(object sender, EventArgs e)
        {
            DateTime fechaNacimiento = dtpFecha.Value;

            // Obtener la fecha actual
            DateTime fechaActual = DateTime.Today;

            // Calcular la edad
            int edad = CalcularEdad(fechaNacimiento, fechaActual);

            // Mostrar la edad en el Label
            lblEdad.Text = edad.ToString();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            command.Connection = conexion;

            command.CommandText = "INSERT INTO `pacientes` (NombreyApellido, FechasNaciemiento, Edad, Telefono, Direccion, Ocupacion, Peso) VALUES ('" + txtNombre.Text + "', '" + dtpFecha.Value + "', '" + lblEdad.Text + "', '" + txtTelefono.Text + "', '" + txtDireccion.Text + "', '" + (txtOcupacion1.Text + " " + txtOcupacion2.Text) + "', '" + txtPeso.Text + "')";
            command.ExecuteNonQuery();
            command.CommandText = "INSERT INTO `otros` (MedocamentosToma, operaciones, Otrasterapias, metalesCuerpo, otrosproblemas) VALUES ('" + tomMedic() + "', '" + operaciones() + "', '" + txtOtrasOperaciones.Text + "','" + metales() + "', '" + txtTipoProblemas.Text + "')";
            command.ExecuteNonQuery();
            string idPaciente = "SELECT idPaciente FROM pacientes WHERE NombreyApellido = '" + txtNombre.Text + "'";
            command.ExecuteNonQuery();
            if(txtNombre.Text != "")
            {
                command.Connection = conexion;
                command.CommandText = "INSERT INTO `Consultas` (IdPaciente, Fecha, Motivo, Observaciones) VALUES ('"+idPaciente+"', '"+dtpConsulta.Value+"', '"+txtMotivo.Text+"', '"+txtRegistro.Text+"')";

                command.ExecuteNonQuery();//para guardar lo que est aingresando 
                conexion.Close();
                MessageBox.Show("Los datos se guardaron correctamente");
                btnAgregar.Enabled = true;
            }
        }
        /*INSERT INTO `Consultas` (`IdPaciente`, `Fecha`, `Motivo`, `Observaciones`) VALUES (?, ?, ?, ?)*/
    }


    
}
