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
    public partial class BusquedaModifica : Form
    {
        public String newNombre {get;set;}

        /// <summary>
        /// Constructor del formulario de modificacion del nombre de una entidad.
        /// </summary>
        public BusquedaModifica()
        {
            InitializeComponent();
        }

        // Boton para aceptar el cambio de nombre de una entidad.
        private void button1_Click(object sender, EventArgs e)
        {
            this.newNombre = textBox1.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
