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
        long rango;
        long tamIndice = 40;
        public bool bandChanged { get; set; }
        readonly List<Indice> indicesVigentes = new List<Indice>();
        Atributo attrLlave = new Atributo();
        int indiceLlave;

        public CuadroDeDatosIndexado(Entidad e, long pMem, long tamDat)
        {
            ent = e;
            posMemoria = pMem;
            tamDato = tamDat;

            foreach(Atributo atr in ent.listaAtributos)
            {
                if (atr.apSigAtributo != -2 && atr.apSigAtributo != -4)
                {
                    numAtributos++;
                    atributosVigentes.Add(atr);
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

            inicia_dataGridIndices();

            //if()

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
            if(textBox1.Text.Length > 0)
            {
                try 
                {
                    rango = long.Parse(textBox1.Text);
                    textBox1.ReadOnly = true;
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
            int celdaSeleccionada = dataGridView1.CurrentRow.Index;
            bool incompatible = false;
            List<object> datos = new List<object>();

            for(int i = 0; i < dataGridView1.CurrentRow.Cells.Count; i++)
            {
                if(dataGridView1.CurrentRow.Cells[i].ToString() != "")
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

                Dato nuevoDato = new Dato(ent);

                foreach (Object obj in datos)
                {
                    nuevoDato.datos.Add(obj);
                }

                inserta_dato_indice(nuevoDato);
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
            if(ent.apIndices == -1)
            {
                ent.listaIndices.Add(crea_indice(dat));
            }
            else
            {
                // Se buscara el indice cuyo intervalo acepte el dato que se desea insertar.
            }
        }

        private Indice crea_indice(Dato d)
        {
            Indice nuevoIndice = new Indice();
            atrLlave = d.regresa_llave_primaria();
            indiceLlave = d.indice_llave_primaria();

            char tipo = atrLlave.tipo;
            dynamic valor;
            dynamic valIni = 0;
            dynamic valFin = 0;
            bool intervaloEncontrado = false;

            switch(tipo)
            {
                case 'I': valor = Convert.ToInt32(d.datos[indiceLlave]);
                    Convert.ChangeType(valIni, typeof(int));
                    Convert.ChangeType(valFin, typeof(int));

                    valIni = 0;
                    valFin = valIni + rango;

                    do{
                        if(valFin > valor && valor > valIni)
                        {
                            nuevoIndice.srt_valorInicial(valIni);
                            nuevoIndice.srt_valorFinal(valFin);
                            nuevoIndice.srt_posIndice(posMemoria);
                            posMemoria = posMemoria + tamIndice;
                            nuevoIndice.srt_apDatos(d.posDato);

                            intervaloEncontrado = true;
                        }
                        else
                        {
                            valIni += rango;
                            valFin += rango;
                        }
                    }while(intervaloEncontrado == false);
                    break;
                case 'L': valor = Convert.ToInt64(d.datos[indiceLlave]);
                    Convert.ChangeType(valIni, typeof(long));
                    Convert.ChangeType(valFin, typeof(long));

                    valIni = 0;
                    valFin = valIni + rango;

                    do{
                        if(valFin > valor && valor > valIni)
                        {
                            nuevoIndice.srt_valorInicial(valIni);
                            nuevoIndice.srt_valorFinal(valFin);
                            nuevoIndice.srt_posIndice(posMemoria);
                            posMemoria = posMemoria + tamIndice;
                            nuevoIndice.srt_apDatos(d.posDato);

                            intervaloEncontrado = true;
                        }
                        else
                        {
                            valIni += rango;
                            valFin += rango;
                        }
                    }while(intervaloEncontrado == false);
                    break;
                case 'F': valor = Convert.ToSingle(d.datos[indiceLlave]);
                    Convert.ChangeType(valIni, typeof(float));
                    Convert.ChangeType(valFin, typeof(float));

                    valIni = 0;
                    valFin = valIni + rango;

                    do{
                        if(valFin > valor && valor > valIni)
                        {
                            nuevoIndice.srt_valorInicial(valIni);
                            nuevoIndice.srt_valorFinal(valFin);
                            nuevoIndice.srt_posIndice(posMemoria);
                            posMemoria = posMemoria + tamIndice;
                            nuevoIndice.srt_apDatos(d.posDato);

                            intervaloEncontrado = true;
                        }
                        else
                        {
                            valIni += rango;
                            valFin += rango;
                        }
                    }while(intervaloEncontrado == false);
                    break;
                case 'D': valor = Convert.ToDouble(d.datos[indiceLlave]);
                    Convert.ChangeType(valIni, typeof(double));
                    Convert.ChangeType(valFin, typeof(double));

                    valIni = 0;
                    valFin = valIni + rango;

                    do{
                        if(valFin > valor && valor > valIni)
                        {
                            nuevoIndice.srt_valorInicial(valIni);
                            nuevoIndice.srt_valorFinal(valFin);
                            nuevoIndice.srt_posIndice(posMemoria);
                            posMemoria = posMemoria + tamIndice;
                        }
                        else
                        {
                            valIni += rango;
                            valFin += rango;
                        }
                    }while(intervaloEncontrado == false);
                    break;
                case 'C': valor = Convert.ToChar(d.datos[indiceLlave]);
                    Convert.ChangeType(valIni, typeof(char));
                    Convert.ChangeType(valFin, typeof(char));

                    valIni = 'a';
                    valFin = valIni + rango;

                    do{
                        if(valFin > valor && valor > valIni)
                        {
                            nuevoIndice.srt_valorInicial(valIni);
                            nuevoIndice.srt_valorFinal(valFin);
                            nuevoIndice.srt_posIndice(posMemoria);
                            posMemoria = posMemoria + tamIndice;
                        }
                        else
                        {
                            valIni += rango;
                            valFin += rango;
                        }
                    }while(intervaloEncontrado == false);
                    break;
                default: // No hay mas tipos de datos que pueden ser llave primaria, asi que esto se dejara vacio.
                    break;
            }

            toolStripStatusLabel1.Text = "Indice creado y dato insertado con exito.";

            return nuevoIndice;
        }

        public Entidad regresa_entidad()
        {
            return ent;
        }

        public long regresa_posMemoria()
        {
            return posMemoria;
        }

        public bool regresa_seCambio()
        {
            return bandChanged;
        }
    }
}
