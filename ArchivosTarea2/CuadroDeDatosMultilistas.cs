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
                }
            }

            InitializeComponent();

            dataGridView2.ReadOnly = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

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
