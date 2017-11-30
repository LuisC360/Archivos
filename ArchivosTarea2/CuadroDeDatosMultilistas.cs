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
    /// Clase que representara el cuadro de dialogo en el que se manipularan los datos mediante el uso de multilistas.
    /// </summary>
    public partial class CuadroDeDatosMultilistas : Form
    {
        /// <summary>
        /// El arreglo de cabeceras.
        /// </summary>
        public long[] cabeceras { get; set; }
        /// <summary>
        /// Booleano que nos avisara si se hicieron cambios en los datos (insercion, eliminacion y modificacion), para de esa forma
        /// saber si se tendra que actualizar el archivo.
        /// </summary>
        public bool seCambio { get; set; }
        /// <summary>
        /// La entidad actual sobre la que se insertaran los datos.
        /// </summary>
        public Entidad ent { get; set; }
        /// <summary>
        /// La posicion actual en memoria.
        /// </summary>
        public long posMemoria { get; set; }
        /// <summary>
        /// El apuntador a multilistas de la entidad.
        /// </summary>
        public long apMultilistas { get; set; }
        /// <summary>
        /// El numero de atributos totales (excluyendo a los que se hayan eliminado).
        /// </summary>
        readonly int numAtributos;
        /// <summary>
        /// El atributo que sera la llave primaria.
        /// </summary>
        readonly Atributo atrLlave;
        /// <summary>
        /// El indice de la llave primaria en la lista de atributos de la entidad.
        /// </summary>
        readonly int indiceLlave;
        /// <summary>
        /// El tamaño en bytes del dato.
        /// </summary>
        readonly long tamDato;
        /// <summary>
        /// El tamaño en bytes de la cabecera.
        /// </summary>
        readonly long tamCabecera = 8;
        /// <summary>
        /// La lista de atributos vigentes de la entidad (los que no se hayan eliminado).
        /// </summary>
        readonly List<Atributo> atributosVigentes = new List<Atributo>();
        /// <summary>
        /// La lista de atributos que seran llaves de busqueda.
        /// </summary>
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

                    if(atr.esLlaveDeBusqueda == true)
                    {
                        atributosBusqueda.Add(atr);
                    }
                }
            }

            foreach(Atributo atr in atributosVigentes)
            {
                tamDato += tamCabecera;
            }

            if(apMultilistas != -1)
            {

            }
            else
            {
                inicia_cabeceras();
            }

            InitializeComponent();

            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            dataGridView2.ReadOnly = true;
            rellenaLavesBusqueda();
            rellena_dataGrid_cabeceras();
            rellena_dataGrid_datos();

            inicia_dataGrid_cabeceras();

            toolStripStatusLabel1.Text = "Listo.";
        }

        /// <summary>
        /// Boton para insertar un dato.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">EventArgs.</param>
        private void button4_Click(object sender, EventArgs e)
        {
            int celdaSeleccionada = dataGridView1.CurrentRow.Index;
            bool incompatible = false;
            List<object> datos = new List<object>();

            for(int i = 0; i < dataGridView1.CurrentRow.Cells.Count - atributosVigentes.Count - 2; i++)
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

        /// <summary>
        /// Boton para modificar un dato.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">EventArgs.</param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
            {
                dynamic llaveBuscar = convierte_dato_a_encontrar(textBox1.Text);
                Dato datoModificar = new Dato();
                bool datoEncontrado = false;
                List<Dato> datosOrdenados = ent.listaDatos.OrderBy(o => o.datos[indiceLlave]).ToList();

                foreach (Dato dat in datosOrdenados)
                {
                    if (dat.apSigDato != -2 && dat.apSigDato != -4)
                    {
                        dynamic llaveComparar = dat.datos[indiceLlave];

                        if (llaveComparar == llaveBuscar)
                        {
                            datoModificar = dat;
                            datoEncontrado = true;
                            break;
                        }
                    }                  
                }

                if (datoEncontrado == true)
                {

                }
                else
                {
                    toolStripStatusLabel1.Text = "Error, llave primaria no encontrada.";
                }
            }
            else
            {
                toolStripStatusLabel1.Text = "Error, escribe una llave primaria.";
            }
        }

        /// <summary>
        /// Boton para eliminar un dato.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">EventArgs.</param>
        private void button2_Click(object sender, EventArgs e)
        {
            if(textBox1.Text.Length > 0)
            {
                dynamic llaveBuscar = convierte_dato_a_encontrar(textBox1.Text);
                Dato datoEliminar = new Dato();
                Dato datoAnterior = new Dato();
                bool datoEncontrado = false;
                int contadorDatos = 0;
                List<Dato> datosOrdenados = ent.listaDatos.OrderBy(o => o.datos[indiceLlave]).ToList();

                foreach(Dato dat in datosOrdenados)
                {
                    if (dat.apSigDato != -2 && dat.apSigDato != -4)
                    {
                        dynamic llaveComparar = dat.datos[indiceLlave];

                        if (llaveComparar == llaveBuscar)
                        {
                            datoEliminar = dat;
                            datoEncontrado = true;
                            break;
                        }
                        else
                        {
                            datoAnterior = dat;
                            contadorDatos++;
                        }
                    }
                }

                if(datoEncontrado == true)
                {
                    // Si era el ultimo dato
                    if(contadorDatos == datosOrdenados.Count - 1)
                    {
                        datoEliminar.apSigDato = -4;
                        datoAnterior.apSigDato = -3;                       
                    }
                    // Si era el primer dato
                    else if(contadorDatos == 0 && datoAnterior.posDato == 0)
                    {
                        datoEliminar.apSigDato = -2;
                    }
                    // Si estaba entre 2 datos
                    else if(datoAnterior.posDato != 0)
                    {
                        datoEliminar.apSigDato = -2;

                        int indiceAnterior = datosOrdenados.IndexOf(datoAnterior);
                        bool dEncontrado = false;

                        for(int i = indiceAnterior + 2; i < datosOrdenados.Count; i++)
                        {
                            if(datosOrdenados[i].apSigDato != -2 && datosOrdenados[i].apSigDato != -4)
                            {
                                datoAnterior.apSigDato = datosOrdenados[i].posDato;
                                dEncontrado = true;
                                break;
                            }
                        }

                        if(dEncontrado == false)
                        {
                            datoAnterior.apSigDato = -3;
                        }
                    }

                    // Actualizar las llaves de busqueda
                    actualizacion_cabeceras_eliminacion();
                    inicia_dataGrid_cabeceras();
                    inicia_dataGrid_datos();
                    toolStripStatusLabel1.Text = "Dato eliminado con exito.";
                    seCambio = true;
                }
                else
                {
                    toolStripStatusLabel1.Text = "Error, llave primaria no encontrada."; 
                }
            }
            else
            {
                toolStripStatusLabel1.Text = "Error, escribe una llave primaria.";
            }
        }

        /// <summary>
        /// Boton para mostrar solo los datos de una llave de busqueda definida por la seleccion hecha en el comboBox.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">EventArgs.</param>
        private void button3_Click(object sender, EventArgs e)
        {
            if(textBox2.Text.Length > 0)
            {
                int selectedIndex = comboBox1.SelectedIndex;

                if(selectedIndex != -1)
                {
                    if(ent.listaAtributos[selectedIndex].esLlaveDeBusqueda == false && ent.listaAtributos[selectedIndex].esLlavePrimaria == true)
                    {
                        dynamic llaveBuscar = convierte_dato_a_encontrar(textBox2.Text);
                        Dato dat = new Dato();
                        List<Dato> datosOrdenados = ent.listaDatos.OrderBy(o => o.datos[indiceLlave]).ToList();
                        bool encontrado = false;

                        foreach (Dato dato in datosOrdenados)
                        {
                            if (dato.apSigDato != -2 && dato.apSigDato != -4)
                            {
                                dynamic llaveComparar = dato.datos[indiceLlave];

                                if (llaveComparar == llaveBuscar)
                                {
                                    encontrado = true;
                                    dat = dato;
                                    break;
                                }
                            }
                        }

                        if(encontrado == true)
                        {
                            dataGridView1.Rows.Clear();

                            dataGridView1.ColumnCount = atributosVigentes.Count + atributosVigentes.Count + 2;
                            dataGridView1.ColumnHeadersVisible = true;

                            string[] fila = new string[atributosVigentes.Count + atributosVigentes.Count + 2];
                            List<String[]> filas = new List<string[]>();
                            int count = 0;

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
                            count++;

                            if (dat.apSigDato == -3)
                            {
                                String menos = "-1";
                                fila[count] = menos;
                            }
                            else
                            {
                                fila[count] = dat.apSigDato.ToString();
                            }
                            count++;

                            for (int j = 0; j < dat.apuntadoresLlaveBusq.Count; j++)
                            {
                                fila[count] = dat.apuntadoresLlaveBusq[j].ToString();
                                count++;
                            }

                            filas.Add(fila);

                            foreach (string[] arr in filas)
                            {
                                dataGridView1.Rows.Add(arr);
                            }

                            toolStripStatusLabel1.Text = "Listo.";
                        }
                        else
                        {
                            toolStripStatusLabel1.Text = "Error, llave primaria no encontrada.";
                        }
                    }
                    else if(ent.listaAtributos[selectedIndex].esLlaveDeBusqueda == true)
                    {
                        dynamic llaveBuscar = convierte_dato_busqueda(textBox2.Text, ent.listaAtributos[selectedIndex]);
                        List<Dato> datosOrdenados = ent.listaDatos.OrderBy(o => o.datos[indiceLlave]).ToList();
                        List<Dato> datosEncontrados = new List<Dato>();

                        foreach (Dato dato in datosOrdenados)
                        {
                            if (dato.apSigDato != -2 && dato.apSigDato != -4)
                            {
                                dynamic llaveComparar = dato.datos[selectedIndex];

                                if (llaveComparar == llaveBuscar)
                                {
                                    datosEncontrados.Add(dato);
                                }
                            }
                        }

                        if(datosEncontrados.Count > 0)
                        {
                            dataGridView1.Rows.Clear();

                            dataGridView1.ColumnCount = atributosVigentes.Count + atributosVigentes.Count + 2;
                            dataGridView1.ColumnHeadersVisible = true;

                            string[] fila = new string[atributosVigentes.Count + atributosVigentes.Count + 2];
                            List<String[]> filas = new List<string[]>();
                            int count = 0;

                            foreach(Dato dat in datosEncontrados)
                            {
                                if (dat.apSigDato != -2 && dat.apSigDato != -4)
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
                                    count++;

                                    if (dat.apSigDato == -3)
                                    {
                                        String menos = "-1";
                                        fila[count] = menos;
                                    }
                                    else
                                    {
                                        fila[count] = dat.apSigDato.ToString();
                                    }
                                    count++;

                                    for (int j = 0; j < dat.apuntadoresLlaveBusq.Count; j++)
                                    {
                                        fila[count] = dat.apuntadoresLlaveBusq[j].ToString();
                                        count++;
                                    }

                                    filas.Add(fila);
                                    fila = new string[atributosVigentes.Count + atributosVigentes.Count + 2];
                                    count = 0;
                                }
                            }

                            foreach (string[] arr in filas)
                            {
                                dataGridView1.Rows.Add(arr);
                            }

                            toolStripStatusLabel1.Text = "Listo.";
                        }
                        else
                        {
                            toolStripStatusLabel1.Text = "Error, no hay ningun dato con este valor de llave de busqueda.";
                        }
                    }
                }
                else
                {
                    toolStripStatusLabel1.Text = "Error, selecciona una llave primaria o llave de busqueda.";
                }
            }
            else
            {
                toolStripStatusLabel1.Text = "Error, introduce una llave primaria o llave de busqueda.";
            }
        }

        /// <summary>
        /// Boton con el cual se mostraran todos los datos en el dataGridView correspondiente.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">EventArgs.</param>
        private void button6_Click(object sender, EventArgs e)
        {
            inicia_dataGrid_datos();
        }

        /// <summary>
        /// Boton con el cual se mostraran los datos del dataGridView correspondiente en base a un orden especificado por el
        /// comboBox correspondiente.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">EventArgs.</param>
        private void button7_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboBox2.SelectedIndex;

            if(selectedIndex > -1)
            {
                dataGridView1.Rows.Clear();

                dataGridView1.ColumnCount = atributosVigentes.Count + atributosVigentes.Count + 2;
                dataGridView1.ColumnHeadersVisible = true;

                string[] fila = new string[atributosVigentes.Count + atributosVigentes.Count + 2];
                List<String[]> filas = new List<string[]>();
                int count = 0;

                List<Dato> datosOrdenados = ent.listaDatos.OrderBy(o => o.datos[selectedIndex]).ToList();

                foreach(Dato dat in datosOrdenados)
                {
                    if (dat.apSigDato != -2 && dat.apSigDato != -4)
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
                        count++;

                        if (dat.apSigDato == -3)
                        {
                            String menos = "-1";
                            fila[count] = menos;
                        }
                        else
                        {
                            fila[count] = dat.apSigDato.ToString();
                        }
                        count++;

                        for (int j = 0; j < dat.apuntadoresLlaveBusq.Count; j++)
                        {
                            fila[count] = dat.apuntadoresLlaveBusq[j].ToString();
                            count++;
                        }

                        filas.Add(fila);
                        fila = new string[atributosVigentes.Count + atributosVigentes.Count + 2];
                        count = 0;
                    }
                }

                foreach (string[] arr in filas)
                {
                    dataGridView1.Rows.Add(arr);
                }
            }
        }

        /// <summary>
        /// Boton para cerrar el cuadro de manipulacion de datos.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">EventArgs.</param>
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
                String ll = new string(atr.nombre);

                comboBox1.Items.Add(ll);
                comboBox2.Items.Add(ll);
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
        /// Metodo que pondra los nombres de los atributos vigentes como los nombres de cada columna en el dataGridView correspondiente
        /// a los datos, ademas de poner las columnas correspondientes a los atributos que sean llave de busqueda.
        /// </summary>
        private void rellena_dataGrid_datos()
        {
            dataGridView1.ColumnCount = atributosVigentes.Count + atributosVigentes.Count + 2;
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

            for(int j = 0; j < atributosVigentes.Count; j++)
            {
                String columnName1 = new string(atributosVigentes[j].nombre);
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

            dataGridView1.ColumnCount = atributosVigentes.Count + atributosVigentes.Count + 2;
            dataGridView1.ColumnHeadersVisible = true;

            string[] fila = new string[atributosVigentes.Count + atributosVigentes.Count + 2];
            List<String[]> filas = new List<string[]>();
            int count = 0;

            List<Dato> datosOrdenados = ent.listaDatos.OrderBy(o => o.datos[indiceLlave]).ToList();

            foreach (Dato dat in datosOrdenados)
            {
                if (dat.apSigDato != -2 && dat.apSigDato != -4)
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
                    count++;

                    if (dat.apSigDato == -3)
                    {
                        String menos = "-1";
                        fila[count] = menos;
                    }
                    else
                    {
                        fila[count] = dat.apSigDato.ToString();
                    }
                    count++;

                    for (int j = 0; j < dat.apuntadoresLlaveBusq.Count; j++)
                    {
                        fila[count] = dat.apuntadoresLlaveBusq[j].ToString();
                        count++;
                    }

                    filas.Add(fila);
                    fila = new string[atributosVigentes.Count + atributosVigentes.Count + 2];
                    count = 0;
                }
            }

            foreach (string[] arr in filas)
            {
                dataGridView1.Rows.Add(arr);
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
                if (data.apSigDato != -2 && data.apSigDato != -4)
                {
                    if (data.posDato != -1)
                    {
                        if (data != dat)
                        {
                            if (data.datos[indiceLlave].ToString() == dat.datos[indiceLlave].ToString())
                            {
                                repetido = true;
                                break;
                            }
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
                Dato datoComparar = new Dato();
                Dato datoAnterior = new Dato();
                bool encontrado = false;

                // Se tiene que recorrer la lista de atributos para averiguar cuales son llaves de busqueda.
                for (int j = 0; j < ent.listaAtributos.Count; j++)
                {
                    if (ent.listaAtributos[j].esLlaveDeBusqueda == true)
                    {
                        List<Dato> datosOrdenados = ent.listaDatos.OrderBy(o => o.datos[j]).ToList();

                        // Se debe de recorrer la lista de cabeceras para saber en donde comenzaremos a recorrer la lista
                        // de datos.
                        long apCabecera = ent.listaCabeceras[j].return_apDatos();

                        do
                        {
                            if (datoAnterior.posDato == 0)
                            {
                                foreach (Dato dat in datosOrdenados)
                                {
                                    if (dat.apSigDato != -2 && dat.apSigDato != -4)
                                    {
                                        long posDato = dat.posDato;

                                        // Se tiene que comenzar a recorrer a partir de ese dato
                                        if (posDato == apCabecera)
                                        {
                                            datoComparar = dat;
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                bool enc = false;

                                foreach(Dato dat in datosOrdenados)
                                {
                                    if (dat.apSigDato != -2 && dat.apSigDato != -4)
                                    {
                                        long posDato = dat.posDato;

                                        if (posDato == datoAnterior.apuntadoresLlaveBusq[j])
                                        {
                                            datoComparar = dat;
                                            enc = true;
                                            break;
                                        }
                                    }
                                }

                                if(enc == false)
                                {
                                    // Si sera el ultimo dato
                                    if (ent.listaAtributos[j].tipo == 'S')
                                    {
                                        datoAnterior.apuntadoresLlaveBusq[j] = dato.posDato;
                                        encontrado = true;
                                    }
                                    else
                                    {
                                        datoAnterior.apuntadoresLlaveBusq[j] = dato.posDato;
                                        encontrado = true;
                                    }
                                }
                            }

                            if (encontrado == false)
                            {
                                dynamic valorDato = datoComparar.datos[j];
                                dynamic valorNuevoDato = dato.datos[j];

                                if (ent.listaAtributos[j].tipo != 'S')
                                {
                                    if (valorDato > valorNuevoDato)
                                    {
                                        // Si ira entre dos datos
                                        if (datoAnterior.posDato != 0)
                                        {
                                            dato.apuntadoresLlaveBusq[j] = datoComparar.posDato;
                                            datoAnterior.apuntadoresLlaveBusq[j] = dato.posDato;
                                        }
                                        else
                                        {
                                            ent.listaCabeceras[j].str_apDatos(dato.posDato);
                                            dato.apuntadoresLlaveBusq[j] = datoComparar.posDato;
                                        }

                                        encontrado = true;
                                    }
                                    else
                                    {
                                        datoAnterior = datoComparar;
                                    }
                                }
                                else
                                {
                                    if (string.Compare(valorDato, valorNuevoDato) > 0)
                                    {
                                        // Si ira entre dos datos
                                        if (datoAnterior.posDato != 0)
                                        {
                                            dato.apuntadoresLlaveBusq[j] = datoComparar.posDato;
                                            datoAnterior.apuntadoresLlaveBusq[j] = dato.posDato;
                                        }
                                        else
                                        {
                                            ent.listaCabeceras[j].str_apDatos(dato.posDato);
                                            dato.apuntadoresLlaveBusq[j] = datoComparar.posDato;
                                        }

                                        encontrado = true;
                                    }
                                    else
                                    {
                                        datoAnterior = datoComparar;
                                    }
                                }
                            }
                        } while (encontrado == false);

                        datoComparar = new Dato();
                        datoAnterior = new Dato();
                        encontrado = false;
                    }
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
        /// Metodo con el cual se actualizaran los apuntadores de los datos y las cabeceras despues de haber hecho una 
        /// eliminacion de un dato.
        /// </summary>
        private void actualizacion_cabeceras_eliminacion()
        {
            for (int j = 0; j < atributosVigentes.Count; j++)
            {
                List<Dato> datosOrdenados = ent.listaDatos.OrderBy(o => o.datos[j]).ToList();

                // Si el atributo no es llave de busqueda pero es llave primaria.
                if (atributosVigentes[j].esLlaveDeBusqueda == false && atributosVigentes[j].esLlavePrimaria == true)
                {
                    for (int k = 0; k < datosOrdenados.Count; k++)
                    {
                        if (datosOrdenados[k].apSigDato != -2 && datosOrdenados[k].apSigDato != -4)
                        {
                            // Si el dato que le sigue al actual es el eliminado y hay un dato despues
                            if ((k + 1) < datosOrdenados.Count && (datosOrdenados[k + 1].apSigDato == -2 || datosOrdenados[k + 1].apSigDato == -4)
                                && (k + 2) < datosOrdenados.Count)
                            {
                                // Se tiene que buscar un dato que ahora estara enlazado al dato actual en cuando a esa llave de busqueda
                                bool encontrado = false;

                                for (int l = k + 2; l < datosOrdenados.Count; l++)
                                {
                                    if (datosOrdenados[l].apSigDato != -2 && datosOrdenados[l].apSigDato != -4)
                                    {
                                        datosOrdenados[k].apSigDato = datosOrdenados[l].posDato;
                                        encontrado = true;
                                        break;
                                    }
                                }

                                if (encontrado == false)
                                {
                                    datosOrdenados[k].apSigDato = -3;
                                }
                            }
                            // Si el dato que le sigue al actual es el eliminado y no hay un dato despues
                            else if((k + 1) < datosOrdenados.Count && (datosOrdenados[k + 1].apSigDato == -2 || datosOrdenados[k + 1].apSigDato == -4)
                                && (k + 2) == datosOrdenados.Count)
                            {
                                datosOrdenados[k].apSigDato = -3;
                            }
                        }
                    }
                }
                else if(atributosVigentes[j].esLlaveDeBusqueda == true)
                {
                    for (int k = 0; k < datosOrdenados.Count; k++)
                    {
                        // Si se habia borrado el dato de la cabecera
                        if(k == 0 && (datosOrdenados[k].apSigDato == -2 || datosOrdenados[k].apSigDato == -4))
                        {
                            bool encontrado = false;

                            // Se debe de buscar un dato que sera ahora la nueva cabecera
                            for(int l = k + 1; l < datosOrdenados.Count; l++)
                            {
                                if(datosOrdenados[l].apSigDato != -2 && datosOrdenados[l].apSigDato != -4)
                                {
                                    ent.listaCabeceras[j].str_apDatos(datosOrdenados[l].posDato);
                                    encontrado = true;
                                    break;
                                }
                            }

                            if(encontrado == false)
                            {
                                ent.listaCabeceras[j].str_apDatos(-1);
                            }
                        }
                        else if((k+1) < datosOrdenados.Count && (datosOrdenados[k+1].apSigDato == -2 || datosOrdenados[k+1].apSigDato == -4) 
                            && (k+2) < datosOrdenados.Count)
                        {
                            bool encontrado = false;

                            for(int l = k + 2; l < datosOrdenados.Count; l++)
                            {
                                if(datosOrdenados[l].apSigDato != -2 && datosOrdenados[l].apSigDato != -4)
                                {
                                    datosOrdenados[k].apuntadoresLlaveBusq[j] = datosOrdenados[l].posDato;
                                    encontrado = true;
                                    break;
                                }
                            }

                            if(encontrado == false)
                            {
                                datosOrdenados[k].apuntadoresLlaveBusq[j] = -1;
                            }
                        }
                        else if((k + 1) < datosOrdenados.Count && (datosOrdenados[k + 1].apSigDato == -2 || datosOrdenados[k + 1].apSigDato == -4)
                            && (k + 2) == datosOrdenados.Count)
                        {
                            datosOrdenados[k].apuntadoresLlaveBusq[j] = -1;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Metodo con el cual se actualizaran los apuntadores de los datos y las cabeceras despues de haber hecho una 
        /// modificacion de un dato.
        /// </summary>
        private void actualizacion_cabeceras_modificacion()
        {

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
            if (valida_dato(datoInsertar) == false)
            {
                if (ent.listaDatos.Count > 0)
                {
                    List<Dato> datosOrdenados = ent.listaDatos.OrderBy(o => o.datos[indiceLlave]).ToList();
                    bool encontrado = false;
                    Dato datoAnterior = new Dato();

                    for (int i = 0; i < datosOrdenados.Count; i++)
                    {
                        dynamic datoLlavePrim = datosOrdenados[i].datos[indiceLlave];
                        dynamic datoInsertarLlavePrim = datoInsertar.datos[indiceLlave];

                        if (datoLlavePrim > datoInsertarLlavePrim)
                        {
                            datoInsertar.apSigDato = datosOrdenados[i].posDato;
                            datoInsertar.posDato = posMemoria;
                            posMemoria += tamDato;
                            datoAnterior.apSigDato = datoInsertar.posDato;
                            ent.listaDatos.Add(datoInsertar);
                            encontrado = true;
                            break;
                        }
                        else
                        {
                            datoAnterior = datosOrdenados[i];
                        }
                    }

                    if(encontrado == false)
                    {
                        datoInsertar.posDato = posMemoria;
                        posMemoria += tamDato;
                        datoAnterior.apSigDato = datoInsertar.posDato;
                        ent.listaDatos.Add(datoInsertar);
                    }
                }
                else
                {
                    datoInsertar.posDato = posMemoria;
                    posMemoria += tamDato;
                    ent.listaDatos.Add(datoInsertar);
                }

                actualiza_cabeceras(datoInsertar);
                inicia_dataGrid_cabeceras();
                inicia_dataGrid_datos();
                seCambio = true;
                toolStripStatusLabel1.Text = "Dato insertado con exito.";
            }
            else
            {
                toolStripStatusLabel1.Text = "Error, llave primaria duplicada.";
            }
        }

        /// <summary>
        /// Metodo que convierte el string en el textBox al valor de llave primaria que se busca eliminar o modificar.
        /// </summary>
        /// <param name="dato">La cadena a convertir a un valor de llave primaria. Si es una cadena, no se tendra que hacer
        /// conversion.</param>
        /// <returns>La cadena ya convertida en el valor correspondiente a una llave primaria junto con el tipo de dato de
        /// esa llave.</returns>
        private dynamic convierte_dato_a_encontrar(String dato)
        {
            dynamic datoEnc = 0;

            switch (atrLlave.tipo)
            {
                case 'I':
                    try
                    {
                        datoEnc = Convert.ToInt32(dato);
                    }
                    catch
                    {
                        toolStripStatusLabel1.Text = "Error en la conversion.";
                    }
                    break;
                case 'L':
                    try
                    {
                        datoEnc = Convert.ToInt64(dato);
                    }
                    catch
                    {
                        toolStripStatusLabel1.Text = "Error en la conversion.";
                    }
                    break;
                case 'F':
                    try
                    {
                        datoEnc = Convert.ToSingle(dato);
                    }
                    catch
                    {
                        toolStripStatusLabel1.Text = "Error en la conversion.";
                    }
                    break;
                case 'D':
                    try
                    {
                        datoEnc = Convert.ToDouble(dato);
                    }
                    catch
                    {
                        toolStripStatusLabel1.Text = "Error en la conversion.";
                    }
                    break;
                case 'C':
                    try
                    {
                        datoEnc = Convert.ToChar(dato);
                    }
                    catch
                    {
                        toolStripStatusLabel1.Text = "Error en la conversion.";
                    }
                    break;
                default: datoEnc = dato;
                    break;
            }

            return datoEnc;
        }

        /// <summary>
        /// Metodo con el que se convierte una cadena a un valor de llave de busqueda seleccionado en el comboBox de seleccion
        /// de llave de busqueda.
        /// </summary>
        /// <param name="dato">La cadena que se desea convertir.</param>
        /// <param name="atr">El atributo que es llave de busqueda y cuyo tipo influira en la conversion de la cadena.</param>
        /// <returns>La cadena ya convertida en el valor correspondiente a una llave primaria junto con el tipo de dato de
        /// esa llave.</returns>
        private dynamic convierte_dato_busqueda(String dato, Atributo atr)
        {
            dynamic datoEnc = 0;

            switch (atr.tipo)
            {
                case 'I':
                    try
                    {
                        datoEnc = Convert.ToInt32(dato);
                    }
                    catch
                    {
                        toolStripStatusLabel1.Text = "Error en la conversion.";
                    }
                    break;
                case 'L':
                    try
                    {
                        datoEnc = Convert.ToInt64(dato);
                    }
                    catch
                    {
                        toolStripStatusLabel1.Text = "Error en la conversion.";
                    }
                    break;
                case 'F':
                    try
                    {
                        datoEnc = Convert.ToSingle(dato);
                    }
                    catch
                    {
                        toolStripStatusLabel1.Text = "Error en la conversion.";
                    }
                    break;
                case 'D':
                    try
                    {
                        datoEnc = Convert.ToDouble(dato);
                    }
                    catch
                    {
                        toolStripStatusLabel1.Text = "Error en la conversion.";
                    }
                    break;
                case 'C':
                    try
                    {
                        datoEnc = Convert.ToChar(dato);
                    }
                    catch
                    {
                        toolStripStatusLabel1.Text = "Error en la conversion.";
                    }
                    break;
                default: datoEnc = dato;
                    break;
            }

            return datoEnc;
        }

        // Funciones de retorno de informacion.
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

        public long regresa_ap_cabeceras()
        {
            return ent.listaCabeceras[0].return_posCabecera();
        }

        public List<Dato> regresa_lista_datos()
        {
            return ent.listaDatos;
        }       
    }
}
