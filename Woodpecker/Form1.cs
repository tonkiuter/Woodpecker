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
            conexion.ConnectionString = "server=localhost; user id=daherzl; database=woodpecker";
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

        private void agregarPedido_btn_Click_1(object sender, EventArgs e)
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

        void cargar_pedido()
        {
            /*comboBox_misCultivos.Items.Clear();
            MySqlConnection conexion = Conectar();
        String sentencia = "SELECT nombre FROM mis_cultivos";
        MySqlCommand comando = new MySqlCommand(sentencia, conexion);
        conexion.Open();
            comando.ExecuteNonQuery();
            DataTable dt = new DataTable();
        MySqlDataAdapter da = new MySqlDataAdapter(comando);
        da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
            comboBox_misCultivos.Items.Add(dr["nombre"].ToString());
            }
            conexion.Close();*/
        }
        private void agregarPedido_Click(object sender, EventArgs e)
        {
            cargar_pedido();
            MySqlConnection conexion = Conectar();
            String sentencia = "INSERT INTO pedido (id, cliente, producto, cantidad, total, fecha_pedido, fecha_entrega )VALUES " + "('" + textBox_idPedido.Text + "'," +"'"+textBox_cliente.Text+"',"+ "'" + textBox_producto.Text + "'," + textBox_cantidad.Text + "," + textBox_total.Text+ ","+  "'" + fechaPedido.Value.ToString("yyyy-MM-dd") + "',"+ "'" + fechaEntrega.Value.ToString("yyyy-MM-dd") + "')";
            MySqlCommand comando = new MySqlCommand(sentencia, conexion);
            MessageBox.Show(sentencia);
            conexion.Open();
            comando.ExecuteNonQuery();
            conexion.Close();
            MessageBox.Show("Se ha agregado un pedido");

        }

        private void button3_Click(object sender, EventArgs e)
        {
            tabControl_inventario.SelectTab(0);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            tabControl_inventario.SelectTab(1);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            tabControl_inventario.SelectTab(2);
        }
    }
}
