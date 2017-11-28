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
        public long apMultilistas { get; set; }
        readonly int numAtributos;
        readonly Atributo atrLlave;
        readonly int indiceLlave;
        readonly long tamDato;
        readonly long tamCabecera = 8;
        readonly List<Atributo> atributosVigentes = new List<Atributo>();
        readonly List<Atributo> atributosBusqueda = new List<Atributo>();

        /// <summary>
        /// Constructor de la ventana de manipulacion de datos en multilistas.
        /// </summary>
        /// <param name="e">La entidad con las cabeceras que apuntaran a los datos con multilistas.</param>
        /// <param name="pMem">La posicion actual en memoria.</param>
        /// <param name="tD">El tamaño actual del dato.</param>
        /// <param name="apM">El apuntador a las multilistas de la entidad.</param>
        public CuadroDeDatosMultilistas(Entidad e, long pMem, long tD, long apM)
        {
            ent = e;
            posMemoria = pMem;
            tamDato = tD;
            apMultilistas = apM;
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

            if(apMultilistas != -1)
            {

            }
            else
            {
                inicia_cabeceras();
            }

            InitializeComponent();

            dataGridView2.ReadOnly = true;
            rellenaLavesBusqueda();
            rellena_dataGrid_cabeceras();
            rellena_dataGrid_datos();

            inicia_dataGrid_cabeceras();
        }

        // Boton para insertar un dato.
        private void button4_Click(object sender, EventArgs e)
        {
            int celdaSeleccionada = dataGridView1.CurrentRow.Index;
            bool incompatible = false;
            List<object> datos = new List<object>();

            for(int i = 0; i < dataGridView1.CurrentRow.Cells.Count - atributosBusqueda.Count - 2; i++)
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

            nuevoDato.inicia_apuntadores_busqueda();

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
        /// Metodo que rellenara el dataGridView correspondiente a las cabeceras con la informacion de estas.
        /// </summary>
        private void inicia_dataGrid_cabeceras()
        {
            dataGridView2.Rows.Clear();

            dataGridView2.ColumnCount = atributosVigentes.Count();
            dataGridView2.ColumnHeadersVisible = true;

            string[] fila = new string[atributosVigentes.Count()];
            List<String[]> filas = new List<string[]>();

            for(int i = 0; i < ent.listaCabeceras.Count; i++)
            {
                long apDat = ent.listaCabeceras[i].return_apDatos();
                fila[i] = apDat.ToString();
            }

            filas.Add(fila);

            foreach(String[] s in filas)
            {
                dataGridView2.Rows.Add(s);
            }
        }

        /// <summary>
        /// Metodo que pondra el nombre de los atributos vigentes como los nombres de cada columna en el dataGridView correspondiente
        /// a los datos, ademas de poner las columnas correspondientes a los atributos que sean llave de busqueda.
        /// </summary>
        private void rellena_dataGrid_datos()
        {
            dataGridView1.ColumnCount = atributosVigentes.Count + atributosBusqueda.Count + 2;
            dataGridView1.ColumnHeadersVisible = true;
            int columnCount = 0;

            for(int i = 0; i < atributosVigentes.Count; i++)
            {
                String columnName = new string(atributosVigentes[i].nombre);
                dataGridView1.Columns[i].Name = columnName;
                columnCount++;
            }

            dataGridView1.Columns[columnCount].Name = "Pos. Dato";
            columnCount++;
            dataGridView1.Columns[columnCount].Name = "Ap. Sig. Dato";
            columnCount++;

            for(int j = 0; j < atributosBusqueda.Count; j++)
            {
                String columnName1 = new string(atributosBusqueda[j].nombre);
                String columnName2 = "Ap_" + columnName1;
                dataGridView1.Columns[columnCount].Name = columnName2;
                columnCount++;
            }
        }

        /// <summary>
        /// Metodo que rellenara el dataGridView correspondiente a los datos con la informacion de estos.
        /// </summary>
        private void inicia_dataGrid_datos()
        {
            dataGridView1.Rows.Clear();

            dataGridView1.ColumnCount = atributosVigentes.Count + atributosBusqueda.Count + 2;
            dataGridView1.ColumnHeadersVisible = true;

            string[] fila = new string[atributosVigentes.Count + atributosBusqueda.Count + 2];
            List<String[]> filas = new List<string[]>();
            int count = 0;

            foreach(Dato dat in ent.listaDatos)
            {
                for (int i = 0; i < dat.datos.Count; i++)
                {
                    if (dat.datos[i] is char[])
                    {
                        char[] arr = (char[])dat.datos[i];
                        String objeto = new string(arr);
                        fila[i] = objeto;
                    }
                    else
                    {
                        fila[i] = dat.datos[i].ToString();
                    }
                    count++;
                }

                fila[count] = dat.posDato.ToString();

                filas.Add(fila);
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
        /// Metodo con el cual se actualizaran las cabeceras, unicamente modificandose aquellas que sean llaves de busqueda.
        /// </summary>
        private void actualiza_cabeceras(Dato dato)
        {
            if (ent.listaDatos.Count > 1)
            {
                for(int i = 0; i < ent.listaDatos.Count; i++)
                {
                    
                }
            }
            else
            {
                for (int i = 0; i < atributosVigentes.Count; i++)
                {
                    if (atributosVigentes[i].esLlaveDeBusqueda == true)
                    {
                        ent.listaCabeceras[i].str_apDatos(dato.posDato);
                    }
                }
            }
        }

        /// <summary>
        /// Metodo con el cual se iniciaran las cabeceras para las multilistas.
        /// </summary>
        private void inicia_cabeceras()
        {
            for(int i = 0; i < atributosVigentes.Count; i++)
            {
                Cabecera cab = new Cabecera(-1);

                if(i == 0)
                {
                    cab.str_posCabecera(posMemoria);
                }

                posMemoria += tamCabecera;
                ent.listaCabeceras.Add(cab);
            }
        }

        /// <summary>
        /// Metodo con el cual se realizara la insercion del dato via multilistas.
        /// </summary>
        /// <param name="datoInsertar">El dato que se desea insertar.</param>
        private void insercion_dato(Dato datoInsertar)
        {
            if(valida_dato(datoInsertar) == false)
            {
                if (ent.listaDatos.Count > 0)
                {
                    Dato datoAnterior = new Dato();

                    for (int i = 0; i < ent.listaDatos.Count; i++)
                    {
                        dynamic datoLlavePrim = ent.listaDatos[i].datos[indiceLlave];
                        dynamic datoInsertarLlavePrim = datoInsertar.datos[indiceLlave];

                        if (datoLlavePrim > datoInsertarLlavePrim)
                        {
                            datoInsertar.apSigDato = ent.listaDatos[i].posDato;
                            datoInsertar.posDato = posMemoria;
                            posMemoria += tamDato;
                            datoAnterior.apSigDato = datoInsertar.posDato;
                            ent.listaDatos.Add(datoInsertar);
                        }
                        else
                        {
                            datoAnterior = ent.listaDatos[i];
                        }
                    }
                }
                else
                {
                    datoInsertar.posDato = posMemoria;
                    posMemoria += tamDato;
                    ent.listaDatos.Add(datoInsertar);
                }

                actualiza_cabeceras(datoInsertar);                
            }
            else
            {
                toolStripStatusLabel1.Text = "Error, llave primaria duplicada.";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool busca_llave_busqueda()
        {
            bool encontrada = false;

            return encontrada;
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
