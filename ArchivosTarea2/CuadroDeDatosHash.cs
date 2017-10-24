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
    public partial class CuadroDeDatosHash : Form
    {
        public long numCajones { get; set; }
        public long regPorCubeta { get; set; }
        public bool seCambio { get; set; }
        public Entidad ent { get; set; }
        public long posMemoria { get; set; }
        readonly int numAtributos;
        readonly Atributo atrLlave;
        readonly int indiceLlave;
        long tamDato;
        long tamCajon = 8;
        long tamCubeta = 8;
        readonly List<Atributo> atributosVigentes = new List<Atributo>();

        /// <summary>
        /// Constructor de la ventana para la manipulacion de datos mediante hash estatica.
        /// </summary>
        /// <param name="e">La entidad en la que se insertaran los cajones.</param>
        /// <param name="pMem">La posicion actual de memoria.</param>
        /// <param name="tamDato">El tamaño del dato a insertar.</param>
        /// <param name="numC">El numero de cajones.</param>
        /// <param name="regCub">El numero de registros por cubeta.</param>
        public CuadroDeDatosHash(Entidad e, long pMem, long tamDato, long numC, long regCub)
        {
            ent = e;
            posMemoria = pMem;
            numCajones = numC;
            regPorCubeta = regCub;
            int indLlave = 0;

            foreach (Atributo atr in ent.listaAtributos)
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

            manejo_dataGrid_cajones();
            manejo_dataGrid_cubetas();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text.Length > 0 && textBox2.Text.Length > 0)
            {
                try
                {
                    numCajones = Int64.Parse(textBox1.Text);
                    regPorCubeta = Int64.Parse(textBox2.Text);
                    long cantReg = regPorCubeta;
                    seCambio = true;

                    tamCajon = tamCajon * numCajones;
                    tamCubeta = tamCubeta + (tamCubeta * cantReg);

                    textBox1.Enabled = false;
                    textBox2.Enabled = false;
                    button1.Enabled = false;

                    manejo_dataGrid_cajones();
                }
                catch
                {
                    toolStripStatusLabel1.Text = "Error. Favor de introducir valores validos.";
                }
            }
            else
            {
                toolStripStatusLabel1.Text = "Error. Favor de poner valores en los campos correspondientes.";
            }
        }

        private void manejo_dataGrid_cajones()
        {
            dataGridView1.ColumnCount = (int)numCajones;
            dataGridView1.ColumnHeadersVisible = true;

            for(int i = 0; i < numCajones; i++)
            {
                dataGridView1.Columns[i].Name = (i + 1).ToString();
            }
        }

        private void manejo_dataGrid_cubetas()
        {
            dataGridView2.ColumnCount = (int)regPorCubeta + 1;
            dataGridView2.ColumnHeadersVisible = true;
            dataGridView2.RowCount = (int)numCajones;

            for(int i = 1; i < regPorCubeta + 1; i++)
            {
                dataGridView2.Columns[i].Name = (i + 1).ToString();
            }
        }

        // Boton que cierra la ventana actual
        private void button7_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public long regresa_apuntador_cajones()
        {
            return ent.apCajones;
        }

        public long regresa_posMemoria()
        {
            return posMemoria;
        }

        public long regresa_numCajones()
        {
            return numCajones;
        }

        public long regresa_regPorCubeta()
        {
            return regPorCubeta;
        }

        public bool regresa_seCambio()
        {
            return seCambio;
        }

        public List<Cajon> regresa_lista_cajones()
        {
            return ent.listaCajones;
        }
    }
}
