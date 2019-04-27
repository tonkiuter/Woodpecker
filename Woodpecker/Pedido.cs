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
using System.Web;
using System.Web.UI;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;



namespace Woodpecker
{
        public partial class Form1 : Form
        {
        int cantidad_productos = 1;
        AutoCompleteStringCollection autotext_clientes = new AutoCompleteStringCollection();
       
                                                  //PESTAÑA "NUEVO", OPCION PARA AGREGAR NUEVOS PEDIDOS
        
       private void agregarPedido_btn_Click_1(object sender, EventArgs e) // Hacer click en "Nuevo", hacer visible la tab pedidos y cargar los clientes existentes
        {
            DateTime today = DateTime.Today;
            tab_pedidos.Visible = true;
            tab_pedidos.SelectTab(0);
            comboBox_producto1.SelectedIndex = -1;
            textBox_cliente.Clear();
            textBox_cantidad1.Clear();
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
        private void textBox_cliente_Validating(object sender, CancelEventArgs e) // Validación para detectar clientes nuevos
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
        private void agregarPedido_Click(object sender, EventArgs e) // Leer datos ingresados por el usuario e insertar un pedido a la base de datos
        {
            //Recuperar id del cliente
            int IDcliente = 0;
            int IDpedido = 0;
            MySqlConnection conexion = Conectar();
            String sentenciaBuscarCliente = "SELECT IDcliente FROM clientes WHERE nombre = '" + textBox_cliente.Text + "'"; 
            MySqlCommand comando1 = new MySqlCommand(sentenciaBuscarCliente, conexion);
            conexion.Open();
            comando1.ExecuteNonQuery();
            MySqlDataReader MyReader;
            MyReader = comando1.ExecuteReader();
            while (MyReader.Read())
            {
                IDcliente = MyReader.GetInt16("IDcliente");
            }
            MyReader.Close();

            //Insertar en tabla pedido y recuperar el ID insertado
            String sentenciaPedido = "INSERT INTO pedido (IDcliente, total, fecha_pedido, fecha_entrega ) VALUES " + " (" + IDcliente + "," + "" + textBox_total.Text.Substring(1, textBox_total.Text.Length - 1) +"," + "'" + fechaPedido.Value.ToString("yyyy-MM-dd") + "'," + "'" + fechaEntrega.Value.ToString("yyyy-MM-dd") + "');";
            String pedidoInsertado = "SELECT LAST_INSERT_ID() ";      
            MySqlCommand comando = new MySqlCommand(sentenciaPedido, conexion);
            comando.ExecuteNonQuery();
            MySqlCommand comando5 = new MySqlCommand(pedidoInsertado, conexion);
            comando5.ExecuteNonQuery();
            MyReader = comando5.ExecuteReader();
             while (MyReader.Read())
             {
                 IDpedido = Int32.Parse(MyReader.GetString(0));
             }
             MyReader.Close();

            //Insertar en tabla pedido_producto por cada producto regstrado por el usuario
            for (int i = 1; i<cantidad_productos+1; i++)
            {
                String sentencia4 = "";
                String sentencia3 = "";
                int IDproducto = 0;
                
                    if (i == 1)
                    {
                    sentencia4 = "SELECT IDproducto FROM producto WHERE nombre = '" + comboBox_producto1.Text + "'";
                    MySqlCommand comando2 = new MySqlCommand(sentencia4, conexion);
                    comando2.ExecuteNonQuery();
                    MyReader = comando2.ExecuteReader();
                    while (MyReader.Read())
                    {
                        IDproducto = Int32.Parse(MyReader.GetString(0));
                    }
                    MyReader.Close();
                    sentencia3 = "INSERT INTO pedido_producto (IDpedido, IDproducto, cantidad, precio) VALUES " + " ("+ IDpedido +", " + "" + IDproducto + "," + "'" + textBox_cantidad1.Text + "'," + "'" + textBox_precio1.Text.Substring(1, textBox_precio1.Text.Length - 1) + "')";
                    }
                    else if(i == 2)
                    {
                        sentencia4 = "SELECT IDproducto FROM producto WHERE nombre = '" + comboBox_producto2.Text + "'";
                    MySqlCommand comando2 = new MySqlCommand(sentencia4, conexion);
                    comando2.ExecuteNonQuery();
                    MyReader = comando2.ExecuteReader();
                    while (MyReader.Read())
                    {
                        IDproducto = Int32.Parse(MyReader.GetString(0));
                    }
                    MyReader.Close();
                    sentencia3 = "INSERT INTO pedido_producto (IDpedido, IDproducto, cantidad, precio) VALUES " + " (" + IDpedido + ", " + "" + IDproducto + "," + "'" + textBox_cantidad2.Text + "'," + "'" + textBox_precio2.Text.Substring(1, textBox_precio2.Text.Length - 1) + "')";
                    }
                    else if (i == 3)
                    {
                        sentencia4 = "SELECT IDproducto FROM producto WHERE nombre = '" + comboBox_producto3.Text + "'";
                    MySqlCommand comando2 = new MySqlCommand(sentencia4, conexion);
                    comando2.ExecuteNonQuery();
                    MyReader = comando2.ExecuteReader();
                    while (MyReader.Read())
                    {
                        IDproducto = Int32.Parse(MyReader.GetString(0));
                    }
                    MyReader.Close();
                    sentencia3 = "INSERT INTO pedido_producto (IDpedido, IDproducto, cantidad, precio) VALUES " + " (" + IDpedido + ", " + "" + IDproducto + "," + "'" + textBox_cantidad3.Text + "'," + "'" + textBox_precio3.Text.Substring(1, textBox_precio3.Text.Length - 1) + "')";
                    }
                    else if (i == 4)
                    {
                        sentencia4 = "SELECT IDproducto FROM producto WHERE nombre = '" + comboBox_producto4.Text + "'";
                    MySqlCommand comando2 = new MySqlCommand(sentencia4, conexion);
                    comando2.ExecuteNonQuery();
                    MyReader = comando2.ExecuteReader();
                    while (MyReader.Read())
                    {
                        IDproducto = Int32.Parse(MyReader.GetString(0));
                    }
                    MyReader.Close();
                    sentencia3 = "INSERT INTO pedido_producto (IDpedido, IDproducto, cantidad, precio) VALUES " + " (" + IDpedido + ", " + "" + IDproducto + "," + "'" + textBox_cantidad4.Text + "'," + "'" + textBox_precio4.Text.Substring(1, textBox_precio4.Text.Length - 1) + "')";
                    }
                    else if (i == 5)
                    {
                        sentencia4 = "SELECT IDproducto FROM producto WHERE nombre = '" + comboBox_producto5.Text + "'";
                    MySqlCommand comando2 = new MySqlCommand(sentencia4, conexion);
                    comando2.ExecuteNonQuery();
                    MyReader = comando2.ExecuteReader();
                    while (MyReader.Read())
                    {
                        IDproducto = Int32.Parse(MyReader.GetString(0));
                    }
                    MyReader.Close();
                    sentencia3 = "INSERT INTO pedido_producto (IDpedido, IDproducto, cantidad, precio) VALUES " + " (" + IDpedido + ", " + "" + IDproducto + "," + "'" + textBox_cantidad5.Text + "'," + "'" + textBox_precio5.Text.Substring(1, textBox_precio5.Text.Length - 1) + "')";
                    }
                    else if (i == 6)
                    {
                        sentencia4 = "SELECT IDproducto FROM producto WHERE nombre = '" + comboBox_producto6.Text + "'";
                    MySqlCommand comando2 = new MySqlCommand(sentencia4, conexion);
                    comando2.ExecuteNonQuery();
                    MyReader = comando2.ExecuteReader();
                    while (MyReader.Read())
                    {
                        IDproducto = Int32.Parse(MyReader.GetString(0));
                    }
                    MyReader.Close();
                    sentencia3 = "INSERT INTO pedido_producto (IDpedido, IDproducto, cantidad, precio) VALUES " + " (" + IDpedido + ", " + "" + IDproducto + "," + "'" + textBox_cantidad6.Text + "'," + "'" + textBox_precio6.Text.Substring(1, textBox_precio6.Text.Length - 1) + "')";
                    }
                    else if (i == 7)
                    {
                        sentencia4 = "SELECT IDproducto FROM producto WHERE nombre = '" + comboBox_producto7.Text + "'";
                    MySqlCommand comando2 = new MySqlCommand(sentencia4, conexion);
                    comando2.ExecuteNonQuery();
                    MyReader = comando2.ExecuteReader();
                    while (MyReader.Read())
                    {
                        IDproducto = Int32.Parse(MyReader.GetString(0));
                    }
                    MyReader.Close();
                    sentencia3 = "INSERT INTO pedido_producto (IDpedido, IDproducto, cantidad, precio) VALUES " + " (" + IDpedido + ", " + "" + IDproducto + "," + "'" + textBox_cantidad7.Text + "'," + "'" + textBox_precio7.Text.Substring(1, textBox_precio7.Text.Length - 1) + "')";
                    }
                    else if (i == 8)
                    {

                        sentencia4 = "SELECT IDproducto FROM producto WHERE nombre = '" + comboBox_producto8.Text + "'";
                    MySqlCommand comando2 = new MySqlCommand(sentencia4, conexion);
                    comando2.ExecuteNonQuery();
                    MyReader = comando2.ExecuteReader();
                    while (MyReader.Read())
                    {
                        IDproducto = Int32.Parse(MyReader.GetString(0));
                    }
                    MyReader.Close();
                    sentencia3 = "INSERT INTO pedido_producto (IDpedido, IDproducto, cantidad, precio) VALUES " + " (" + IDpedido + ", " + "" + IDproducto + "," + "'" + textBox_cantidad8.Text + "'," + "'" + textBox_precio8.Text.Substring(1, textBox_precio8.Text.Length - 1) + "')";
                    }

                MySqlCommand comando3 = new MySqlCommand(sentencia3, conexion);
                comando3.ExecuteNonQuery();
            }

            conexion.Close();
            MessageBox.Show("Se ha agregado un pedido");
            comboBox_producto1.SelectedIndex = -1;
            textBox_cliente.Clear();
            textBox_cantidad1.Clear();
            folio_pedido();
        }
      
        private void comboBox_productos_SelectedIndexChanged(object sender, EventArgs e) // Mostrar el precio desde la base de datos del producto correspondiente. 
        {                                                                                // Calcular total cada que el usuario cambie de producto.   
            double precio = mostrarPrecio(comboBox_producto1.Text);
            textBox_precio1.Text = "$ " + precio.ToString("0.00");
            calcularTotal();
        }

        private void comboBox_producto2_SelectedIndexChanged(object sender, EventArgs e)
        {
            double precio = mostrarPrecio(comboBox_producto2.Text);
            textBox_precio2.Text = "$ " + precio.ToString("0.00");
            calcularTotal();
        }

        private void comboBox_producto3_SelectedIndexChanged(object sender, EventArgs e)
        {
            double precio = mostrarPrecio(comboBox_producto3.Text);
            textBox_precio3.Text = "$ " + precio.ToString("0.00");
            calcularTotal();
        }

        private void comboBox_producto4_SelectedIndexChanged(object sender, EventArgs e)
        {
            double precio = mostrarPrecio(comboBox_producto4.Text);
            textBox_precio4.Text = "$ " + precio.ToString("0.00");
            calcularTotal();
        }

        private void comboBox_producto5_SelectedIndexChanged(object sender, EventArgs e)
        {
            double precio = mostrarPrecio(comboBox_producto5.Text);
            textBox_precio5.Text = "$ " + precio.ToString("0.00");
            calcularTotal();
        }

        private void comboBox_producto6_SelectedIndexChanged(object sender, EventArgs e)
        {
            double precio = mostrarPrecio(comboBox_producto6.Text);
            textBox_precio6.Text = "$ " + precio.ToString("0.00");
            calcularTotal();
        }

        private void comboBox_producto7_SelectedIndexChanged(object sender, EventArgs e)
        {
            double precio = mostrarPrecio(comboBox_producto7.Text);
            textBox_precio7.Text = "$ " + precio.ToString("0.00");
            calcularTotal();
        }

        private void comboBox_producto8_SelectedIndexChanged(object sender, EventArgs e)
        {
            double precio = mostrarPrecio(comboBox_producto8.Text);
            textBox_precio8.Text = "$ " + precio.ToString("0.00");
            calcularTotal();
        }
        private void textBox_cantidad_TextChanged(object sender, EventArgs e)   //Calcular total cada que la cantidad de un producto sea cambiada por el usuario
        {
            calcularTotal();
        }
        private void textBox_cantidad2_TextChanged(object sender, EventArgs e)
        {
            calcularTotal();
        }

        private void textBox_cantidad3_TextChanged(object sender, EventArgs e)
        {
            calcularTotal();
        }

        private void textBox_cantidad4_TextChanged(object sender, EventArgs e)
        {
            calcularTotal();
        }

        private void textBox_cantidad5_TextChanged(object sender, EventArgs e)
        {
            calcularTotal();
        }

        private void textBox_cantidad6_TextChanged(object sender, EventArgs e)
        {
            calcularTotal();
        }

        private void textBox_cantidad7_TextChanged(object sender, EventArgs e)
        {
            calcularTotal();
        }

        private void textBox_cantidad8_TextChanged(object sender, EventArgs e)
        {
            calcularTotal();
        }


        void cargar_productos() // Método para leer los productos existentes en la base de datos y cargarlos a los combobox para mostrar opciones de productos.
        {
            comboBox_producto1.Items.Clear();
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
                comboBox_producto1.Items.Add(dr["nombre"].ToString());
                comboBox_producto2.Items.Add(dr["nombre"].ToString());
                comboBox_producto3.Items.Add(dr["nombre"].ToString());
                comboBox_producto4.Items.Add(dr["nombre"].ToString());
                comboBox_producto5.Items.Add(dr["nombre"].ToString());
                comboBox_producto6.Items.Add(dr["nombre"].ToString());
                comboBox_producto7.Items.Add(dr["nombre"].ToString());
                comboBox_producto8.Items.Add(dr["nombre"].ToString());
                comboBox_productoEncontrado.Items.Add(dr["nombre"].ToString());
            }
            conexion.Close();
        }
        double mostrarPrecio(string boxproducto) // Método para mostrar el precio de un producto seleccionado desde la base de datos.
        {
            double precio = 0;
            MySqlConnection conexion = Conectar();
            String sentencia = "SELECT precio FROM producto WHERE nombre = '" + boxproducto + "'";
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
            return precio;
        }

