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
        AutoCompleteStringCollection autotext_clientes = new AutoCompleteStringCollection();
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

        private void Form1_Load(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
        }

        void folio_pedido()
        {
            int folio = 0;
            MySqlConnection conexion = Conectar();
            String sentencia = "SELECT id FROM pedido ORDER BY id DESC LIMIT 1";
            MySqlCommand comando = new MySqlCommand(sentencia, conexion);
            conexion.Open();
            comando.ExecuteNonQuery();
            MySqlDataReader MyReader;
            MyReader = comando.ExecuteReader();
            while (MyReader.Read())
            {
                folio = (MyReader.GetInt16("id"));
                folio = folio + 1;        
            }
            if(folio == 0)
            {
                folio = 1;
            }
            MyReader.Close();
            conexion.Close();
            var folio_padded = folio.ToString().PadLeft(4, '0');
            textBox_idPedido.Text = folio_padded;

        }
        private void agregarPedido_btn_Click_1(object sender, EventArgs e)
        {
            DateTime today = DateTime.Today;
            tab_pedidos.Visible = true;
            tab_pedidos.SelectTab(0);
            comboBox_producto.SelectedIndex = -1;
            textBox_cliente.Clear();
            textBox_cantidad.Clear();
            cargar_productos();
            folio_pedido();

            MySqlConnection conexion = Conectar();
            String sentencia = "SELECT nombre FROM clientes";
            MySqlCommand comando = new MySqlCommand(sentencia, conexion);
            conexion.Open();
            comando.ExecuteNonQuery();
            MySqlDataReader MyReader;
            MyReader = comando.ExecuteReader();
            while (MyReader.Read())
            {
                autotext_clientes.Add(MyReader.GetString(0));
            }

            textBox_cliente.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox_cliente.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBox_cliente.AutoCompleteCustomSource = autotext_clientes;

            MyReader.Close();
            conexion.Close();       
        }
        private void pestañaModificarPedido_Btn_Click(object sender, EventArgs e)
        {
            tab_pedidos.Visible = true;
            tab_pedidos.SelectTab(1);
            textBox_buscarFolio.Clear();
            cargar_productos();

            MySqlConnection conexion = Conectar();
            String sentencia = "SELECT nombre FROM clientes";
            MySqlCommand comando = new MySqlCommand(sentencia, conexion);
            conexion.Open();
            comando.ExecuteNonQuery();
            MySqlDataReader MyReader;
            MyReader = comando.ExecuteReader();
            while (MyReader.Read())
            {
                autotext_clientes.Add(MyReader.GetString(0));
            }

            textBox_clienteEncontrado.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox_clienteEncontrado.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBox_clienteEncontrado.AutoCompleteCustomSource = autotext_clientes;

            MyReader.Close();
            conexion.Close();
        }

        private void facturarPedido_Btn_Click(object sender, EventArgs e)
        {
            tab_pedidos.SelectTab(3);
        }

        private void mostrarPedido_Btn_Click(object sender, EventArgs e)
        {
            tab_pedidos.Visible = true;
            tab_pedidos.SelectTab(2);
        }

        void cargar_productos()
        {
            comboBox_producto.Items.Clear();
            comboBox_productoEncontrado.Items.Clear();
            MySqlConnection conexion = Conectar();
            String sentencia = "SELECT nombre FROM producto";
            MySqlCommand comando = new MySqlCommand(sentencia, conexion);
            conexion.Open();
            comando.ExecuteNonQuery();
            DataTable dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(comando);
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                comboBox_producto.Items.Add(dr["nombre"].ToString());
                comboBox_productoEncontrado.Items.Add(dr["nombre"].ToString());
            }
            conexion.Close();
        }
        private void agregarPedido_Click(object sender, EventArgs e)
        {
            MySqlConnection conexion = Conectar();
            String sentencia = "INSERT INTO pedido (id, cliente, producto, precio, cantidad, total, fecha_pedido, fecha_entrega )VALUES " + "('" + textBox_idPedido.Text + "'," +"'"+textBox_cliente.Text+"',"+ "'" + comboBox_producto.Text + "'," + "'" + textBox_precio.Text.Substring(1, textBox_precio.Text.Length - 1) + "'," + textBox_cantidad.Text + "," + textBox_total.Text.Substring(1, textBox_total.Text.Length - 1) + ","+  "'" + fechaPedido.Value.ToString("yyyy-MM-dd") + "',"+ "'" + fechaEntrega.Value.ToString("yyyy-MM-dd") + "')";
            MySqlCommand comando = new MySqlCommand(sentencia, conexion);
            conexion.Open();
            comando.ExecuteNonQuery();
            conexion.Close();
            MessageBox.Show("Se ha agregado un pedido");
            comboBox_producto.SelectedIndex = -1;
            textBox_cliente.Clear();
            textBox_cantidad.Clear();
            folio_pedido();
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

        private void comboBox_productos_SelectedIndexChanged(object sender, EventArgs e)
        {
            double precio = 0;
            MySqlConnection conexion = Conectar();
            String sentencia = "SELECT precio FROM producto WHERE nombre = '" + comboBox_producto.Text + "'"; 
            MySqlCommand comando = new MySqlCommand(sentencia, conexion);
            conexion.Open();
            comando.ExecuteNonQuery();
            MySqlDataReader MyReader;
            MyReader = comando.ExecuteReader();
            while (MyReader.Read())
            {
                precio = MyReader.GetDouble("precio");
            }
            MyReader.Close();
            conexion.Close();
            textBox_precio.Text = "$ " +  precio.ToString("0.00");
            calcular_total();
        }

        private void textBox_cantidad_TextChanged(object sender, EventArgs e)
        {
            calcular_total();
            if (textBox_cantidad.Text == "")
            {
                calcular_total();
            }
        }

        void calcular_total()
        {
            float precio = 0;
            float cantidad = 0;
            if (textBox_cantidad.Text == "")
            {
                precio = 0;
                cantidad = 0;
            }
            bool b1 = string.IsNullOrEmpty(textBox_cantidad.Text);
            precio = float.Parse(textBox_precio.Text.Substring(1, textBox_precio.Text.Length - 1));
            if (b1)
            {
                cantidad = 0;
            }
            else
            {
                cantidad = int.Parse(textBox_cantidad.Text);
            }

            float total = precio * cantidad;
            textBox_total.Text = "$ " + total.ToString("0.00");
        }

        private void textBox_cliente_Validating(object sender, CancelEventArgs e)
        {
            bool b1 = string.IsNullOrEmpty(textBox_cliente.Text);
            if (!b1)
            {
                if (!autotext_clientes.Contains(textBox_cliente.Text))
                {
                    DialogResult dialogResult = MessageBox.Show("Este cliente no se encuentra registrado. ¿Registrarlo ahora?", "Cliente nuevo", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        tabControl1.SelectTab(3);
                        tabControl_clientes.SelectTab(0);
                    }
                }
            }
           
        }

        private void textBox_buscarFolio_TextChanged(object sender, EventArgs e)
        {
            textBox_clienteEncontrado.Clear();
            comboBox_productoEncontrado.SelectedIndex = -1;
            textBox_precioEncontrado.Clear();
            textBox_cantidadEncontrada.Clear();
            textBox_totalEncontrado.Clear();

            bool b1 = string.IsNullOrEmpty(textBox_buscarFolio.Text);
            if (!b1)
            {
                MySqlConnection conexion = Conectar();
                String sentencia = "SELECT * FROM pedido WHERE id = " + textBox_buscarFolio.Text + "";
                conexion.Open();
                MySqlCommand comando = new MySqlCommand(sentencia, conexion);
                comando.ExecuteNonQuery();
                DataTable dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(comando);
                da.Fill(dt);

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        textBox_clienteEncontrado.Text = dt.Rows[0]["cliente"].ToString();
                        comboBox_productoEncontrado.Text = dt.Rows[0]["producto"].ToString();
                        textBox_precioEncontrado.Text = "$ " + dt.Rows[0]["precio"].ToString();
                        textBox_cantidadEncontrada.Text = dt.Rows[0]["cantidad"].ToString();
                        textBox_totalEncontrado.Text = "$ " + dt.Rows[0]["total"].ToString();
                        fechaPedido_Encontrada.Text = dt.Rows[0]["fecha_pedido"].ToString();
                        fechaEntrega_Encontrada.Text = dt.Rows[0]["fecha_entrega"].ToString();
                    }

                }
                conexion.Close();
            }        
        }

        private void textBox_cantidadEncontrada_TextChanged(object sender, EventArgs e)
        {
            calcular_totalModificado();
            if (textBox_cantidadEncontrada.Text == "")
            {
                calcular_totalModificado();
            }
        }

        private void comboBox_productoEncontrado_SelectedIndexChanged(object sender, EventArgs e)
        {
            double precio = 0;
            MySqlConnection conexion = Conectar();
            String sentencia = "SELECT precio FROM producto WHERE nombre = '" + comboBox_productoEncontrado.Text + "'";
            MySqlCommand comando = new MySqlCommand(sentencia, conexion);
            conexion.Open();
            comando.ExecuteNonQuery();
            MySqlDataReader MyReader;
            MyReader = comando.ExecuteReader();
            while (MyReader.Read())
            {
                precio = MyReader.GetDouble("precio");
            }
            MyReader.Close();
            conexion.Close();
            textBox_precioEncontrado.Text = "$ " + precio.ToString("0.00");
            calcular_totalModificado();
        }
        void calcular_totalModificado()
        {
            float precio = 0;
            float cantidad = 0;
            if (textBox_cantidadEncontrada.Text == "")
            {
                precio = 0;
                cantidad = 0;
            }
            bool b1 = string.IsNullOrEmpty(textBox_cantidadEncontrada.Text);
            bool b2 = string.IsNullOrEmpty(textBox_precioEncontrado.Text);
            if (!b2)
            {
                precio = float.Parse(textBox_precioEncontrado.Text.Substring(1, textBox_precioEncontrado.Text.Length - 1));
            }
           
            if (b1)
            {
                cantidad = 0;
            }
            else
            {
                cantidad = int.Parse(textBox_cantidadEncontrada.Text);
            }

            float total = precio * cantidad;
            textBox_totalEncontrado.Text = "$ " + total.ToString("0.00");
        }

        private void modificarPedido_Btn_Click(object sender, EventArgs e)
        {
            MySqlConnection conexion = Conectar();
            String sentencia = "UPDATE pedido SET cliente = '" + textBox_clienteEncontrado.Text + "', producto = '" + comboBox_productoEncontrado.Text + "', precio = " + textBox_precioEncontrado.Text.Substring(1, textBox_precioEncontrado.Text.Length - 1) + ", cantidad = " + textBox_cantidadEncontrada.Text + ", total = " + textBox_totalEncontrado.Text.Substring(1, textBox_totalEncontrado.Text.Length - 1) + ", fecha_pedido = '" + fechaPedido_Encontrada.Value.ToString("yyyy-MM-dd") + "', fecha_entrega = '" + fechaEntrega_Encontrada.Value.ToString("yyyy-MM-dd") + "'";
            MessageBox.Show(sentencia);
            MySqlCommand comando = new MySqlCommand(sentencia, conexion);
            conexion.Open();
            comando.ExecuteNonQuery();
            conexion.Close();
            MessageBox.Show("Se ha modificado el pedido");
            comboBox_producto.SelectedIndex = -1;
            textBox_cliente.Clear();
            textBox_cantidad.Clear();
        }

        private void consultarFolio_TextChanged(object sender, EventArgs e)
        {
            consultarCliente.Clear();
            consultarProducto.Clear();
            consultarPrecio.Clear();
            consultarCantidad.Clear();
            consultarTotal.Clear();
            consultarFechaEntrega.Clear();
            consultarFechaPedido.Clear();

            bool b1 = string.IsNullOrEmpty(consultarFolio.Text);
            if (!b1)
            {
                MySqlConnection conexion = Conectar();
                String sentencia = "SELECT * FROM pedido WHERE id = " + consultarFolio.Text + "";
                conexion.Open();
                MySqlCommand comando = new MySqlCommand(sentencia, conexion);
                comando.ExecuteNonQuery();
                DataTable dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(comando);
                da.Fill(dt);

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        consultarCliente.Text = dt.Rows[0]["cliente"].ToString();
                        consultarProducto.Text = dt.Rows[0]["producto"].ToString();
                        consultarPrecio.Text = "$ " + dt.Rows[0]["precio"].ToString();
                        consultarCantidad.Text = dt.Rows[0]["cantidad"].ToString();
                        consultarTotal.Text = "$ " + dt.Rows[0]["total"].ToString();
                        consultarFechaPedido.Text = DateTime.Parse(dt.Rows[0]["fecha_pedido"].ToString()).ToString("dd/MM/yyyy");
                        consultarFechaEntrega.Text = DateTime.Parse(dt.Rows[0]["fecha_entrega"].ToString()).ToString("dd/MM/yyyy");
                    }

                }
                conexion.Close();
            }
        }
    }
}
