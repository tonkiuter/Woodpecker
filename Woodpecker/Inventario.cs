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
        private void inventarioProductosBtn_Click(object sender, EventArgs e) //
        {
            tabControl_inventario.SelectTab(0);
        }

        private void inventarioMaterialesBtn_Click(object sender, EventArgs e)
        {
            tabControl_inventario.SelectTab(1);
        }

        private void inventarioMaderaBtn_Click(object sender, EventArgs e)
        {
            tabControl_inventario.SelectTab(2);
        }
    }
}