        void calcularTotal()  // Método para obtener valores de los precios y cantidades, verificar si está vacío para asignar 0 como cantidad y precio. Calcular total.
        {
            float precio1 = textBox_precio1.Text == "" ? 0 : float.Parse(textBox_precio1.Text.Substring(1, textBox_precio1.Text.Length - 1));
            float precio2 = textBox_precio2.Text == "" ? 0 : float.Parse(textBox_precio2.Text.Substring(1, textBox_precio2.Text.Length - 1));
            float precio3 = textBox_precio3.Text == "" ? 0 : float.Parse(textBox_precio3.Text.Substring(1, textBox_precio3.Text.Length - 1));
            float precio4 = textBox_precio4.Text == "" ? 0 : float.Parse(textBox_precio4.Text.Substring(1, textBox_precio4.Text.Length - 1));
            float precio5 = textBox_precio5.Text == "" ? 0 : float.Parse(textBox_precio5.Text.Substring(1, textBox_precio5.Text.Length - 1));
            float precio6 = textBox_precio6.Text == "" ? 0 : float.Parse(textBox_precio6.Text.Substring(1, textBox_precio6.Text.Length - 1));
            float precio7 = textBox_precio7.Text == "" ? 0 : float.Parse(textBox_precio7.Text.Substring(1, textBox_precio7.Text.Length - 1));
            float precio8 = textBox_precio8.Text == "" ? 0 : float.Parse(textBox_precio8.Text.Substring(1, textBox_precio8.Text.Length - 1));

            int cantidad1 = textBox_cantidad1.Text == "" ? 0 : int.Parse(textBox_cantidad1.Text);
            int cantidad2 = textBox_cantidad2.Text == "" ? 0 : int.Parse(textBox_cantidad2.Text);
            int cantidad3 = textBox_cantidad3.Text == "" ? 0 : int.Parse(textBox_cantidad3.Text);
            int cantidad4 = textBox_cantidad4.Text == "" ? 0 : int.Parse(textBox_cantidad4.Text);
            int cantidad5 = textBox_cantidad5.Text == "" ? 0 : int.Parse(textBox_cantidad5.Text);
            int cantidad6 = textBox_cantidad6.Text == "" ? 0 : int.Parse(textBox_cantidad6.Text);
            int cantidad7 = textBox_cantidad7.Text == "" ? 0 : int.Parse(textBox_cantidad7.Text);
            int cantidad8 = textBox_cantidad8.Text == "" ? 0 : int.Parse(textBox_cantidad8.Text);

            float total = (precio1 * cantidad1) + (precio2 * cantidad2) + (precio3 * cantidad3) + (precio4 * cantidad4) + (precio5 * cantidad5) + (precio6 * cantidad6) +
                (precio7 * cantidad7) + (precio8 * cantidad8);

            textBox_total.Text = "$ " + total.ToString("0.00");
        }
        void folio_pedido() // Método para mostrar el folio siguiente al último registrado en la base de datos
        {
            int folio = 0;
            MySqlConnection conexion = Conectar();
            String sentencia = "SELECT IDpedido FROM pedido ORDER BY IDpedido DESC LIMIT 1";
            MySqlCommand comando = new MySqlCommand(sentencia, conexion);
            conexion.Open();
            comando.ExecuteNonQuery();
            MySqlDataReader MyReader;
            MyReader = comando.ExecuteReader();
            while (MyReader.Read())
            {
                folio = (MyReader.GetInt16("IDpedido"));
                folio = folio + 1;
            }
            if (folio == 0)
            {
                folio = 1;
            }
            MyReader.Close();
            conexion.Close();
            var folio_padded = folio.ToString().PadLeft(4, '0');
            textBox_idPedido.Text = folio_padded;
        }

