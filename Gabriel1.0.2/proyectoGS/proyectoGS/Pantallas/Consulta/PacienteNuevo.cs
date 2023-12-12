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

namespace proyectoGS.Pantallas.Consulta
{
    public partial class PacienteNuevo : Form
    {
        NpgsqlConnection conexion = new NpgsqlConnection("Host=localhost;Port=5432;Username=postgres;Password=1234;Database=medicinaChina");
        NpgsqlCommand command = new NpgsqlCommand();
        public PacienteNuevo()
        {
            InitializeComponent();
            txtMedicamentos.Enabled = false;
            txtMetales.Enabled = false;   
            txtOperaciones.Enabled = false;
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
        public string TomMedic()
        {

            if (radioButton1.Checked == true)
            {
                return txtMedicamentos.Text;
                
            }
            else if (radioButton2.Checked == true)
            {
                return "-";
            }
            else
            {
                return string.Empty;
            }

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

   
        

        private void PacienteNuevo_Load(object sender, EventArgs e)
        {
            dtpFecha.Value = DateTime.Now;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                conexion.Open();
                command.Connection = conexion;
                
                    // Insertar en la tabla 'pacientes'
                    command.CommandText = "INSERT INTO public.pacientes(nombre,apellido, fecha_nacimiento, telefono, direccion, ocupacion, descripcionOcupacion , peso, medicamentos_que_toma, otras_terapias, operaciones, metales_en_el_cuerpo, otros_problemas)" +
                        "VALUES(@nombre,@apellido, @fechaNacimiento, @telefono, @direccion, @ocupacion, @descripcionOcupacion ,@peso, @medicamentos, @otrasTerapias, @operaciones, @metales, @otrosProblemas)";

                    command.Parameters.AddWithValue("@nombre", txtNombre.Text);
                    command.Parameters.AddWithValue("@apellido", textApellido.Text);
                    command.Parameters.AddWithValue("@fechaNacimiento", dtpFecha.Value);
                    command.Parameters.AddWithValue("@telefono", txtTelefono.Text);
                    command.Parameters.AddWithValue("@direccion", txtDireccion.Text);
                    command.Parameters.AddWithValue("@ocupacion", txtOcupacion1.Text);
                    command.Parameters.AddWithValue("@descripcionOcupacion", txtOcupacion2.Text);
                    command.Parameters.AddWithValue("@peso", txtPeso.Text);
                    command.Parameters.AddWithValue("@medicamentos", TomMedic());
                    command.Parameters.AddWithValue("@otrasTerapias", txtOtrasOperaciones.Text);
                    command.Parameters.AddWithValue("@operaciones", operaciones());
                    command.Parameters.AddWithValue("@metales", metales());
                    command.Parameters.AddWithValue("@otrosProblemas", txtTipoProblemas.Text);

                    validaciones();


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

        private void validaciones ()
        {
            int a = 0;
            if (txtNombre.Text== "")
            {
                MessageBox.Show("Complete el nombre y apellido correctamente correctamente");

            }
            else
            {
                a = a + 1;
            }
            if ( dtpFecha.Value >= DateTime.Today)
            {
               
              MessageBox.Show("Complete la fecha de nacimiento correctamente correctamente");

            }
            else
            {
               a = a + 1; 
            }

            if (!radioButton1.Checked  && !radioButton2.Checked )
            {
                MessageBox.Show("Seleccione una opccion si toma algun medicamento");
            }
            else
            {
                a = a + 1;
                if (radioButton1.Checked && txtMedicamentos.Text == "")
                {
                    MessageBox.Show("Completar el campo si toma algun medicamento");
                }
            }
            if (!radioButton3.Checked && !radioButton4.Checked )
            {
               
                MessageBox.Show("Seleccione una opccion si tiene operacopnes realizadas");
            }
            else
            {
                a = a + 1;
                if (radioButton4.Checked && txtOperaciones.Text == "")
                {
                    MessageBox.Show("Completar el campo si tiene operaciones");
                }
            }
            if (!radioButton5.Checked && !radioButton6.Checked )
            {
                
                MessageBox.Show("Seleccione una opccion si si tiene metales en el cuerpo");

            }
            else
            {
                a = a + 1;
                if (radioButton6.Checked && txtMetales.Text== "")
                {
                    MessageBox.Show("Completar el campo si tiene metales en el cuerpo");
                }
            }

            if (txtPeso.Text == "0" || txtPeso.Text == "")
            {
                MessageBox.Show("Complete la peso correctamente correctamente");
            }
            else
            {
                a = a + 1;
            }
            if (txtTelefono.Text == "")
            {
                MessageBox.Show("Complete la telefono correctamente correctamente");
            }
            else
            {
                a = a + 1;
            }

            if ( txtDireccion.Text == "")
            {
                MessageBox.Show("Complete la direccion correctamente correctamente");
            }
            else
            {
                a = a + 1;
            }
            if ( txtOcupacion1.Text == "")
            {
                MessageBox.Show("Complete la ocupacion correctamente correctamente");
            }
            else
            {
                a = a + 1;
            }
            if ( txtOcupacion2.Text == "")
            {
                MessageBox.Show("Complete la tarea que desempeña correctamente correctamente");
            }
            else
            {
                a = a + 1;
            }
            if ( txtOtrasOperaciones.Text == "")
            {
                MessageBox.Show("Complete si realizo otras terapias correctamente correctamente");
            }
            else
            {
                a = a + 1;
            }
            if ( txtTipoProblemas.Text == "")
            {
                MessageBox.Show("Complete el campo tipo de problema correctamente correctamente");
            }
            else
            {
                a = a + 1;
            }

           if (a == 12)
           {


                if (MessageBox.Show("Los datos se guardaron correctamente\n" +
                     "Desea terminar la operación?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    command.ExecuteNonQuery();
                    Close();
                }

                command.Parameters.Clear();
           }


        }

        private void txtPeso_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true; // Ignorar la tecla presionada
            }
        }

        private void txtTelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Ignorar la tecla presionada
            }
        }

        private void dtpFecha_ValueChanged_1(object sender, EventArgs e)
        {
            DateTime fechaNacimiento = dtpFecha.Value;

            // Obtener la fecha actual
            DateTime fechaActual = DateTime.Today;

            // Calcular la edad
            int edad = CalcularEdad(fechaNacimiento, fechaActual);

            // Mostrar la edad en el Label
            lblEdad.Text = edad.ToString();

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
           if (radioButton1.Checked != false)
            {
                txtMedicamentos.Enabled = true;
            }
            else
            {
                txtMedicamentos.Enabled = false;
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked != false)
            {
                txtOperaciones.Enabled = true;
            }
            else
            {
                txtOperaciones.Enabled = false;
            }
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton6.Checked != false)
            {
                txtMetales.Enabled = true;
            }
            else
            {
                txtMetales.Enabled = false;
            }
        }
    }
}
