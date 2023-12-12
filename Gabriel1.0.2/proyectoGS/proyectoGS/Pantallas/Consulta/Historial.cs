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
    public partial class Historial : Form
    {
        NpgsqlConnection conexion = new NpgsqlConnection("Host=localhost;Port=5432;Username=postgres;Password=1234;Database=medicinaChina");
        NpgsqlCommand command = new NpgsqlCommand();
        private int numero;
        public Historial(int numero)
        {
            InitializeComponent();
            this.numero = numero;
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
                using (NpgsqlConnection conexion = new NpgsqlConnection("Host=localhost;Port=5432;Username=postgres;Password=1234;Database=medicinaChina"))
                {
                    conexion.Open();

                  

                        using (NpgsqlCommand comandoConsulta = new NpgsqlCommand("SELECT fecha, motivo, observaciones FROM consultas WHERE idPaciente = " + numero, conexion))
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
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}");
            }
        }
    }
}