        //BOTONES AGREGAR Y QUITAR PRODUCTOS

        private void agregarProductoBtn_Click(object sender, EventArgs e)
        {
            agregarProductoBtn.Visible = false;         //Habilitar visivilidad de botones correspondientes
            quitarProductoBtn1.Visible = true;
            quitarProductoBtn2.Visible = true;
            agregarProductoBtn2.Visible = true;
            comboBox_producto2.Visible = true;
            textBox_precio2.Visible = true;
            textBox_cantidad2.Visible = true;
            cantidad_productos++;
        }

        private void agregarProductoBt2_Click(object sender, EventArgs e)
        {
            agregarProductoBtn2.Visible = false;
            quitarProductoBtn2.Visible = true;
            quitarProductoBtn3.Visible = true;
            agregarProductoBtn3.Visible = true;
            comboBox_producto3.Visible = true;
            textBox_precio3.Visible = true;
            textBox_cantidad3.Visible = true;
            cantidad_productos++;
        }

        private void agregarProductoBtn3_Click(object sender, EventArgs e)
        {
            agregarProductoBtn3.Visible = false;
            quitarProductoBtn3.Visible = true;
            quitarProductoBtn4.Visible = true;
            agregarProductoBtn4.Visible = true;
            comboBox_producto4.Visible = true;
            textBox_precio4.Visible = true;
            textBox_cantidad4.Visible = true;
            cantidad_productos++;
        }

