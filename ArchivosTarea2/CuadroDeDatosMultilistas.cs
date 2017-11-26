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
            rellena_dataGrid_cabeceras();
            rellena_dataGrid_datos();
        }

        // Boton para insertar un dato.
        private void button4_Click(object sender, EventArgs e)
        {
            int celdaSeleccionada = dataGridView1.CurrentRow.Index;
            bool incompatible = false;
            List<object> datos = new List<object>();

            for(int i = 0; i < dataGridView1.CurrentRow.Cells.Count - atributosBusqueda.Count; i++)
            {
                if (dataGridView1.CurrentRow.Cells[i].ToString() != "")
                {
                    Atributo atr = atributosVigentes[i];
                    char tipoAtr = atr.tipo;

                    var tipo = valida_atributo(tipoAtr);

                    dynamic strTipo = tipo;
                    Object resultado = null;

                    if (strTipo == typeof(int))
                    {
                        try
                        {
                            resultado = Convert.ChangeType(this.dataGridView1.Rows[celdaSeleccionada].Cells[i].Value, typeof(int));
                            datos.Add(resultado);
                        }
                        catch
                        {
                            MessageBox.Show("Error, tipo de dato incompatible / campo vacio");
                            incompatible = true;
                            return;
                        }
                    }
                    else if (strTipo == typeof(char))
                    {
                        try
                        {
                            resultado = Convert.ChangeType(this.dataGridView1.Rows[celdaSeleccionada].Cells[i].Value, typeof(char));

                            if (resultado.ToString().Length > 1)
                            {
                                MessageBox.Show("Error, tamaño de atributo exedido.");
                                return;
                            }

                            datos.Add(resultado);
                        }
                        catch
                        {
                            MessageBox.Show("Error, tipo de dato incompatible / campo vacio");
                            incompatible = true;
                            return;
                        }
                    }
                    else if (strTipo == typeof(string))
                    {
                        try
                        {
                            resultado = Convert.ChangeType(this.dataGridView1.Rows[celdaSeleccionada].Cells[i].Value, typeof(string));
                            String res = resultado.ToString();

                            if (res.Length > (atr.bytes / 2))
                            {
                                int start = Convert.ToInt32(atr.bytes);
                                start = start / 2;
                                int count = res.Length - start;
                                res = res.Remove(start, count);
                            }

                            datos.Add(res.ToLowerInvariant());
                        }
                        catch
                        {
                            MessageBox.Show("Error, valor de cadena excedida.");
                            incompatible = true;
                            return;
                        }
                    }
                    else if (strTipo == typeof(float))
                    {
                        try
                        {
                            resultado = Convert.ChangeType(this.dataGridView1.Rows[celdaSeleccionada].Cells[i].Value, typeof(float));
                            datos.Add(resultado);
                        }
                        catch
                        {
                            MessageBox.Show("Error, tipo de dato incompatible / campo vacio");
                            incompatible = true;
                            return;
                        }
                    }
                    else if (strTipo == typeof(double))
                    {
                        try
                        {
                            resultado = Convert.ChangeType(this.dataGridView1.Rows[celdaSeleccionada].Cells[i].Value, typeof(double));
                            datos.Add(resultado);
                        }
                        catch
                        {
                            MessageBox.Show("Error, tipo de dato incompatible / campo vacio");
                            incompatible = true;
                            return;
                        }
                    }
                    else if (strTipo == typeof(long))
                    {
                        try
                        {
                            resultado = Convert.ChangeType(this.dataGridView1.Rows[celdaSeleccionada].Cells[i].Value, typeof(long));
                            datos.Add(resultado);
                        }
                        catch
                        {
                            MessageBox.Show("Error, tipo de dato incompatible / campo vacio");
                            incompatible = true;
                            return;
                        }
                    }

                    if (incompatible == true)
                    {
                        return;
                    }
                }
                else
                {
                    toolStripStatusLabel1.Text = "Error, no se pueden dejar campos vacios.";
                    break;
                }
            }

            Dato nuevoDato = new Dato(ent);

            foreach (Object obj in datos)
            {
                nuevoDato.datos.Add(obj);
            }

            insercion_dato(nuevoDato);
        }

        // Boton para modificar un dato.
        private void button1_Click(object sender, EventArgs e)
        {

        }

        // Boton para eliminar un dato.
        private void button2_Click(object sender, EventArgs e)
        {

        }
        
        // Boton para mostrar solo los datos de una llave de busqueda definida por la seleccion hecha en el comboBox.
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

        /// <summary>
        /// Metodo que pondra el nombre de los atributos vigentes como los nombres de cada columna en el dataGridView correspondiente
        /// a las cabeceras.
        /// </summary>
        private void rellena_dataGrid_cabeceras()
        {
            dataGridView2.ColumnCount = atributosVigentes.Count;
            dataGridView2.ColumnHeadersVisible = true;

            for (int i = 0; i < atributosVigentes.Count; i++)
            {
                String columnName = new string(atributosVigentes[i].nombre);
                dataGridView2.Columns[i].Name = columnName;
            }
        }

        /// <summary>
        /// Metodo que pondra el nombre de los atributos vigentes como los nombres de cada columna en el dataGridView correspondiente
        /// a los datos, ademas de poner las columnas correspondientes a los atributos que sean llave de busqueda.
        /// </summary>
        private void rellena_dataGrid_datos()
        {
            dataGridView1.ColumnCount = atributosVigentes.Count + atributosBusqueda.Count;
            dataGridView1.ColumnHeadersVisible = true;
            int columnCount = 0;

            for(int i = 0; i < atributosVigentes.Count; i++)
            {
                String columnName = new string(atributosVigentes[i].nombre);
                dataGridView1.Columns[i].Name = columnName;
                columnCount++;
            }

            for(int j = 0; j < atributosBusqueda.Count; j++)
            {
                String columnName1 = new string(atributosBusqueda[j].nombre);
                String columnName2 = "Ap_" + columnName1;
                dataGridView1.Columns[columnCount].Name = columnName2;
                columnCount++;
            }
        }

        /// <summary>
        /// Metodo que regresa el tipo de dato acorde al valor dado por el atributo.
        /// </summary>
        /// <param name="tatr">El caracter que define el tipo de atributo de cada valor del dato.</param>
        /// <returns>El tipo de dato del valor del dato.</returns>
        private static dynamic valida_atributo(char tatr)
        {
            var tipoAtr = typeof(int);

            switch (tatr)
            {
                case 'I':
                    tipoAtr = typeof(int);
                    break;
                case 'F':
                    tipoAtr = typeof(float);
                    break;
                case 'C':
                    tipoAtr = typeof(char);
                    break;
                case 'S':
                    tipoAtr = typeof(string);
                    break;
                case 'D':
                    tipoAtr = typeof(double);
                    break;
                case 'L':
                    tipoAtr = typeof(long);
                    break;
                default: // Default vacio
                    break;
            }

            return tipoAtr;
        }

        /// <summary>
        /// Metodo con el que se validara que el dato a insertar no tenga llave primaria igual al de un dato ya insertado.
        /// </summary>
        /// <param name="dat">El dato que se desea insertar.</param>
        /// <returns>Booleano que nos dira si el dato tiene llave primaria repetida o no.</returns>
        private bool valida_dato(Dato dat)
        {
            bool repetido = false;

            foreach(Dato data in ent.listaDatos)
            {
                if(data.posDato != -1)
                {
                    if(data != dat)
                    {
                        if(data.datos[indiceLlave].ToString() == dat.datos[indiceLlave].ToString())
                        {
                            repetido = true;
                            break;
                        }
                    }
                }
            }

            return repetido;
        }

        /// <summary>
        /// Metodo con el cual se realizara la insercion del dato via multilistas.
        /// </summary>
        /// <param name="datoInsertar"></param>
        private void insercion_dato(Dato datoInsertar)
        {
            if(valida_dato(datoInsertar) == false)
            {
                ent.listaDatos.Add(datoInsertar);
            }
            else
            {
                toolStripStatusLabel1.Text = "Error, llave primaria duplicada.";
            }
        }

        // Funciones de retorno de informacion.
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
