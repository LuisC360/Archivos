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
    /// <summary>
    /// La clase que representara la ventana principal del proyecto.
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// La lista de datos totales. 
        /// </summary>
        List<object> data = new List<object>();
        /// <summary>
        /// La lista de entidades.
        /// </summary>
        List<Entidad> entidades = new List<Entidad>();
        /// <summary>
        /// Lista que contendra los tipos de dato disponibles.
        /// </summary>
        List<char> tiposDato = new List<char>();
        /// <summary>
        /// Lista para entidades leidas.
        /// </summary>
        List<Entidad> entidadesLeidas = new List<Entidad>();
        /// <summary>
        /// Tamaño de una entidad.
        /// </summary>
        readonly int tamEntidad = 62;
        /// <summary>
        /// Tamaño de un atributo.
        /// </summary>
        readonly int tamAtributo = 56;
        /// <summary>
        /// Tamaño de un dato, en un principio.
        /// </summary>
        int tamDato = 8;
        /// <summary>
        /// Tamaño de un cajon.
        /// </summary>
        readonly int tamCajon = 8;
        /// <summary>
        /// Tamaño de una cubeta.
        /// </summary>
        readonly int tamCubeta = 8;
        /// <summary>
        /// Valores que posee una entidad (para lectura de archivo).
        /// </summary>
        char[] nombre = { '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0',  
                                    '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0' ,'\0' ,'\n'};
        char[] nombreAT = { '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0',  
                                    '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0' ,'\0' ,'\n'};
        /// <summary>
        /// Booleano para verificar que se abrio un archivo.
        /// </summary>
        Boolean seAbrio;
        /// <summary>
        /// Numero que nos ayudara a saber en que posicion en memoria nos encontramos.
        /// </summary>
        long posicionMemoria = 8;
        /// <summary>
        /// Entidad actual que se ha eliminado del archivo.
        /// </summary>
        Entidad entidadEliminada;
        /// <summary>
        /// Atributo actual que se ha eliminado del archivo.
        /// </summary>
        Atributo atributoEliminado;
        /// <summary>
        /// El rango para los archivos secuenciales indexados.
        /// </summary>
        long rango;
        /// <summary>
        /// El numero de cajones para los archivos de hash estatica.
        /// </summary>
        long numCajones;
        /// <summary>
        /// El numero de cubetas por cajon para los archivos de hash estatica.
        /// </summary>
        long regPorCajon;
        /// <summary>
        /// El tipo de ordenamiento del archivo (0- Secuencial ordenado, 1- Secuencial indezado, 3- Hash estatica, 4- Multilistas).
        /// </summary>
        int tipo;

        /// <summary>
        /// Constructor para la ventana principal.
        /// </summary>
        public Form1()
        {
            this.Location = new Point(100, 100);
            InitializeComponent();           
            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            comboBox3.Enabled = false;
            button9.Enabled = false;
            button10.Enabled = false;
            button11.Enabled = false;
            button12.Enabled = false;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
            rellena_lista_tipo();
            toolStripStatusLabel1.Text = "Listo.";
            dataGridView1.ReadOnly = true;
            dataGridView2.ReadOnly = true;
        }

        /// <summary>
        /// Boton para abrir archivo.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">EventArgs.</param>
        private void button1_Click(object sender, EventArgs e)
        {
            data.Clear();
            entidadEliminada = new Entidad();
            atributoEliminado = new Atributo();

            if(textBox1.Text.Length > 0)
            {
                // Si el archivo no existe, se crea
                if (File.Exists(@"C:\Users\DanielCorona\Documents\Projects\ArchivosTarea2Respaldo\ArchivosTarea2\ArchivosTarea2\bin\Debug\" + textBox1.Text) == false)
                {
                    using(SeleccionTipo selTip = new SeleccionTipo())
                    {
                        var cuadroTipo = selTip.ShowDialog();

                        if(cuadroTipo == DialogResult.OK)
                        {
                            tipo = selTip.regresa_tipo();
                        }
                        else if(cuadroTipo == DialogResult.Cancel)
                        {
                            toolStripStatusLabel1.Text = "Error, seleccione un tipo de ordenamiento de datos.";
                            return;
                        }
                    }

                    switch(tipo)
                    {
                        // SECUENCIAL ORDENADO
                        case 0: crea_archivo(textBox1.Text);

                                // Funcion que nos auxiliara con el manejo del dataGridView
                                manejo_dataGrid(textBox1.Text);

                                textBox2.ReadOnly = false;
                                textBox3.ReadOnly = false;
                                comboBox1.Enabled = true;
                                comboBox2.Enabled = true;
                                button9.Enabled = true;
                               
                                break;
                        // SECUENCIAL INDEXADO
                        case 1: crea_archivo_indexado(textBox1.Text);

                                // Funcion que nos auxiliara con el manejo del dataGridView
                                manejo_dataGrid_indexado(textBox1.Text);

                                textBox2.ReadOnly = false;
                                textBox3.ReadOnly = false;
                                comboBox1.Enabled = true;
                                comboBox2.Enabled = true;
                                button10.Enabled = true;

                                posicionMemoria += 8;
                                break;
                        // HASH ESTATICA
                        case 2: crea_archivo_hash(textBox1.Text);

                                manejo_dataGrid_hash(textBox1.Text);

                                textBox2.ReadOnly = false;
                                textBox3.ReadOnly = false;
                                comboBox1.Enabled = true;
                                comboBox2.Enabled = true;
                                button11.Enabled = true;

                                posicionMemoria += 16;
                                break;
                        // MULTILISTAS
                        case 3: crea_archivo_multilistas(textBox1.Text);

                                manejo_dataGrid_multilistas(textBox1.Text);

                                textBox2.ReadOnly = false;
                                textBox3.ReadOnly = false;
                                comboBox1.Enabled = true;
                                comboBox2.Enabled = true;
                                comboBox3.Enabled = true;
                                button12.Enabled = true;

                                break;
                        default: // Default vacio
                                break;
                    }

                    toolStripStatusLabel1.Text = "Archivo creado con exito.";
                }
                else // Si el archivo ya existe, se abre
                {
                    try
                    {
                        using (SeleccionTipo selTip = new SeleccionTipo())
                        {
                            var cuadroTipo = selTip.ShowDialog();

                            if (cuadroTipo == DialogResult.OK)
                            {
                                tipo = selTip.regresa_tipo();
                            }
                            else if (cuadroTipo == DialogResult.Cancel)
                            {
                                toolStripStatusLabel1.Text = "Error, seleccione un tipo de ordenamiento de datos.";
                                return;
                            }
                        }

                        switch(tipo)
                        {
                            case 0: manejo_dataGrid(textBox1.Text);

                                    textBox2.ReadOnly = false;
                                    textBox3.ReadOnly = false;
                                    comboBox1.Enabled = true;
                                    comboBox2.Enabled = true;
                                    button9.Enabled = true;
                                    break;
                            case 1: manejo_dataGrid_indexado(textBox1.Text);

                                    textBox2.ReadOnly = false;
                                    textBox3.ReadOnly = false;
                                    comboBox1.Enabled = true;
                                    comboBox2.Enabled = true;
                                    button10.Enabled = true;
                                    break;
                            case 2: manejo_dataGrid_hash(textBox1.Text);

                                    textBox2.ReadOnly = false;
                                    textBox3.ReadOnly = false;
                                    comboBox1.Enabled = true;
                                    comboBox2.Enabled = true;
                                    button11.Enabled = true;
                                    break;
                            case 3: manejo_dataGrid_multilistas(textBox1.Text);

                                    textBox2.ReadOnly = false;
                                    textBox3.ReadOnly = false;
                                    comboBox1.Enabled = true;
                                    comboBox2.Enabled = true;
                                    comboBox3.Enabled = true;
                                    button12.Enabled = true;
                                    break;
                        }
                        

                        toolStripStatusLabel1.Text = "Archivo abierto con exito."; 
                    }
                    catch
                    {
                        toolStripStatusLabel1.Text = "Error.";
                    }
                }
            }
        }

        /// <summary>
        /// Boton para añadir Entidad.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">EventArgs.</param>
        private void button2_Click(object sender, EventArgs e)
        {
            if(seAbrio == true && textBox2.Text.Length > 0 && textBox2.Text.Length < 29)
            {
                // Si el archivo especificado no existe, se crea
                if (File.Exists(@"C:\Users\DanielCorona\Documents\Projects\ArchivosTarea2Respaldo\ArchivosTarea2\ArchivosTarea2\bin\Debug\" + 
                    textBox1.Text) == false)
                {
                    switch(tipo)
                    { 
                        case 0: crea_archivo(textBox1.Text);

                                manejo_dataGrid(textBox1.Text);
                            
                                break;
                        case 1: crea_archivo_indexado(textBox1.Text);

                                manejo_dataGrid_indexado(textBox1.Text);

                                break;
                        case 2: crea_archivo_hash(textBox1.Text);

                                manejo_dataGrid_hash(textBox1.Text);

                                break;
                        case 3: crea_archivo_multilistas(textBox1.Text);

                                manejo_dataGrid_multilistas(textBox1.Text);

                                break;
                    }

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
                            long cabecera = 0;

                            switch(tipo)
                            {
                                case 0:
                                case 3:
                                    cabecera = 8;
                                    break;
                                case 1: cabecera = 16;
                                    break;
                                case 2: cabecera = 24;
                                    break;
                            }

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
                                switch(tipo)
                                {
                                    case 0:
                                    case 3:
                                        entidad.posEntidad += 8;
                                        break;
                                    case 1: entidad.posEntidad += 16;
                                        break;
                                    case 2: entidad.posEntidad += 24;
                                        break;
                                    default: // Default vacio
                                        break;
                                }
                            }

                            if (entidades.Count == 0 && data.Count == 1)
                            {
                                data.Clear();
                                data.Add(cabecera);
                            }

                            data.Add(entidad);
                            entidades.Add(entidad);

                            switch(tipo)
                            {
                                case 0: escribe_archivo(textBox1.Text);

                                        entidadesLeidas = new List<Entidad>();

                                        manejo_dataGrid(textBox1.Text);
                                        break;
                                case 1: escribe_archivo_indexado(textBox1.Text);

                                        entidadesLeidas = new List<Entidad>();

                                        manejo_dataGrid_indexado(textBox1.Text);
                                        break;
                                case 2: escribe_archivo_hash(textBox1.Text);

                                        entidadesLeidas = new List<Entidad>();

                                        manejo_dataGrid_hash(textBox1.Text);
                                        break;
                                case 3: escribe_archivo_multilistas(textBox1.Text);

                                        entidadesLeidas = new List<Entidad>();

                                        manejo_dataGrid_multilistas(textBox1.Text);
                                        break;
                            }
                            
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

        /// <summary>
        /// Boton para modificar una Entidad (o al menos solo el nombre de este).
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">EventArgs.</param>
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

        /// <summary>
        /// Boton para eliminar una Entidad.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">EventArgs.</param>
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
                            entidadEliminada.apSigEntidad = -2;
                            entidadEliminar.apSigEntidad = -2;
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

                                switch(tipo)
                                {
                                    case 0: escribe_archivo(textBox1.Text);

                                            manejo_dataGrid(textBox1.Text);
                                            break;
                                    case 1: escribe_archivo_indexado(textBox1.Text);

                                            manejo_dataGrid_indexado(textBox1.Text);
                                            break;
                                    case 2: escribe_archivo_hash(textBox1.Text);

                                            manejo_dataGrid_hash(textBox1.Text);
                                            break;
                                    case 3: escribe_archivo_multilistas(textBox1.Text);

                                            manejo_dataGrid_multilistas(textBox1.Text);
                                            break;
                                }
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

                                switch (tipo)
                                {
                                    case 0: escribe_archivo(textBox1.Text);

                                            manejo_dataGrid(textBox1.Text);
                                            break;
                                    case 1: escribe_archivo_indexado(textBox1.Text);

                                            manejo_dataGrid_indexado(textBox1.Text);
                                            break;
                                    case 2: escribe_archivo_hash(textBox1.Text);

                                            manejo_dataGrid_hash(textBox1.Text);
                                            break;
                                    case 3: escribe_archivo_multilistas(textBox1.Text);

                                            manejo_dataGrid_multilistas(textBox1.Text);
                                            break;
                                }
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

                            switch (tipo)
                            {
                                case 0: escribe_archivo(textBox1.Text);

                                        manejo_dataGrid(textBox1.Text);
                                        break;
                                case 1: escribe_archivo_indexado(textBox1.Text);

                                        manejo_dataGrid_indexado(textBox1.Text);
                                        break;
                                case 2: escribe_archivo_hash(textBox1.Text);

                                        manejo_dataGrid_hash(textBox1.Text);
                                        break;
                                case 3: escribe_archivo_multilistas(textBox1.Text);

                                        manejo_dataGrid_multilistas(textBox1.Text);
                                        break;
                            }
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
        static Atributo regresa_atributo(String nombreAtributo, Entidad entidadSeleccionada)
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
        /// 1- Refrescar e inicializar ambos dataGridView.
        /// 2- Leer el archivo binario especificado en el parametro "archivo".
        /// 3- Poblar el dataGridView con los datos del archivo en el orden correspondiente.
        /// </summary>
        /// <param name="archivo">Nombre del archivo a abrirse.</param>
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

            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                if (bandCabecera == false)
                {
                    long header = reader.ReadInt64();
                    data.Add(header);
                    bandCabecera = true;
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

                    Entidad nEntidad = new Entidad(nombre, apAtr, apDt, posIn, apSigE);

                    posicionMemoria = posicionMemoria + tamEntidad;

                    // Verificar si la entidad tiene atributos
                    if(nEntidad.apAtributos != -1)
                    {
                        tamDato = 16;

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
            String[] fila;
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

                    apAt = 0;
                    apDat = 0;
                    posInic = 0;
                    apSigAt = 0;

                    filas.Add(fila);
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
        /// En esta funcion se haran 3 cosas importantes:
        /// 1- Refrescar e inicializar ambos dataGridView.
        /// 2- Leer el archivo binario indexado especificado en el parametro "archivo".
        /// 3- Poblar el dataGridView con los datos del archivo en el orden correspondiente.
        /// </summary>
        /// <param name="archivo">Nombre del archivo a abrirse.</param>
        public void manejo_dataGrid_indexado(String archivo)
        {
            posicionMemoria = 16;
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
            dataGridView1.Columns[2].Name = "Ap. Indices";
            dataGridView1.Columns[3].Name = "Pos. Inicial";
            dataGridView1.Columns[4].Name = "Ap. Sig. Entidad";

            // Se lee el archivo binario
            FileStream streamR = new FileStream(archivo, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(streamR);

            Boolean bandCabecera = false;
            Boolean bandRango = false;

            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                if (bandCabecera == false)
                {
                    long header = reader.ReadInt64();
                    data.Add(header);
                    bandCabecera = true;
                }

                if(bandRango == false)
                {
                    rango = reader.ReadInt64();
                    bandRango = true;
                }

                if (reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    for (int i = 0; i < nombre.Length; i++)
                    {
                        char car = reader.ReadChar();
                        nombre[i] = car;
                    }

                    long apAtr = reader.ReadInt64();
                    long apInd = reader.ReadInt64();
                    long posIn = reader.ReadInt64();
                    long apSigE = reader.ReadInt64();

                    Entidad nEntidad = new Entidad(nombre, apAtr, apInd, posIn, apSigE, 0);

                    posicionMemoria = posicionMemoria + tamEntidad;

                    // Verificar si la entidad tiene atributos
                    if (nEntidad.apAtributos != -1)
                    {
                        tamDato = 16;

                        lee_atributos_de_entidad(streamR, reader, nEntidad);

                        if (nEntidad.apIndices != -1)
                        {
                            lee_indices_de_entidad(streamR, reader, nEntidad);
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
            String[] fila;
            String nombreEntidad = "";
            long apAt = 0;
            long apIndi = 0;
            long posInic = 0;
            long apSigAt = 0;

            foreach (Entidad ent in entidadesLeidas)
            {
                if (ent.apSigEntidad > -2)
                {
                    nombreEntidad = new string(ent.nombre);
                    apAt += ent.apAtributos;
                    apIndi += ent.apIndices;
                    posInic += ent.posEntidad;
                    apSigAt += ent.apSigEntidad;

                    if (ent.apAtributos < -1)
                    {
                        apAt = -1;
                    }

                    if (ent.apDatos < -1)
                    {
                        apIndi = -1;
                    }

                    fila = new string[] { nombreEntidad, apAt.ToString(), apIndi.ToString(), posInic.ToString(), apSigAt.ToString() };

                    nombreEntidad = "";
                    apAt = 0;
                    apIndi = 0;
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
        /// En esta funcion se haran 3 cosas importantes:
        /// 1- Refrescar e inicializar ambos dataGridView.
        /// 2- Leer el archivo binario con hash estatica especificado en el parametro "archivo".
        /// 3- Poblar el dataGridView con los datos del archivo en el orden correspondiente.
        /// </summary>
        /// <param name="archivo">Nombre del archivo a abrirse.</param>
        public void manejo_dataGrid_hash(String archivo)
        {
            posicionMemoria = 24;
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
            dataGridView1.Columns[2].Name = "Ap. Cajones";
            dataGridView1.Columns[3].Name = "Pos. Inicial";
            dataGridView1.Columns[4].Name = "Ap. Sig. Entidad";

            // Se lee el archivo binario
            FileStream streamR = new FileStream(archivo, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(streamR);

            Boolean bandCabecera = false;
            Boolean bandNumCajones = false;
            Boolean bandRegCubetas = false;
            long archivoPos;

            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                if (bandCabecera == false)
                {
                    long header = reader.ReadInt64();
                    data.Add(header);
                    bandCabecera = true;
                }

                if (bandNumCajones == false)
                {
                    numCajones = reader.ReadInt64();
                    bandNumCajones = true;
                }

                if (bandRegCubetas == false)
                {
                    regPorCajon = reader.ReadInt64();
                    bandRegCubetas = true;
                }

                if (reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    for (int i = 0; i < nombre.Length; i++)
                    {
                        char car = reader.ReadChar();
                        nombre[i] = car;
                    }

                    long apAtr = reader.ReadInt64();
                    long apCaj = reader.ReadInt64();
                    long posIn = reader.ReadInt64();
                    long apSigE = reader.ReadInt64();
                    long diferencia = 0;
                    archivoPos = reader.BaseStream.Position;

                    Entidad nEntidad = new Entidad(nombre, apAtr, apCaj, posIn, apSigE, diferencia);

                    posicionMemoria = posicionMemoria + tamEntidad;

                    // Verificar si la entidad tiene atributos
                    if (nEntidad.apAtributos != -1)
                    {
                        tamDato = 8;

                        lee_atributos_de_entidad(streamR, reader, nEntidad);

                        if (nEntidad.apCajones != -1)
                        {
                            for(int i = 0; i < numCajones; i++)
                            {
                                lee_cajones_de_entidad(streamR, reader, nEntidad);
                            }
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
            long apCajon = 0;
            long posInic = 0;
            long apSigAt = 0;

            foreach (Entidad ent in entidadesLeidas)
            {
                if (ent.apSigEntidad > -2)
                {
                    nombreEntidad = new string(ent.nombre);
                    apAt += ent.apAtributos;
                    apCajon += ent.apCajones;
                    posInic += ent.posEntidad;
                    apSigAt += ent.apSigEntidad;

                    if (ent.apAtributos < -1)
                    {
                        apAt = -1;
                    }

                    if (ent.apDatos < -1)
                    {
                        apCajon = -1;
                    }

                    fila = new string[] { nombreEntidad, apAt.ToString(), apCajon.ToString(), posInic.ToString(), apSigAt.ToString() };

                    nombreEntidad = "";
                    apAt = 0;
                    apCajon = 0;
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
        /// En esta funcion se haran 3 cosas importantes:
        /// 1- Refrescar e inicializar ambos dataGridView.
        /// 2- Leer el archivo binario con multilistas especificado en el parametro "archivo".
        /// 3- Poblar el dataGridView con los datos del archivo en el orden correspondiente.
        /// </summary>
        /// <param name="archivo">Nombre del archivo a abrirse.</param>
        public void manejo_dataGrid_multilistas(String archivo)
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
            dataGridView1.Columns[2].Name = "Ap. Cabeceras";
            dataGridView1.Columns[3].Name = "Pos. Inicial";
            dataGridView1.Columns[4].Name = "Ap. Sig. Entidad";

            // Se lee el archivo binario
            FileStream streamR = new FileStream(archivo, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(streamR);

            Boolean bandCabecera = false;

            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                if (bandCabecera == false)
                {
                    long header = reader.ReadInt64();
                    data.Add(header);
                    bandCabecera = true;
                }

                if (reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    for (int i = 0; i < nombre.Length; i++)
                    {
                        char car = reader.ReadChar();
                        nombre[i] = car;
                    }

                    long apAtr = reader.ReadInt64();
                    long apCabec = reader.ReadInt64();
                    long posIn = reader.ReadInt64();
                    long apSigE = reader.ReadInt64();
                    double dif = 0;

                    Entidad nEntidad = new Entidad(nombre, apAtr, apCabec, posIn, apSigE, dif);

                    posicionMemoria = posicionMemoria + tamEntidad;

                    // Verificar si la entidad tiene atributos
                    if (nEntidad.apAtributos != -1)
                    {
                        lee_atributos_de_entidad(streamR, reader, nEntidad);

                        if (nEntidad.apCabeceras != -1)
                        {
                            lee_cabeceras_de_entidad(reader, nEntidad);
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
            String[] fila;
            String nombreEntidad = "";
            long apAt = 0;
            long apCab = 0;
            long posInic = 0;
            long apSigAt = 0;

            foreach (Entidad ent in entidadesLeidas)
            {
                if (ent.apSigEntidad > -2)
                {
                    nombreEntidad = new string(ent.nombre);
                    apAt += ent.apAtributos;
                    apCab += ent.apCabeceras;
                    posInic += ent.posEntidad;
                    apSigAt += ent.apSigEntidad;

                    if (ent.apAtributos < -1)
                    {
                        apAt = -1;
                    }

                    if (ent.apDatos < -1)
                    {
                        apCab = -1;
                    }

                    fila = new string[] { nombreEntidad, apAt.ToString(), apCab.ToString(), posInic.ToString(), apSigAt.ToString() };

                    apAt = 0;
                    apCab = 0;
                    posInic = 0;
                    apSigAt = 0;

                    filas.Add(fila);
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
        /// Funcion recursiva que se encargara de leer los indices del achivo.
        /// </summary>
        /// <param name="f">El FileStream con el que se manipulan los archivos.</param>
        /// <param name="r">El lector de archivos binarios.</param>
        /// <param name="ent">La entidad que contiene los atributos.</param>
        void lee_indices_de_entidad(FileStream f, BinaryReader r, Entidad ent)
        {
            Indice ind = new Indice();

            long valIn = r.ReadInt64();
            long valFin = r.ReadInt64();
            long posInd = r.ReadInt64();
            long apSig = r.ReadInt64();
            long apDatos = r.ReadInt64();

            ind.srt_valorInicial(valIn);
            ind.srt_valorFinal(valFin);
            ind.srt_posIndice(posInd);
            ind.srt_apSigIndice(apSig);
            ind.srt_apDatos(apDatos);

            ent.listaIndices.Add(ind);

            if(apDatos != -1)
            {
                lee_datos_de_indice(f, r, ent, ind);
            }

            if(apSig != -1)
            {
                lee_indices_de_entidad(f, r, ent);
            }
        }

        /// <summary>
        /// Funcion recursiva que se encargara de leer los datos del indice.
        /// </summary>
        /// <param name="f">El FileStream con el que se manipulan los archivos.</param>
        /// <param name="r">El lector de archivos binarios.</param>
        /// <param name="ind">La entidad que contiene los atributos.</param>
        void lee_datos_de_indice(FileStream f, BinaryReader r, Entidad ent, Indice ind)
        {
            Dato dataRead = new Dato(ent);

            // Primero se recorre la lista de atributos
            foreach (Atributo atr in ent.listaAtributos)
            {
                // Verificamos si el atributo no fue borrado
                if (atr.apSigAtributo != -2 && atr.apSigAtributo != -4)
                {
                    // Si no fue borrado, entonces revisamos de que tipo es ese atributo para leer el dato
                    if (atr.tipo == 'I')
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

                            for (int i = 0; i < chara.Length; i++)
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
                    switch (atr.tipo)
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

            ind.datosIndice.Add(dataRead);

            if (dataRead.apSigDato != -1 && dataRead.apSigDato != -4)
            {
                lee_datos_de_indice(f, r, ent, ind);
            }
        }

        /// <summary>
        /// Funcion que se encargara de leer los apuntadores a cajones de una cubeta. En este caso no sera una funcion
        /// recursiva debido a que el numero de veces que se leera un cajon dependera del numero de cajones especificados en el
        /// comienzo del archivo.
        /// </summary>
        /// <param name="f">El FileStream con el que se manipulan los archivos.</param>
        /// <param name="r">El lector de archivos binarios.</param>
        /// <param name="ent">La entidad que contiene los atributos.</param>
        void lee_cajones_de_entidad(FileStream f, BinaryReader r, Entidad ent)
        {
            Cajon caj = new Cajon();

            long apCub = r.ReadInt64();

            caj.str_apuntadorCubeta(apCub);

            if(apCub != -1)
            {
                List<Cubeta> cubL = new List<Cubeta>();
                lee_cubetas_de_cajon(f, r, caj, ent, cubL);
            }

            ent.listaCajones.Add(caj);
            posicionMemoria += tamCajon;
        }

        /// <summary>
        /// Funcin encargada de leer las cubetas de un cajon. Se debe de declarar una lista para albergar los cajones, y al encontrarse el
        /// cajon con el apuntador al siguiente, se debe de ver si este es igual a -1 o no. Si es el caso, entonces se debe de agregar
        /// la lista de cubetas al cajon correspondiente, y si no, se agrega la lista de cubetas al cajon, se declara una nueva lista y
        /// se llama a la misma funcion de lectura de cajones.
        /// </summary>
        /// <param name="f">El FileStream con el que se manipulan los archivos.</param>
        /// <param name="r">El lector de archivos binarios.</param>
        /// <param name="c">El cajon en el que se pondran las cubetas.</param>
        /// <param name="ent">La entidad que contiene los atributos.</param>
        /// <param name="cubL">La lista individual de cubetas, la cual contendra todas las cubetas del cajon, junto con la cubeta enlace.</param>
        void lee_cubetas_de_cajon(FileStream f, BinaryReader r, Cajon c, Entidad ent, List<Cubeta> cubL)
        {
            Cubeta cub = new Cubeta();

            long apDat = r.ReadInt64();
            cub.str_apDato(apDat);

            if(apDat != -1)
            {
                lee_datos_de_cubeta(r, cub, ent);
            }

            long apSig = r.ReadInt64();
            cub.str_apSigCubeta(apSig);

            // Si hay otra lista de cubetas despues de la que ya se leyo.
            if(apSig != -1 && apSig != 0)
            {
                cubL.Add(cub);
                posicionMemoria += tamCubeta;
                c.listaCubetas.Add(cubL);
                cubL = new List<Cubeta>();

                lee_cubetas_de_cajon(f, r, c, ent, cubL);
            }
            // Si ya no hay mas cubetas que leer.
            else if(apSig == -1)
            {
                cubL.Add(cub);
                posicionMemoria += tamCubeta;
                c.listaCubetas.Add(cubL);
            }
            // Si la cubeta correspondiente alberga un apuntador a dato valido.
            else if(apSig == 0)
            {
                cubL.Add(cub);
                posicionMemoria += tamCubeta;
                lee_cubetas_de_cajon(f, r, c, ent, cubL);
            }
        }

        /// <summary>
        /// Funcion con la que se leera un dato para ponerlo en la cubeta correspondiente.
        /// </summary>
        /// <param name="r">El lector de archivos binarios.</param>
        /// <param name="c">La cubeta en la que se pondra el dato.</param>
        /// <param name="ent">La entidad que contiene los atributos.</param>
        void lee_datos_de_cubeta(BinaryReader r, Cubeta c, Entidad ent)
        {
            Dato dataRead = new Dato(ent);

            // Primero se recorre la lista de atributos
            foreach (Atributo atr in ent.listaAtributos)
            {
                // Verificamos si el atributo no fue borrado
                if (atr.apSigAtributo != -2 && atr.apSigAtributo != -4)
                {
                    // Si no fue borrado, entonces revisamos de que tipo es ese atributo para leer el dato
                    if (atr.tipo == 'I')
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

                            for (int i = 0; i < chara.Length; i++)
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
                    switch (atr.tipo)
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

            posicionMemoria += tamDato;

            c.str_datoCubeta(dataRead);
        }

        /// <summary>
        /// Funcion con la que se leera una a una las cabeceras de la entidad.
        /// </summary>
        /// <param name="r">El lector de archivos binarios.</param>
        /// <param name="ent">La entidad que contiene los atributos.</param>
        void lee_cabeceras_de_entidad(BinaryReader r, Entidad ent)
        {
            foreach(Atributo atr in ent.listaAtributos)
            {
                if (atr.apSigAtributo != -2 && atr.apSigAtributo != -4)
                {
                    long apDatos = r.ReadInt64();

                    Cabecera cab = new Cabecera(apDatos);

                    posicionMemoria += 8;
                    ent.listaCabeceras.Add(cab);
                }
                else
                {
                    r.ReadInt64();
                }
            }
        }

        /// <summary>
        /// En esta funcion se rellenará el dataGridView correspondiente a los atributos de una entidad.
        /// </summary>
        /// <param name="ent">La entidad que posee la lista de atributos.</param>
        void manejo_dataGrid_atributos(Entidad ent)
        {
            dataGridView2.Rows.Clear();

            if (tipo != 3)
            {
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
                char tipoD;
                long longitud = 0;
                long posAt = 0;
                long apSigAt = 0;
                bool isLlave;

                foreach (Atributo at in ent.listaAtributos)
                {
                    if (at.apSigAtributo != -2 && at.apSigAtributo != -4)
                    {
                        nombreAtributo = new String(at.nombre);
                        tipoD = at.tipo;
                        longitud = at.bytes;
                        posAt = at.posAtributo;
                        apSigAt = at.apSigAtributo;
                        isLlave = at.esLlavePrimaria;

                        if (apSigAt < -1)
                        {
                            apSigAt = -1;
                        }

                        if (at.tipo == 'S')
                        {
                            longitud = longitud / 2;
                        }

                        fila = new string[] { nombreAtributo, tipoD.ToString(), longitud.ToString(), posAt.ToString(), apSigAt.ToString(), isLlave.ToString() };

                        filas.Add(fila);
                    }
                }

                foreach (string[] arr in filas)
                {
                    dataGridView2.Rows.Add(arr);
                }
            }
            else
            {
                dataGridView2.ColumnCount = 7;
                dataGridView2.ColumnHeadersVisible = true;

                dataGridView2.Columns[0].Name = "Nombre";
                dataGridView2.Columns[1].Name = "Tipo";
                dataGridView2.Columns[2].Name = "Longitud";
                dataGridView2.Columns[3].Name = "Pos. del Atributo";
                dataGridView2.Columns[4].Name = "Ap. Sig. Atributo";
                dataGridView2.Columns[5].Name = "Es llave primaria";
                dataGridView2.Columns[6].Name = "Es llave de busqueda.";

                List<String[]> filas = new List<string[]>();
                String[] fila = new string[] { };
                String nombreAtributo = "";
                char tipoD;
                long longitud = 0;
                long posAt = 0;
                long apSigAt = 0;
                bool isLlave;
                bool isBusqueda;

                foreach (Atributo at in ent.listaAtributos)
                {
                    if (at.apSigAtributo != -2 && at.apSigAtributo != -4)
                    {
                        nombreAtributo = new String(at.nombre);
                        tipoD = at.tipo;
                        longitud = at.bytes;
                        posAt = at.posAtributo;
                        apSigAt = at.apSigAtributo;
                        isLlave = at.esLlavePrimaria;
                        isBusqueda = at.esLlaveDeBusqueda;

                        if (apSigAt < -1)
                        {
                            apSigAt = -1;
                        }

                        if (at.tipo == 'S')
                        {
                            longitud = longitud / 2;
                        }

                        fila = new string[] { nombreAtributo, tipoD.ToString(), longitud.ToString(), posAt.ToString(), apSigAt.ToString(), isLlave.ToString(), isBusqueda.ToString() };

                        filas.Add(fila);
                    }
                }

                foreach (string[] arr in filas)
                {
                    dataGridView2.Rows.Add(arr);
                }
            }
        }

        /// <summary>
        /// Funcion recursiva que se encargara de leer todos los atributos de una entidad en un archivo.
        /// </summary>
        /// <param name="f">El FileStream con el que se manipulan los archivos.</param>
        /// <param name="r">El lector de archivos binarios.</param>
        /// <param name="ent">La entidad que contiene los atributos.</param>
        void lee_atributos_de_entidad(FileStream f, BinaryReader r, Entidad ent)
        {
            Atributo nAtributo = new Atributo();
            bool isSearch = false;

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

            if(tipo == 3)
            {
                isSearch = r.ReadBoolean();
            }

            long apSigAt = r.ReadInt64();

            if (tipo != 3)
            {
                nAtributo = new Atributo(nombreAT, type, lon, posAt, isKey, apSigAt);
                posicionMemoria = posicionMemoria + tamAtributo;
            }
            else
            {
                nAtributo = new Atributo(nombreAT, type, lon, posAt, isKey, isSearch, apSigAt);
                posicionMemoria = posicionMemoria + tamAtributo + 1;
            }

            ent.listaAtributos.Add(nAtributo);

            if (apSigAt != -2 && apSigAt != -4)
            {
                if (nAtributo.tipo == 'S')
                {
                    long bytes = nAtributo.bytes / 2;
                    tamDato += Convert.ToInt32(bytes);
                }
                else
                {
                    tamDato += Convert.ToInt32(nAtributo.bytes);
                }

                if(tipo == 3)
                {
                    if(isSearch == true)
                    {
                        tamDato += 8;
                    }
                }
            }

            nombreAT = new char[30];
            nombreAT[29] = '\n';

            if(apSigAt != -1 && apSigAt != -4)
            {
                lee_atributos_de_entidad(f, r, ent);
            }

            ent.tamDato = tamDato;
        }

        /// <summary>
        /// Funcion recursiva que se encargara de de leer los datos de la entidad.
        /// </summary>
        /// <param name="f">El FileStream con el que se manipulan los archivos.</param>
        /// <param name="r">El lector de archivos binarios.</param>
        /// <param name="ent">La entidad que contiene la lista de datos.</param>
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
        /// Funcion recursiva que se encargara de de leer los datos de la entidad para un archivo con multilistas.
        /// </summary>
        /// <param name="f">El FileStream con el que se manipulan los archivos.</param>
        /// <param name="r">El lector de archivos binarios.</param>
        /// <param name="ent">La entidad que contiene la lista de datos.</param>
        void lee_datos_de_entidad_multilistas(FileStream f, BinaryReader r, Entidad ent)
        {
            Dato dataRead = new Dato(ent);

            // Primero se recorre la lista de atributos
            foreach (Atributo atr in ent.listaAtributos)
            {
                // Verificamos si el atributo no fue borrado
                if (atr.apSigAtributo != -2 && atr.apSigAtributo != -4)
                {
                    // Si no fue borrado, entonces revisamos de que tipo es ese atributo para leer el dato
                    if (atr.tipo == 'I')
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

                            for (int i = 0; i < chara.Length; i++)
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
                    switch (atr.tipo)
                    {
                        case 'I':
                            r.ReadInt32();
                            break;
                        case 'F':
                            r.ReadSingle();
                            break;
                        case 'C':
                            r.ReadChar();
                            break;
                        case 'D':
                            r.ReadDouble();
                            break;
                        case 'L':
                            r.ReadInt64();
                            break;
                        case 'S':
                            char[] chara = new char[atr.bytes / 2];

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

            foreach(Atributo atr in ent.listaAtributos)
            {
                if(atr.apSigAtributo != -2 && atr.apSigAtributo != -4)
                {
                    long apCab = r.ReadInt64();

                    dataRead.apuntadoresLlaveBusq.Add(apCab);
                }
                else
                {
                    r.ReadInt64();
                }
            }

            posicionMemoria += tamDato;

            ent.listaDatos.Add(dataRead);
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
        /// Este metodo nos creara un nuevo archivo binario para el metodo de ordenacion secuencial indexada, por lo que antes de escribir
        /// las entidades, se escribira primero el rango de valores.
        /// </summary>
        /// <param name="nombreArchivo">El nombre del archivo a crearse.</param>
        public void crea_archivo_indexado(String nombreArchivo)
        {
            FileStream stream = new FileStream(nombreArchivo, FileMode.Create, FileAccess.Write);
            BinaryWriter writer = new BinaryWriter(stream);
            long cabecera = -1;
            long rango = 0;

            data.Add(cabecera);
            data.Add(rango);

            for (int i = 0; i < data.Count; i++)
            {
                if (i == 0)
                {
                    writer.Write(cabecera);
                }
                else if(i == 1)
                {
                    writer.Write(rango);
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
        /// Este metodo nos creara un nuevo archivo binario para el metodo de ordenacion por hash estatica, por lo que antes de escribir
        /// las entidades, se escribiran primero el numero de cajones y de registros por cubeta.
        /// </summary>
        /// <param name="nombreArchivo">El nombre del archivo a crearse.</param>
        public void crea_archivo_hash(String nombreArchivo)
        {
            FileStream stream = new FileStream(nombreArchivo, FileMode.Create, FileAccess.Write);
            BinaryWriter writer = new BinaryWriter(stream);
            long cabecera = -1;
            long cajones = 0;
            long registros = 0;

            data.Add(cabecera);
            data.Add(cajones);
            data.Add(registros);

            for(int i = 0; i < data.Count; i++)
            {
                if(i == 0)
                {
                    writer.Write(cabecera);
                }
                else if(i == 1)
                {
                    writer.Write(cajones);
                }
                else if(i == 2)
                {
                    writer.Write(registros);
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
        /// Esta funcion nos creara un nuevo archivo binario para el metodo de ordenacion por multilistas, por lo que no es necesario
        /// añadir algo entre la cabecera y la primer entidad.
        /// </summary>
        /// <param name="nombreArchivo">El nombre del archivo a crearse.</param>
        public void crea_archivo_multilistas(String nombreArchivo)
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
                        writer.Write(entidades[j].apCabeceras);
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
        /// <param name="archivo">El nombre del archivo que sera actualizado.</param>
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
                    break;
                }
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
        /// Esta funcion es parecida a la de crea_archivo_indexado, pero difiere en el hecho de que no crea el archivo, solo lo actualiza.
        /// </summary>
        /// <param name="archivo">El nombre del archivo que sera actualizado.</param>
        public void escribe_archivo_indexado(String archivo)
        {
            FileStream stream = new FileStream(archivo, FileMode.Create, FileAccess.Write);
            BinaryWriter writer = new BinaryWriter(stream);
            long cabecera = 16;

            for (int i = 0; i < data.Count; i++)
            {
                if (i == 0)
                {
                    writer.Write(cabecera);
                    break;
                }
            }

            writer.Write(rango);

            for (int j = 0; j < entidades.Count; j++)
            {
                for (int k = 0; k < entidades[j].nombre.Length; k++)
                {
                    writer.Write(entidades[j].nombre[k]);
                }

                writer.Write(entidades[j].apAtributos);
                writer.Write(entidades[j].apIndices);
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

                if(entidades[j].apIndices != -1)
                {
                    for(int l = 0; l < entidades[j].listaIndices.Count; l++)
                    {
                        long vIn, vF, posIn, apSigIn, apDat;

                        vIn = entidades[j].listaIndices[l].regresa_valInicial();
                        vF = entidades[j].listaIndices[l].regresa_valFinal();
                        posIn = entidades[j].listaIndices[l].regresa_posIndice();
                        apSigIn = entidades[j].listaIndices[l].regresa_apSigIndice();
                        apDat = entidades[j].listaIndices[l].regresa_apDatos();

                        writer.Write(vIn);
                        writer.Write(vF);
                        writer.Write(posIn);
                        writer.Write(apSigIn);
                        writer.Write(apDat);

                        for(int n = 0; n < entidades[j].listaIndices[l].datosIndice.Count; n++)
                        {
                            for(int m = 0; m < entidades[j].listaIndices[l].datosIndice[n].datos.Count; m++)
                            {
                                if(entidades[j].listaIndices[l].datosIndice[n].listaAtributosDato[m].tipo == 'I')
                                {
                                    writer.Write(Convert.ToInt32(entidades[j].listaIndices[l].datosIndice[n].datos[m]));
                                }
                                else if(entidades[j].listaIndices[l].datosIndice[n].listaAtributosDato[m].tipo == 'F')
                                {
                                    writer.Write(Convert.ToSingle(entidades[j].listaIndices[l].datosIndice[n].datos[m]));
                                }
                                else if (entidades[j].listaIndices[l].datosIndice[n].listaAtributosDato[m].tipo == 'L')
                                {
                                    writer.Write(Convert.ToInt64(entidades[j].listaIndices[l].datosIndice[n].datos[m]));
                                }
                                else if (entidades[j].listaIndices[l].datosIndice[n].listaAtributosDato[m].tipo == 'D')
                                {
                                    writer.Write(Convert.ToDouble(entidades[j].listaIndices[l].datosIndice[n].datos[m]));
                                }
                                else if (entidades[j].listaIndices[l].datosIndice[n].listaAtributosDato[m].tipo == 'C')
                                {
                                    writer.Write(Convert.ToChar(entidades[j].listaIndices[l].datosIndice[n].datos[m]));
                                }
                                else if (entidades[j].listaIndices[l].datosIndice[n].listaAtributosDato[m].tipo == 'S')
                                {
                                    String nuSt = "";

                                    if (entidades[j].listaIndices[l].datosIndice[n].datos[m] is char[])
                                    {
                                        nuSt = new string((char[])entidades[j].listaIndices[l].datosIndice[n].datos[m]);
                                    }

                                    char[] cadenaTemporal = 
                                            new char[entidades[j].listaIndices[l].datosIndice[n].listaAtributosDato[m].bytes / 2];

                                    for (int i = 0; i < entidades[j].listaIndices[l].datosIndice[n].listaAtributosDato[m].bytes / 2; i++)
                                    {
                                        cadenaTemporal[i] = '\0';
                                    }

                                    if (nuSt.Length > 0)
                                    {
                                        for (int o = 0; o < nuSt.Length; o++)
                                        {
                                            cadenaTemporal[o] = nuSt[o];
                                        }
                                    }
                                    else
                                    {
                                        for (int o = 0; o < entidades[j].listaIndices[l].datosIndice[n].datos[m].ToString().Length; o++)
                                        {
                                            cadenaTemporal[o] = entidades[j].listaIndices[l].datosIndice[n].datos[m].ToString()[o];
                                        }
                                    }

                                    for (int p = 0; p < cadenaTemporal.Length; p++)
                                    {
                                        writer.Write(cadenaTemporal[p]);
                                    }
                                }
                            }

                            writer.Write(entidades[j].listaIndices[l].datosIndice[n].apSigDato);
                        }
                        
                    }
                }
            }

            writer.Close();
            stream.Close();
        }

        /// <summary>
        /// Esta funcion es parecida a la de crea_archivo_hash, pero difiere en el hecho de que no crea el archivo, solo lo actualiza.
        /// </summary>
        /// <param name="archivo">El nombre del archivo que sera actualizado.</param>
        public void escribe_archivo_hash(String archivo)
        {
            FileStream stream = new FileStream(archivo, FileMode.Create, FileAccess.Write);
            BinaryWriter writer = new BinaryWriter(stream);
            long cabecera = 24;

            for (int i = 0; i < data.Count; i++)
            {
                if (i == 0)
                {
                    writer.Write(cabecera);
                    break;
                }
            }

            writer.Write(numCajones);
            writer.Write(regPorCajon);

            for (int j = 0; j < entidades.Count; j++)
            {
                for (int k = 0; k < entidades[j].nombre.Length; k++)
                {
                    writer.Write(entidades[j].nombre[k]);
                }

                writer.Write(entidades[j].apAtributos);
                writer.Write(entidades[j].apCajones);
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

                if(entidades[j].apCajones != -1)
                {
                    for(int k = 0; k < entidades[j].listaCajones.Count; k++)
                    {
                        long apCubeta = entidades[j].listaCajones[k].regresa_apuntadorCubeta();

                        writer.Write(apCubeta);

                        if(apCubeta != -1)
                        {
                            // Recorre la lista de listas de cubetas del cajon
                            for(int l = 0; l < entidades[j].listaCajones[k].listaCubetas.Count; l++)
                            {
                                // Recorre cada cubeta del la lista de listas
                                for(int m = 0; m < entidades[j].listaCajones[k].listaCubetas[l].Count; m++)
                                {
                                    if(m == entidades[j].listaCajones[k].listaCubetas[l].Count - 1)
                                    {
                                        long apDato = entidades[j].listaCajones[k].listaCubetas[l][m].regresa_apDato();
                                        long apSigCub = entidades[j].listaCajones[k].listaCubetas[l][m].regresa_apSigCubeta();

                                        writer.Write(apDato);
                                        writer.Write(apSigCub);
                                    }
                                    else
                                    {
                                        long apDato = entidades[j].listaCajones[k].listaCubetas[l][m].regresa_apDato();

                                        writer.Write(apDato);

                                        if(apDato != -1)
                                        {
                                            Dato dat = entidades[j].listaCajones[k].listaCubetas[l][m].regresa_datoCubeta();

                                            for(int n = 0; n < dat.datos.Count; n++)
                                            {
                                                if (dat.listaAtributosDato[n].tipo == 'I')
                                                {
                                                    writer.Write(Convert.ToInt32(dat.datos[n]));
                                                }
                                                else if (dat.listaAtributosDato[n].tipo == 'F')
                                                {
                                                    writer.Write(Convert.ToSingle(dat.datos[n]));
                                                }
                                                else if (dat.listaAtributosDato[n].tipo == 'L')
                                                {
                                                    writer.Write(Convert.ToInt64(dat.datos[n]));
                                                }
                                                else if (dat.listaAtributosDato[n].tipo == 'D')
                                                {
                                                    writer.Write(Convert.ToDouble(dat.datos[n]));
                                                }
                                                else if (dat.listaAtributosDato[n].tipo == 'C')
                                                {
                                                    writer.Write(Convert.ToChar(dat.datos[n]));
                                                }
                                                else if (dat.listaAtributosDato[n].tipo == 'S')
                                                {
                                                    String nuSt = "";

                                                    if (dat.datos[n] is char[])
                                                    {
                                                        nuSt = new string((char[])dat.datos[n]);
                                                    }

                                                    char[] cadenaTemporal =
                                                        new char[dat.listaAtributosDato[n].bytes / 2];

                                                    for (int i = 0; i < dat.listaAtributosDato[n].bytes / 2; i++)
                                                    {
                                                        cadenaTemporal[i] = '\0';
                                                    }

                                                    if (nuSt.Length > 0)
                                                    {
                                                        for (int o = 0; o < nuSt.Length; o++)
                                                        {
                                                            cadenaTemporal[o] = nuSt[o];
                                                        }
                                                    }
                                                    else
                                                    {
                                                        for (int o = 0; o < dat.datos[n].ToString().Length; o++)
                                                        {
                                                            cadenaTemporal[o] = dat.datos[n].ToString()[o];
                                                        }
                                                    }

                                                    for (int p = 0; p < cadenaTemporal.Length; p++)
                                                    {
                                                        writer.Write(cadenaTemporal[p]);
                                                    }
                                                }
                                            }
                                        }

                                        long apSigCub = entidades[j].listaCajones[k].listaCubetas[l][m].regresa_apSigCubeta();

                                        writer.Write(apSigCub);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            writer.Close();
            stream.Close();
        }

        /// <summary>
        /// Esta funcion es parecida a la de crea_archivo_hash, pero difiere en el hecho de que no crea el archivo, solo lo actualiza.
        /// </summary>
        /// <param name="archivo">El nombre del archivo que sera actualizado</param>
        public void escribe_archivo_multilistas(String archivo)
        {
            FileStream stream = new FileStream(archivo, FileMode.Create, FileAccess.Write);
            BinaryWriter writer = new BinaryWriter(stream);
            long cabecera = 8;

            for (int i = 0; i < data.Count; i++)
            {
                if (i == 0)
                {
                    writer.Write(cabecera);
                    break;
                }
            }

            for (int j = 0; j < entidades.Count; j++)
            {
                for (int k = 0; k < entidades[j].nombre.Length; k++)
                {
                    writer.Write(entidades[j].nombre[k]);
                }

                writer.Write(entidades[j].apAtributos);
                writer.Write(entidades[j].apCabeceras);
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
                        writer.Write(entidades[j].listaAtributos[l].esLlaveDeBusqueda);
                        writer.Write(entidades[j].listaAtributos[l].apSigAtributo);
                    }
                }

                if(entidades[j].apCabeceras != -1)
                {
                    for(int n = 0; n < entidades[j].listaCabeceras.Count; n++)
                    {
                        writer.Write(Convert.ToInt64(entidades[j].listaCabeceras[n].return_apDatos()));
                    }
                }

                if (entidades[j].listaDatos.Count > 0)
                {
                    for (int n = 0; n < entidades[j].listaDatos.Count; n++)
                    {
                        for (int m = 0; m < entidades[j].listaDatos[n].datos.Count; m++)
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
                            else if (entidades[j].listaDatos[n].listaAtributosDato[m].tipo == 'S')
                            {
                                String nuSt = "";

                                if (entidades[j].listaDatos[n].datos[m] is char[])
                                {
                                    nuSt = new string((char[])entidades[j].listaDatos[n].datos[m]);
                                }

                                char[] cadenaTemporal = new char[entidades[j].listaDatos[n].listaAtributosDato[m].bytes / 2];

                                for (int i = 0; i < entidades[j].listaDatos[n].listaAtributosDato[m].bytes / 2; i++)
                                {
                                    cadenaTemporal[i] = '\0';
                                }

                                if (nuSt.Length > 0)
                                {
                                    for (int o = 0; o < nuSt.Length; o++)
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

                                for (int p = 0; p < cadenaTemporal.Length; p++)
                                {
                                    writer.Write(cadenaTemporal[p]);
                                }
                            }
                        }

                        writer.Write(entidades[j].listaDatos[n].apSigDato);

                        for(int o = 0; o < entidades[j].listaDatos[n].apuntadoresLlaveBusq.Count; o++)
                        {
                            writer.Write(entidades[j].listaDatos[n].apuntadoresLlaveBusq[o]);
                        }
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

            comboBox3.Items.Add("Si");
            comboBox3.Items.Add("No");
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
                default: // Default vacio
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
        static bool verifica_llave_primaria(Entidad ent)
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

        /// <summary>
        /// Boton para añadir un atributo a una entidad.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">EventArgs.</param>
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
                            if (tipo != 3)
                            {
                                if (comboBox1.SelectedIndex > -1 && comboBox2.SelectedIndex > -1)
                                {
                                    insercion_atributo(entidadEncontrada);
                                }
                                else
                                {
                                    toolStripStatusLabel1.Text = "Error, selecciona el tipo de dato y si es llave primaria.";
                                }
                            }
                            else
                            {
                                if(comboBox1.SelectedIndex > -1 && comboBox2.SelectedIndex > -1 && comboBox3.SelectedIndex > -1)
                                {
                                    insercion_atributo(entidadEncontrada);
                                }
                                else
                                {
                                    toolStripStatusLabel1.Text = "Error, selecciona el tipo de dato, si es llave primaria y si es" +
                                        " llave de busqueda.";
                                }
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

        /// <summary>
        /// Metodo que permitira la insersión de un atributo en una entidad.
        /// </summary>
        /// <param name="entidadEncontrada">La entidad en la que se insertará el atributo.</param>
        private void insercion_atributo(Entidad entidadEncontrada)
        {
            // Se tiene que validar si ya se tiene un atributo que es llave primaria
            if (comboBox2.SelectedIndex == 0)
            {
                bool yaEsLlave = false;

                foreach (Atributo at in entidadEncontrada.listaAtributos)
                {
                    if (at.esLlavePrimaria == true)
                    {
                        yaEsLlave = true;
                        break;
                    }
                }

                if (yaEsLlave == false)
                {
                    if (tipo != 3 || (tipo == 3 && comboBox3.SelectedItem.ToString() == "No"))
                    {
                        if (entidadEncontrada.listaAtributos.Count > 0)
                        {
                            int byteSize = escoje_num_bytes();

                            if (tipo != 3)
                            {
                                Atributo nuevoAtributo = new Atributo(textBox3.Text, comboBox1.SelectedItem.ToString(),
                                    byteSize, comboBox2.SelectedItem.ToString());
                                nuevoAtributo.posAtributo = posicionMemoria;
                                posicionMemoria += tamAtributo;
                                entidadEncontrada.listaAtributos.Add(nuevoAtributo);
                                entidadEncontrada.listaAtributos[entidadEncontrada.listaAtributos.Count - 2].apSigAtributo =
                                    nuevoAtributo.posAtributo;
                            }
                            else
                            {
                                Atributo nuevoAtributo = new Atributo(textBox3.Text, comboBox1.SelectedItem.ToString(),
                                    byteSize, comboBox2.SelectedItem.ToString(), comboBox3.SelectedItem.ToString());
                                nuevoAtributo.posAtributo = posicionMemoria;
                                posicionMemoria += tamAtributo + 1;
                                entidadEncontrada.listaAtributos.Add(nuevoAtributo);
                                entidadEncontrada.listaAtributos[entidadEncontrada.listaAtributos.Count - 2].apSigAtributo =
                                    nuevoAtributo.posAtributo;
                            }

                            actualiza_archivo_atributo(entidadEncontrada);
                        }
                        else
                        {
                            int byteSize = escoje_num_bytes();

                            if (tipo != 3)
                            {
                                Atributo nuevoAtributo = new Atributo(textBox3.Text, comboBox1.SelectedItem.ToString(),
                                    byteSize, comboBox2.SelectedItem.ToString());
                                nuevoAtributo.posAtributo = posicionMemoria;
                                posicionMemoria += tamAtributo;
                                entidadEncontrada.listaAtributos.Add(nuevoAtributo);
                                entidadEncontrada.apAtributos = nuevoAtributo.posAtributo;
                            }
                            else
                            {
                                Atributo nuevoAtributo = new Atributo(textBox3.Text, comboBox1.SelectedItem.ToString(),
                                    byteSize, comboBox2.SelectedItem.ToString(), comboBox3.SelectedItem.ToString());
                                nuevoAtributo.posAtributo = posicionMemoria;
                                posicionMemoria += tamAtributo + 1;
                                entidadEncontrada.listaAtributos.Add(nuevoAtributo);
                                entidadEncontrada.apAtributos = nuevoAtributo.posAtributo;
                            }

                            actualiza_archivo_atributo(entidadEncontrada);
                        }
                    }
                    else if(tipo == 3 && comboBox3.SelectedItem.ToString() == "Si")
                    {
                        toolStripStatusLabel1.Text = "Error, una llave primaria no puede ser llave de busqueda.";
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

                    if (tipo != 3)
                    {
                        Atributo nuevoAtributo =
                            new Atributo(textBox3.Text, comboBox1.SelectedItem.ToString(), byteSize, comboBox2.SelectedItem.ToString());
                        nuevoAtributo.posAtributo = posicionMemoria;
                        posicionMemoria += tamAtributo;
                        entidadEncontrada.listaAtributos.Add(nuevoAtributo);
                        entidadEncontrada.listaAtributos[entidadEncontrada.listaAtributos.Count - 2].apSigAtributo =
                            nuevoAtributo.posAtributo;
                    }
                    else
                    {
                        Atributo nuevoAtributo =
                            new Atributo(textBox3.Text, comboBox1.SelectedItem.ToString(), byteSize, comboBox2.SelectedItem.ToString(), comboBox3.SelectedItem.ToString());
                        nuevoAtributo.posAtributo = posicionMemoria;
                        posicionMemoria += tamAtributo + 1;
                        entidadEncontrada.listaAtributos.Add(nuevoAtributo);
                        entidadEncontrada.listaAtributos[entidadEncontrada.listaAtributos.Count - 2].apSigAtributo =
                            nuevoAtributo.posAtributo;
                    }

                    actualiza_archivo_atributo(entidadEncontrada);
                }
                else
                {
                    int byteSize = escoje_num_bytes();

                    if (tipo != 3)
                    {
                        Atributo nuevoAtributo =
                            new Atributo(textBox3.Text, comboBox1.SelectedItem.ToString(), byteSize, comboBox2.SelectedItem.ToString());
                        nuevoAtributo.posAtributo = posicionMemoria;
                        posicionMemoria += tamAtributo;
                        entidadEncontrada.listaAtributos.Add(nuevoAtributo);
                        entidadEncontrada.apAtributos = nuevoAtributo.posAtributo;
                    }
                    else
                    {
                        Atributo nuevoAtributo =
                            new Atributo(textBox3.Text, comboBox1.SelectedItem.ToString(), byteSize, comboBox2.SelectedItem.ToString(), comboBox3.SelectedItem.ToString());
                        nuevoAtributo.posAtributo = posicionMemoria;
                        posicionMemoria += tamAtributo + 1;
                        entidadEncontrada.listaAtributos.Add(nuevoAtributo);
                        entidadEncontrada.apAtributos = nuevoAtributo.posAtributo;
                    }

                    actualiza_archivo_atributo(entidadEncontrada);
                }
            }
        }

        /// <summary>
        /// Metodo con el cual se actualizara un archivo tras la insercion de un atributo.
        /// </summary>
        private void actualiza_archivo_atributo(Entidad entidadEncontrada)
        {
            switch (tipo)
            {
                case 0:
                    escribe_archivo(textBox1.Text);

                    manejo_dataGrid(textBox1.Text);
                    break;
                case 1:
                    escribe_archivo_indexado(textBox1.Text);

                    manejo_dataGrid_indexado(textBox1.Text);
                    break;
                case 2:
                    escribe_archivo_hash(textBox1.Text);

                    manejo_dataGrid_hash(textBox1.Text);
                    break;
                case 3:
                    escribe_archivo_multilistas(textBox1.Text);

                    manejo_dataGrid_multilistas(textBox1.Text);
                    break;
            }

            manejo_dataGrid_atributos(entidadEncontrada);

            toolStripStatusLabel1.Text = "Atributo añadido con exito.";
        }

        /// <summary>
        /// Boton para modificar el nombre y tipo de dato de un atributo.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">EventArgs.</param>
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
                            int busq = 0;
                            bool esBusqueda = false;

                            using (ModificadorAtributo modificaAt = new ModificadorAtributo(tipo))
                            {
                                var nuevoAtributo = modificaAt.ShowDialog();

                                if (nuevoAtributo == DialogResult.OK)
                                {
                                    nuevoNombreAtributo = modificaAt.newNombre;
                                    nuevoTipoAtributo = modificaAt.newTipo;
                                    nuevosBytesAtributo = modificaAt.newBytes;
                                    if(tipo == 3)
                                    {
                                        busq = modificaAt.esBusqueda;
                                    }

                                    if(modificaAt.esLlave == 0)
                                    {
                                        esLlave = true;

                                        if(tipo == 3 && busq == 0)
                                        {
                                            toolStripStatusLabel1.Text = "Error, una llave primaria no puede ser llave de busqueda.";
                                            return;
                                        }
                                        else if(tipo == 3)
                                        {
                                            esBusqueda = false;
                                        }
                                    }
                                    else
                                    {
                                        esLlave = false;

                                        if(tipo == 3 && busq == 0)
                                        {
                                            esBusqueda = true;
                                        }
                                        else if(tipo == 3 && busq == 1)
                                        {
                                            esBusqueda = false;
                                        }
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

                                switch(tipo)
                                {
                                    case 0:
                                        escribe_archivo(textBox1.Text);

                                        entidadesLeidas = new List<Entidad>();

                                        manejo_dataGrid(textBox1.Text);

                                        manejo_dataGrid_atributos(entidadEncontrada);
                                        break;
                                    case 1:
                                        escribe_archivo_indexado(textBox1.Text);

                                        entidadesLeidas = new List<Entidad>();

                                        manejo_dataGrid_indexado(textBox1.Text);

                                        manejo_dataGrid_atributos(entidadEncontrada);
                                        break;
                                    case 2:
                                        escribe_archivo_hash(textBox1.Text);

                                        entidadesLeidas = new List<Entidad>();

                                        manejo_dataGrid_hash(textBox1.Text);

                                        manejo_dataGrid_atributos(entidadEncontrada);
                                        break;
                                    case 3:
                                        escribe_archivo_multilistas(textBox1.Text);

                                        entidadesLeidas = new List<Entidad>();

                                        manejo_dataGrid_multilistas(textBox1.Text);

                                        manejo_dataGrid_atributos(entidadEncontrada);
                                        break;
                                }
                                
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

                                switch (tipo)
                                {
                                    case 0:
                                        escribe_archivo(textBox1.Text);

                                        entidadesLeidas = new List<Entidad>();

                                        manejo_dataGrid(textBox1.Text);

                                        manejo_dataGrid_atributos(entidadEncontrada);
                                        break;
                                    case 1:
                                        escribe_archivo_indexado(textBox1.Text);

                                        entidadesLeidas = new List<Entidad>();

                                        manejo_dataGrid_indexado(textBox1.Text);

                                        manejo_dataGrid_atributos(entidadEncontrada);
                                        break;
                                    case 2:
                                        escribe_archivo_hash(textBox1.Text);

                                        entidadesLeidas = new List<Entidad>();

                                        manejo_dataGrid_hash(textBox1.Text);

                                        manejo_dataGrid_atributos(entidadEncontrada);
                                        break;
                                    case 3:
                                        escribe_archivo_multilistas(textBox1.Text);

                                        entidadesLeidas = new List<Entidad>();

                                        manejo_dataGrid_hash(textBox1.Text);

                                        manejo_dataGrid_atributos(entidadEncontrada);
                                        break;
                                }

                                toolStripStatusLabel1.Text = "Atributo modificado con exito.";
                            }
                            else
                            {
                                toolStripStatusLabel1.Text = "Error, nombre de atributo duplicado.";
                            }
                        }
                        else
                        {
                            toolStripStatusLabel1.Text = "Error, este atributo no existe en esta entidad";
                        }
                    }
                    else
                    {
                        toolStripStatusLabel1.Text = "Error, entidad no encontrada.";
                    }
                }
                else
                {
                    toolStripStatusLabel1.Text = "Error, esta atributo no existe";
                }
            }
        }

        /// <summary>
        /// Boton para eliminar un atributo (solo se requiere el nombre).
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">EventArgs.</param>
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

                            switch(tipo)
                            {
                                case 0:
                                    escribe_archivo(textBox1.Text);

                                    manejo_dataGrid(textBox1.Text);
                                    break;
                                case 1:
                                    escribe_archivo_indexado(textBox1.Text);

                                    manejo_dataGrid_indexado(textBox1.Text);
                                    break;
                                case 2:
                                    escribe_archivo_hash(textBox1.Text);

                                    manejo_dataGrid_hash(textBox1.Text);
                                    break;
                                case 3:
                                    escribe_archivo_multilistas(textBox1.Text);

                                    manejo_dataGrid_multilistas(textBox1.Text);
                                    break;
                            }

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
        /// Boton que muestra los atributos de una entidad escrita en el textBox correspondiente.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">EventArgs.</param>
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

        /// <summary>
        /// Boton que abre una ventana donde se insertaran los datos de forma secuencial ordenada.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">EventArgs.</param>
        private void button9_Click(object sender, EventArgs e)
        {
            if(textBox2.Text.Length > 0 && validacion(textBox2.Text) == true)
            {
                Entidad ent = busca_entidad(textBox2.Text);

                if(ent.apAtributos > -1 && hay_llave_primaria(ent) == true)
                {
                    using (CuadroDeDatos datosEntidad = new CuadroDeDatos(ent, posicionMemoria, ent.apDatos, ent.tamDato))
                    {
                        var cuadroDatos = datosEntidad.ShowDialog();

                        if(cuadroDatos == DialogResult.Cancel)
                        {
                            posicionMemoria = datosEntidad.posMemoria;
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
        private static bool hay_llave_primaria(Entidad ent)
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

        /// <summary>
        /// Boton que abre una ventana donde se insertaran los datos de forma secuencial indexada.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">EventArgs.</param>
        private void button10_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Length > 0 && validacion(textBox2.Text) == true)
            {
                Entidad ent = busca_entidad(textBox2.Text);

                if (ent.apAtributos > -1 && hay_llave_primaria(ent) == true)
                {
                    button9.Enabled = false;
                    
                    using(CuadroDeDatosIndexado datosIndexado = new CuadroDeDatosIndexado(ent, posicionMemoria, ent.tamDato, rango))
                    {
                        var cuadroIndice = datosIndexado.ShowDialog();

                        if(cuadroIndice == DialogResult.Cancel || cuadroIndice == DialogResult.OK)
                        {
                            ent.apIndices = datosIndexado.regresa_apuntador_listas();
                            posicionMemoria = datosIndexado.regresa_posMemoria();
                            ent.listaIndices = datosIndexado.regresa_listaIndices();
                            rango = datosIndexado.regresa_rango();

                            if(datosIndexado.regresa_seCambio() == true)
                            {
                                // Escribe archivo
                                escribe_archivo_indexado(textBox1.Text);

                                // Manejo dataGrid
                                manejo_dataGrid_indexado(textBox1.Text);
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
        /// Boton que abre una ventana donde se insertaran los datos via hash estatica.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">EventArgs.</param>
        private void button11_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Length > 0 && validacion(textBox2.Text) == true)
            {
                Entidad ent = busca_entidad(textBox2.Text);

                if (ent.apAtributos > -1 && hay_llave_primaria(ent) == true)
                {
                    using (CuadroDeDatosHash datosHash = new CuadroDeDatosHash(ent, posicionMemoria, ent.tamDato, numCajones, regPorCajon))
                    {
                        var cuadroHash = datosHash.ShowDialog();

                        if(cuadroHash == DialogResult.OK)
                        {
                            ent.apCajones = datosHash.regresa_apuntador_cajones();
                            posicionMemoria = datosHash.regresa_posMemoria();
                            ent.listaCajones = datosHash.regresa_lista_cajones();
                            numCajones = datosHash.regresa_numCajones();
                            regPorCajon = datosHash.regresa_regPorCubeta();

                            if(datosHash.regresa_seCambio() == true)
                            {
                                // Escribe archivo
                                escribe_archivo_hash(textBox1.Text);

                                // Manejo dataGrid
                                manejo_dataGrid_hash(textBox1.Text);
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
        /// Boton que abre una ventana donde se insertaran los datos via multilistas.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">EventArgs.</param>
        private void button12_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Length > 0 && validacion(textBox2.Text) == true)
            {
                Entidad ent = busca_entidad(textBox2.Text);

                if (ent.apAtributos > -1 && hay_llave_primaria(ent) == true)
                {
                    using (CuadroDeDatosMultilistas datosMultilistas = new CuadroDeDatosMultilistas(ent, posicionMemoria, ent.tamDato, ent.apCabeceras))
                    {
                        var cuadroMultilistas = datosMultilistas.ShowDialog();

                        if(cuadroMultilistas == DialogResult.OK)
                        {
                            ent.apCabeceras = datosMultilistas.regresa_apuntador_cabeceras();
                            posicionMemoria = datosMultilistas.regresa_posicion_memoria();
                            ent.listaCabeceras = datosMultilistas.regresa_lista_cabeceras();

                            if(datosMultilistas.regresa_se_cambio())
                            {
                                // Escribe archivo
                                escribe_archivo_multilistas(textBox1.Text);

                                // Manejo dataGrid
                                manejo_dataGrid_multilistas(textBox1.Text);
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
    }
}