        private void agregarProductoBtn4_Click(object sender, EventArgs e)
        {
            agregarProductoBtn4.Visible = false;
            quitarProductoBtn4.Visible = true;
            quitarProductoBtn5.Visible = true;
            agregarProductoBtn5.Visible = true;
            comboBox_producto5.Visible = true;
            textBox_precio5.Visible = true;
            textBox_cantidad5.Visible = true;
            cantidad_productos++;
        }

        private void agregarProductoBtn5_Click(object sender, EventArgs e)
        {
            agregarProductoBtn5.Visible = false;
            quitarProductoBtn5.Visible = true;
            quitarProductoBtn6.Visible = true;
            agregarProductoBtn6.Visible = true;
            comboBox_producto6.Visible = true;
            textBox_precio6.Visible = true;
            textBox_cantidad6.Visible = true;
            cantidad_productos++;
        }

        private void agregarProductoBtn6_Click(object sender, EventArgs e)
        {
            agregarProductoBtn6.Visible = false;
            quitarProductoBtn6.Visible = true;
            quitarProductoBtn7.Visible = true;
            agregarProductoBtn7.Visible = true;
            comboBox_producto7.Visible = true;
            textBox_precio7.Visible = true;
            textBox_cantidad7.Visible = true;
            cantidad_productos++;
        }

