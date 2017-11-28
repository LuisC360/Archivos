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
    /// <summary>
    /// Clase que representa la ventana de modificacion de el nombre de un atributo.
    /// </summary>
    public partial class BusquedaModifica : Form
    {
        /// <summary>
        /// El nuevo nombre del atributo.
        /// </summary>
        public String newNombre {get;set;}

        /// <summary>
        /// Constructor del formulario de modificacion del nombre de una entidad.
        /// </summary>
        public BusquedaModifica()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Boton para aceptar el cambio de nombre de una entidad y que cierra el cuadro de dialogo.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">EventArgs.</param>
        private void button1_Click(object sender, EventArgs e)
        {
            this.newNombre = textBox1.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
