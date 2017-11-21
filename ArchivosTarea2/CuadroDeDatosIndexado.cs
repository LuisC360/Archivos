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
        readonly Atributo atrLlave;
        readonly List<Atributo> atributosVigentes = new List<Atributo>();
        public long posMemoria { get; set; }
        long tamDato;
        public long rango { get; set; }
        long tamIndice = 40;
        public bool bandChanged { get; set; }
        readonly List<Indice> indicesVigentes = new List<Indice>();
        readonly int indiceLlave;

        /// <summary>
        /// Constructor del cuadro de insercion, modificacion y eliminacion de datos indexados.
        /// </summary>
        /// <param name="e">La entidad en la que se insertaran los indices y los datos.</param>
        /// <param name="pMem">La posicion actual de memoria.</param>
        /// <param name="tamDat">El tamaño actual del dato.</param>
        /// <param name="rang">El rango definido para los indices.</param>
        public CuadroDeDatosIndexado(Entidad e, long pMem, long tamDat, long rang)
        {
            ent = e;
            posMemoria = pMem;
            tamDato = tamDat;
            rango = rang;
            int indLlave = 0;

            this.Location = new Point(100, 100);

            foreach (Atributo atr in ent.listaAtributos)
            {
                if (atr.apSigAtributo != -2 && atr.apSigAtributo != -4)
                {
                    numAtributos++;
                    atributosVigentes.Add(atr);

                    if(atr.esLlavePrimaria == true)
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

            foreach (Indice ind in ent.listaIndices)
            {
                long apSig = ind.regresa_apSigIndice();

                if (apSig != -2 && apSig != -4)
                {
                    numIndices++;
                    indicesVigentes.Add(ind);
                }
            }

            if(ent.listaIndices.Count > 0)
            {
                posiciones_datos();
            }

            InitializeComponent();

            inicia_dataGridIndices();

            inicia_dataGridDatos();

            rellena_dataGridIndices();
             
            if(rango > 0)
            {
                textBox1.Text = rango.ToString();
                textBox1.Enabled = false;
                button1.Enabled = false;
            }

            dataGridView1.ReadOnly = true;
        }

        private void inicia_dataGridIndices()
        {
            dataGridView1.ColumnCount = 5;
            dataGridView1.ColumnHeadersVisible = true;

            dataGridView1.Columns[0].Name = "Val. inicial";
            dataGridView1.Columns[1].Name = "Val. final";
            dataGridView1.Columns[2].Name = "Pos. indice";
            dataGridView1.Columns[3].Name = "Ap. Sig. Indice";
            dataGridView1.Columns[4].Name = "Ap. Datos";
        }

        private void inicia_dataGridDatos()
        {
            dataGridView2.ColumnCount = numAtributos + 2;
            dataGridView2.ColumnHeadersVisible = true;
            int j = 0;

            for(int i = 0; i < ent.listaAtributos.Count; i++)
            {
                if(ent.listaAtributos[i].apSigAtributo != -2 && ent.listaAtributos[i].apSigAtributo != -4)
                {
                    char[] nombre = new char[30];

                    for (int k = 0; k < ent.listaAtributos[i].nombre.Length; k++)
                    {
                        nombre[k] = ent.listaAtributos[i].nombre[k];
                    }

                    string nombreAtributo = new string(nombre);

                    dataGridView2.Columns[j].Name = nombreAtributo;
                    j++;
                }
            }

            dataGridView2.Columns[j].Name = "Pos. Dato.";
            j++;
            dataGridView2.Columns[j].Name = "Ap. Sig. Dato";
        }

        /// <summary>
        /// Metodo con el que se rellena el dataGridView de los indices con su informacion correspondiente.
        /// </summary>
        private void rellena_dataGridIndices()
        {
            dataGridView1.Rows.Clear();

            dataGridView1.ColumnCount = 5;
            dataGridView1.ColumnHeadersVisible = true;

            string[] fila = new string[5];
            List<String[]> filas = new List<string[]>();
            int j = 0;

            for (int i = 0; i < ent.listaIndices.Count; i++)
            {
                long apSig = ent.listaIndices[i].regresa_apSigIndice();

                // Si el indice no fue eliminado
                if (apSig != -2 && apSig != -4)
                {
                    fila[j] = ent.listaIndices[i].regresa_valInicial().ToString();
                    j++;
                    fila[j] = ent.listaIndices[i].regresa_valFinal().ToString();
                    j++;
                    fila[j] = ent.listaIndices[i].regresa_posIndice().ToString();
                    j++;
                    fila[j] = ent.listaIndices[i].regresa_apSigIndice().ToString();
                    j++;
                    fila[j] = ent.listaIndices[i].regresa_apDatos().ToString();

                    j = 0;
                    filas.Add(fila);
                    fila = new string[5];
                }
            }

            foreach(string[] f in filas)
            {
                dataGridView1.Rows.Add(f);
            }
        }

        /// <summary>
        /// Metodo que pone las posiciones en memoria de los datos almacenados en los indices.
        /// </summary>
        private void posiciones_datos()
        {
            foreach(Indice ind in ent.listaIndices)
            {
                Dato anterior = new Dato();

                for(int i = 0; i < ind.datosIndice.Count; i++)
                {
                    if(i == 0 && ind.datosIndice[i].apSigDato != -3 && ind.datosIndice[i].apSigDato != -4)
                    {
                        long apD = ind.regresa_apDatos();
                        ind.datosIndice[i].posDato = apD;
                        anterior = ind.datosIndice[i];
                    }
                    else if(anterior.apSigDato == -1 && i > 0 && ind.datosIndice[i].apSigDato != -3 && ind.datosIndice[i].apSigDato != -4)
                    {
                        ind.datosIndice[i].posDato = ind.regresa_apDatos();
                        anterior = ind.datosIndice[i];
                    }
                    else if(ind.datosIndice[i].apSigDato != -3 && ind.datosIndice[i].apSigDato != -4)
                    {
                        ind.datosIndice[i].posDato = anterior.apSigDato;
                        anterior = ind.datosIndice[i];
                    }
                }
            }
        }

        // Boton que define el rango, dependiendo de que tipo de dato sea la llave primaria.
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0 && rango == 0)
            {
                try
                {
                    rango = long.Parse(textBox1.Text);
                    textBox1.ReadOnly = true;
                    button1.Enabled = false;
                    bandChanged = true;
                }
                catch
                {
                    toolStripStatusLabel1.Text = "Error, introduzca un valor de rango valido.";
                }
            }
            else
            {
                toolStripStatusLabel1.Text = "Error, introduzca un valor de rango.";
            }
        }

        // Boton con el que se insertara un dato. Primero se capturará el dato, despues se buscara el indice que le corresponda, y si no
        // existe dicho indice, se va a crear, para despues insertar el dato en el indice.
        private void button5_Click(object sender, EventArgs e)
        {
            int celdaSeleccionada = dataGridView2.CurrentRow.Index;
            bool incompatible = false;
            List<object> datos = new List<object>();

            for (int i = 0; i < dataGridView2.CurrentRow.Cells.Count - 2; i++)
            {
                if (dataGridView2.CurrentRow.Cells[i].ToString() != "")
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
                            resultado = Convert.ChangeType(this.dataGridView2.Rows[celdaSeleccionada].Cells[i].Value, typeof(int));
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
                            resultado = Convert.ChangeType(this.dataGridView2.Rows[celdaSeleccionada].Cells[i].Value, typeof(char));

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
                            resultado = Convert.ChangeType(this.dataGridView2.Rows[celdaSeleccionada].Cells[i].Value, typeof(string));
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
                            resultado = Convert.ChangeType(this.dataGridView2.Rows[celdaSeleccionada].Cells[i].Value, typeof(float));
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
                            resultado = Convert.ChangeType(this.dataGridView2.Rows[celdaSeleccionada].Cells[i].Value, typeof(double));
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
                            resultado = Convert.ChangeType(this.dataGridView2.Rows[celdaSeleccionada].Cells[i].Value, typeof(long));
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

            inserta_dato_indice(nuevoDato);
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
                case 'I': tipoAtr = typeof(int);
                    break;
                case 'F': tipoAtr = typeof(float);
                    break;
                case 'C': tipoAtr = typeof(char);
                    break;
                case 'S': tipoAtr = typeof(string);
                    break;
                case 'D': tipoAtr = typeof(double);
                    break;
                case 'L': tipoAtr = typeof(long);
                    break;
                default: // Default vacio
                    break;
            }

            return tipoAtr;
        }

        /// <summary>
        /// Funcion que inserta un dato en un indice, dicho indice estará en la lista de indices de la entidad.
        /// </summary>
        /// <param name="dat">El dato que se va a insertar.</param>
        private void inserta_dato_indice(Dato dat)
        {
            // Si no hay indices en la lista de indices de la entidad.
            if (ent.apIndices == -1)
            {
                Indice nuevo = crea_indice(dat);               
                actualiza_dataGrid_indices();
                actualiza_dataGrid_datos(nuevo);
            }
            else
            {
                // Se buscara el indice cuyo intervalo acepte el dato que se desea insertar.
                Indice encontrado = busca_indice(dat);
                actualiza_dataGrid_indices();
                actualiza_dataGrid_datos(encontrado);
            }

            bandChanged = true;
        }

        /// <summary>
        /// Metodo que nos ayudara a buscar un indice sobre el que podamos meter nuestro dato nuevo. Si no cabe en ningun indice, entonces
        /// se creara uno nuevo.
        /// </summary>
        /// <param name="dat">El nuevo dato que se desea insertar.</param>
        private Indice busca_indice(Dato dat)
        {
            bool encontro = false;
            Indice indiceEncontrado = new Indice();

            switch (atrLlave.tipo)
            {
                case 'I':
                    int valorI = Convert.ToInt32(dat.datos[indiceLlave]);

                    foreach (Indice ind in ent.listaIndices)
                    {
                        int valorInicial = Convert.ToInt32(ind.regresa_valInicial());
                        int valorFinal = Convert.ToInt32(ind.regresa_valFinal());

                        // Si se encontro el indice
                        if (valorFinal >= valorI && valorI >= valorInicial)
                        {
                            // Validar que no exista un dato con la misma llave primaria
                            if(valida_llave_primaria(ind, dat) == false)
                            {
                                dat.posDato = posMemoria;
                                ind.datosIndice[ind.datosIndice.Count - 1].apSigDato = dat.posDato;
                                ind.datosIndice.Add(dat);
                                posMemoria = posMemoria + tamDato;
                                indiceEncontrado = ind;
                                encontro = true;
                            }
                            else
                            {
                                toolStripStatusLabel1.Text = "Error, llave primaria repetida.";
                            }
                            break;
                        }
                    }
                    break;
                case 'L':
                    long valorL = Convert.ToInt64(dat.datos[indiceLlave]);

                    foreach (Indice ind in ent.listaIndices)
                    {
                        long valorInicial = Convert.ToInt64(ind.regresa_valInicial());
                        long valorFinal = Convert.ToInt64(ind.regresa_valFinal());

                        // Si se encontro el indice
                        if (valorFinal >= valorL && valorL >= valorInicial)
                        {
                            // Validar que no exista un dato con la misma llave primaria
                            if(valida_llave_primaria(ind, dat) == false)
                            {
                                dat.posDato = posMemoria;
                                ind.datosIndice[ind.datosIndice.Count - 1].apSigDato = dat.posDato;
                                ind.datosIndice.Add(dat);
                                posMemoria = posMemoria + tamDato;
                                indiceEncontrado = ind;
                                encontro = true;
                            }
                            else
                            {
                                toolStripStatusLabel1.Text = "Error, llave primaria repetida.";
                            }
                            break;
                        }
                    }
                    break;
                case 'F':
                    float valorF = Convert.ToSingle(dat.datos[indiceLlave]);

                    foreach (Indice ind in ent.listaIndices)
                    {
                        float valorInicial = Convert.ToSingle(ind.regresa_valInicial());
                        float valorFinal = Convert.ToSingle(ind.regresa_valFinal());

                        // Si se encontro el indice
                        if (valorFinal >= valorF && valorF >= valorInicial)
                        {
                            // Validar que no exista un dato con la misma llave primaria
                            if(valida_llave_primaria(ind, dat) == false)
                            {
                                dat.posDato = posMemoria;
                                ind.datosIndice[ind.datosIndice.Count - 1].apSigDato = dat.posDato;
                                ind.datosIndice.Add(dat);
                                posMemoria = posMemoria + tamDato;
                                indiceEncontrado = ind;
                                encontro = true;
                            }
                            else
                            {
                                toolStripStatusLabel1.Text = "Error, llave primaria repetida.";
                            }
                            break;
                        }
                    }
                    break;
                case 'D':
                    double valorD = Convert.ToDouble(dat.datos[indiceLlave]);

                    foreach (Indice ind in ent.listaIndices)
                    {
                        double valorInicial = Convert.ToDouble(ind.regresa_valInicial());
                        double valorFinal = Convert.ToDouble(ind.regresa_valFinal());

                        // Si se encontro el indice
                        if (valorFinal >= valorD && valorD >= valorInicial)
                        {
                            // Validar que no exista un dato con la misma llave primaria
                            if(valida_llave_primaria(ind, dat) == false)
                            {
                                dat.posDato = posMemoria;
                                ind.datosIndice[ind.datosIndice.Count - 1].apSigDato = dat.posDato;
                                ind.datosIndice.Add(dat);
                                posMemoria = posMemoria + tamDato;
                                indiceEncontrado = ind;
                                encontro = true;
                            }
                            else
                            {
                                toolStripStatusLabel1.Text = "Error, llave primaria repetida.";
                            }
                            break;
                        }
                    }
                    break;
                case 'C':
                    char valor = Convert.ToChar(dat.datos[indiceLlave]);

                    foreach (Indice ind in ent.listaIndices)
                    {
                        char valorInicial = Convert.ToChar(ind.regresa_valInicial());
                        char valorFinal = Convert.ToChar(ind.regresa_valFinal());

                        // Si se encontro el indice
                        if (valorFinal >= valor && valor >= valorInicial)
                        {
                            // Validar que no exista un dato con la misma llave primaria
                            if(valida_llave_primaria(ind, dat) == false)
                            {
                                dat.posDato = posMemoria;
                                ind.datosIndice[ind.datosIndice.Count - 1].apSigDato = dat.posDato;
                                ind.datosIndice.Add(dat);
                                posMemoria = posMemoria + tamDato;
                                indiceEncontrado = ind;

                                ind.datosIndice = ind.datosIndice.OrderBy(o => o.datos[indiceLlave]).ToList();

                                encontro = true;
                            }
                            else
                            {
                                toolStripStatusLabel1.Text = "Error, llave primaria repetida.";
                            }
                            break;
                        }
                    }
                    break;
                default: // Default vacio
                    break;
             }
         
             // Si no se encontro un indice apropiado para este dato, hay que crearlo.
             if(encontro == false)
             {
                 indiceEncontrado = crea_indice(dat);
             }

             return indiceEncontrado;
        }

        /// <summary>
        /// Metodo que actualiza el dataGrid de los datos correspondientes a un indice.
        /// </summary>
        /// <param name="i">El indice que contiene la lista de datos.</param>
        private void actualiza_dataGrid_datos(Indice i)
        {
            dataGridView2.Rows.Clear();

            List<String[]> filas = new List<string[]>();
            String[] fila = new string[atributosVigentes.Count + 2];
            int count = 0;

            List<Dato> datosOrdenados = i.datosIndice.OrderBy(o => o.datos[indiceLlave]).ToList();

            foreach (Dato dat in datosOrdenados)
            {
                foreach(object obj in dat.datos)
                {
                    if(obj is char[])
                    {
                        char[] arr = (char[])obj;
                        String objeto = new string(arr);
                        fila[count] = objeto;
                    }
                    else
                    {
                        fila[count] = obj.ToString();
                    }
                    count++;
                }
                fila[count] = dat.posDato.ToString();
                count++;

                if (dat.apSigDato != -2)
                {
                    fila[count] = dat.apSigDato.ToString();
                }
                else
                {
                    fila[count] = "-1";
                }

                if (dat.apSigDato != -3 && dat.apSigDato != -4)
                {
                    filas.Add(fila);
                }
                fila = new string[atributosVigentes.Count + 2];
                count = 0;
            }

            foreach (string[] arr in filas)
            {
                dataGridView2.Rows.Add(arr);
            }
        }

        /// <summary>
        /// Metodo con el que se validara que el dato que se desea insertar en la lista de datos del indice no este repetido, respecto a su
        /// llave primaria.
        /// </summary>
        /// <param name="ind">El indice que tiene la lista de datos sobre la que se desea insertar el dato.</param>
        /// <param name="dat">El dato que se desea insertar en la lista de datos del indice.</param>
        /// <returns>Booleano que indica si la llave primaria del dato existe o no.</returns>
        private bool valida_llave_primaria(Indice ind, Dato dat)
        {
            bool yaExiste = false;

            switch(atrLlave.tipo)
            {
                case 'I':
                    foreach(Dato d in ind.datosIndice)
                    {
                        int datoComparar = Convert.ToInt32(d.datos[indiceLlave]);
                        int datoInsertar = Convert.ToInt32(dat.datos[indiceLlave]);

                        if(datoComparar == datoInsertar)
                        {
                            yaExiste = true;
                            break;
                        }
                    }
                    break;
                case 'L':
                    foreach(Dato d in ind.datosIndice)
                    {
                        long datoComparar = Convert.ToInt64(d.datos[indiceLlave]);
                        long datoInsertar = Convert.ToInt64(dat.datos[indiceLlave]);

                        if(datoComparar == datoInsertar)
                        {
                            yaExiste = true;
                            break;
                        }
                    }
                    break;
                case 'F':
                    foreach(Dato d in ind.datosIndice)
                    {
                        float datoComparar = Convert.ToSingle(d.datos[indiceLlave]);
                        float datoInsertar = Convert.ToSingle(dat.datos[indiceLlave]);

                        if(datoComparar == datoInsertar)
                        {
                            yaExiste = true;
                            break;
                        }
                    }
                    break;
                case 'D':
                    foreach(Dato d in ind.datosIndice)
                    {
                        double datoComparar = Convert.ToDouble(d.datos[indiceLlave]);
                        double datoInsertar = Convert.ToDouble(dat.datos[indiceLlave]);

                        if(datoComparar == datoInsertar)
                        {
                            yaExiste = true;
                            break;
                        }
                    }
                    break;
                case 'C':
                    foreach(Dato d in ind.datosIndice)
                    {
                        char datoComparar = Convert.ToChar(d.datos[indiceLlave]);
                        char datoInsertar = Convert.ToChar(dat.datos[indiceLlave]);

                        if(datoComparar == datoInsertar)
                        {
                            yaExiste = true;
                            break;
                        }
                    }
                    break;
                case 'S':
                    foreach(Dato d in ind.datosIndice)
                    {
                        String datoComparar = Convert.ToString(d.datos[indiceLlave]);
                        String datoInsertar = Convert.ToString(dat.datos[indiceLlave]);

                        if(datoComparar == datoInsertar)
                        {
                            yaExiste = true;
                            break;
                        }
                    }
                    break;
            }

            return yaExiste;
        }

        /// <summary>
        /// Metodo que actualizara el dataGrid de los indices, ordenandolos de forma ascendente.
        /// </summary>
        private void actualiza_dataGrid_indices()
        {
            List<Indice> ordenada = ordena_indices(ent);

            dataGridView1.Rows.Clear();

            List<String[]> filas = new List<string[]>();
            String[] fila = new string[5];
            int count = 0;

            foreach(Indice ind in ordenada)
            {
                long vI = ind.regresa_valInicial();
                long vF = ind.regresa_valFinal();
                long pI = ind.regresa_posIndice();
                long aS = ind.regresa_apSigIndice();
                long aD = ind.regresa_apDatos();

                fila[count] = vI.ToString();
                count++;
                fila[count] = vF.ToString();
                count++;
                fila[count] = pI.ToString();
                count++;
                if (aS != -3)
                {
                    fila[count] = aS.ToString();
                }
                else
                {
                    fila[count] = "-1";
                }
                count++;
                fila[count] = aD.ToString();

                if (aS != -2 && aS != -4)
                {
                    filas.Add(fila);
                }

                fila = new string[5];
                count = 0;
            }

            foreach (string[] arr in filas)
            {
                dataGridView1.Rows.Add(arr);
            }

            actualiza_listas(ordenada);
        }

        /// <summary>
        /// Metodos con el que se actualizara la lista de indices vigentes (que no han sido eliminados) y de indices de la entidad.
        /// </summary>
        /// <param name="indicesOrdenados">La lista con los indices ya ordenados.</param>
        private void actualiza_listas(List<Indice> indicesOrdenados)
        {
            indicesVigentes.Clear();

            foreach (Indice ind in indicesOrdenados)
            {
                long sigInd = ind.regresa_apSigIndice();

                if (sigInd != -3 && sigInd != -4)
                {
                    indicesVigentes.Add(ind);
                }
            }

            ent.listaIndices.Clear();

            foreach (Indice ind in indicesOrdenados)
            {
                ent.listaIndices.Add(ind);
            }
        }

        /// <summary>
        /// Funcion que ordena los indices de la lista de indices de la entidad, de acuerdo a un orden ascendente.
        /// </summary>
        /// <param name="e">La entidad que contiene la lista de indices.</param>
        private List<Indice> ordena_indices(Entidad e)
        {
            List<Indice> ordenada = new List<Indice>();

            switch(atrLlave.tipo)
            {
                case 'I':
                    ordenada = e.listaIndices.OrderBy(o => Convert.ToInt32(o.regresa_valInicial())).ToList();
                    break;
                case 'L':
                    ordenada = e.listaIndices.OrderBy(o => Convert.ToInt64(o.regresa_valInicial())).ToList();
                    break;
                case 'F':
                    ordenada = e.listaIndices.OrderBy(o => Convert.ToSingle(o.regresa_valInicial())).ToList();
                    break;
                case 'D':
                    ordenada = e.listaIndices.OrderBy(o => Convert.ToDouble(o.regresa_valInicial())).ToList();
                    break;
                case 'C':
                    ordenada = e.listaIndices.OrderBy(o => Convert.ToChar(o.regresa_valInicial())).ToList();
                    break;
                case 'S':
                    ordenada = e.listaIndices.OrderBy(o => Convert.ToString(o.regresa_valInicial())).ToList();
                    break;
            }

            for(int i = 0; i < ordenada.Count; i++)
            {
                if(i == 0)
                {
                    ent.apIndices = ordenada[i].regresa_posIndice();
                }

                if((i+1) < ordenada.Count)
                {
                    ordenada[i].srt_apSigIndice(ordenada[i+1].regresa_posIndice());
                }
            }

            return ordenada;
        }

        /// <summary>
        /// Metodo que crea un nuevo indice en donde se insertara un dato. El rango sera determinado por el tipo de dato de la llave
        /// primaria.
        /// </summary>
        /// <param name="d">El dato que sera insertado.</param>
        /// <returns>El nuevo indice con el dato insertado.</returns>
        private Indice crea_indice(Dato d)
        {
            Indice nuevoIndice = new Indice();

            char tipo = atrLlave.tipo;
            dynamic valor;
            dynamic valIni = 0;
            dynamic valFin = 0;
            bool intervaloEncontrado = false;

            switch (tipo)
            {
                case 'I': valor = Convert.ToInt32(d.datos[indiceLlave]);
                    Convert.ChangeType(valIni, typeof(int));
                    Convert.ChangeType(valFin, typeof(int));

                    valIni = 0;
                    valFin = rango - 1;

                    do
                    {
                        if (valFin >= valor && valor >= valIni)
                        {
                            nuevoIndice.srt_valorInicial(valIni);
                            nuevoIndice.srt_valorFinal(valFin);
                            nuevoIndice.srt_posIndice(posMemoria);
                            posMemoria = posMemoria + tamIndice;
                            d.posDato = posMemoria;
                            posMemoria = posMemoria + tamDato;
                            nuevoIndice.srt_apDatos(d.posDato);
                            nuevoIndice.datosIndice.Add(d);
                            ent.apIndices = nuevoIndice.regresa_posIndice();

                            intervaloEncontrado = true;
                        }
                        else
                        {
                            valIni += rango;
                            valFin += rango;
                        }
                    } while (intervaloEncontrado == false);
                    break;
                case 'L': valor = Convert.ToInt64(d.datos[indiceLlave]);
                    Convert.ChangeType(valIni, typeof(long));
                    Convert.ChangeType(valFin, typeof(long));

                    valIni = 0;
                    valFin = rango - 1;

                    do
                    {
                        if (valFin >= valor && valor >= valIni)
                        {
                            nuevoIndice.srt_valorInicial(valIni);
                            nuevoIndice.srt_valorFinal(valFin);
                            nuevoIndice.srt_posIndice(posMemoria);
                            posMemoria = posMemoria + tamIndice;
                            d.posDato = posMemoria;
                            posMemoria = posMemoria + tamDato;
                            nuevoIndice.srt_apDatos(d.posDato);
                            nuevoIndice.datosIndice.Add(d);
                            ent.apIndices = nuevoIndice.regresa_posIndice();

                            intervaloEncontrado = true;
                        }
                        else
                        {
                            valIni += rango;
                            valFin += rango;
                        }
                    } while (intervaloEncontrado == false);
                    break;
                case 'F': valor = Convert.ToSingle(d.datos[indiceLlave]);
                    Convert.ChangeType(valIni, typeof(float));
                    Convert.ChangeType(valFin, typeof(float));

                    valIni = 0;
                    valFin = rango - 1;

                    do
                    {
                        if (valFin >= valor && valor >= valIni)
                        {
                            nuevoIndice.srt_valorInicial(valIni);
                            nuevoIndice.srt_valorFinal(valFin);
                            nuevoIndice.srt_posIndice(posMemoria);
                            posMemoria = posMemoria + tamIndice;
                            d.posDato = posMemoria;
                            posMemoria = posMemoria + tamDato;
                            nuevoIndice.srt_apDatos(d.posDato);
                            nuevoIndice.datosIndice.Add(d);
                            ent.apIndices = nuevoIndice.regresa_posIndice();

                            intervaloEncontrado = true;
                        }
                        else
                        {
                            valIni += rango;
                            valFin += rango;
                        }
                    } while (intervaloEncontrado == false);
                    break;
                case 'D': valor = Convert.ToDouble(d.datos[indiceLlave]);
                    Convert.ChangeType(valIni, typeof(double));
                    Convert.ChangeType(valFin, typeof(double));

                    valIni = 0;
                    valFin = rango - 1;

                    do
                    {
                        if (valFin >= valor && valor >= valIni)
                        {
                            nuevoIndice.srt_valorInicial(valIni);
                            nuevoIndice.srt_valorFinal(valFin);
                            nuevoIndice.srt_posIndice(posMemoria);
                            posMemoria = posMemoria + tamIndice;
                            d.posDato = posMemoria;
                            posMemoria = posMemoria + tamDato;
                            nuevoIndice.srt_apDatos(d.posDato);
                            nuevoIndice.datosIndice.Add(d);
                            ent.apIndices = nuevoIndice.regresa_posIndice();

                            intervaloEncontrado = true;
                        }
                        else
                        {
                            valIni += rango;
                            valFin += rango;
                        }
                    } while (intervaloEncontrado == false);
                    break;
                case 'C': valor = Convert.ToChar(d.datos[indiceLlave]);
                    Convert.ChangeType(valIni, typeof(char));
                    Convert.ChangeType(valFin, typeof(char));

                    valIni = 'a';
                    valFin = valIni + rango;

                    do
                    {
                        if (valFin >= valor && valor >= valIni)
                        {
                            nuevoIndice.srt_valorInicial(valIni);
                            nuevoIndice.srt_valorFinal(valFin);
                            nuevoIndice.srt_posIndice(posMemoria);
                            posMemoria = posMemoria + tamIndice;
                            d.posDato = posMemoria;
                            posMemoria = posMemoria + tamDato;
                            nuevoIndice.srt_apDatos(d.posDato);
                            nuevoIndice.datosIndice.Add(d);
                            ent.apIndices = nuevoIndice.regresa_posIndice();

                            intervaloEncontrado = true;
                        }
                        else
                        {
                            valIni += rango;
                            valFin += rango;
                        }
                    } while (intervaloEncontrado == false);
                    break;
                case 'S': valor = Convert.ToString(d.datos[indiceLlave]);
                    Convert.ChangeType(valIni, typeof(string));
                    Convert.ChangeType(valFin, typeof(string));

                    valIni = 'a';

                    break;
                default: // Default vacio
                    break;
            }

            toolStripStatusLabel1.Text = "Indice creado y dato insertado con exito.";

            indicesVigentes.Add(nuevoIndice);
            ent.listaIndices.Add(nuevoIndice);

            return nuevoIndice;
        }

        public long regresa_apuntador_listas()
        {
            return ent.apIndices;
        }

        public long regresa_posMemoria()
        {
            return posMemoria;
        }

        public long regresa_rango()
        {
            return rango;
        }

        public bool regresa_seCambio()
        {
            return bandChanged;
        }

        public List<Indice> regresa_listaIndices()
        {
            return ent.listaIndices;
        }

        // Boton que cierra el cuadro actual.
        private void button6_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // Boton para modificar un dato.
        private void button2_Click(object sender, EventArgs e)
        {
            bool encontradoYmodificado = false;

            if(textBox2.Text.Length > 0)
            {
                dynamic valorBuscar = 0;

                // Convertimos el valor escrito en el textBox al tipo de dato correspondiente al de la llave primaria
                switch(atrLlave.tipo)
                {
                    case 'I':
                        valorBuscar = Int32.Parse(textBox2.Text);
                        break;
                    case 'F':
                        valorBuscar = float.Parse(textBox2.Text);
                        break;
                    case 'L':
                        valorBuscar = Int64.Parse(textBox2.Text);
                        break;
                    case 'D':
                        valorBuscar = Double.Parse(textBox2.Text);
                        break;
                    case 'C':
                        valorBuscar = Char.Parse(textBox2.Text);
                        break;
                    case 'S':
                        valorBuscar = textBox2.Text;
                        break;
                    default:
                        // DEFAULT VACIO
                        break;
                }

                // Recorremos la lista de indices en busqueda de el que contenga el rango adecuado.
                foreach(Indice ind in ent.listaIndices)
                {
                    Indice indiceI = ind;

                    dynamic vI = ind.regresa_valInicial();
                    dynamic vF = ind.regresa_valFinal();

                    // Si la llave primaria no es de tipo String
                    if(atrLlave.tipo != 'S')
                    {
                        if(vF >= valorBuscar && valorBuscar >= vI)
                        {
                            // Si encontramos el indice, ahora hay que encontrar el indice que corresponda a ese dato.
                            dynamic datoBuscar = 0;

                            foreach(Dato dat in ind.datosIndice)
                            {
                                Dato datoI = dat;

                                switch(atrLlave.tipo)
                                {
                                    case 'I': datoBuscar = Convert.ToInt32(datoI.datos[indiceLlave]);
                                        break;
                                    case 'F': datoBuscar = Convert.ToSingle(datoI.datos[indiceLlave]);
                                        break;
                                    case 'L': datoBuscar = Convert.ToInt64(datoI.datos[indiceLlave]);
                                        break;
                                    case 'D': datoBuscar = Convert.ToDouble(datoI.datos[indiceLlave]);
                                        break;
                                    case 'C': datoBuscar = Convert.ToChar(datoI.datos[indiceLlave]);
                                        break;
                                    case 'S': datoBuscar = Convert.ToString(datoI.datos[indiceLlave]);
                                        break;
                                }

                                // Si encontramos el dato
                                if(datoBuscar == valorBuscar)
                                {
                                    using(ModificaDatoIndexado modIndex = new ModificaDatoIndexado(indiceI, datoI, ent, indiceLlave))
                                    {
                                        var modifica = modIndex.ShowDialog();

                                        if(modifica == DialogResult.OK)
                                        {
                                            datoI = modIndex.regresa_datoIndexado();

                                            // Si se cambio la llave primaria del dato
                                            if(modIndex.regresa_llavePrimariaCambiada() == true)
                                            {
                                                revisa_cambio(datoI, indiceI);
                                            }

                                            actualiza_dataGrid_indices();
                                            actualiza_dataGrid_datos(indiceI);

                                            encontradoYmodificado = true;
                                        }
                                    }
                                }

                                if(encontradoYmodificado == true)
                                {
                                    break;
                                }
                            }
                        }

                        if(encontradoYmodificado == true)
                        {
                            break;
                        }
                    }
                    else
                    {
                        // WIP
                    }
                }
            }
            else
            {
                toolStripStatusLabel1.Text = "Error, no se ha introducido una llave primaria.";
            }
        }

        /// <summary>
        /// Metodo que revisa que el cambio de dato haga que se tenga que cambiar el dato a otro indice, y, si ese es el caso, tambien
        /// verificara si el indice sobre el que se desea insertar el dato exista. Si no es el caso, entonces el indice sera creado.
        /// </summary>
        /// <param name="d">El dato al que se cambiara de indice de ser necesario.</param>
        /// <param name="i">El indice actual del dato. Si solo tenia el dato a cambiar de indice, dicho indice sera eliminado.</param>
        private void revisa_cambio(Dato d, Indice i)
        {
            // Primero se debe revisar si el dato aun cabe dentro del indice en el que esta, en cuyo caso no se debe tomar accion alguna.
            dynamic valorLlavePrimaria = d.datos[indiceLlave];
            dynamic valorInicial = i.regresa_valInicial();
            dynamic valorFinal = i.regresa_valFinal();

            if(atrLlave.tipo != 'S')
            {
                if(valorFinal > valorLlavePrimaria && valorLlavePrimaria > valorInicial)
                {
                    // No se debe de tomar accion alguna puesto a que el valor de la llave primaria aun esta dentro de los valores del indice
                    // en el que esta el dato con dicha llave.
                }
                else
                {   
                    // Se debe revisar si el dato que se quiere cambiar de indice era el unico dato del indice en cuestion.
                    if(i.datosIndice.Count == 1)
                    {
                        i.srt_apDatos(-1);
                        i.datosIndice.Clear();
                    }
                    else
                    {
                        List<Dato> datosRespaldo = new List<Dato>();

                        for(int a = 0; a < i.datosIndice.Count; a++)
                        {
                            if (i.datosIndice[a] != d)
                            {
                                if ((a + 1) < i.datosIndice.Count && i.datosIndice[a + 1] == d && (a + 2) < i.datosIndice.Count)
                                {
                                    i.datosIndice[a].apSigDato = i.datosIndice[a + 2].posDato;
                                }
                                else if((a + 1) < i.datosIndice.Count && i.datosIndice[a + 1] == d)
                                {
                                    i.datosIndice[a].apSigDato = -1;
                                }

                                datosRespaldo.Add(i.datosIndice[a]);
                            }
                            else if(i.datosIndice[a] == d && a == 0 && (a+1) < i.datosIndice.Count)
                            {
                                i.srt_apDatos(i.datosIndice[a+1].posDato);
                                datosRespaldo.Add(i.datosIndice[a+1]);
                            }
                        }

                        i.datosIndice.Clear();
                        i.datosIndice = datosRespaldo;
                    }

                    busca_indice(d);
                }
            }
            else
            {
                // WIP
            }
        }

        // Boton para eliminar un dato (y quiza hasta el indice si se da el caso).
        private void button3_Click(object sender, EventArgs e)
        {
            bool encontradoYeliminado = false;

            if(textBox2.Text.Length > 0)
            {
                dynamic valorBuscar = 0;

                // Convertimos el valor escrito en el textBox al tipo de dato correspondiente al de la llave primaria
                switch (atrLlave.tipo)
                {
                    case 'I':
                        valorBuscar = Int32.Parse(textBox2.Text);
                        break;
                    case 'F':
                        valorBuscar = float.Parse(textBox2.Text);
                        break;
                    case 'L':
                        valorBuscar = Int64.Parse(textBox2.Text);
                        break;
                    case 'D':
                        valorBuscar = Double.Parse(textBox2.Text);
                        break;
                    case 'C':
                        valorBuscar = Char.Parse(textBox2.Text);
                        break;
                    case 'S':
                        valorBuscar = textBox2.Text;
                        break;
                    default:
                        // DEFAULT VACIO
                        break;
                }

                // Recorremos la lista de indices en busqueda de el que contenga el rango adecuado.
                foreach (Indice ind in ent.listaIndices)
                {
                    Indice indiceI = ind;

                    dynamic vI = ind.regresa_valInicial();
                    dynamic vF = ind.regresa_valFinal();

                    // Si la llave primaria no es de tipo String
                    if (atrLlave.tipo != 'S')
                    {
                        if (vF >= valorBuscar && valorBuscar >= vI)
                        {
                            // Si encontramos el indice, ahora hay que encontrar el indice que corresponda a ese dato.
                            dynamic datoBuscar = 0;

                            foreach (Dato dat in ind.datosIndice)
                            {
                                Dato datoI = dat;

                                switch (atrLlave.tipo)
                                {
                                    case 'I': datoBuscar = Convert.ToInt32(datoI.datos[indiceLlave]);
                                        break;
                                    case 'F': datoBuscar = Convert.ToSingle(datoI.datos[indiceLlave]);
                                        break;
                                    case 'L': datoBuscar = Convert.ToInt64(datoI.datos[indiceLlave]);
                                        break;
                                    case 'D': datoBuscar = Convert.ToDouble(datoI.datos[indiceLlave]);
                                        break;
                                    case 'C': datoBuscar = Convert.ToChar(datoI.datos[indiceLlave]);
                                        break;
                                }

                                // Si encontramos el dato
                                if (datoBuscar == valorBuscar)
                                {
                                    // Se debe revisar si el dato a eliminar era el unico dentro del indice
                                    // Si era el unico dato del indice, entonces se actualizara el apuntador al siguiente dato de dicho
                                    // dato a -4
                                    if(indiceI.datosIndice.Count == 1)
                                    {
                                        indiceI.datosIndice[0].apSigDato = -4;
                                        indiceI.srt_apDatos(-2);
                                        encontradoYeliminado = true;
                                    }
                                    else
                                    {
                                        // Si no era el unico, habra que revisar en que posicion esta el dato a eliminar
                                        for(int it = 0; it < indiceI.datosIndice.Count; it++)
                                        {
                                            // Si el dato examinado no es el dato a eliminar, y el dato a examinar no fue eliminado con
                                            // anterioridad
                                            if(indiceI.datosIndice[it] != datoI && indiceI.datosIndice[it].apSigDato != -3
                                                && indiceI.datosIndice[it].apSigDato != -4)
                                            {
                                                // Si el siguiente dato a examinar es el dato que se busca eliminar y hay datos despues
                                                if ((it + 1) < indiceI.datosIndice.Count && indiceI.datosIndice[it + 1] == datoI &&
                                                    (it + 2) < indiceI.datosIndice.Count)
                                                {
                                                    // Se debe de buscar si el dato posterior a eliminar no fue eliminado tambien
                                                    bool todosEliminados = false;

                                                    for(int iu = it + 2; iu < indiceI.datosIndice.Count; iu++)
                                                    {
                                                        // Si se encontro un dato posterior que no fue eliminado.
                                                        if(indiceI.datosIndice[iu].apSigDato != -3 && 
                                                            indiceI.datosIndice[iu].apSigDato != -4)
                                                        {
                                                            indiceI.datosIndice[it + 1].apSigDato = -3;
                                                            indiceI.datosIndice[it].apSigDato = indiceI.datosIndice[iu].posDato;
                                                            break;
                                                        }

                                                        if(iu == indiceI.datosIndice.Count)
                                                        {
                                                            todosEliminados = true;
                                                        }
                                                    }
                                                    
                                                    // Si al final no hubo datos posteriores al eliminado que tampoco fueron eliminados
                                                    if(todosEliminados == true)
                                                    {
                                                        encontradoYeliminado = true;
                                                        indiceI.datosIndice[it].apSigDato = -2;
                                                        indiceI.datosIndice[it + 1].apSigDato = -3;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        encontradoYeliminado = true;
                                                        break;
                                                    }
                                                }
                                                // Si el siguiente dato a examinar el el dato que se busca eliminar y no hay datos despues
                                                else if ((it + 1) < indiceI.datosIndice.Count && indiceI.datosIndice[it + 1] == datoI)
                                                {
                                                    encontradoYeliminado = true;
                                                    indiceI.datosIndice[it].apSigDato = -2;
                                                    indiceI.datosIndice[it + 1].apSigDato = -4;
                                                    break;
                                                }
                                            }
                                            // Si el dato examinado no es el dato a eliminar, y dicho dato examinado tambien fue eliminado
                                            else if (indiceI.datosIndice[it] != datoI && indiceI.datosIndice[it].apSigDato == -3
                                                && indiceI.datosIndice[it].apSigDato == -4)
                                            {
                                                // Si el siguiente dato del que se esta examinando es el dato a eliminar y hay datos despues
                                                if ((it + 1) < indiceI.datosIndice.Count && indiceI.datosIndice[it + 1] == datoI &&
                                                    (it + 2) < indiceI.datosIndice.Count)
                                                {
                                                    // Se debe de buscar si el dato posterior a eliminar no fue eliminado tambien
                                                    bool todosEliminados = false;

                                                    for (int iu = it + 2; iu < indiceI.datosIndice.Count; iu++)
                                                    {
                                                        // Si se encontro un dato posterior que no fue eliminado.
                                                        if (indiceI.datosIndice[iu].apSigDato != -3 &&
                                                            indiceI.datosIndice[iu].apSigDato != -4)
                                                        {
                                                            indiceI.datosIndice[it + 1].apSigDato = -3;
                                                            indiceI.srt_apDatos(indiceI.datosIndice[iu].posDato);
                                                            break;
                                                        }

                                                        if (iu == indiceI.datosIndice.Count)
                                                        {
                                                            todosEliminados = true;
                                                        }
                                                    }

                                                    // Si al final no hubo datos posteriores al eliminado que tampoco fueron eliminados
                                                    if (todosEliminados == true)
                                                    {
                                                        encontradoYeliminado = true;
                                                        indiceI.datosIndice[it + 1].apSigDato = -3;
                                                        indiceI.srt_apDatos(-2);
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        encontradoYeliminado = true;
                                                        break;
                                                    }
                                                }
                                                else if ((it + 1) < indiceI.datosIndice.Count && indiceI.datosIndice[it + 1] == datoI)
                                                {
                                                    encontradoYeliminado = true;
                                                    indiceI.srt_apDatos(-2);
                                                    indiceI.datosIndice[it + 1].apSigDato = -4;
                                                    break;
                                                }
                                            }
                                            // Si es el primer dato de la lista de datos del indice es el dato a eliminar
                                            else if(it == 0 && indiceI.datosIndice[it] == datoI)
                                            {
                                                bool todosEliminados = true;

                                                for(int iu = it +1; iu < indiceI.datosIndice.Count; iu++)
                                                {
                                                    if(indiceI.datosIndice[iu].apSigDato != -3 && indiceI.datosIndice[iu].apSigDato != -4)
                                                    {
                                                        indiceI.srt_apDatos(indiceI.datosIndice[iu].posDato);
                                                        indiceI.datosIndice[it].apSigDato = -3;
                                                        encontradoYeliminado = true;
                                                        todosEliminados = false;
                                                        break;
                                                    }
                                                }

                                                if(todosEliminados == true)
                                                {
                                                    indiceI.srt_apDatos(-2);
                                                    break;
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (encontradoYeliminado == true)
                                {
                                    break;
                                }
                            }
                        }

                        if (encontradoYeliminado == true)
                        {
                            break;
                        }
                    }
                    else
                    {
                        // WIP
                    }
                }

                bandChanged = true;
                rellena_dataGridIndices();
                muestra_dataGrid_datos();
            }
            else
            {
                toolStripStatusLabel1.Text = "Error, no se ha introducido una llave primaria.";
            }
        }

        // Boton que muestra los datos ligados a un indice.
        private void button4_Click(object sender, EventArgs e)
        {
            muestra_dataGrid_datos();
        }

        /// <summary>
        /// Metodo que hace las llamadas correspondientes para mostrar los datos de un indice en el dataGridView correspondiente.
        /// </summary>
        private void muestra_dataGrid_datos()
        {
            dynamic valorInicial = dataGridView1.CurrentRow.Cells[0].Value;

            Indice muestraIndice = busca_indice(valorInicial);

            actualiza_dataGrid_datos(muestraIndice);
        }

        /// <summary>
        /// Metodo con el cual se buscara un indice en base a su valor inicial.
        /// </summary>
        /// <param name="vI">El valor inicial del indice a buscar.</param>
        /// <returns>Un indice que puede estar con un valor inicial vacio en caso de no ser encontrado.</returns>
        private Indice busca_indice(dynamic vI)
        {
            Indice encontrar = new Indice();

            foreach(Indice ind in ent.listaIndices)
            {
                dynamic valorInicialComparar = ind.regresa_valInicial();
                String compare = valorInicialComparar.ToString();

                if (compare == vI)
                {
                    encontrar = ind;
                }
            }

            return encontrar;
        }
    }
}