        private void agregarProductoBtn7_Click(object sender, EventArgs e)
        {
            agregarProductoBtn7.Visible = false;
            quitarProductoBtn7.Visible = true;
            quitarProductoBtn8.Visible = true;
            quitarProductoBtn8.Visible = true;
            comboBox_producto8.Visible = true;
            textBox_precio8.Visible = true;
            textBox_cantidad8.Visible = true;
            cantidad_productos++;
        }
        private void quitarProductoBtn1_Click(object sender, EventArgs e)
        {
            comboBox_producto1.SelectedIndex = -1;
            textBox_precio1.Clear();
            textBox_cantidad1.Clear();
            calcularTotal();
            cantidad_productos--;
        }

        private void quitarProductoBtn2_Click(object sender, EventArgs e)
        {
            comboBox_producto2.SelectedIndex = -1;
            textBox_precio2.Clear();
            textBox_cantidad2.Clear();
            calcularTotal();
            cantidad_productos--;
        }

        private void quitarProductoBtn3_Click(object sender, EventArgs e)
        {
            comboBox_producto3.SelectedIndex = -1;
            textBox_precio3.Clear();
            textBox_cantidad3.Clear();
            calcularTotal();
            cantidad_productos--;
        }

        private void quitarProductoBtn4_Click(object sender, EventArgs e)
        {
            comboBox_producto4.SelectedIndex = -1;
            textBox_precio4.Clear();
            textBox_cantidad4.Clear();
            calcularTotal();
            cantidad_productos--;
        }

