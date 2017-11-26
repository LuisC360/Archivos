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
    public partial class ModificaDatoHash : Form
    {
        readonly Entidad ent;
        public Cajon cajon { get; set; }
        public Dato dato { get; set; }
        public bool llavePrimariaCambiada { get; set; }
        int atributosVigentes;
        readonly int indiceLlavePrimaria;
        readonly Dato datoRespaldo;
        readonly List<Atributo> listaAtributosVigentes = new List<Atributo>();

        /// <summary>
        /// Constructor de la ventana de modificacion de un dato en hash estatica.
        /// </summary>
        /// <param name="c">El cajon que contiene la cubeta que contiene a su vez el dato a ser modificado.</param>
        /// <param name="d">El dato a ser modificado.</param>
        /// <param name="e">La entidad que contiene toda la informacion requerida.</param>
        /// <param name="indK">El indice de la llave primaria.</param>
        public ModificaDatoHash(Cajon c, Dato d, Entidad e, int indK)
        {
            cajon = c;
            dato = d;
            ent = e;
            indiceLlavePrimaria = indK;
            datoRespaldo = new Dato();

            this.Location = new Point(100, 100);
            InitializeComponent();

            rellena_dataGrid();
            crea_referencia_nuevo_dato();
        }

        /// <summary>
        /// Metodo que crea un respaldo del dato a modificar para ver si se cambio el dato llave primaria.
        /// </summary>
        private void crea_referencia_nuevo_dato()
        {
            foreach (object d in dato.datos)
            {
                datoRespaldo.datos.Add(d);
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
        /// Metodo que rellena el dataGrid con la informacion del dato a modificar.
        /// </summary>
        private void rellena_dataGrid()
        {
            foreach (Atributo atr in ent.listaAtributos)
            {
                if (atr.apSigAtributo != -2 && atr.apSigAtributo != -4)
                {
                    atributosVigentes++;
                    listaAtributosVigentes.Add(atr);
                }
            }

            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            dataGridView1.ColumnCount = atributosVigentes + 1;
            dataGridView1.ColumnHeadersVisible = true;

            for(int i = 0; i < atributosVigentes; i++)
            {
                String nombreAtr = new String(listaAtributosVigentes[i].nombre);
                dataGridView1.Columns[i].Name = nombreAtr;
            }

            dataGridView1.Columns[atributosVigentes].Name = "Pos. Dato";

            String[] fila = new string[atributosVigentes + 1];
            int count = 0;

            for (int j = 0; j < atributosVigentes; j++)
            {
                char tipo = listaAtributosVigentes[j].tipo;
                dynamic dat = dato.datos[j];

                switch (tipo)
                {
                    case 'I':
                        dat = Convert.ToInt32(dat);
                        break;
                    case 'F':
                        dat = Convert.ToSingle(dat);
                        break;
                    case 'L':
                        dat = Convert.ToInt64(dat);
                        break;
                    case 'D':
                        dat = Convert.ToDouble(dat);
                        break;
                    case 'C':
                        dat = Convert.ToChar(dat);
                        break;
                    case 'S':
                        if (dat is char[])
                        {
                            char[] arr = (char[])dat;
                            String objeto = new string(arr);
                            dat = objeto;
                        }
                        dat = Convert.ToString(dat);
                        break;
                }

                fila[count] = dat.ToString();
                count++;
            }

            fila[count] = dato.posDato.ToString();

            dataGridView1.Rows.Add(fila);
        }

        // Boton de captura de los datos y de cierre de la ventana de cambios.
        private void button1_Click(object sender, EventArgs e)
        {
            int celdaSeleccionada = dataGridView1.CurrentRow.Index;
            bool incompatible = false;

            for (int i = 0; i < dataGridView1.CurrentRow.Cells.Count - 1; i++)
            {
                if (dataGridView1.CurrentRow.Cells[i].ToString() != "")
                {
                    Atributo at = listaAtributosVigentes[i];
                    bool casillaLlavePrimaria = false;

                    if (at.esLlavePrimaria == true)
                    {
                        casillaLlavePrimaria = true;
                    }

                    char tipoAtr = at.tipo;

                    var tipo = valida_atributo(tipoAtr);

                    dynamic strTipo = tipo;
                    Object resultado = null;

                    if (strTipo == typeof(int))
                    {
                        try
                        {
                            resultado = Convert.ChangeType(this.dataGridView1.Rows[celdaSeleccionada].Cells[i].Value, typeof(int));
                            dato.datos[i] = resultado;

                            if (casillaLlavePrimaria == true)
                            {
                                int valA = Convert.ToInt32(datoRespaldo.datos[i]);
                                int valB = Convert.ToInt32(resultado);

                                if (valA != valB)
                                {
                                    if (verifica_llave_primaria(resultado, dato) == true)
                                    {
                                        MessageBox.Show("Error, llave primaria duplicada.");
                                        return;
                                    }

                                    llavePrimariaCambiada = true;
                                }
                            }
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

                            dato.datos[i] = resultado;

                            if (casillaLlavePrimaria == true)
                            {
                                char valA = Convert.ToChar(datoRespaldo.datos[i]);
                                char valB = Convert.ToChar(resultado);

                                if (valA != valB)
                                {
                                    if (verifica_llave_primaria(resultado, dato) == true)
                                    {
                                        MessageBox.Show("Error, llave primaria duplicada.");
                                        return;
                                    }

                                    llavePrimariaCambiada = true;
                                }
                            }
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

                            if (res.Length > (at.bytes / 2))
                            {
                                int start = Convert.ToInt32(at.bytes);
                                start = start / 2;
                                int count = res.Length - start;
                                res = res.Remove(start, count);
                            }

                            dato.datos[i] = res.ToLowerInvariant();

                            if (casillaLlavePrimaria == true)
                            {
                                string valA = new String(datoRespaldo.datos[i].ToString().ToCharArray());
                                string valB = Convert.ToString(resultado);

                                if (valA.Equals(valB) == false)
                                {
                                    if (verifica_llave_primaria(resultado, dato) == true)
                                    {
                                        MessageBox.Show("Error, llave primaria duplicada.");
                                        return;
                                    }

                                    llavePrimariaCambiada = true;
                                }
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Error, tipo de dato incompatible / campo vacio");
                            incompatible = true;
                            return;
                        }
                    }
                    else if (strTipo == typeof(float))
                    {
                        try
                        {
                            resultado = Convert.ChangeType(this.dataGridView1.Rows[celdaSeleccionada].Cells[i].Value, typeof(float));
                            dato.datos[i] = resultado;

                            if (casillaLlavePrimaria == true)
                            {
                                float valA = Convert.ToSingle(datoRespaldo.datos[i]);
                                float valB = Convert.ToSingle(resultado);

                                if (valA != valB)
                                {
                                    if (verifica_llave_primaria(resultado, dato) == true)
                                    {
                                        MessageBox.Show("Error, llave primaria duplicada.");
                                        return;
                                    }

                                    llavePrimariaCambiada = true;
                                }
                            }
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
                            dato.datos[i] = resultado;

                            if (casillaLlavePrimaria == true)
                            {
                                double valA = Convert.ToDouble(datoRespaldo.datos[i]);
                                double valB = Convert.ToDouble(resultado);

                                if (valA != valB)
                                {
                                    if (verifica_llave_primaria(resultado, dato) == true)
                                    {
                                        MessageBox.Show("Error, llave primaria duplicada.");
                                        return;
                                    }

                                    llavePrimariaCambiada = true;
                                }
                            }
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
                            dato.datos[i] = resultado;

                            if (casillaLlavePrimaria == true)
                            {
                                long valA = Convert.ToInt64(datoRespaldo.datos[i]);
                                long valB = Convert.ToInt64(resultado);

                                if (valA != valB)
                                {
                                    if (verifica_llave_primaria(resultado, dato) == true)
                                    {
                                        MessageBox.Show("Error, llave primaria duplicada.");
                                        return;
                                    }

                                    llavePrimariaCambiada = true;
                                }
                            }
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
                    MessageBox.Show("Error, no se puede dejar un campo vacio.");
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Funcion que verifica si la llave primaria del dato que se cambio no es la misma que la llave primaria de otro dato.
        /// </summary>
        /// <param name="llave">La nueva llave primaria del dato modificado.</param>
        /// <param name="modificado">El dato modificado.</param>
        /// <returns>Booleano que sera 'true' si la llave primaria del nuevo dato esta ya asignada a otro dato.</returns>
        private bool verifica_llave_primaria(dynamic llave, Dato modificado)
        {
            bool duplicada = false;

            foreach(Cajon caj in ent.listaCajones)
            {
                if(caj.regresa_apuntadorCubeta() != -2)
                {
                    foreach(List<Cubeta> listCub in caj.listaCubetas)
                    {
                        for(int i = 0; i < listCub.Count - 1; i++)
                        {
                            if (listCub[i].regresa_apDato() != -1 && listCub[i].regresa_datoCubeta() != modificado &&
                                listCub[i].regresa_datoCubeta().apSigDato != -2)
                            {
                                dynamic llaveComparar = listCub[i].regresa_datoCubeta().datos[indiceLlavePrimaria];

                                if(llaveComparar == llave)
                                {
                                    duplicada = true;
                                    break;
                                }
                            }
                        }

                        if(duplicada == true)
                        {
                            break;
                        }
                    }
                }

                if(duplicada == true)
                {
                    break;
                }
            }

            return duplicada;
        }

        // Funciones de retorno de informacion a la ventana de manipulacion de datos en hash estatica.
        public Dato regresa_datoHash()
        {
            return dato;
        }

        public bool regresa_llavePrimariaCambiada()
        {
            return llavePrimariaCambiada;
        }
        // Funciones de retorno de informacion a la ventana de manipulacion de datos en hash estatica.
    }
}
