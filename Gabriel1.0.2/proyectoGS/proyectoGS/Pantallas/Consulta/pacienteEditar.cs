using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace proyectoGS.Pantallas.Consulta
{
    public partial class pacienteEditar : Form
    {
        NpgsqlConnection conexion = new NpgsqlConnection("Host=localhost;Port=5432;Username=postgres;Password=1234;Database=medicinaChina");
        NpgsqlCommand command = new NpgsqlCommand();
        private int numero;

        public pacienteEditar(int numero)
        {
            InitializeComponent();
            this.numero = numero;
            txtNombre.Enabled = false;
            textApellido.Enabled = false;   
            dtpFecha.Enabled = false;
            txtTelefono.Enabled = false;
            txtDireccion.Enabled = false;
            txtOcupacion1.Enabled = false;
            txtOcupacion2.Enabled = false;
            txtPeso.Enabled = false;
            txtMedicamentos.Enabled = false;
            txtOtrasOperaciones.Enabled = false;
            txtOperaciones.Enabled = false;
            txtMetales.Enabled = false;
            txtTipoProblemas.Enabled = false;
            btnAgregar.Enabled = false;

        }

        private void pacienteEditar_Load(object sender, EventArgs e)
        {
            try
            {
                conexion.Open();

                command.Connection = conexion;
                command.CommandText = "SELECT nombre ,apellido, fecha_nacimiento, telefono, direccion, ocupacion, descripcionOcupacion , peso, medicamentos_que_toma, otras_terapias, operaciones, metales_en_el_cuerpo, otros_problemas FROM pacientes WHERE idPaciente = '" + numero + "'";
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Asignar los valores a los controles
                        txtNombre.Text = reader["nombre"].ToString();
                        textApellido.Text = reader["apellido"].ToString();
                        dtpFecha.Value = Convert.ToDateTime(reader["fecha_nacimiento"]);
                        txtTelefono.Text = reader["telefono"].ToString();
                        txtDireccion.Text = reader["direccion"].ToString();
                        txtOcupacion1.Text = reader["ocupacion"].ToString(); 
                        txtOcupacion2.Text = reader["descripcionOcupacion"].ToString();
                        txtPeso.Text = reader["peso"].ToString();
                        txtMedicamentos.Text = reader["medicamentos_que_toma"].ToString();
                        txtOtrasOperaciones.Text = reader["otras_terapias"].ToString();
                        txtOperaciones.Text = reader["operaciones"].ToString();
                        txtMetales.Text = reader["metales_en_el_cuerpo"].ToString();
                        txtTipoProblemas.Text = reader["otros_problemas"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos: " + ex.Message);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                    conexion.Close();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            txtNombre.Enabled = true;
            textApellido.Enabled = true;
            dtpFecha.Enabled = true;
            txtTelefono.Enabled = true;
            txtDireccion.Enabled = true;
            txtOcupacion1.Enabled = true;
            txtOcupacion2.Enabled = true;
            txtPeso.Enabled = true;
            txtMedicamentos.Enabled = true;
            txtOtrasOperaciones.Enabled = true;
            txtOperaciones.Enabled = true;
            txtMetales.Enabled = true;
            txtTipoProblemas.Enabled = true;
            btnAgregar.Enabled = true;
            btnCancelar.Enabled = false;

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                conexion.Open();
                command.Connection = conexion;

                // Actualizar en la tabla 'pacientes'
                command.CommandText = "UPDATE public.pacientes" +
                    " SET nombre = @nombre,  apellido = @apellido,fecha_nacimiento = @fechaNacimiento, " +
                    "telefono = @telefono, direccion = @direccion, ocupacion = @ocupacion, descripcionocupacion = @descripcionOcupacion, " +
                    "peso = @peso, medicamentos_que_toma = @medicamentos, otras_terapias = @otrasTerapias, operaciones = @operaciones, " +
                    "metales_en_el_cuerpo = @metales, otros_problemas = @otrosProblemas" +
                    " WHERE idpaciente = @numero";

                command.Parameters.AddWithValue("@nombre", txtNombre.Text);
                command.Parameters.AddWithValue("@apellido", textApellido.Text);
                command.Parameters.AddWithValue("@fechaNacimiento", dtpFecha.Value);
                command.Parameters.AddWithValue("@telefono", txtTelefono.Text);
                command.Parameters.AddWithValue("@direccion", txtDireccion.Text);
                command.Parameters.AddWithValue("@ocupacion", txtOcupacion1.Text);
                command.Parameters.AddWithValue("@descripcionOcupacion", txtOcupacion2.Text);
                command.Parameters.AddWithValue("@peso", txtPeso.Text);
                command.Parameters.AddWithValue("@medicamentos", txtMedicamentos.Text);
                command.Parameters.AddWithValue("@otrasTerapias", txtOtrasOperaciones.Text);
                command.Parameters.AddWithValue("@operaciones", txtOperaciones.Text);
                command.Parameters.AddWithValue("@metales", txtMetales.Text);
                command.Parameters.AddWithValue("@otrosProblemas", txtTipoProblemas.Text);
                command.Parameters.AddWithValue("@numero", numero);

                command.ExecuteNonQuery();

                if (MessageBox.Show("Los datos se guardaron correctamente\n" +
                    "Desea terminar la operacion?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) Close();

                command.Parameters.Clear();

                conexion.Close();


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
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
        private int CalcularEdad(DateTime fechaNacimiento, DateTime fechaActual)
        {
            int edad = fechaActual.Year - fechaNacimiento.Year;
            if (fechaActual.Month < fechaNacimiento.Month || (fechaActual.Month == fechaNacimiento.Month && fechaActual.Day < fechaNacimiento.Day))
            {
                edad--;
            }
            return edad;
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
    }
}
