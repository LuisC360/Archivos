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
    public partial class CuadroDeDatosIndexado : Form
    {
        public Entidad ent { get; set; }
        readonly int numAtributos;
        readonly int numIndices;
        Atributo atrLlave;
        readonly List<Atributo> atributosVigentes = new List<Atributo>();
        public long posMemoria { get; set; }
        long tamDato;
        public long apDatos { get; set; }
        long rango;
        public bool bandChanged { get; set; }
        readonly List<Indice> indicesVigentes = new List<Indice>();

        public CuadroDeDatosIndexado(Entidad e, long pMem, long apDat, long tamDat)
        {
            ent = e;
            posMemoria = pMem;
            apDatos = apDat;
            tamDato = tamDat;

            foreach(Atributo atr in ent.listaAtributos)
            {
                if (atr.apSigAtributo != -2 && atr.apSigAtributo != -4)
                {
                    numAtributos++;
                }
            }

            foreach(Indice ind in ent.listaIndices)
            {
                long apSig = ind.regresa_apSigIndice();

                if(apSig != -2 && apSig != -4)
                {
                    numIndices++;
                }
            }

            InitializeComponent();

            //if()

            dataGridView1.ReadOnly = true;
        }

        private void rellena_dataGridIndices()
        {
            dataGridView1.ColumnCount = 5;
            dataGridView1.ColumnHeadersVisible = true;
            int j = 0;

            for(int i = 0; i < ent.listaIndices.Count; i++)
            {
                long apSig = ent.listaIndices[i].regresa_apSigIndice();

                if(apSig != -2 && apSig != -4)
                {
                    
                }
            }
        }

        // Boton que define el rango, dependiendo de que tipo de dato sea la llave primaria.
        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
