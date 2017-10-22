using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArchivosTarea2
{
    public partial class SeleccionTipo : Form
    {
        public int tipo { get; set; }

        public SeleccionTipo()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tipo = 0;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tipo = 1;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public int regresa_tipo()
        {
            return tipo;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tipo = 2;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
