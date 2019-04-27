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
  
        private void buscarDirectorio_TextChanged(object sender, EventArgs e)
        {
            AutoCompleteStringCollection autotext_directorio = new AutoCompleteStringCollection();

            bool b1 = string.IsNullOrEmpty(buscarDirectorio.Text);
            if (!b1)
            {
                MySqlConnection conexion = Conectar();
                String sentencia = "";


                if (radioBtn_Nombre.Checked)
                {

                    sentencia = "SELECT proveedor FROM directorio";
                    conexion.Open();
                    MySqlCommand comando = new MySqlCommand(sentencia, conexion);
                    comando.ExecuteNonQuery();
                    MySqlDataReader MyReader;
                    MyReader = comando.ExecuteReader();
                    while (MyReader.Read())
                    {
                        autotext_directorio.Add(MyReader.GetString(0));
                    }

                    buscarDirectorio.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    buscarDirectorio.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    buscarDirectorio.AutoCompleteCustomSource = autotext_directorio;

                    MyReader.Close();
                    conexion.Close();

                }
                else if (radioBtn_Material.Checked)
                {

                    sentencia = "SELECT material FROM directorio";
                    conexion.Open();
                    MySqlCommand comando = new MySqlCommand(sentencia, conexion);
                    comando.ExecuteNonQuery();
                    MySqlDataReader MyReader;
                    MyReader = comando.ExecuteReader();
                    while (MyReader.Read())
                    {
                        autotext_directorio.Add(MyReader.GetString(0));
                    }

                    buscarDirectorio.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    buscarDirectorio.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    buscarDirectorio.AutoCompleteCustomSource = autotext_directorio;

                    MyReader.Close();
                    conexion.Close();

                }

                /*
                MySqlConnection conexion = Conectar();
                String sentencia = "SELECT * FROM directorio";
                conexion.Open();
                MySqlCommand comando = new MySqlCommand(sentencia, conexion);
                comando.ExecuteNonQuery();
                DataTable dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(comando);
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    mostrarDirectorio.ReadOnly = true;
                    mostrarDirectorio.DataSource = da;
                }
                */
            }
        }
    }
}
