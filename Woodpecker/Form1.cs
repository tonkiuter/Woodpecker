using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Drawing.Drawing2D;
using System.IO;


namespace Woodpecker
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }
        MySqlConnection Conectar()
        {
            MySqlConnection conexion = new MySqlConnection();
            conexion.ConnectionString = "server=localhost; user id=daherzl; database=woodpecker";
            return conexion;
        }

        private void Form1_Load(object sender, EventArgs e) // Centrar la información de la carpintería al inicio
        {
            richTextBox1.SelectAll();
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
            this.reportViewer1.RefreshReport();
        }


    }
}
