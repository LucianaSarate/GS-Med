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

namespace proyectoGS.Pantallas
{
    public partial class HistoraialConsultas : Form
    {
        NpgsqlConnection conexion = new NpgsqlConnection("Host=localhost;Port=5432;Username=postgres;Password=1234;Database=medicinaChina");
        NpgsqlCommand command = new NpgsqlCommand();
        private NpgsqlDataAdapter adaptador;
        private DataSet datos;
        public HistoraialConsultas()
        {
            InitializeComponent();
            this.Load += new EventHandler(Consulta_Load);
            this.Load += TuFormulario_Load;
        }
        private void Consulta_Load(object sender, EventArgs e)
        {
            LlenarComboBox();
        }
        private void TuFormulario_Load(object sender, EventArgs e)
        {
            
            dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            dataGridView1.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);

            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void LlenarComboBox()
        {
            try
            {
                // Abre la conexión a la base de datos
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
                conexion.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (NpgsqlConnection conexion = new NpgsqlConnection("Host=localhost;Port=5432;Username=postgres;Password=1234;Database=medicinaChina"))
                {
                    conexion.Open();


                    using (NpgsqlCommand comandoIdPaciente = new NpgsqlCommand("SELECT idPaciente FROM pacientes WHERE nombre = @nombre AND apellido = @apellido", conexion))
                    {
                        int idPaciente = comboBox1.SelectedIndex;

                        using (NpgsqlCommand comandoConsulta = new NpgsqlCommand("SELECT fecha, motivo, observaciones FROM consultas WHERE idPaciente = " + idPaciente, conexion))
                        {
                            using (NpgsqlDataReader reader = comandoConsulta.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    DataGridViewRow row = new DataGridViewRow();

                                    DataGridViewTextBoxCell cell1 = new DataGridViewTextBoxCell();
                                    cell1.Value = ((DateTime)reader["fecha"]).ToShortDateString(); 
                                    row.Cells.Add(cell1);

                                    DataGridViewTextBoxCell cell2 = new DataGridViewTextBoxCell();
                                    cell2.Value = reader["motivo"].ToString();
                                    row.Cells.Add(cell2);

                                    DataGridViewTextBoxCell cell3 = new DataGridViewTextBoxCell();
                                    cell3.Value = reader["observaciones"].ToString();
                                    row.Cells.Add(cell3);

                                    dataGridView1.Rows.Add(row);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}");
            }
        }
    }
}
