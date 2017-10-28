﻿using System;
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
        Cajon cajonActual = new Cajon();

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

            int nC = (int)numCajones;
            int rC = (int)regPorCubeta;

            foreach (Atributo atr in ent.listaAtributos)
            {
                if (atr.apSigAtributo != -2 && atr.apSigAtributo != -4)
                {
                    numAtributos++;
                    atributosVigentes.Add(atr);
                    tamDato += atr.bytes;

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

            if (numCajones > 0 && regPorCubeta > 0)
            {
                textBox1.Text = nC.ToString();
                textBox2.Text = rC.ToString();

                textBox1.Enabled = false;
                textBox2.Enabled = false;

                button1.Enabled = false;

                pon_posiciones_datos();
                manejo_dataGrid_cajones();
                rellena_dataGrid_cajones();
                inicia_dataGrid_datos();
            }
        }

        /// <summary>
        /// Metodo con el cual se colocaran las posiciones de los datos, ya que, como esta informacion no se almacena en el archivo, debemos
        /// guiarnos usando los apuntadores a dato de cada cubeta
        /// </summary>
        private void pon_posiciones_datos()
        {
            foreach(Cajon caj in ent.listaCajones)
            {
                foreach(List<Cubeta> cub in caj.listaCubetas)
                {
                    foreach(Cubeta c in cub)
                    {
                        long apDato = c.regresa_apDato();

                        if(apDato != -1)
                        {
                            Dato dat = c.regresa_datoCubeta();
                            dat.posDato = apDato;
                        }
                    }
                }
            }
        }

        // Boton para poner el tamaño del cajon y el numero de registros por cubeta.
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

                        if(i == 0)
                        {
                            nuevoC.str_posCajon(ent.apCajones);
                        }

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
                    toolStripStatusLabel1.Text = "Valores agregados con exito.";
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

        /// <summary>
        /// Funcion con la que se iniciara el dataGridView con el que se mostraran los cajones de una entidad.
        /// </summary>
        private void manejo_dataGrid_cajones()
        {
            dataGridView1.ColumnCount = (int)numCajones;
            dataGridView1.ColumnHeadersVisible = true;

            for(int i = 0; i < numCajones; i++)
            {
                dataGridView1.Columns[i].Name = (i + 1).ToString();
            }
        }

        /// <summary>
        /// Funcion con la que se va a rellenar el dataGridView de los cajones con la informacion de los apuntadores de estos.
        /// </summary>
        private void rellena_dataGrid_cajones()
        {
            dataGridView1.Rows.Clear();

            dataGridView1.ColumnCount = (int)numCajones;
            dataGridView1.ColumnHeadersVisible = true;

            string[] fila = new string[(int)numCajones];
            List<String[]> filas = new List<string[]>();

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

        /// <summary>
        /// Funcion con la que se va a rellenar el dataGridView de las cubetas con la informacion de de estos.
        /// </summary>
        /// <param name="caj">El cajon al que pertenece la lista de listas de cubetas.</param>
        private void rellena_dataGrid_cubetas(Cajon caj)
        {
            dataGridView2.Rows.Clear();

            dataGridView2.ColumnCount = (int)regPorCubeta + 1;
            dataGridView2.ColumnHeadersVisible = true;

            string[] fila = new string[(int)regPorCubeta + 1];
            List<String[]> filas = new List<string[]>();

            for (int i = 0; i < caj.listaCubetas.Count; i++)
            {
                for(int j = 0; j < caj.listaCubetas[i].Count; j++)
                {
                    if (j == caj.listaCubetas[i].Count - 1)
                    {
                        long apSig = caj.listaCubetas[i][j].regresa_apSigCubeta();
                        fila[j] = apSig.ToString();
                    }
                    else
                    {
                        long apDato = caj.listaCubetas[i][j].regresa_apDato();
                        fila[j] = apDato.ToString();
                    }
                }

                filas.Add(fila);
                fila = new string[(int)regPorCubeta + 1];
            }

            foreach(string[] f in filas)
            {
                dataGridView2.Rows.Add(f);
            }
        }

        /// <summary>
        /// Funcion con la que se rellenara el dataGridView correspondiente 
        /// </summary>
        /// <param name="cub">La cubeta que contiene el dato a mostrarse.</param>
        private void rellena_dataGrid_datos(Cubeta cub)
        {
            dataGridView3.Rows.Clear();

            dataGridView3.ColumnCount = numAtributos + 1;
            dataGridView3.ColumnHeadersVisible = true;

            string[] fila = new string[(int)regPorCubeta + 1];
            List<String[]> filas = new List<string[]>();
            Dato datoCubeta = cub.regresa_datoCubeta();
            int count = 0;

            for (int i = 0; i < datoCubeta.datos.Count; i++ )
            {
                if(datoCubeta.datos[i] is char[])
                {
                    char[] arr = (char[])datoCubeta.datos[i];
                    String objeto = new string(arr);
                    fila[i] = objeto;
                }
                else
                {
                    fila[i] = datoCubeta.datos[i].ToString();
                }
                count++;
            }

            fila[count] = datoCubeta.posDato.ToString();

            filas.Add(fila);

            foreach (string[] arr in filas)
            {
                dataGridView3.Rows.Add(arr);
            }
        }

        /// <summary>
        /// Funcion con la que se rellenaran las columnas del dataGridView correspondiente a las cubetas. 
        /// </summary>
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

        /// <summary>
        /// Metodo que inicia las columnas necesarias para poder poner datos en estas.
        /// </summary>
        private void inicia_dataGrid_datos()
        {
            dataGridView3.ColumnCount = numAtributos + 1;
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
        }

        // Boton para insertar datos a un registro dentro de una cubeta. El criterio que decidira en que numero de cubeta se insertara sera
        // mediante el uso de la funcion hash "centro de cuadrados", a la que despues se le aplicara la funcion hash "modulo 10". Al
        // resultado de cada funcion hash se le debe de sumar 1.
        private void button6_Click(object sender, EventArgs e)
        {
            int celdaSeleccionada = dataGridView3.CurrentRow.Index;
            bool incompatible = false;
            List<object> datos = new List<object>();

            for (int i = 0; i < dataGridView3.CurrentRow.Cells.Count - 1; i++)
            {
                if (dataGridView3.CurrentRow.Cells[i].ToString() != "")
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
                            resultado = Convert.ChangeType(this.dataGridView3.Rows[celdaSeleccionada].Cells[i].Value, typeof(int));
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
                            resultado = Convert.ChangeType(this.dataGridView3.Rows[celdaSeleccionada].Cells[i].Value, typeof(char));

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
                            resultado = Convert.ChangeType(this.dataGridView3.Rows[celdaSeleccionada].Cells[i].Value, typeof(string));
                            String res = resultado.ToString();

                            if (res.Length > (atr.bytes / 2))
                            {
                                int start = Convert.ToInt32(atr.bytes);
                                start = start / 2;
                                int count = res.Length - start;
                                res = res.Remove(start, count);
                            }

                            datos.Add(res.ToLower());
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
                            resultado = Convert.ChangeType(this.dataGridView3.Rows[celdaSeleccionada].Cells[i].Value, typeof(float));
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
                            resultado = Convert.ChangeType(this.dataGridView3.Rows[celdaSeleccionada].Cells[i].Value, typeof(double));
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
                            resultado = Convert.ChangeType(this.dataGridView3.Rows[celdaSeleccionada].Cells[i].Value, typeof(long));
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

            inserta_dato_hash(nuevoDato);
        }

        /// <summary>
        /// Funcion con la que se insertara un dato en un cajon. Primero se aplicara la funcion hash, despues se validara si existe la
        /// cubeta correspondiente, si no, entonces se debe crear la cubeta con el numero de cajones correspondiente, para despues insertar
        /// el dato en dicha cubeta, y despues poner la posicion de dicha cubeta en la posicion del cajon que marque la funcion hash.
        /// </summary>
        /// <param name="dat">El dato que se desea insertar.</param>
        private void inserta_dato_hash(Dato dat)
        {
            if (valida_dato(dat) == false)
            {
                int valHash = funcion_hash(dat);
                Cubeta cubetaSender = new Cubeta();

                // Si el valor hash resultante es mayor al numero de cajones que tenemos.
                if (valHash > numCajones)
                {
                    double multiploMasProximo = valHash / numCajones;
                    int redondeado = Convert.ToInt32(Math.Floor(multiploMasProximo));

                    valHash = valHash - (redondeado * (int)numCajones);
                }

                // Si no hay cubetas
                if (ent.listaCajones[valHash].regresa_apuntadorCubeta() == -1)
                {
                    // Se crea la cubeta y se coloca el dato en la primer posicion de esta.
                    List<Cubeta> newCubeta = regresa_cubetas(dat);

                    ent.listaCajones[valHash].listaCubetas.Add(newCubeta);
                    ent.listaCajones[valHash].str_apuntadorCubeta(newCubeta[0].regresa_posCubeta());
                    cubetaSender = newCubeta[0];
                    seCambio = true;
                }
                else
                {
                    // Se debe validar si hay espacios dispobibles en la cubeta
                    bool libre = false;

                    foreach (List<Cubeta> listcub in ent.listaCajones[valHash].listaCubetas)
                    {
                        foreach (Cubeta cub in listcub)
                        {
                            if (cub.regresa_apSigCubeta() == 0)
                            {
                                if (cub.regresa_apDato() == -1)
                                {
                                    dat.posDato = posMemoria;
                                    cub.str_datoCubeta(dat);
                                    cub.str_apDato(dat.posDato);
                                    posMemoria += tamDato;
                                    cubetaSender = cub;
                                    libre = true;
                                    break;
                                }
                            }
                        }
                    }

                    // Si no se encontro un espacio disponible en la cubeta, se debe de crear una cubeta nueva
                    if (libre == false)
                    {
                        List<Cubeta> nuevasCubetas = regresa_cubetas(dat);
                        int ultimoValor = Convert.ToInt32(regPorCubeta - 1);

                        // Se debe de ligar la cubeta nueva con la anterior usando la posicion de la nueva cubeta
                        ent.listaCajones[valHash].listaCubetas[ent.listaCajones[valHash].listaCubetas.Count - 1][ultimoValor].str_apSigCubeta(nuevasCubetas[0].regresa_posCubeta());
                        cubetaSender = nuevasCubetas[0];
                    }
                }

                rellena_dataGrid_cajones();
                manejo_dataGrid_cubetas();
                rellena_dataGrid_cubetas(ent.listaCajones[valHash]);
                rellena_dataGrid_datos(cubetaSender);
                toolStripStatusLabel1.Text = "Dato insertado con exito.";
                seCambio = true;
            }
            else
            {
                toolStripStatusLabel1.Text = "Error, llave primaria duplicada.";
            }
        }

        /// <summary>
        /// La funcion hash que se aplicara. En este caso, sera la funcion centro de cuadrados, a la cual despues se le aplicara la funcion
        /// modulo 10. 
        /// </summary>
        /// <param name="dat">El dato que se quiere insertar.</param>
        /// <returns>El valor hash que decidira en que cajon ira la cubeta que contendra el dato.</returns>
        private int funcion_hash(Dato dat)
        {
            int valorHash = 0;

            var valorLlavePrimaria = dat.datos[indiceLlave];

            String convertToASCII = valorLlavePrimaria.ToString();
            byte[] bytes = Encoding.ASCII.GetBytes(convertToASCII);

            String valueASCII = "";
            StringBuilder bld = new StringBuilder();

            foreach(byte b in bytes)
            {
                bld.Append(b.ToString());
            }

            valueASCII = bld.ToString();

            double ASCIIdouble = Convert.ToDouble(valueASCII);            
            double potencia = 2;

            double cuadrado = Math.Pow(ASCIIdouble, potencia);
            int cuadradoInt = Convert.ToInt32(cuadrado);

            int digitos = cuadradoInt.ToString().Length;

            if(digitos % 2 == 0)
            {
                int mitad = digitos / 2;

                String subString1 = cuadradoInt.ToString().Substring(0, mitad);
                String subString2 = cuadradoInt.ToString().Substring(mitad, mitad);

                Char c1 = subString1[subString1.Length - 1];
                Char c2 = subString2[0];

                String digitosCentrales = "" + c1 + c2;
                int digitosFinales = Convert.ToInt32(digitosCentrales);

                valorHash = (digitosFinales % 10) + 1;
            }
            else
            {
                int mitad = digitos - 1 / 2;

                Char c1 = cuadradoInt.ToString()[mitad + 1];

                String digitosCentrales = "" + c1;
                int digitosFinales = Convert.ToInt32(digitosCentrales);

                valorHash = (digitosFinales % 10) + 1;
            }

            return valorHash;
        }

        /// <summary>
        /// Funcion que crea ua nueva cubeta, que contendra en su primer elemento el dato correspondiente.
        /// </summary>
        /// <param name="dat">El dato a insertar en la lista de cubetas.</param>
        /// <returns>La lista de cubetas con el dato insertado en la primer posicion.</returns>
        private List<Cubeta> regresa_cubetas(Dato dat)
        {
            List<Cubeta> listaCubetas = new List<Cubeta>();

            for(int i = 0; i < regPorCubeta; i++)
            {
                listaCubetas.Add(new Cubeta());
            }

            Cubeta cubetaEnlace = new Cubeta();
            cubetaEnlace.str_apSigCubeta(-1);

            listaCubetas.Add(cubetaEnlace);
            
            listaCubetas[0].str_posCubeta(posMemoria);
            posMemoria += tamCubeta;

            dat.posDato = posMemoria;
            listaCubetas[0].str_datoCubeta(dat);
            listaCubetas[0].str_apDato(dat.posDato);
            posMemoria += tamDato;

            return listaCubetas;
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

        // Boton que muestra en el dataGrid correspondiente las cubetas correspondientes a un cajon, seleccionando la celda correspondiente
        // a dicho cajon en el dataGridView correspondiente. 
        private void button4_Click(object sender, EventArgs e)
        {
            long valorSeleccionado = Convert.ToInt64(dataGridView1.CurrentCell.Value);
            Cajon cajonEncontrado = new Cajon();

            foreach(Cajon caj in ent.listaCajones)
            {
                long apCubeta = caj.regresa_apuntadorCubeta();

                if(apCubeta == valorSeleccionado)
                {
                    cajonEncontrado = caj;
                    break;
                }
            }

            if(cajonEncontrado.regresa_apuntadorCubeta() != -1)
            {
                cajonActual = cajonEncontrado; 
                muestra_cubetas(cajonEncontrado);
            }
            else
            {
                toolStripStatusLabel1.Text = "Este cajon no posee cubetas.";
            }
        }

        /// <summary>
        /// Metodo que mostrara en el dataGridView correspondiente las cubetas correspondientes a un cajon en especifico.
        /// </summary>
        /// <param name="caj">El cajon con las cubetas a mostrar.</param>
        private void muestra_cubetas(Cajon caj)
        {
            dataGridView2.Rows.Clear();

            dataGridView2.ColumnCount = (int)regPorCubeta + 1;
            dataGridView2.ColumnHeadersVisible = true;

            List<String[]> filas = new List<string[]>();
            String[] fila = new string[(int)regPorCubeta + 1];

            foreach (List<Cubeta> cub in caj.listaCubetas)
            {
                for (int i = 0; i < dataGridView2.ColumnCount; i++)
                {
                    if(i == (dataGridView2.ColumnCount - 1))
                    {
                        fila[i] = cub[i].regresa_apSigCubeta().ToString();
                    }
                    else
                    {
                        fila[i] = cub[i].regresa_apDato().ToString();
                    }
                }

                filas.Add(fila);
            }

            foreach (string[] arr in filas)
            {
                dataGridView2.Rows.Add(arr);
            }
        }

        /// <summary>
        /// Metodo con el que se validara que no haya un dato con la misma llave primaria que el dato que se desea insertar.
        /// </summary>
        /// <param name="dat">El dato que se desea insertar.</param>
        /// <returns>Booleano que indica si el dato que se desea insertar se puede insertar o no.</returns>
        private bool valida_dato(Dato dat)
        {
            bool repetido = false;
            bool encontrado = false;

            foreach (Cajon caj in ent.listaCajones)
            {
                foreach (List<Cubeta> cubL in caj.listaCubetas)
                {
                    foreach (Cubeta c in cubL)
                    {
                        long apDato = c.regresa_apDato();

                        if (apDato != -1)
                        {
                            Dato datoComparar = c.regresa_datoCubeta();

                            if (dat.datos[indiceLlave].ToString() == datoComparar.datos[indiceLlave].ToString())
                            {
                                repetido = true;
                                encontrado = true;
                                break;
                            }
                        }
                    }

                    if (encontrado == true)
                    {
                        break;
                    }
                }

                if(encontrado == true)
                {
                    break;
                }
            }

            return repetido;
        }

        // Boton con el que se mostraran los datos de una cubeta seleccionada en el dataGridView correspondiente.
        private void button5_Click(object sender, EventArgs e)
        {
            long cajonSeleccionado = Convert.ToInt64(dataGridView2.CurrentCell.Value);
            Cubeta cubetaEncontrada = new Cubeta();
            bool bandEncontrada = false;

            foreach(List<Cubeta> cub in cajonActual.listaCubetas)
            {
                foreach(Cubeta c in cub)
                {
                    long apDato = c.regresa_apDato();

                    if(cajonSeleccionado == apDato)
                    {
                        cubetaEncontrada = c;
                        bandEncontrada = true;
                        break;
                    }
                }

                if(bandEncontrada == true)
                {
                    break;
                }
            }

            if(cubetaEncontrada.regresa_apDato() == -1)
            {
                toolStripStatusLabel1.Text = "Error, esta cubeta no tiene ningun dato.";
            }
            else
            {
                rellena_dataGrid_datos(cubetaEncontrada);
            }
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
