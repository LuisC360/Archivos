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
    /// Clase que representa la ventana de modificacion de un dato en multilistas.
    /// </summary>
    public partial class ModificaDatoMultilistas : Form
    {
        /// <summary>
        /// Bandera que nos dira si se cambio o no la llave primaria del dato.
        /// </summary>
        public bool llavePrimariaCambiada { get; set; }
        /// <summary>
        /// La entidad que posee el dato a modificarse.
        /// </summary>
        readonly Entidad ent;
        /// <summary>
        /// El dato a modificarse.
        /// </summary>
        public Dato dato { get; set; }
        /// <summary>
        /// Bandera que nos dira si se cambio algun atributo que sea llave de busqueda, independientemente de cual haya sido.
        /// </summary>
        public bool llaveBusquedaCambiada { get; set; }
        /// <summary>
        /// El numero de atributos vigentes de la entidad (aquellos que no han sido eliminados).
        /// </summary>
        int atributosVigentes;
        /// <summary>
        /// La lista de atributos vigentes de la entidad (aquellos que no han sido eliminados).
        /// </summary>
        readonly List<Atributo> listaAtributosVigentes = new List<Atributo>();
        /// <summary>
        /// El indice de la llave primaria en la lista de atributos del dato.
        /// </summary>
        int indiceLlavePrimaria;
        /// <summary>
        /// Un respaldo del dato a modificarse para verificar si se cambio la llave primaria o alguna llave de busqueda.
        /// </summary>
        readonly Dato datoRespaldo;

        /// <summary>
        /// Constructor de una nueva ventana de modificacion de dato.
        /// </summary>
        public ModificaDatoMultilistas()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Boton que capturara la nueva informacion del dato y cerrara la ventana actual.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">EventArgs.</param>
        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Funcion que regresa el dato o registro modificado.
        /// </summary>
        /// <returns>El dato o registro modificado.</returns>
        public Dato regresa_dato_multilistas()
        {
            return dato;
        }

        /// <summary>
        /// Funcion que regresa la bandera de si se cambio la llave primaria.
        /// </summary>
        /// <returns>La bandera de si se cambio la llave primaria.</returns>
        public bool regresa_llave_primaria_cambiada()
        {
            return llavePrimariaCambiada;
        }
    }
}
