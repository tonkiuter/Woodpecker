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
            conexion.ConnectionString = "server=localhost; user id=daherzl; database=sisag";
            return conexion;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab(0);        
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab(0);
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab(0);
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab(0);
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab(0);
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab(0);
        }

        private void agregarPedido_btn_Click(object sender, EventArgs e)
        {
            tab_pedidos.SelectTab(0);
        }

        private void modificarPedido_Btn_Click(object sender, EventArgs e)
        {
            tab_pedidos.SelectTab(1);
        }

        private void eliminarPedido_Btn_Click(object sender, EventArgs e)
        {
            tab_pedidos.SelectTab(3);
        }

        private void mostrarPedido_Btn_Click(object sender, EventArgs e)
        {
            tab_pedidos.SelectTab(2);
        }

    }
}
