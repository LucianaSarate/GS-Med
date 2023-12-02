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
using Npgsql;
using proyectoGS.Entidades;

namespace proyectoGS
{
    public partial class pacientes : Form
    {
        NpgsqlConnection conexion = new NpgsqlConnection("Host=localhost;Port=5432;Username=postgres;Password=1234;Database=medicinaChina");
        NpgsqlCommand command = new NpgsqlCommand();
        public pacientes()
        {
            InitializeComponent();           
        }
        private void pacientes_Load(object sender, EventArgs e)
        {
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
        private void txtTelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Ignorar la tecla presionada
            }
        }

        private void txtPeso_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true; // Ignorar la tecla presionada
            }
        }
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                conexion.Open();
                command.Connection = conexion;

                // Insertar en la tabla 'pacientes'
                command.CommandText = "INSERT INTO public.pacientes(nombreapellido, fecha_nacimiento, telefono, direccion, ocupacion, descripcionOcupacion , peso, medicamentos_que_toma, otras_terapias, operaciones, metales_en_el_cuerpo, otros_problemas)" +
                    "VALUES(@nombre, @fechaNacimiento, @telefono, @direccion, @ocupacion, @descripcionOcupacion ,@peso, @medicamentos, @otrasTerapias, @operaciones, @metales, @otrosProblemas)";

                command.Parameters.AddWithValue("@nombre", txtNombre.Text);
                command.Parameters.AddWithValue("@fechaNacimiento", dtpFecha.Value);
                command.Parameters.AddWithValue("@telefono", txtTelefono.Text);
                command.Parameters.AddWithValue("@direccion", txtDireccion.Text);
                command.Parameters.AddWithValue("@ocupacion", txtOcupacion1.Text);
                command.Parameters.AddWithValue("@descripcionOcupacion", txtOcupacion2.Text);
                command.Parameters.AddWithValue("@peso", decimal.Parse(txtPeso.Text));
                command.Parameters.AddWithValue("@medicamentos", tomMedic());
                command.Parameters.AddWithValue("@otrasTerapias", txtOtrasOperaciones.Text);
                command.Parameters.AddWithValue("@operaciones", operaciones());
                command.Parameters.AddWithValue("@metales", metales());
                command.Parameters.AddWithValue("@otrosProblemas", txtTipoProblemas.Text);

                command.ExecuteNonQuery();

                // Obtener el ID del paciente recién insertado
                command.CommandText = "SELECT lastval()";
                int idPaciente = Convert.ToInt32(command.ExecuteScalar());

                // Insertar en la tabla 'consultas'
                command.CommandText = "INSERT INTO public.consultas(idpaciente, fecha, motivo, observaciones)" +
                    "VALUES (@idPaciente, @fechaConsulta, @motivo, @observaciones)";

                command.Parameters.AddWithValue("@idPaciente", idPaciente);
                command.Parameters.AddWithValue("@fechaConsulta", dtpConsulta.Value);
                command.Parameters.AddWithValue("@motivo", txtMotivo.Text);
                command.Parameters.AddWithValue("@observaciones", txtRegistro.Text);

                command.ExecuteNonQuery();

                MessageBox.Show("Los datos se guardaron correctamente");
                LimpiarCampos();
                command.Parameters.Clear();


                conexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                command.Parameters.Clear();
            }

        }

        private void LimpiarCampos()
        {
            // Limpiar o restablecer los valores de los campos del formulario
            txtNombre.Clear();
            dtpFecha.Value = DateTime.Today;
            txtTelefono.Clear();
            txtDireccion.Clear();
            txtOcupacion1.Clear();
            txtOcupacion2.Clear();
            txtPeso.Clear();
            txtMedicamentos.Clear();
            txtMotivo.Clear();
            txtRegistro.Clear();
            txtMetales.Clear();
            txtOtrasOperaciones.Clear();
            txtRegistro.Clear();
            txtTipoProblemas.Clear();

            // Limpiar otros campos según sea necesario

            // También puedes desmarcar los radio buttons si es necesario
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
            radioButton5.Checked = false;
            radioButton6.Checked = false;
            // También puedes reiniciar otros controles según sea necesario
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Desea cancelar la operacion?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) LimpiarCampos();
        }


    }


    
}
