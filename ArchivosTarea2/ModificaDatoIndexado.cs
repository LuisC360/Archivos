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
    public partial class ModificaDatoIndexado : Form
    {
        readonly Entidad ent;
        public Indice ind { get; set; }
        public Dato dat { get; set; }
        public bool llavePrimariaCambiada { get; set; }
        readonly List<Atributo> listaAtributosVigentes = new List<Atributo>();
        int atributosVigentes;
        readonly int indexLlavePrimaria;
        readonly Dato datoRespaldo;

        /// <summary>
        /// Constructor de la ventana de modificación de un dato indexado.
        /// </summary>
        /// <param name="i">El indice actual en el que esta el dato.</param>
        /// <param name="d">El dato a modificar.</param>
        /// <param name="e">La entidad que contiene la lisya con los indices.</param>
        /// <param name="indK">El indice de la llave primaria en la lista de atributos.</param>
        public ModificaDatoIndexado(Indice i, Dato d, Entidad e, int indK)
        {
            ent = e;
            ind = i;
            dat = d;
            indexLlavePrimaria = indK;
            datoRespaldo = new Dato();

            InitializeComponent();

            rellena_data_grid();
            crea_referencia_nuevo_dato();
        }

        /// <summary>
        /// Metodo que crea un respaldo del dato a modificar para ver si se cambio el dato llave primaria.
        /// </summary>
        private void crea_referencia_nuevo_dato()
        {
            foreach (object d in dat.datos)
            {
                datoRespaldo.datos.Add(d);
            }
        }

        /// <summary>
        /// Metodo que rellena el dataGrid con la informacion del dato a modificarse.
        /// </summary>
        private void rellena_data_grid()
        {
            // Contamos el numero de columnas que se añadiran al dataGridView
            foreach (Atributo atr in ent.listaAtributos)
            {
                if (atr.apSigAtributo != -2 && atr.apSigAtributo != -4)
                {
                    atributosVigentes++;
                    listaAtributosVigentes.Add(atr);
                }
            }

            // Inicia el dataGridView
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            dataGridView1.ColumnCount = atributosVigentes + 2;
            dataGridView1.ColumnHeadersVisible = true;

            for (int i = 0; i < atributosVigentes; i++)
            {
                String nombreAtr = new String(listaAtributosVigentes[i].nombre);
                dataGridView1.Columns[i].Name = nombreAtr;
            }

            dataGridView1.Columns[atributosVigentes].Name = "Pos. Dato";
            dataGridView1.Columns[atributosVigentes + 1].Name = "Ap. Sig. Dato.";

            // Rellena el dataGridView con la informacion del dato a modificar
            String[] fila = new string[atributosVigentes + 2];
            int count = 0;

            for (int j = 0; j < atributosVigentes; j++)
            {
                char tipo = listaAtributosVigentes[j].tipo;
                dynamic dato = dat.datos[j];

                switch (tipo)
                {
                    case 'I': dato = Convert.ToInt32(dato);
                        break;
                    case 'F': dato = Convert.ToSingle(dato);
                        break;
                    case 'L': dato = Convert.ToInt64(dato);
                        break;
                    case 'D': dato = Convert.ToDouble(dato);
                        break;
                    case 'C': dato = Convert.ToChar(dato);
                        break;
                    case 'S': dato = Convert.ToString(dato);
                        break;
                }

                fila[count] = dato.ToString();
                count++;
            }

            fila[count] = dat.posDato.ToString();
            count++;
            fila[count] = dat.apSigDato.ToString();

            dataGridView1.Rows.Add(fila);
        }

        // Boton que acepta los cambios realizados.
        private void button1_Click(object sender, EventArgs e)
        {
            int celdaSeleccionada = dataGridView1.CurrentRow.Index;
            bool incompatible = false;

            for (int i = 0; i < dataGridView1.CurrentRow.Cells.Count - 2; i++)
            {
                if(dataGridView1.CurrentRow.Cells[i].ToString() != "")
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
                            dat.datos[i] = resultado;

                            if (casillaLlavePrimaria == true)
                            {
                                int valA = Convert.ToInt32(datoRespaldo.datos[i]);
                                int valB = Convert.ToInt32(resultado);

                                if (valA != valB)
                                {                                   
                                    if (verifica_llave_primaria(resultado, dat) == true)
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

                            dat.datos[i] = resultado;

                            if (casillaLlavePrimaria == true)
                            {
                                char valA = Convert.ToChar(datoRespaldo.datos[i]);
                                char valB = Convert.ToChar(resultado);

                                if (valA != valB)
                                {
                                    if (verifica_llave_primaria(resultado, dat) == true)
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

                            dat.datos[i] = res.ToLowerInvariant();

                            if (casillaLlavePrimaria == true)
                            {
                                string valA = new String(datoRespaldo.datos[i].ToString().ToCharArray());
                                string valB = Convert.ToString(resultado);

                                if (valA.Equals(valB) == false)
                                {
                                    if (verifica_llave_primaria(resultado, dat) == true)
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
                            dat.datos[i] = resultado;

                            if (casillaLlavePrimaria == true)
                            {
                                float valA = Convert.ToSingle(datoRespaldo.datos[i]);
                                float valB = Convert.ToSingle(resultado);

                                if (valA != valB)
                                {
                                    if (verifica_llave_primaria(resultado, dat) == true)
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
                            dat.datos[i] = resultado;

                            if (casillaLlavePrimaria == true)
                            {
                                double valA = Convert.ToDouble(datoRespaldo.datos[i]);
                                double valB = Convert.ToDouble(resultado);

                                if (valA != valB)
                                {
                                    if (verifica_llave_primaria(resultado, dat) == true)
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
                            dat.datos[i] = resultado;

                            if (casillaLlavePrimaria == true)
                            {
                                long valA = Convert.ToInt64(datoRespaldo.datos[i]);
                                long valB = Convert.ToInt64(resultado);

                                if (valA != valB)
                                {
                                    if (verifica_llave_primaria(resultado, dat) == true)
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
        /// Metodo con el que se validara si la nueva llave primaria no existe dentro de la lista de indices de la entidad.
        /// </summary>
        /// <param name="llave">El nuevo valor de llave primaria del dato modificado.</param>
        /// <returns>Bandera que indica si la llave primaria esta duplicada.</returns>
        private bool verifica_llave_primaria(dynamic llave, Dato modificado)
        {
            bool duplicada = false;

            foreach(Indice ind in ent.listaIndices)
            {
                if(ind.regresa_apDatos() != -2)
                {
                    foreach(Dato dt in ind.datosIndice)
                    {
                        if(dt.apSigDato != -3 && dt.apSigDato != -4)
                        {
                            dynamic llaveComparar = dt.datos[indexLlavePrimaria];

                            if(llaveComparar == llave)
                            {
                                duplicada = true;
                                break;
                            }
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

        public bool regresa_llavePrimariaCambiada()
        {
            return llavePrimariaCambiada;
        }

        public Dato regresa_datoIndexado()
        {
            return dat;
        }
    }
}