        private void quitarProductoBtn5_Click(object sender, EventArgs e)
        {
            comboBox_producto5.SelectedIndex = -1;
            textBox_precio5.Clear();
            textBox_cantidad5.Clear();
            calcularTotal();
            cantidad_productos--;
        }

        private void quitarProductoBtn6_Click(object sender, EventArgs e)
        {
            comboBox_producto6.SelectedIndex = -1;
            textBox_precio6.Clear();
            textBox_cantidad6.Clear();
            calcularTotal();
            cantidad_productos--;
        }

        private void quitarProductoBtn7_Click(object sender, EventArgs e)
        {
            comboBox_producto7.SelectedIndex = -1;
            textBox_precio7.Clear();
            textBox_cantidad7.Clear();
            calcularTotal();
            cantidad_productos--;
        }

        private void quitarProductoBtn8_Click(object sender, EventArgs e)
        {
            comboBox_producto8.SelectedIndex = -1;
            textBox_precio8.Clear();
            textBox_cantidad8.Clear();
            calcularTotal();
            cantidad_productos--;
        }

        //Generar la factura
        private void generarFacturaBtn_Click(object sender, EventArgs e)
        {
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    StringBuilder sb = new StringBuilder();

                    //Generar Header de la factura
                    sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");
                    sb.Append("<tr><td align='center' style='background-color: #18B5F0' colspan = '2'><b>Factura</b></td></tr>");
                    sb.Append("<tr><td colspan = '2'></td></tr>");
                    sb.Append("<tr><td><b>No. de folio: </b>");
                    sb.Append(textBox_idPedido.Text);
                    sb.Append("</td><td align = 'right'><b>Fecha: </b>");
                    sb.Append(DateTime.Now);
                    sb.Append(" </td></tr>");
                    sb.Append("<tr><td colspan = '2'><b>Nombre de la empresa: </b>");
                    sb.Append("Carpinterías Woodpecker");
                    sb.Append("</td></tr>");
                    sb.Append("</table>");
                    sb.Append("<br />");

                    //Generar items
                    /* sb.Append("<table border = '1'>");
                     sb.Append("<tr>");
                     foreach (DataColumn column in dt.Columns)
                     {
                         sb.Append("<th style = 'background-color: #D20B0C;color:#ffffff'>");
                         sb.Append(column.ColumnName);
                         sb.Append("</th>");
                     }
                     sb.Append("</tr>");
                     foreach (DataRow row in dt.Rows)
                     {
                         sb.Append("<tr>");
                         foreach (DataColumn column in dt.Columns)
                         {
                             sb.Append("<td>");
                             sb.Append(row[column]);
                             sb.Append("</td>");
                         }
                         sb.Append("</tr>");
                     }
                     sb.Append("<tr><td align = 'right' colspan = '");
                     sb.Append(dt.Columns.Count - 1);
                     sb.Append("'>Total</td>");
                     sb.Append("<td>");
                     sb.Append(dt.Compute("sum(Total)", ""));
                     sb.Append("</td>");
                     sb.Append("</tr></table>");
                     */

                    /*Export HTML String como PDF.
                    StringReader sr = new StringReader(sb.ToString());
                    Document pdfDoc = new Document();
                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);
                    pdfDoc.Open();
                    htmlparser.Parse(sr);
                    pdfDoc.Close();
                    HttpContext.Current.Response.ContentType = "application/pdf";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=Factura_" + textBox_idPedido.Text + ".pdf");
                    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    HttpContext.Current.Response.Write(pdfDoc);
                    HttpContext.Current.Response.End();
                    MessageBox.Show("here");*/

                    var exportFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    var exportFile = System.IO.Path.Combine(exportFolder, "Factura-" + textBox_idPedido.Text + ".pdf");

                    using (var writer = new PdfWriter(exportFile)){
                        using (var pdf = new PdfDocument(writer)){
                            var doc = new Document(pdf);
                            doc.Add(new Paragraph("Hello World"));

                        }
                    }
                    }
                    
                }
            }
        

        // ***********************************************  PESTAÑA CONSULTAR PEDIDO, OPCIÓN PARA BUSCAR Y MOSTRAR PEDIDOS  ***************************************

        private void mostrarPedido_Btn_Click(object sender, EventArgs e)
        {
            tab_pedidos.Visible = true;
            tab_pedidos.SelectTab(2);
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
            MySqlCommand comando = new MySqlCommand(sentencia, conexion);
            conexion.Open();
            comando.ExecuteNonQuery();
            conexion.Close();
            MessageBox.Show("Se ha modificado el pedido");
            comboBox_producto1.SelectedIndex = -1;
            textBox_cliente.Clear();
            textBox_cantidad1.Clear();
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

        // ***************************************************** PESTAÑA MODIFICAR PEDIDO *******************************************************************
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

    }

}
