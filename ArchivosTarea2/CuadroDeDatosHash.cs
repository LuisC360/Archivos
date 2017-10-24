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

            if (numCajones > 0)
            {
                manejo_dataGrid_cajones();
                rellena_dataGrid_cajones();
                inicia_dataGrid_datos();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text.Length > 0 && textBox2.Text.Length > 0)
            {
                try
                {
                    numCajones = Int64.Parse(textBox1.Text);
                    regPorCubeta = Int64.Parse(textBox2.Text);
                    ent.apCajones = posMemoria;
                    long cantReg = regPorCubeta;
                    seCambio = true;

                    for(int i = 0; i < numCajones; i++)
                    {
                        Cajon nuevoC = new Cajon();

                        ent.listaCajones.Add(nuevoC);
                    }

                    tamCajon = tamCajon * numCajones;
                    tamCubeta = tamCubeta + (tamCubeta * cantReg);
                    posMemoria += tamCajon;

                    textBox1.Enabled = false;
                    textBox2.Enabled = false;
                    button1.Enabled = false;

                    manejo_dataGrid_cajones();
                    rellena_dataGrid_cajones();
                    inicia_dataGrid_datos();
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

        private void rellena_dataGrid_cajones()
        {
            dataGridView1.ColumnCount = (int)numCajones;
            dataGridView1.ColumnHeadersVisible = true;

            string[] fila = new string[(int)numCajones];
            List<String[]> filas = new List<string[]>();
            int j = 0;

            for(int i = 0; i < ent.listaCajones.Count; i++)
            {
                long apCub = ent.listaCajones[i].regresa_apuntadorCubeta();
                fila[i] = apCub.ToString();
            }

            filas.Add(fila);

            foreach (string[] f in filas)
            {
                dataGridView1.Rows.Add(f);
            }
        }

        private void manejo_dataGrid_cubetas()
        {
            dataGridView2.ColumnCount = (int)regPorCubeta + 1;
            dataGridView2.ColumnHeadersVisible = true;

            if (numCajones > 0)
            {
                dataGridView2.RowCount = (int)numCajones;

                for (int i = 1; i < regPorCubeta + 1; i++)
                {
                    dataGridView2.Columns[i].Name = (i + 1).ToString();
                }
            }           
        }

        private void inicia_dataGrid_datos()
        {
            dataGridView3.ColumnCount = numAtributos + 2;
            dataGridView3.ColumnHeadersVisible = true;
            int j = 0;

            for (int i = 0; i < ent.listaAtributos.Count; i++)
            {
                if (ent.listaAtributos[i].apSigAtributo != -2 && ent.listaAtributos[i].apSigAtributo != -4)
                {
                    char[] nombre = new char[30];

                    for (int k = 0; k < ent.listaAtributos[i].nombre.Length; k++)
                    {
                        nombre[k] = ent.listaAtributos[i].nombre[k];
                    }

                    string nombreAtributo = new string(nombre);

                    dataGridView3.Columns[j].Name = nombreAtributo;
                    j++;
                }
            }

            dataGridView3.Columns[j].Name = "Pos. Dato.";
            j++;
            dataGridView3.Columns[j].Name = "Ap. Sig. Dato";
        }

        // Boton para insertar datos a un registro dentro de una cubeta. El criterio que decidira en que numero de cubeta se insertara sera
        // mediante el uso de la funcion hash "centro de cuadrados", a la que despues se le aplicara la funcion hash "modulo 10". Al
        // resultado de cada funcion hash se le debe de sumar 1.
        private void button6_Click(object sender, EventArgs e)
        {
            int celdaSeleccionada = dataGridView2.CurrentRow.Index;
            bool incompatible = false;
            List<object> datos = new List<object>();
        }



        // Boton que cierra la ventana actual.
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
