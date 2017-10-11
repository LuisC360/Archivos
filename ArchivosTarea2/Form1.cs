using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ArchivosTarea2
{
    public partial class Form1 : Form
    {
        // La lista de entidades (por modificar)
        List<object> data = new List<object>();
        // Para lectura
        List<Entidad> entidades = new List<Entidad>();
        // Lista que contendra los tipos de dato disponibles
        List<char> tiposDato = new List<char>();
        // Lista para entidades leidas
        List<Entidad> entidadesLeidas = new List<Entidad>();
        // Tamaño de una entidad
        int tamEntidad = 62;
        // Tamaño de un atributo
        int tamAtributo = 56;
        // Tamaño de un dato
        int tamDato = 8;
        // Valores que posee una entidad (para lectura de archivo)
        char[] nombre = { '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0',  
                                    '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0' ,'\0' ,'\n'};
        char[] nombreAT = { '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0',  
                                    '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0' ,'\0' ,'\n'};
        long apAtributos;
        long apDatos;
        long posInicial;
        long apSigEntidad;
        // Booleano para verificar que se abrio un archivo
        Boolean seAbrio;
        // Numero que nos ayudara a saber en que posicion en memoria nos encontramos
        long posicionMemoria = 8;
        Entidad entidadEliminada;
        Atributo atributoEliminado;
        // El rango para los archivos secuuenciales indexados
        long rango;
        // Tipos de archivo que se manejaran. Para el caso de secuencial ordenado, sera el caso por defecto.
        bool secIndexado = false;
        bool hashEstatica = false;
        bool multiLlave = false;

        public Form1()
        {
            InitializeComponent();
            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            rellena_lista_tipo();
            toolStripStatusLabel1.Text = "Listo.";
        }

        // Boton para abrir archivo
        private void button1_Click(object sender, EventArgs e)
        {
            data.Clear();
            entidadEliminada = new Entidad();
            atributoEliminado = new Atributo();

            if(textBox1.Text.Length > 0)
            {
                // Si el archivo no existe, se crea
                if (File.Exists(@"C:\Users\DanielCorona\Documents\Visual Studio 2013\Projects\ArchivosTarea2Respaldo\ArchivosTarea2\ArchivosTarea2\bin\Debug\" + textBox1.Text) == false)
                {
                    // Funcion que creara el archivo binario
                    crea_archivo(textBox1.Text);

                    // Funcion que nos auxiliara con el manejo del dataGridView
                    manejo_dataGrid(textBox1.Text);

                    textBox2.ReadOnly = false;
                    textBox3.ReadOnly = false;
                    comboBox1.Enabled = true;
                    comboBox2.Enabled = true;
                    
                    toolStripStatusLabel1.Text = "Archivo creado con exito.";
                }
                else // Si el archivo ya existe, se abre
                {
                    try
                    {
                        manejo_dataGrid(textBox1.Text);

                        textBox2.ReadOnly = false;
                        textBox3.ReadOnly = false;
                        comboBox1.Enabled = true;
                        comboBox2.Enabled = true;

                        toolStripStatusLabel1.Text = "Archivo abierto con exito."; 
                    }
                    catch
                    {
                        toolStripStatusLabel1.Text = "Error.";
                    }
                }
            }
        }

        // Boton para añadir Entidad
        private void button2_Click(object sender, EventArgs e)
        {
            if(seAbrio == true && textBox2.Text.Length > 0 && textBox2.Text.Length < 29)
            {
                // Si el archivo especificado no existe, se crea
                if (File.Exists(@"C:\Users\DanielCorona\Documents\Visual Studio 2013\Projects\ArchivosTarea2Respaldo\ArchivosTarea2\ArchivosTarea2\bin\Debug\" + 
                    textBox1.Text) == false)
                {
                    crea_archivo(textBox1.Text);

                    manejo_dataGrid(textBox1.Text);

                    toolStripStatusLabel1.Text = "Archivo creado con exito.";
                }
                else // Si el archivo ya existe
                {
                    try
                    {
                        // Validacion
                        if (validacion(textBox2.Text) == false)
                        {
                            // Se crea una lista actualizada
                            List<Entidad> listaActualizada = new List<Entidad>();
                            long cabecera = 8;

                            // Se meten a esta lista actualizada todas las Entidades salvo las ultimas dos
                            for (int i = 0; i < entidades.Count - 1; i++)
                            {
                                listaActualizada.Add(entidades[i]);
                            }

                            // Se crea la entidad nueva
                            Entidad entidad = new Entidad(textBox2.Text);
                            Entidad ultimaEntidad;

                            if (entidades.Count != 0)
                            {
                                ultimaEntidad = entidades[entidades.Count - 1];

                                entidad.posEntidad = posicionMemoria;
                                ultimaEntidad.apSigEntidad = entidad.posEntidad;

                                data.Clear();
                                entidades.Clear();
                                data.Add(cabecera);

                                foreach (Entidad ent in listaActualizada)
                                {
                                    data.Add(ent);
                                    entidades.Add(ent);
                                }

                                data.Add(ultimaEntidad);
                                entidades.Add(ultimaEntidad);
                            }
                            else
                            {
                                entidad.posEntidad += 8;
                            }

                            if (entidades.Count == 0 && data.Count == 1)
                            {
                                data.Clear();
                                data.Add(cabecera);
                            }

                            data.Add(entidad);
                            entidades.Add(entidad);

                            escribe_archivo(textBox1.Text);

                            entidadesLeidas = new List<Entidad>();

                            manejo_dataGrid(textBox1.Text);

                            toolStripStatusLabel1.Text = "Archivo abierto con exito.";
                        }
                        else
                        {
                            toolStripStatusLabel1.Text = "Error, entidad ya existente.";
                        }
                    }
                    catch(Exception err)
                    {
                        toolStripStatusLabel1.Text = "Error.";
                        MessageBox.Show(err.ToString());
                    }
                }
            }
            else if(seAbrio == false)
            {
                toolStripStatusLabel1.Text = "Error, defina un nombre de archivo.";
            }
            else if(textBox2.Text.Length == 0)
            {
                toolStripStatusLabel1.Text = "Error, defina un nombre de entidad.";
            }
            else if(textBox2.Text.Length < 29)
            {
                toolStripStatusLabel1.Text = "Error, longitud de cadena no valida.";
            }
        }

        // Boton para modificar una Entidad (o al menos solo el nombre de este)
        private void button3_Click(object sender, EventArgs e)
        {
            if (seAbrio == true && textBox2.Text.Length > 0)
            {
                if (validacion(textBox2.Text) == true)
                {
                    String nuevoNombreEntidad = "";

                    using(BusquedaModifica buscaM = new BusquedaModifica())
                    {
                        var nuevoNombre = buscaM.ShowDialog();

                        if(nuevoNombre == DialogResult.OK)
                        {
                            nuevoNombreEntidad = buscaM.newNombre;
                        }
                    }

                    if (validacion(nuevoNombreEntidad) == false)
                    {
                        char[] nuevoNombreArr = new char[30];
                        nuevoNombreArr[29] = '\n';
                        char[] viejoNombreArr = new char[30];
                        viejoNombreArr[29] = '\n';

                        for (int i = 0; i < nuevoNombreEntidad.Length; i++)
                        {
                            nuevoNombreArr[i] = nuevoNombreEntidad[i];
                        }

                        for (int i = 0; i < textBox2.Text.Length; i++)
                        {
                            viejoNombreArr[i] = textBox2.Text[i];
                        }

                        Entidad eN = new Entidad();

                        foreach (Entidad ent in entidades)
                        {
                            if (ent.nombre.SequenceEqual(viejoNombreArr) == true)
                            {
                                eN = ent;
                                break;
                            }
                        }

                        actualiza_archivo_entidad(eN, nuevoNombreArr, eN.apAtributos, eN.apDatos, eN.posEntidad, eN.apSigEntidad);

                        entidadesLeidas = new List<Entidad>();

                        manejo_dataGrid(textBox1.Text);

                        toolStripStatusLabel1.Text = "Entidad modificada con exito.";
                    }
                    else
                    {
                        toolStripStatusLabel1.Text = "Error, nuevo nombre coincide con entidad ya existente.";
                    }
                }
                else
                {
                    toolStripStatusLabel1.Text = "Error, no se encontro la entidad.";
                }
            }
        }

        // Boton para eliminar una Entidad
        private void button4_Click(object sender, EventArgs e)
        {
            if(seAbrio == true)
            {
                char[] nombreEntidadA = new char[30];
                nombreEntidadA[29] = '\n';
                Boolean seEncontro = false;
                Entidad entidadEliminar = new Entidad();

                if(validacion(textBox2.Text) == true)
                {
                    seEncontro = true;

                    for(int i = 0; i < textBox2.Text.Length; i++)
                    {
                        nombreEntidadA[i] = textBox2.Text[i];
                    }
                }

                if (seEncontro == false)
                {
                    toolStripStatusLabel1.Text = "Error, entidad no encontrada.";
                }
                else
                {
                    // Se busca la entidad a ser eliminada
                    foreach(Entidad ent in entidades)
                    {
                        if (ent.nombre.SequenceEqual(nombreEntidadA) == true)
                        {
                            entidadEliminar = ent;
                            if (entidadEliminar.apDatos > -1)
                            {
                                toolStripStatusLabel1.Text = "Error, la entidad tiene datos y no se puede eliminar.";
                                return;
                            }
                            entidadEliminada = entidadEliminar;
                            entidadEliminada.apSigEntidad = entidadEliminar.apSigEntidad = -2;
                            break;
                        }
                    }
                   
                    List<Entidad> dataRespaldo = new List<Entidad>();

                    foreach (Entidad enti in entidades)
                    {
                        if(enti != entidadEliminar)
                        {
                            dataRespaldo.Add(enti);
                        }
                    }

                    for (int i = 0; i < entidades.Count; i++ )
                    {
                        // Si la entidad a eliminar es la primera o la unica
                        if(entidades[0] == entidadEliminar)
                        {
                            // Si es la unica
                            if(entidades.Count == 1)
                            {
                                entidadEliminar.apSigEntidad = -2;

                                entidadesLeidas.Clear();

                                escribe_archivo(textBox1.Text);

                                manejo_dataGrid(textBox1.Text);

                                break;
                            }
                            else // Si es la primera
                            {
                                entidadesLeidas.Clear();

                                foreach(Entidad en in entidades)
                                {
                                    if (en != entidadEliminar)
                                    {
                                        if (en == dataRespaldo[0])
                                        {
                                            data[0] = en.posEntidad;
                                        }                                        
                                    }
                                    else
                                    {
                                        en.apSigEntidad = -2;
                                    }
                                }

                                escribe_archivo(textBox1.Text);

                                manejo_dataGrid(textBox1.Text);

                                break;
                            }
                        }
                        else if(entidades[i+1] == entidadEliminar && i+2 < data.Count)
                        {
                            // Si la entidad a eliminar esta entre 2 entidades
                            for (int it = 0; it < entidades.Count; it++ )
                            {
                                // Si entidad1 -> entidadAeliminar -> entidad3
                                if((it+1) < entidades.Count && entidades[it+1] == entidadEliminar && (it+2) < entidades.Count)
                                {
                                    entidades[it].apSigEntidad += tamEntidad;
                                    entidades[it + 1].apSigEntidad = -2;
                                }
                                else if((it+1) < entidades.Count && entidades[it+1] == entidadEliminar) // Si entidad1 -> entidadAeliminar
                                {
                                    entidades[it].apSigEntidad = -1;
                                    entidades[it + 1].apSigEntidad = -2;
                                }
                                else if(entidades[it] == entidadEliminar)
                                {
                                    entidades[it].apSigEntidad = -2;
                                }
                            }

                            escribe_archivo(textBox1.Text);

                            manejo_dataGrid(textBox1.Text);

                            break;
                        }                        
                    }

                    if(entidadEliminar.apAtributos < 0)
                    {
                        dataGridView2.Rows.Clear();

                        dataGridView2.ColumnCount = 6;
                        dataGridView2.ColumnHeadersVisible = true;

                        dataGridView2.Columns[0].Name = "Nombre";
                        dataGridView2.Columns[1].Name = "Tipo";
                        dataGridView2.Columns[2].Name = "Longitud";
                        dataGridView2.Columns[3].Name = "Pos. del Atributo";
                        dataGridView2.Columns[4].Name = "Ap. Sig. Atributo";
                        dataGridView2.Columns[5].Name = "Es llave primaria";
                    }

                    toolStripStatusLabel1.Text = "Entidad eliminada con exito.";
                }
            }
        }

        /// <summary>
        /// En esta funcion se validara si una entidad ya ha sido insertado en nuestra lista de entidades
        /// </summary>
        /// <param name="nombreAtributo">El nombre de la entidad que se desea insertar</param>
        /// <returns>Un booleano que indica si la entidad ya existe o no</returns>
        public bool validacion(String nombreEntidad)
        {
            bool yaExiste = false;

            char[] nombreComparar = {'\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0',  
                                    '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0' ,'\0' ,'\n'};
            int it = 0;

            foreach(char c in nombreEntidad)
            {
                nombreComparar[it] = c;
                it++;
            }

            foreach(Entidad e in entidades)
            {
                if(e.nombre.SequenceEqual(nombreComparar) && e.apSigEntidad > -2)
                {
                    yaExiste = true;
                    break;
                }
            }

            return yaExiste;
        }

        /// <summary>
        /// En esta funcion se validara si un atributo ya ha sido insertado en nuestra lista de atributos dentro
        /// de una entidad
        /// </summary>
        /// <param name="nombreAtributo">El nombre del atributo que se desea insertar</param>
        /// <param name="entidadSeleccionada">La entidad que contiene la lista de atributos sobre la que se hara la busqueda</param>
        /// <returns>Un booleano que indica si el atributo ya existe o no</returns>
        static bool valida_atributo(String nombreAtributo, Entidad entidadSeleccionada)
        {
            bool yaExiste = false;

            char[] nombreComparar = {'\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0',  
                                    '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0' ,'\0' ,'\n'};
            int it = 0;

            foreach (char c in nombreAtributo)
            {
                nombreComparar[it] = c;
                it++;
            }

            foreach(Atributo at in entidadSeleccionada.listaAtributos)
            {
                if(at.nombre.SequenceEqual(nombreComparar) && at.apSigAtributo != -2 && at.apSigAtributo != -4)
                {
                    yaExiste = true;
                    break;
                }
            }

            return yaExiste;
        }

        /// <summary>
        /// Sobrecarga de la funcion de validacion. En este caso, si el atributo en cuestion va a tener el mismo nombre que tenia 
        /// anteriormente, se cambiara de todos modos
        /// </summary>
        /// <param name="nombreAtributo">El nombre "nuevo" que se le dara al atributo</param>
        /// <param name="entidadSeleccionada">La entidad que contiene a la lista de atributos</param>
        /// <param name="at">El atributo al que se le pretende cambiar el nombre</param>
        /// <returns></returns>
        static bool es_mismo_atributo(String nombreAtributo, Entidad entidadSeleccionada, Atributo at)
        {
            bool esElMismo = false;

            char[] nombreComparar = {'\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0',  
                                    '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0' ,'\0' ,'\n'};
            int it = 0;

            foreach (char c in nombreAtributo)
            {
                nombreComparar[it] = c;
                it++;
            }

            foreach (Atributo a in entidadSeleccionada.listaAtributos)
            {
                if (a.nombre.SequenceEqual(nombreComparar) && at.apSigAtributo != -2 && at.apSigAtributo != -4)
                {
                    esElMismo = true;
                    break;
                }
            }

            return esElMismo;
        }

        /// <summary>
        /// Funcion que devolvera un atributo encontrado en la lista de atributos de la entidad
        /// </summary>
        /// <param name="nombreAtributo">El "nuevo" nombre del atributo</param>
        /// <param name="entidadSeleccionada">La entidad con la lista en la que se buscara el atributo</param>
        /// <returns>Un atributo que puede o estar vacio o existir dentro de la lista de atributos de la entidad seleccionada</returns>
        Atributo regresa_atributo(String nombreAtributo, Entidad entidadSeleccionada)
        {
            Atributo atr = new Atributo();

            char[] nombreComparar = {'\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0',  
                                    '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0' ,'\0' ,'\n'};
            int it = 0;

            foreach (char c in nombreAtributo)
            {
                nombreComparar[it] = c;
                it++;
            }

            foreach (Atributo a in entidadSeleccionada.listaAtributos)
            {
                if (a.nombre.SequenceEqual(nombreComparar) && a.apSigAtributo != -2 && a.apSigAtributo != -4)
                {
                    atr = a;
                    break;
                }
            }

            return atr;
        }

        /// <summary>
        /// Esta funcion buscara una entidad dentro de nuestra lista de entidades mediante su nombre. Si la entidad existe, se devolvera
        /// dicha entidad
        /// </summary>
        /// <param name="nombreEntidad">Nombre de la entidad a ser buscada</param>
        /// <returns>Regresa la entidad que se busco en caso de un resultado positivo. En caso contrario, devolvera una entidad vacia,
        /// la cual sera identificada si su posicion inicial es igual a 0</returns>
        Entidad busca_entidad(String nombreEntidad)
        {
            Entidad entidadDevolver = new Entidad();

            char[] nombreComparar = {'\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0',  
                                    '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0' ,'\0' ,'\n'};
            int it = 0;

            foreach (char c in nombreEntidad)
            {
                nombreComparar[it] = c;
                it++;
            }

            foreach(Entidad ent in entidades)
            {
                if(ent.nombre.SequenceEqual(nombreComparar) && ent.apSigEntidad > -2)
                {
                    entidadDevolver = ent;
                    break;
                }
            }

            return entidadDevolver;
        }

        /// <summary>
        /// En esta funcion se haran 3 cosas importantes:
        /// 1- Refrescar e inicializar ambos dataGridView
        /// 2- Leer el archivo binario especificado en el parametro "archivo"
        /// 3- Poblar el dataGridView con los datos del archivo en el orden correspondiente
        /// </summary>
        /// <param name="archivo">Nombre del archivo a abrirse</param>
        public void manejo_dataGrid(String archivo)
        {
            posicionMemoria = 8;
            tamDato = 8;

            data.Clear();
            entidades.Clear();
            entidadesLeidas.Clear();

            // Refrescar el dataGridView
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            // Se inicializa el dataGridView correspondiente
            dataGridView1.ColumnCount = 5;
            dataGridView1.ColumnHeadersVisible = true;            

            dataGridView1.Columns[0].Name = "Nombre";
            dataGridView1.Columns[1].Name = "Ap. Atributos";
            dataGridView1.Columns[2].Name = "Ap. Datos";
            dataGridView1.Columns[3].Name = "Pos. Inicial";
            dataGridView1.Columns[4].Name = "Ap. Sig. Entidad";            

            // Se lee el archivo binario
            FileStream streamR = new FileStream(archivo, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(streamR);

            Boolean bandCabecera = false;
            long archivoPos;

            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                if (bandCabecera == false)
                {
                    long header = reader.ReadInt64();
                    data.Add(header);
                    bandCabecera = true;
                }

                if(secIndexado == true)
                {
                    long rang = reader.ReadInt64();
                    rango = rang;
                    button9.Enabled = false;
                }

                if (reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    for (int i = 0; i < nombre.Length; i++)
                    {
                        char car = reader.ReadChar();
                        nombre[i] = car;
                    }

                    long apAtr = reader.ReadInt64();
                    long apDt = reader.ReadInt64();
                    long posIn = reader.ReadInt64();
                    long apSigE = reader.ReadInt64();
                    archivoPos = reader.BaseStream.Position;

                    Entidad nEntidad = new Entidad(nombre, apAtr, apDt, posIn, apSigE);

                    posicionMemoria = posicionMemoria + tamEntidad;

                    // Verificar si la entidad tiene atributos
                    if(nEntidad.apAtributos != -1)
                    {
                        lee_atributos_de_entidad(streamR, reader, nEntidad);

                        if (nEntidad.apDatos != -1)
                        {
                            lee_datos_de_entidad(streamR, reader, nEntidad);
                        }
                    }
                    else
                    {
                        dataGridView2.Rows.Clear();

                        dataGridView2.ColumnCount = 6;
                        dataGridView2.ColumnHeadersVisible = true;

                        dataGridView2.Columns[0].Name = "Nombre";
                        dataGridView2.Columns[1].Name = "Tipo";
                        dataGridView2.Columns[2].Name = "Longitud";
                        dataGridView2.Columns[3].Name = "Pos. del Atributo";
                        dataGridView2.Columns[4].Name = "Ap. Sig. Atributo";
                        dataGridView2.Columns[5].Name = "Es llave primaria";
                    }

                    // Verificar si la entidad no fue eliminada
                    entidadesLeidas.Add(nEntidad);
                    data.Add(nEntidad);
                    entidades.Add(nEntidad);

                    nombre = new char[30];
                    nombre[29] = '\n';
                }
            }

            // Se popula el primer dataGridView con los datos de las entidades
            List<String[]> filas = new List<string[]>();
            String[] fila = new string[] { };
            String nombreEntidad = "";
            long apAt = 0;
            long apDat = 0;
            long posInic = 0;
            long apSigAt = 0;

            foreach (Entidad ent in entidadesLeidas)
            {
                if (ent.apSigEntidad > -2)
                {
                    nombreEntidad = new string(ent.nombre);
                    apAt += ent.apAtributos;
                    apDat += ent.apDatos;
                    posInic += ent.posEntidad;
                    apSigAt += ent.apSigEntidad;

                    if (ent.apAtributos < -1)
                    {
                        apAt = -1;
                    }

                    if(ent.apDatos < -1)
                    {
                        apDat = -1;
                    }

                    fila = new string[] { nombreEntidad, apAt.ToString(), apDat.ToString(), posInic.ToString(), apSigAt.ToString() };

                    nombreEntidad = "";
                    apAt = 0;
                    apDat = 0;
                    posInic = 0;
                    apSigAt = 0;

                    filas.Add(fila);
                    fila = new string[] { };
                }
            }

            foreach (string[] arr in filas)
            {
                dataGridView1.Rows.Add(arr);
            }

            reader.Close();
            streamR.Close();

            seAbrio = true;
        }

        /// <summary>
        /// En esta funcion se rellenará el dataGridView correspondiente a los atributos de una entidad.
        /// </summary>
        /// <param name="ent">La entidad que posee la lista de atributos</param>
        void manejo_dataGrid_atributos(Entidad ent)
        {
            dataGridView2.Rows.Clear();

            dataGridView2.ColumnCount = 6;
            dataGridView2.ColumnHeadersVisible = true;

            dataGridView2.Columns[0].Name = "Nombre";
            dataGridView2.Columns[1].Name = "Tipo";
            dataGridView2.Columns[2].Name = "Longitud";
            dataGridView2.Columns[3].Name = "Pos. del Atributo";
            dataGridView2.Columns[4].Name = "Ap. Sig. Atributo";
            dataGridView2.Columns[5].Name = "Es llave primaria";

            List<String[]> filas = new List<string[]>();
            String[] fila = new string[] { };
            String nombreAtributo = "";
            char tipo;
            long longitud = 0;
            long posAt = 0;
            long apSigAt = 0;
            bool isLlave;

            foreach(Atributo at in ent.listaAtributos)
            {
                if (at.apSigAtributo != -2 && at.apSigAtributo != -4)
                {
                    nombreAtributo = new String(at.nombre);
                    tipo = at.tipo;
                    longitud = at.bytes;
                    posAt = at.posAtributo;
                    apSigAt = at.apSigAtributo;
                    isLlave = at.esLlavePrimaria;

                    if(apSigAt < -1)
                    {
                        apSigAt = -1;
                    }

                    if(at.tipo == 'S')
                    {
                        longitud = longitud / 2;
                    }

                    fila = new string[] { nombreAtributo, tipo.ToString(), longitud.ToString(), posAt.ToString(), apSigAt.ToString(), isLlave.ToString() };

                    nombreAtributo = "";
                    tipo = '\0';
                    longitud = 0;
                    posAt = 0;
                    apSigAt = 0;
                    isLlave = false;

                    filas.Add(fila);
                    fila = new string[] { };
                }
            }

            foreach (string[] arr in filas)
            {
                dataGridView2.Rows.Add(arr);
            }
        }

        /// <summary>
        /// Funcion recursiva que se encargara de leer todos los atributos de una entidad en un archivo
        /// </summary>
        /// <param name="f">El FileStream con el que se manipulan los archivos</param>
        /// <param name="r">El lector de archivos binarios</param>
        /// <param name="ent">La entidad que contiene los atributos</param>
        void lee_atributos_de_entidad(FileStream f, BinaryReader r, Entidad ent)
        {
            // Leer nombre de atributo
            for (int i = 0; i < nombreAT.Length; i++)
            {
                char car = r.ReadChar();
                nombreAT[i] = car;
            }
            
            char type = r.ReadChar();
            long lon = r.ReadInt64();
            long posAt = r.ReadInt64();
            bool isKey = r.ReadBoolean();
            long apSigAt = r.ReadInt64();

            Atributo nAtributo = new Atributo(nombreAT, type, lon, posAt, isKey, apSigAt);

            posicionMemoria = posicionMemoria + tamAtributo;

            ent.listaAtributos.Add(nAtributo);

            if (apSigAt != -2 && apSigAt != -4)
            {
                tamDato += Convert.ToInt32(nAtributo.bytes);
            }

            nombreAT = new char[30];
            nombreAT[29] = '\n';

            if(apSigAt != -1 && apSigAt != -4)
            {
                lee_atributos_de_entidad(f, r, ent);
            }
        }

        /// <summary>
        /// Funcion recursiva que se encargara de de leer los datos de la entidad
        /// </summary>
        /// <param name="f">El FileStream con el que se manipulan los archivos</param>
        /// <param name="r">El lector de archivos binarios</param>
        /// <param name="ent">La entidad que contiene la lista de datos</param>
        void lee_datos_de_entidad(FileStream f, BinaryReader r, Entidad ent)
        {
            Dato dataRead = new Dato(ent);

            // Primero se recorre la lista de atributos
            foreach(Atributo atr in ent.listaAtributos)
            {
                // Verificamos si el atributo no fue borrado
                if(atr.apSigAtributo != -2 && atr.apSigAtributo != -4)
                {
                    // Si no fue borrado, entonces revisamos de que tipo es ese atributo para leer el dato
                    if(atr.tipo == 'I')
                    {
                        try
                        {
                            int datoI = r.ReadInt32();
                            dataRead.datos.Add(datoI);
                        }
                        catch 
                        {
                            toolStripStatusLabel1.Text = "Error al leer los datos.";
                            return;
                        }
                    }
                    else if (atr.tipo == 'F')
                    {
                        try
                        {
                            float datoI = r.ReadSingle();
                            dataRead.datos.Add(datoI);
                        }
                        catch
                        {
                            toolStripStatusLabel1.Text = "Error al leer los datos.";
                            return;
                        }
                    }
                    else if (atr.tipo == 'D')
                    {
                        try
                        {
                            double datoI = r.ReadDouble();
                            dataRead.datos.Add(datoI);
                        }
                        catch
                        {
                            toolStripStatusLabel1.Text = "Error al leer los datos.";
                            return;
                        }
                    }
                    else if (atr.tipo == 'L')
                    {
                        try
                        {
                            long datoI = r.ReadInt64();
                            dataRead.datos.Add(datoI);
                        }
                        catch
                        {
                            toolStripStatusLabel1.Text = "Error al leer los datos.";
                            return;
                        }
                    }
                    else if (atr.tipo == 'C')
                    {
                        try
                        {
                            char datoI = r.ReadChar();
                            dataRead.datos.Add(datoI);
                        }
                        catch
                        {
                            toolStripStatusLabel1.Text = "Error al leer los datos.";
                            return;
                        }
                    }
                    else if (atr.tipo == 'S')
                    {
                        try
                        {
                            char[] chara = new char[atr.bytes / 2];

                            for(int i = 0; i < chara.Length; i++)
                            {
                                chara[i] = r.ReadChar();
                            }

                            dataRead.datos.Add(chara);
                        }
                        catch
                        {
                            toolStripStatusLabel1.Text = "Error al leer los datos.";
                            return;
                        }
                    }
                }
                else
                {
                    // Seek
                    switch(atr.tipo)
                    {
                        case 'I': r.ReadInt32();
                            break;
                        case 'F': r.ReadSingle();
                            break;
                        case 'C': r.ReadChar();
                            break;
                        case 'D': r.ReadDouble();
                            break;
                        case 'L': r.ReadInt64();
                            break;
                        case 'S': char[] chara = new char[atr.bytes / 2];

                                  for (int i = 0; i < chara.Length; i++)
                                  {
                                        chara[i] = r.ReadChar();
                                  }
                                  break;
                    }
                }
            }

            long apSigD = r.ReadInt64();

            dataRead.apSigDato = apSigD;

            posicionMemoria += tamDato;

            ent.listaDatos.Add(dataRead);

            if(dataRead.apSigDato != -1 && dataRead.apSigDato != -4)
            {
                lee_datos_de_entidad(f, r, ent);
            }
        }

        /// <summary>
        /// Esta funcion nos creara un archivo binario nuevo, escribiendo en el la cabecera de la lista de entidades y, de ser
        /// necesario, insertar la primera entidad dentro del archivo con sus valores correspondientes
        /// </summary>
        /// <param name="nombreArchivo">El nombre del archivo a crearse</param>
        public void crea_archivo(String nombreArchivo)
        {
            FileStream stream = new FileStream(nombreArchivo, FileMode.Create, FileAccess.Write);
            BinaryWriter writer = new BinaryWriter(stream);
            long cabecera = -1;

            data.Add(cabecera);

            for (int i = 0; i < data.Count; i++)
            {
                if (i == 0)
                {
                    writer.Write(cabecera);
                }
                else
                {
                    for (int j = 0; j < entidades.Count; j++)
                    {
                        for (int k = 0; k < entidades[j].nombre.Length; k++)
                        {
                            writer.Write(entidades[j].nombre[k]);
                        }

                        writer.Write(entidades[j].apAtributos);
                        writer.Write(entidades[j].apDatos);
                        writer.Write(entidades[j].posEntidad);
                        writer.Write(entidades[j].apSigEntidad);
                    }
                }
            }

            writer.Close();
            stream.Close();
        }

        /// <summary>
        /// Esta funcion es parecida a la de crea_archivo, pero difiere en el hecho de que no crea el archivo, solo lo actualiza
        /// </summary>
        /// <param name="archivo">El nombre del archivo que sera creado</param>
        public void escribe_archivo(String archivo)
        {
            FileStream stream = new FileStream(archivo, FileMode.Create, FileAccess.Write);
            BinaryWriter writer = new BinaryWriter(stream);
            long cabecera = 8;

            for (int i = 0; i < data.Count; i++)
            {
                if (i == 0)
                {
                    writer.Write(cabecera);
                }
            }

            if(secIndexado == true)
            {
                writer.Write(rango);
            }

            for (int j = 0; j < entidades.Count; j++)
            {
                for (int k = 0; k < entidades[j].nombre.Length; k++)
                {
                    writer.Write(entidades[j].nombre[k]);
                }

                writer.Write(entidades[j].apAtributos);
                writer.Write(entidades[j].apDatos);
                writer.Write(entidades[j].posEntidad);
                writer.Write(entidades[j].apSigEntidad);

                if (entidades[j].apAtributos != -1)
                {
                    for (int l = 0; l < entidades[j].listaAtributos.Count; l++)
                    {
                        for (int m = 0; m < entidades[j].listaAtributos[l].nombre.Length; m++)
                        {
                            writer.Write(entidades[j].listaAtributos[l].nombre[m]);
                        }

                        writer.Write(entidades[j].listaAtributos[l].tipo);
                        writer.Write(entidades[j].listaAtributos[l].bytes);
                        writer.Write(entidades[j].listaAtributos[l].posAtributo);
                        writer.Write(entidades[j].listaAtributos[l].esLlavePrimaria);
                        writer.Write(entidades[j].listaAtributos[l].apSigAtributo);
                    }
                }

                if(entidades[j].apDatos != -1)
                {
                    for(int n = 0; n < entidades[j].listaDatos.Count;n++)
                    {
                        for(int m = 0; m < entidades[j].listaDatos[n].datos.Count;m++)
                        {
                            if (entidades[j].listaDatos[n].listaAtributosDato[m].tipo == 'I')
                            {
                                writer.Write(Convert.ToInt32(entidades[j].listaDatos[n].datos[m]));
                            }
                            else if (entidades[j].listaDatos[n].listaAtributosDato[m].tipo == 'F')
                            {
                                writer.Write((float)entidades[j].listaDatos[n].datos[m]);
                            }
                            else if (entidades[j].listaDatos[n].listaAtributosDato[m].tipo == 'L')
                            {
                                writer.Write(Convert.ToInt64(entidades[j].listaDatos[n].datos[m]));
                            }
                            else if (entidades[j].listaDatos[n].listaAtributosDato[m].tipo == 'D')
                            {
                                writer.Write(Convert.ToDouble(entidades[j].listaDatos[n].datos[m]));
                            }
                            else if (entidades[j].listaDatos[n].listaAtributosDato[m].tipo == 'C')
                            {
                                writer.Write(Convert.ToChar(entidades[j].listaDatos[n].datos[m]));
                            }
                            else if(entidades[j].listaDatos[n].listaAtributosDato[m].tipo == 'S')
                            {
                                String nuSt = "";

                                if(entidades[j].listaDatos[n].datos[m] is char[])
                                {
                                    nuSt = new string((char[])entidades[j].listaDatos[n].datos[m]);
                                }

                                char[] cadenaTemporal = new char[entidades[j].listaDatos[n].listaAtributosDato[m].bytes / 2];

                                for(int i = 0; i < entidades[j].listaDatos[n].listaAtributosDato[m].bytes / 2; i++)
                                {
                                    cadenaTemporal[i] = '\0';
                                }

                                if (nuSt.Length > 0)
                                {
                                    for(int o = 0; o < nuSt.Length; o++)
                                    {
                                        cadenaTemporal[o] = nuSt[o];
                                    }
                                }
                                else
                                {
                                    for (int o = 0; o < entidades[j].listaDatos[n].datos[m].ToString().Length; o++)
                                    {
                                        cadenaTemporal[o] = entidades[j].listaDatos[n].datos[m].ToString()[o];
                                    }
                                }

                                for(int p = 0; p < cadenaTemporal.Length; p++)
                                {
                                    writer.Write(cadenaTemporal[p]);
                                }
                            }
                        }

                        writer.Write(entidades[j].listaDatos[n].apSigDato);
                    }
                }
            }

            writer.Close();
            stream.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="nombre"></param>
        /// <param name="apAt"></param>
        /// <param name="apD"></param>
        /// <param name="posE"></param>
        /// <param name="apSE"></param>
        public void actualiza_archivo_entidad(Entidad e, char[] nombre, long apAt, long apD, long posE, long apSE)
        {
            FileStream streamR = new FileStream(textBox1.Text, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            BinaryReader reader = new BinaryReader(streamR);
            BinaryWriter writer = new BinaryWriter(streamR);

            long posEnt = e.posEntidad;
            reader.BaseStream.Seek(posEnt, SeekOrigin.Begin);

            for (int i = 0; i < nombre.Length; i++)
            {
                writer.Write(nombre[i]);
            }

            writer.Write(apAt);
            writer.Write(apD);
            writer.Write(posE);
            writer.Write(apSE);

            if(apAt > -1)
            {
                for (int l = 0; l < e.listaAtributos.Count; l++)
                {
                    for (int m = 0; m < e.listaAtributos[l].nombre.Length; m++)
                    {
                        writer.Write(e.listaAtributos[l].nombre[m]);
                    }

                    writer.Write(e.listaAtributos[l].tipo);
                    writer.Write(e.listaAtributos[l].bytes);
                    writer.Write(e.listaAtributos[l].posAtributo);
                    writer.Write(e.listaAtributos[l].esLlavePrimaria);
                    writer.Write(e.listaAtributos[l].apSigAtributo);
                }
            }

            writer.Close();
            reader.Close();
            streamR.Close();           
        }

        /// <summary>
        /// Funcion que nos rellenara la lista de tipos disponibles que se añadiran al comboBox correspondiente
        /// </summary>
        public void rellena_lista_tipo()
        {
            char[] tipos = { 'I', 'F', 'C', 'S', 'D', 'L', 'B' };

            for (int i = 0; i < tipos.Length; i++ )
            {
                tiposDato.Add(tipos[i]);
            }

            for(int i = 0; i < tiposDato.Count; i++)
            {
                comboBox1.Items.Add(tiposDato[i]);
            }

            comboBox2.Items.Add("Si");
            comboBox2.Items.Add("No");
        }

        /// <summary>
        /// Funcion que, en base al indice del item escojido en el comboBox correspondiente a la seleccion del tipo de dato del 
        /// atributo, regresará el valor del tamaño del tipo de dato correspondiente. Los valores podrian diferir respecto a los vistos
        /// en clase.
        /// </summary>
        /// <returns>Un entero con el tamaño del tipo de dato elegido.</returns>
        public int escoje_num_bytes()
        {
            int numBytes = 0;
            int selectedIndex;

            selectedIndex = comboBox1.SelectedIndex;

            switch(selectedIndex)
            {
                case 0: numBytes = sizeof(int);
                        break;
                case 1: numBytes = sizeof(float);
                        break;
                case 2: numBytes = sizeof(char);
                        break;
                case 3: using (TamString tamaS = new TamString())
                        {
                            var tamStri = tamaS.ShowDialog();

                            if (tamStri == DialogResult.OK)
                            {
                                numBytes = tamaS.numCaracteres * 2;
                            }
                        }
                        break;
                case 4: numBytes = sizeof(double);
                        break;
                case 5: numBytes = sizeof(long);
                        break;
                case 6: numBytes = sizeof(bool);
                        break;
            }

            return numBytes;
        }

        /// <summary>
        /// Funcion que verificará si alguno de los atributos de la entidad ent ya es llave primaria. Esto debido a que no se puede
        /// tener mas de una llave primaria en nuestro archivo.
        /// </summary>
        /// <param name="ent">La entidad que tiene la lista de atributos a ser verificada.</param>
        /// <returns>Un booleano que nos indicara que ya se tiene un atributo como llave primaria.</returns>
        bool verifica_llave_primaria(Entidad ent)
        {
            bool yaTieneLlave = false;

            foreach(Atributo at in ent.listaAtributos)
            {
                if(at.esLlavePrimaria == true && at.apSigAtributo != -2 && at.apSigAtributo != -3)
                {
                    yaTieneLlave = true;
                    break;
                }
            }

            return yaTieneLlave;
        }

        // Boton para añadir un atributo a una entidad.
        private void button5_Click(object sender, EventArgs e)
        {            
            if (seAbrio == true && textBox2.Text.Length > 0 && textBox3.Text.Length > 0)
            {
                // Primero hay que verificar que la entidad a la que se le quiere insertar el atributo exista
                if (validacion(textBox2.Text) == true)
                {
                    // Luego, se busca la entidad en la lista de entidades
                    Entidad entidadEncontrada = busca_entidad(textBox2.Text);

                    if(entidadEncontrada.posEntidad > 0)
                    {
                        // Despues se hace la validacion para ver si ese atributo no esta ya en los atributos de la entidad
                        if(valida_atributo(textBox3.Text, entidadEncontrada) == false)
                        {
                            if(comboBox1.SelectedIndex > -1 && comboBox2.SelectedIndex > -1)
                            {
                                // Se tiene que validar si ya se tiene un atributo que es llave primaria
                                if(comboBox2.SelectedIndex == 0)
                                {
                                    bool yaEsLlave = false;

                                    foreach(Atributo at in entidadEncontrada.listaAtributos)
                                    {
                                        if(at.esLlavePrimaria == true)
                                        {
                                            yaEsLlave = true;
                                            break;
                                        }
                                    }

                                    if(yaEsLlave == false)
                                    {
                                        if (entidadEncontrada.listaAtributos.Count > 0)
                                        {
                                            int byteSize = escoje_num_bytes();
                                            Atributo nuevoAtributo = new Atributo(textBox3.Text, comboBox1.SelectedItem.ToString(), 
                                                byteSize, comboBox2.SelectedItem.ToString());
                                            nuevoAtributo.posAtributo = posicionMemoria;
                                            posicionMemoria += tamAtributo;
                                            entidadEncontrada.listaAtributos.Add(nuevoAtributo);
                                            entidadEncontrada.listaAtributos[entidadEncontrada.listaAtributos.Count - 2].apSigAtributo = 
                                                nuevoAtributo.posAtributo;

                                            // actualiza_archivo_entidad(entidadEncontrada, entidadEncontrada.nombre, entidadEncontrada.apAtributos, 
                                                //entidadEncontrada.apDatos, entidadEncontrada.posEntidad, entidadEncontrada.apSigEntidad);

                                            escribe_archivo(textBox1.Text);

                                            manejo_dataGrid(textBox1.Text);

                                            manejo_dataGrid_atributos(entidadEncontrada);  

                                            toolStripStatusLabel1.Text = "Atributo añadido con exito.";
                                        }
                                        else
                                        {
                                            int byteSize = escoje_num_bytes();
                                            Atributo nuevoAtributo = new Atributo(textBox3.Text, comboBox1.SelectedItem.ToString(), 
                                                byteSize, comboBox2.SelectedItem.ToString());
                                            nuevoAtributo.posAtributo = posicionMemoria;
                                            posicionMemoria += tamAtributo;
                                            entidadEncontrada.listaAtributos.Add(nuevoAtributo);
                                            entidadEncontrada.apAtributos = nuevoAtributo.posAtributo;

                                            //actualiza_archivo_entidad(entidadEncontrada, entidadEncontrada.nombre, entidadEncontrada.apAtributos, entidadEncontrada.apDatos, 
                                                //entidadEncontrada.posEntidad, entidadEncontrada.apSigEntidad);
                                            
                                            escribe_archivo(textBox1.Text);

                                            manejo_dataGrid(textBox1.Text);

                                            manejo_dataGrid_atributos(entidadEncontrada);  

                                            toolStripStatusLabel1.Text = "Atributo añadido con exito.";
                                        }    
                                    }
                                    else
                                    {
                                        toolStripStatusLabel1.Text = "Error, solo puede haber una llave primaria.";
                                    }
                                }           
                                else
                                {
                                    if (entidadEncontrada.listaAtributos.Count > 0)
                                    {
                                        int byteSize = escoje_num_bytes();
                                        Atributo nuevoAtributo = 
                                            new Atributo(textBox3.Text, comboBox1.SelectedItem.ToString(), byteSize, comboBox2.SelectedItem.ToString());
                                        nuevoAtributo.posAtributo = posicionMemoria;
                                        posicionMemoria += tamAtributo;
                                        entidadEncontrada.listaAtributos.Add(nuevoAtributo);
                                        entidadEncontrada.listaAtributos[entidadEncontrada.listaAtributos.Count - 2].apSigAtributo = 
                                            nuevoAtributo.posAtributo;

                                        escribe_archivo(textBox1.Text);

                                        manejo_dataGrid(textBox1.Text);

                                        manejo_dataGrid_atributos(entidadEncontrada);  

                                        toolStripStatusLabel1.Text = "Atributo añadido con exito.";
                                    }
                                    else
                                    {
                                        int byteSize = escoje_num_bytes();
                                        Atributo nuevoAtributo = 
                                            new Atributo(textBox3.Text, comboBox1.SelectedItem.ToString(), byteSize, comboBox2.SelectedItem.ToString());
                                        nuevoAtributo.posAtributo = posicionMemoria;
                                        posicionMemoria += tamAtributo;
                                        entidadEncontrada.listaAtributos.Add(nuevoAtributo);
                                        entidadEncontrada.apAtributos = nuevoAtributo.posAtributo;

                                        escribe_archivo(textBox1.Text);

                                        manejo_dataGrid(textBox1.Text);

                                        manejo_dataGrid_atributos(entidadEncontrada);  

                                        toolStripStatusLabel1.Text = "Atributo añadido con exito.";
                                    }   
                                }                                                              
                            }
                            else
                            {
                                toolStripStatusLabel1.Text = "Error, selecciona el tipo de dato y si es llave primaria.";
                            }
                        }
                        else
                        {
                            toolStripStatusLabel1.Text = "Error, el atributo ya existe en esta entidad.";
                        }
                    }
                    else
                    {
                        toolStripStatusLabel1.Text = "Error, entidad no encontrada.";
                    }
                }
                else
                {
                    toolStripStatusLabel1.Text = "Error, esta entidad no existe.";
                }
            }
            else if(seAbrio == false)
            {
                toolStripStatusLabel1.Text = "Error, no se ha abierto ningun archivo.";
            }
            else if(textBox2.Text.Length == 0)
            {
                toolStripStatusLabel1.Text = "Error, no se ha especificado un nombre de entidad.";
            }
            else if(textBox3.Text.Length == 0)
            {
                toolStripStatusLabel1.Text = "Error, no se ha especificado un nombre de atributo.";
            }
        }

        // Boton para modificar el nombre y tipo de dato de un atributo
        private void button6_Click(object sender, EventArgs e)
        {
            if(seAbrio == true && textBox2.Text.Length > 0 && textBox3.Text.Length > 0)
            {
                if(validacion(textBox2.Text) == true)
                {
                    Entidad entidadEncontrada = busca_entidad(textBox2.Text);

                    if(entidadEncontrada.posEntidad > 0)
                    {
                        // Si el atributo que se quiere modificar existe
                        if(valida_atributo(textBox3.Text, entidadEncontrada) == true)
                        {
                            if(entidadEncontrada.apDatos > -1)
                            {
                                toolStripStatusLabel1.Text = "Error, la entidad correspondiente tiene datos.";
                                return;
                            }

                            String nuevoNombreAtributo = "";
                            char nuevoTipoAtributo = ' ';
                            long nuevosBytesAtributo = 0;
                            bool esLlave = false;

                            using (ModificadorAtributo modificaAt = new ModificadorAtributo())
                            {
                                var nuevoAtributo = modificaAt.ShowDialog();

                                if (nuevoAtributo == DialogResult.OK)
                                {
                                    nuevoNombreAtributo = modificaAt.newNombre;
                                    nuevoTipoAtributo = modificaAt.newTipo;
                                    nuevosBytesAtributo = modificaAt.newBytes;

                                    if(modificaAt.esLlave == 0)
                                    {
                                        esLlave = true;
                                    }
                                    else
                                    {
                                        esLlave = false;
                                    }
                                }
                            }

                            Atributo atrTemp = regresa_atributo(textBox3.Text, entidadEncontrada);

                            // Si es el mismo atributo que se va a cambiar
                            if(es_mismo_atributo(nuevoNombreAtributo,entidadEncontrada, atrTemp) == true)
                            {
                                foreach (Atributo at in entidadEncontrada.listaAtributos)
                                {
                                    if (at.posAtributo == atrTemp.posAtributo)
                                    {
                                        at.tipo = nuevoTipoAtributo;
                                        at.bytes = nuevosBytesAtributo;

                                        // Si ese atributo ya es la unica llave primaria que se tiene
                                        if (at.esLlavePrimaria == true && verifica_llave_primaria(entidadEncontrada) == true && esLlave == true)
                                        {
                                            at.esLlavePrimaria = esLlave;
                                        }
                                        // Si el atributo sera la llave primaria y no hay ya una llave primaria.
                                        else if (at.esLlavePrimaria == false && verifica_llave_primaria(entidadEncontrada) == false && 
                                            esLlave == true)
                                        {
                                            at.esLlavePrimaria = esLlave;
                                        }
                                        // Si el atributo quiere ser la llave primaria pero ya hay una llave primaria
                                        else if (at.esLlavePrimaria == false && verifica_llave_primaria(entidadEncontrada) == true &&
                                            esLlave == true)
                                        {
                                            //toolStripStatusLabel1.Text = "Error, no se puede tener mas de una llave primaria.";
                                            at.esLlavePrimaria = false;
                                        }
                                        // Si no sera la llave primaria
                                        else if(at.esLlavePrimaria == false && esLlave == false)
                                        {
                                            at.esLlavePrimaria = esLlave;
                                        }
                                        // Si era la llave primaria pero ya no lo sera
                                        else if(at.esLlavePrimaria == true && esLlave == false)
                                        {
                                            at.esLlavePrimaria = esLlave;
                                        }
                                        break;
                                    }
                                }

                                escribe_archivo(textBox1.Text);

                                entidadesLeidas = new List<Entidad>();

                                manejo_dataGrid(textBox1.Text);

                                manejo_dataGrid_atributos(entidadEncontrada);

                                toolStripStatusLabel1.Text = "Atributo modificado con exito.";
                            }
                            // Si no es el mismo atributo
                            else if(valida_atributo(nuevoNombreAtributo, entidadEncontrada) == false)
                            {
                                char[] nuevoNombreArr = new char[30];
                                nuevoNombreArr[29] = '\n';
                                char[] viejoNombreArr = new char[30];
                                viejoNombreArr[29] = '\n';

                                for (int i = 0; i < nuevoNombreAtributo.Length; i++)
                                {
                                    nuevoNombreArr[i] = nuevoNombreAtributo[i];
                                }

                                for (int i = 0; i < textBox3.Text.Length; i++)
                                {
                                    viejoNombreArr[i] = textBox3.Text[i];
                                }

                                foreach (Atributo at in entidadEncontrada.listaAtributos)
                                {
                                    if (at.nombre.SequenceEqual(viejoNombreArr) == true)
                                    {
                                        at.nombre = nuevoNombreArr;
                                        at.tipo = nuevoTipoAtributo;
                                        at.bytes = nuevosBytesAtributo;

                                        // Si ese atributo ya es la unica llave primaria que se tiene
                                        if (at.esLlavePrimaria == true && verifica_llave_primaria(entidadEncontrada) == true && esLlave == true)
                                        {
                                            at.esLlavePrimaria = esLlave;
                                        }
                                        // Si el atributo sera la llave primaria y no hay ya una llave primaria.
                                        else if (at.esLlavePrimaria == false && verifica_llave_primaria(entidadEncontrada) == false &&
                                            esLlave == true)
                                        {
                                            at.esLlavePrimaria = esLlave;
                                        }
                                        // Si el atributo quiere ser la llave primaria pero ya hay una llave primaria
                                        else if (at.esLlavePrimaria == false && verifica_llave_primaria(entidadEncontrada) == true &&
                                            esLlave == true)
                                        {
                                            //toolStripStatusLabel1.Text = "Error, no se puede tener mas de una llave primaria.";
                                            at.esLlavePrimaria = false;
                                        }
                                        // Si no sera la llave primaria
                                        else if (at.esLlavePrimaria == false && esLlave == false)
                                        {
                                            at.esLlavePrimaria = esLlave;
                                        }
                                        // Si era la llave primaria pero ya no lo sera
                                        else if (at.esLlavePrimaria == true && esLlave == false)
                                        {
                                            at.esLlavePrimaria = esLlave;
                                        }

                                        break;
                                    }
                                }

                                escribe_archivo(textBox1.Text);

                                entidadesLeidas = new List<Entidad>();

                                manejo_dataGrid(textBox1.Text);

                                manejo_dataGrid_atributos(entidadEncontrada);

                                toolStripStatusLabel1.Text = "Atributo modificado con exito.";
                            }
                            else
                            {
                                toolStripStatusLabel1.Text = "Error, nombre de atributo duplicado.";
                            }
                        }
                        else
                        {
                            toolStripStatusLabel1.Text = "Error, este etributo no existe en esta entidad";
                        }
                    }
                    else
                    {
                        toolStripStatusLabel1.Text = "Error, entidad no econtrada.";
                    }
                }
                else
                {
                    toolStripStatusLabel1.Text = "Error, esta atributo no existe";
                }
            }
        }

        // Boton para eliminar un atributo (solo se requiere el nombre)
        private void button7_Click(object sender, EventArgs e)
        {
            if (seAbrio == true && textBox2.Text.Length > 0 && textBox3.Text.Length > 0)
            {
                // Primero hay que verificar que la entidad en la que esta el atributo exista
                if (validacion(textBox2.Text) == true)
                {
                    // Luego, se busca la entidad en la lista de entidades
                    Entidad entidadEncontrada = busca_entidad(textBox2.Text);

                    if (entidadEncontrada.posEntidad > 0)
                    {
                        // Despues se hace la validacion para ver si ese atributo esta en los atributos de la entidad
                        if (valida_atributo(textBox3.Text, entidadEncontrada) == true)
                        {
                            if (entidadEncontrada.apDatos > -1)
                            {
                                toolStripStatusLabel1.Text = "Error, la entidad correspondiente tiene datos.";
                                return;
                            }

                            char[] nombreAtributoA = new char[30];
                            nombreAtributoA[29] = '\n';
                            Atributo atributoEliminar = new Atributo();
                            List<Atributo> atributosRespaldo = new List<Atributo>();

                            for (int i = 0; i < textBox3.Text.Length; i++)
                            {
                                nombreAtributoA[i] = textBox3.Text[i];
                            }

                            foreach(Atributo at in entidadEncontrada.listaAtributos)
                            {
                                if(at.nombre.SequenceEqual(nombreAtributoA) == true)
                                {
                                    atributoEliminar = at;
                                    atributoEliminado = at;
                                    break;
                                }
                            }

                            foreach(Atributo at in entidadEncontrada.listaAtributos)
                            {
                                if(at.nombre.SequenceEqual(nombreAtributoA) == false)
                                {
                                    atributosRespaldo.Add(at);
                                }
                            }

                            for(int i = 0; i < entidadEncontrada.listaAtributos.Count; i++)
                            {
                                // Los atributos eliminados tienen como valor de apuntador a siguiente atributo -2 o -4
                                if(atributoEliminar == entidadEncontrada.listaAtributos[i])
                                {
                                    if (i == entidadEncontrada.listaAtributos.Count - 1)
                                    {
                                        entidadEncontrada.listaAtributos[i].apSigAtributo = -4;
                                    }
                                    else
                                    {
                                        entidadEncontrada.listaAtributos[i].apSigAtributo = -2;
                                    }

                                    // Si solo habia un atributo
                                    if (i == 0 && entidadEncontrada.listaAtributos.Count == 1)
                                    {
                                        entidadEncontrada.apAtributos = -2;
                                        break;
                                    }
                                    // Si era el primero, se buscara al atributo siguiente que no haya sido eliminado para que el apuntador a atributos
                                    // de la entidad apunte a ese atributo
                                    else if (entidadEncontrada.listaAtributos.Count > 1 && i == 0)
                                    {
                                        bool encontrado = false;

                                        for (int j = i + 1; j < entidadEncontrada.listaAtributos.Count; j++)
                                        {
                                            if (entidadEncontrada.listaAtributos[j].apSigAtributo != -2 && entidadEncontrada.listaAtributos[j].apSigAtributo != -4)
                                            {
                                                entidadEncontrada.apAtributos = entidadEncontrada.listaAtributos[j].posAtributo;
                                                encontrado = true;
                                                break;
                                            }
                                        }

                                        if (encontrado == false)
                                        {
                                            entidadEncontrada.apAtributos = -2;
                                            break;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    // Si no era el primero, hay que recorrer la lista de atributos para ver cual es el antecesor y el sucesor de ese atributo
                                    // para ver si no fueron eliminados tambien
                                    else
                                    {
                                        // Si es el ultimo
                                        bool encontrado = false;

                                        if (i == entidadEncontrada.listaAtributos.Count - 1)
                                        {
                                            for(int j = i-1; j > 0; j--)
                                            {
                                                if(entidadEncontrada.listaAtributos[j].apSigAtributo != -2 && 
                                                    entidadEncontrada.listaAtributos[j].apSigAtributo != -4)
                                                {
                                                    entidadEncontrada.listaAtributos[i].apSigAtributo = -4;
                                                    entidadEncontrada.listaAtributos[j].apSigAtributo = -3;
                                                    encontrado = true;
                                                    break;
                                                }
                                            }

                                            if (encontrado == false)
                                            {
                                                entidadEncontrada.apAtributos = -2;
                                                break;
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                        // Si esta entre 2 atributos
                                        else
                                        {
                                            List<Atributo> atributosAnteriores = new List<Atributo>();
                                            List<Atributo> atributosPosteriores = new List<Atributo>();
                                            Atributo anteriorInmediato = new Atributo();
                                            Atributo posteriorInmediato = new Atributo();

                                            for(int a = 0; a < i; a++)
                                            {
                                                if(entidadEncontrada.listaAtributos[a].apSigAtributo != - 2 &&
                                                    entidadEncontrada.listaAtributos[a].apSigAtributo != -4)
                                                {
                                                    atributosAnteriores.Add(entidadEncontrada.listaAtributos[a]);
                                                }
                                                
                                            }

                                            if (atributosAnteriores.Count > 0)
                                            {
                                                anteriorInmediato = atributosAnteriores[atributosAnteriores.Count - 1];
                                            }

                                            for(int b = i + 1; b < entidadEncontrada.listaAtributos.Count; b++)
                                            {
                                                if (entidadEncontrada.listaAtributos[b].apSigAtributo != -2 &&
                                                    entidadEncontrada.listaAtributos[b].apSigAtributo != -4)
                                                {
                                                    atributosPosteriores.Add(entidadEncontrada.listaAtributos[b]);
                                                }
                                            }

                                            if (atributosPosteriores.Count > 0)
                                            {
                                                posteriorInmediato = atributosPosteriores[0];
                                            }

                                            // Si no se convertira en el primer atributo
                                            if(anteriorInmediato.posAtributo != 0)
                                            {
                                                int indiceAI = entidadEncontrada.listaAtributos.IndexOf(anteriorInmediato);

                                                // Si tiene un atributo posterior
                                                if(posteriorInmediato.posAtributo != 0)
                                                {
                                                    int indicePI = entidadEncontrada.listaAtributos.IndexOf(posteriorInmediato);

                                                    entidadEncontrada.listaAtributos[indiceAI].apSigAtributo = entidadEncontrada.listaAtributos[indicePI].posAtributo;
                                                    break;
                                                }
                                                else
                                                {
                                                    if (indiceAI != entidadEncontrada.listaAtributos.Count - 1)
                                                    {
                                                        entidadEncontrada.listaAtributos[indiceAI].apSigAtributo = -3;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        entidadEncontrada.listaAtributos[indiceAI].apSigAtributo = -1;
                                                        break;
                                                    }
                                                }
                                            }
                                            // Si se convertira en el primer atributo
                                            else 
                                            {
                                                // Si tiene un atributo posterior
                                                if (posteriorInmediato.posAtributo != 0)
                                                {
                                                    int indicePI = entidadEncontrada.listaAtributos.IndexOf(posteriorInmediato);

                                                    entidadEncontrada.apAtributos = entidadEncontrada.listaAtributos[indicePI].posAtributo;
                                                    break;
                                                }
                                                else
                                                {
                                                    entidadEncontrada.apAtributos = -2;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }                                
                            }

                            escribe_archivo(textBox1.Text);

                            manejo_dataGrid(textBox1.Text);

                            manejo_dataGrid_atributos(entidadEncontrada);

                            toolStripStatusLabel1.Text = "Atributo eliminado con exito.";
                        }
                        else
                        {
                            toolStripStatusLabel1.Text = "Error, el atributo no existe en esta entidad.";
                        }
                    }
                    else
                    {
                        toolStripStatusLabel1.Text = "Error, entidad no encontrada.";
                    }
                }
                else
                {
                    toolStripStatusLabel1.Text = "Error, esta entidad no existe.";
                }
            }
            else if (seAbrio == false)
            {
                toolStripStatusLabel1.Text = "Error, no se ha abierto ningun archivo.";
            }
            else if (textBox2.Text.Length == 0)
            {
                toolStripStatusLabel1.Text = "Error, no se ha especificado un nombre de entidad.";
            }
            else if (textBox3.Text.Length == 0)
            {
                toolStripStatusLabel1.Text = "Error, no se ha especificado un nombre de atributo.";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        bool is_unico_atributo(Entidad e)
        {
            bool unicaEntidad = false;
            int contador = 0;

            foreach(Atributo atr in e.listaAtributos)
            {
                if(atr.apSigAtributo != -2 && atr.apSigAtributo != -4)
                {

                }
                else
                {
                    contador++;
                }
            }

            if(contador == 1)
            {
                unicaEntidad = true;
            }

            return unicaEntidad;
        }

        // Boton que muestra los atributos de una entidad escrita en el textBox correspondiente
        private void button8_Click(object sender, EventArgs e)
        {
            if(seAbrio == true && textBox2.Text.Length > 0)
            {
                if(validacion(textBox2.Text) == true)
                {
                    Entidad entidadEncontrada = busca_entidad(textBox2.Text);

                    if(entidadEncontrada.posEntidad > -1)
                    {
                        if(entidadEncontrada.apAtributos > -1)
                        {
                            manejo_dataGrid_atributos(entidadEncontrada);
                        }
                        else
                        {
                            toolStripStatusLabel1.Text = "Error, esta entidad no tiene atributos.";
                        }
                    }
                    else
                    {
                        toolStripStatusLabel1.Text = "Error, entidad no encontrada.";
                    }
                }
                else
                {
                    toolStripStatusLabel1.Text = "Error, esta entidad no existe.";
                }
            }
        }

        // Boton que abre una ventana donde se insertaran los datos de forma secuencial ordenada
        private void button9_Click(object sender, EventArgs e)
        {
            if(textBox2.Text.Length > 0 && validacion(textBox2.Text) == true)
            {
                Entidad ent = busca_entidad(textBox2.Text);

                if(ent.apAtributos > -1 && hay_llave_primaria(ent) == true)
                {
                    button10.Enabled = false;

                    using (CuadroDeDatos datosEntidad = new CuadroDeDatos(ent, posicionMemoria, ent.apDatos, tamDato))
                    {
                        var cuadroDatos = datosEntidad.ShowDialog();

                        if(cuadroDatos == DialogResult.Cancel)
                        {
                            posicionMemoria = datosEntidad.posMemoria;
                            // ent.apDatos = datosEntidad.apDatos;
                            ent.listaDatos = datosEntidad.ent.listaDatos;
                            ent.apDatos = datosEntidad.ent.apDatos;
                            int indiceLlave = datosEntidad.indiceLlave;
                            List<Dato> ordenada = ent.listaDatos.OrderBy(o=>o.datos[indiceLlave]).ToList();
                            ent.listaDatos = ordenada;

                            if (datosEntidad.bandChanged == true)
                            {
                                // Escribe archivo
                                escribe_archivo(textBox1.Text);

                                // Manejo datagridview
                                manejo_dataGrid(textBox1.Text);
                            }
                        }
                    }
                }
                else
                {
                    toolStripStatusLabel1.Text = "Error, esta entidad no tiene atributos o llave primaria.";
                }
            }
        }

        /// <summary>
        /// Funcion que verifica si la lista de atributos de la entidad tiene una llave primaria, ya que de esa forma se podran insertar
        /// datos.
        /// </summary>
        /// <param name="ent">La entidad que tiene la lista de atributos donde se buscara la llave primaria.</param>
        /// <returns>Un booleano que indica si se tiene una llave primaria o no.</returns>
        private bool hay_llave_primaria(Entidad ent)
        {
            bool hayLlave = false;

            foreach(Atributo atr in ent.listaAtributos)
            {
                if(atr.esLlavePrimaria == true)
                {
                    hayLlave = true;
                    break;
                }
            }

            return hayLlave;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // Boton que abre una ventana donde se insertaran los datos de forma secuencial indexada.
        private void button10_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Length > 0 && validacion(textBox2.Text) == true)
            {
                Entidad ent = busca_entidad(textBox2.Text);

                if (ent.apAtributos > -1 && hay_llave_primaria(ent) == true)
                {
                    button9.Enabled = false;

                    
                }
                else
                {
                    toolStripStatusLabel1.Text = "Error, esta entidad no tiene atributos o llave primaria.";
                }
            }
        }
    }
}
