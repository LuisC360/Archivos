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
    public partial class CuadroDeDatosMultilistas : Form
    {
        public long[] cabeceras { get; set; }
        public bool seCambio { get; set; }
        public Entidad ent { get; set; }
        public long posMemoria { get; set; }
        readonly int numAtributos;
        readonly Atributo atrLlave;
        readonly int indiceLlave;
        readonly long tamDato;
        readonly List<Atributo> atributosVigentes = new List<Atributo>();
        readonly List<Atributo> atributosBusqueda = new List<Atributo>();

        /// <summary>
        /// Constructor de la ventana de manipulacion de datos en multilistas.
        /// </summary>
        /// <param name="e">La entidad con las cabeceras que apuntaran a los datos con multilistas.</param>
        /// <param name="pMem">La posicion actual en memoria.</param>
        /// <param name="tD">El tamaño actual del dato.</param>
        public CuadroDeDatosMultilistas(Entidad e, long pMem, long tD)
        {
            ent = e;
            posMemoria = pMem;
            tamDato = tD;
            int indLlave = 0;

            foreach(Atributo atr in ent.listaAtributos)
            {
                if (atr.apSigAtributo != -2 && atr.apSigAtributo != -4)
                {
                    numAtributos++;
                    atributosVigentes.Add(atr);

                    if (atr.esLlavePrimaria == true)
                    {
                        atrLlave = atr;
                        indiceLlave = indLlave;
                    }
                    else
                    {
                        indLlave++;
                    }

                    if(atr.esLlaveDeBusqueda == true)
                    {
                        atributosBusqueda.Add(atr);
                    }
                }
            }

            InitializeComponent();

            dataGridView2.ReadOnly = true;
            rellenaLavesBusqueda();
        }

        // Boton para insertar un dato.
        private void button4_Click(object sender, EventArgs e)
        {

        }

        // Boton para modificar un dato.
        private void button1_Click(object sender, EventArgs e)
        {

        }

        // Boton para eliminar un dato.
        private void button2_Click(object sender, EventArgs e)
        {

        }
        
        // Boton para mostrar solo los datos de una llave de busqueda definida por .
        private void button3_Click(object sender, EventArgs e)
        {

        }

        // Boton para cerrar el cuadro de manipulacion de datos.
        private void button5_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Metodo que pondra en el comboBox correspondiente todos los atributos de la entidad que sean llaves de busqueda.
        /// </summary>
        private void rellenaLavesBusqueda()
        {
            foreach(Atributo atr in atributosVigentes)
            {
                if(atr.esLlaveDeBusqueda == true)
                {
                    String ll = new string(atr.nombre);

                    comboBox1.Items.Add(ll);
                }
            }
        }

        public long regresa_apuntador_cabeceras()
        {
            return ent.apCabeceras;
        }

        public bool regresa_se_cambio()
        {
            return seCambio;
        }

        public long regresa_posicion_memoria()
        {
            return posMemoria;
        }

        public List<Cabecera> regresa_lista_cabeceras()
        {
            return ent.listaCabeceras;
        }
    }
}
