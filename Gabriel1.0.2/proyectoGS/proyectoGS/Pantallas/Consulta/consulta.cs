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
            label3.Text = DateTime.Now.ToShortDateString();
            
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
                adaptador = new NpgsqlDataAdapter("SELECT idpaciente, nombre, apellido FROM pacientes", conexion);
                datos = new DataSet();

                // Llena el DataSet con los datos de la base de datos
                adaptador.Fill(datos, "pacientes");

                // Agrega una fila al inicio con un valor predeterminado
                DataRow row = datos.Tables["pacientes"].NewRow();
                row["idpaciente"] = DBNull.Value; // Puedes usar DBNull.Value o algún valor específico según tu lógica
                row["nombre"] = DBNull.Value;
                row["apellido"] = DBNull.Value;
                datos.Tables["pacientes"].Rows.InsertAt(row, 0);

                // Concatena nombre y apellido antes de asignar a DisplayMember
                datos.Tables["pacientes"].Columns.Add("nombre_completo", typeof(string), "nombre + ' ' + apellido");

                // Asigna el DataSet como fuente de datos para el ComboBox
                comboBox1.DataSource = datos.Tables["pacientes"];
                comboBox1.DisplayMember = "nombre_completo"; // Columna que se mostrará en el ComboBox
                comboBox1.ValueMember = "idpaciente";       // Valor asociado a la selección

                // Configura el modo de autocompletar y la fuente de autocompletar del ComboBox
                comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos: " + ex.Message);
            }
            finally
            {
                // Cierra la conexión a la base de datos
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string l = comboBox1.Text;
            try
            {
                conexion.Open();
                command.Connection = conexion;

                command.CommandText = "SELECT idPaciente FROM pacientes WHERE nombreapellido = '" + comboBox1.Text + "'";
                object result = command.ExecuteScalar();


                if (result != null && result != DBNull.Value)
                {

                    int idPaciente = Convert.ToInt32(result);

                   

                    // Insertar en la tabla 'consultas'
                    command.CommandText = "INSERT INTO public.consultas(idpaciente, fecha, motivo, observaciones)" +
                        "VALUES (@idPaciente, @fechaConsulta, @motivo, @observaciones)";

                    command.Parameters.AddWithValue("@idPaciente", idPaciente);
                    command.Parameters.AddWithValue("@fechaConsulta", DateTime.Parse( label3.Text));
                    command.Parameters.AddWithValue("@motivo", txtDiag.Text);
                    command.Parameters.AddWithValue("@observaciones", txtObser.Text);

                    valid();


                }
                else
                {
                    MessageBox.Show("ingrese un pasiente o el paciente no existe ");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                command.Parameters.Clear();
            }
            finally
            {
                conexion.Close();
            }
        }
        private void LimpiarCampos()
        {
           
            txtObser.Clear();
            txtDiag.Clear();
        }
        private void valid()
        {
            int n = 0;
            if (txtDiag.Text == "")
            {
                MessageBox.Show("Complete la diagnostico");

            }
            else n++;
            if (txtObser.Text == "")
            {
                MessageBox.Show("Complete la observacion");
            }
            else n++;

            if (n == 2)
            {
                command.ExecuteNonQuery();

                MessageBox.Show("Los datos se guardaron correctamente");
                LimpiarCampos();
                command.Parameters.Clear();
                comboBox1.Enabled = true;
            }
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

        private void btnCancelar_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("Desea cancelar la operacion?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) LimpiarCampos();

        }

        private void btnHistorial_Click(object sender, EventArgs e)
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

                        Historial formulario2 = new Historial(idPaciente);
                        formulario2.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void bntNuevo_Click(object sender, EventArgs e)
        {
            try {

                PacienteNuevo paciente = new PacienteNuevo();
                paciente.Show();
                
            }
            catch { }

        }

       private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            { comboBox1.Enabled = true; }
            else { comboBox1.Enabled = false; }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            LlenarComboBox();
        }
    }
   
}
