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
    /// Clase que representara la ventana para poner el tamaño de un atributo que sea String.
    /// </summary>
    public partial class TamString : Form
    {
        /// <summary>
        /// El numero de caracteres totales del String.
        /// </summary>
        public int numCaracteres {get;set;}

        /// <summary>
        /// Constructor de la ventana.
        /// </summary>
        public TamString()
        {
            this.Location = new Point(100, 100);
            InitializeComponent();           
        }

        /// <summary>
        /// Boton que tomara el valor numerico del textBox, lo convertira en un entero y cerrara la ventana actual.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">EventArgs.</param>
        private void button1_Click(object sender, EventArgs e)
        {
            this.numCaracteres = int.Parse(textBox1.Text);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
