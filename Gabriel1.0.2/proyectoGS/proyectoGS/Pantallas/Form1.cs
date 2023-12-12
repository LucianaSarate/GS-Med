using proyectoGS.Pantallas;
using proyectoGS.Pantallas.Consulta;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace proyectoGS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            pMenu();
        }
        private void pMenu()
        {

        }
        private void hidMenu()
        {

        }

        private void showMenu(Panel subMenu)
        {
            if (subMenu.Visible == false)
            {
                pMenu();
                subMenu.Visible = true;
            }
            else
            {
                subMenu.Visible = false;
            }
        }

        private Form activeForm = null;
        private void openForn(Form chilldForm)
        {
            if (activeForm != null) activeForm.Close();
            activeForm = chilldForm;
            chilldForm.TopLevel = false;
            chilldForm.FormBorderStyle = FormBorderStyle.None;
            chilldForm.Dock = DockStyle.Fill;
            panelMostrar.Controls.Add(chilldForm);
            panelMostrar.Tag = chilldForm;
            chilldForm.BringToFront();
            chilldForm.Show();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
          
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            
            if (MessageBox.Show("Desea salir?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) Close();

        }

       
        private void btnConsulta_Click(object sender, EventArgs e)
        {
            openForn(new consulta());

        }

        private void btnHistorial_Click(object sender, EventArgs e)
        {
            openForn(new HistoraialConsultas());
        }

    }
}
