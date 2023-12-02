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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace proyectoGS.Pantallas.Consulta
{
    public partial class consulta : Form
    {
        NpgsqlConnection conexion = new NpgsqlConnection("Host=localhost;Port=5432;Username=postgres;Password=1234;Database=medicinaChina");
        NpgsqlCommand command = new NpgsqlCommand();
        private NpgsqlDataAdapter adaptador;
        private DataSet datos;
        public consulta()
        {
            InitializeComponent();
            // Llena el ComboBox con datos de la base de datos
            this.Load += new EventHandler(Consulta_Load);
        }
        private void Consulta_Load(object sender, EventArgs e)
        {
            // Llena el ComboBox con datos de la base de datos
            LlenarComboBox();
        }
        private void LlenarComboBox()
        {
            try
            {
                // Abre la conexión a la base de datos
                conexion.Open();

                // Crea un adaptador y un DataSet para almacenar los datos
                adaptador = new NpgsqlDataAdapter("SELECT idpaciente, nombreapellido FROM pacientes", conexion);
                datos = new DataSet();

                // Llena el DataSet con los datos de la base de datos
                adaptador.Fill(datos, "pacientes");

                // Asigna el DataSet como fuente de datos para el ComboBox
                comboBox1.DataSource = datos.Tables["pacientes"];
                comboBox1.DisplayMember = "nombreapellido"; // Columna que se mostrará en el ComboBox
                comboBox1.ValueMember = "idpaciente";       // Valor asociado a la selección
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos: " + ex.Message);
            }
            finally
            {
                // Cierra la conexión a la base de datos
                conexion.Close();
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                conexion.Open();
                command.Connection = conexion;

                command.CommandText = "SELECT idPaciente FROM pacientes WHERE nombreapellido = '" + comboBox1.Text + "'";
                int idPaciente = Convert.ToInt32(command.ExecuteScalar());

                // Insertar en la tabla 'consultas'
                command.CommandText = "INSERT INTO public.consultas(idpaciente, fecha, motivo, observaciones)" +
                    "VALUES (@idPaciente, @fechaConsulta, @motivo, @observaciones)";

                command.Parameters.AddWithValue("@idPaciente", idPaciente);
                command.Parameters.AddWithValue("@fechaConsulta", dtpFecha.Value);
                command.Parameters.AddWithValue("@motivo", txtDiag.Text);
                command.Parameters.AddWithValue("@observaciones", txtObser.Text);

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
            txtObser.Clear();
            txtDiag.Clear();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                using (NpgsqlConnection conexion = new NpgsqlConnection("Host=localhost;Port=5432;Username=postgres;Password=1234;Database=medicinaChina"))
                {
                    conexion.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand())
                    {
                        command.Connection = conexion;
                        command.CommandText = "SELECT idPaciente FROM pacientes WHERE nombreapellido = '" + comboBox1.Text + "'";
                        int idPaciente = Convert.ToInt32(command.ExecuteScalar());

                        pacienteEditar formulario2 = new pacienteEditar(idPaciente);
                        formulario2.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

        }

        private void btnHistorial_Click(object sender, EventArgs e)
        {
            //pacienteEditar paciente = new pacienteEditar();
            //paciente.Show();
        }

        private void btnCancelar_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("Desea cancelar la operacion?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) LimpiarCampos();

        }
        // También puedes reiniciar otros controles según sea necesario
    }

   
}
