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
    /// Clase que representa la ventana donde se elegira el tipo de manejo de datos para el archivo.
    /// </summary>
    public partial class SeleccionTipo : Form
    {
        /// <summary>
        /// El tipo de archivo (1- Sec. Ordenada, 2- Sec. Indexada, 3- Hash estatica, 4- Multilistas).
        /// </summary>
        public int tipo { get; set; }

        /// <summary>
        /// Constructor de una nueva instancia de la ventana de seleccion de tipo de manejo de datos.
        /// </summary>
        public SeleccionTipo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Boton que elije "Secuencial ordenada" como el tipo de manejo de datos del archivo.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">EventArgs.</param>
        private void button1_Click(object sender, EventArgs e)
        {
            tipo = 0;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Boton que elije "Secuencial indexada" como el tipo de manejo de datos del archivo.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">EventArgs.</param>
        private void button2_Click(object sender, EventArgs e)
        {
            tipo = 1;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Funcion que regresa el numero de tipo de manejo de datos.
        /// </summary>
        /// <returns>El numero del tipo de manejo de datos.</returns>
        public int regresa_tipo()
        {
            return tipo;
        }

        /// <summary>
        /// Boton que elije "Hash estatica" como el tipo de manejo de datos del archivo.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">EventArgs.</param>
        private void button3_Click(object sender, EventArgs e)
        {
            tipo = 2;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Boton que elije "Multilistas" como el tipo de manejo de datos del archivo.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">EventArgs.</param>
        private void button4_Click(object sender, EventArgs e)
        {
            tipo = 3;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
