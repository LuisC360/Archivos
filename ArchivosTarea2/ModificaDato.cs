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
    public partial class ModificaDato : Form
    {
        public Entidad ent;
        public Dato dat;
        public bool llavePrimariaCambiada = false;
        List<Atributo> listaAtributosVigentes = new List<Atributo>();
        int atributosVigentes;
        int indexLlavePrimaria;      
        readonly Dato datoRespaldo;

        /// <summary>
        /// Constructor de la ventana de modificacion de un dato seleccionado.
        /// </summary>
        /// <param name="e">La entidad que contiene la lista de datos con el dato a modificarse. Se utilizara mas que nada para
        /// llenar las columnas del dataGridView con el nombre de cada atributo que el dato tiene.</param>
        /// <param name="d">El dato a modificarse.</param>
        /// <param name="indexK">El valor del indice de la llave primaria.</param>
        public ModificaDato(Entidad e, Dato d, int indexK)
        {
            ent = e;
            dat = d;
            indexLlavePrimaria = indexK;
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
            foreach(object d in dat.datos)
            {
                datoRespaldo.datos.Add(d);
            }
        }

        // Boton que aceptara los cambios.
        private void button1_Click(object sender, EventArgs e)
        {
            int celdaSeleccionada = dataGridView1.CurrentRow.Index;
            bool incompatible = false;

            for (int i = 0; i < dataGridView1.CurrentRow.Cells.Count - 2; i++)
            {
                if (dataGridView1.CurrentRow.Cells[i].ToString() != "")
                {
                    Atributo at = listaAtributosVigentes[i];
                    bool casillaLlavePrimaria = false;

                    if(at.esLlavePrimaria == true)
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

                            if(casillaLlavePrimaria == true)
                            {
                                int valA = Convert.ToInt32(datoRespaldo.datos[i]);
                                int valB = Convert.ToInt32(resultado);

                                if(valA != valB)
                                {
                                    llavePrimariaCambiada = true;

                                    if(verifica_llave_primaria(resultado, dat) == true)
                                    {
                                        MessageBox.Show("Error, llave primaria duplicada.");
                                        return;
                                    }
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
                                    llavePrimariaCambiada = true;

                                    if (verifica_llave_primaria(resultado, dat) == true)
                                    {
                                        MessageBox.Show("Error, llave primaria duplicada.");
                                        return;
                                    }
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

                            dat.datos[i] = res.ToLower();

                            if (casillaLlavePrimaria == true)
                            {
                                string valA = new String(datoRespaldo.datos[i].ToString().ToCharArray());
                                string valB = Convert.ToString(resultado);

                                if (valA.Equals(valB) == false)
                                {
                                    llavePrimariaCambiada = true;

                                    if (verifica_llave_primaria(resultado, dat) == true)
                                    {
                                        MessageBox.Show("Error, llave primaria duplicada.");
                                        return;
                                    }
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
                                    llavePrimariaCambiada = true;

                                    if (verifica_llave_primaria(resultado, dat) == true)
                                    {
                                        MessageBox.Show("Error, llave primaria duplicada.");
                                        return;
                                    }
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
                                    llavePrimariaCambiada = true;

                                    if (verifica_llave_primaria(resultado, dat) == true)
                                    {
                                        MessageBox.Show("Error, llave primaria duplicada.");
                                        return;
                                    }
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
                                    llavePrimariaCambiada = true;

                                    if (verifica_llave_primaria(resultado, dat) == true)
                                    {
                                        MessageBox.Show("Error, llave primaria duplicada.");
                                        return;
                                    }
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
                    return;
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Metodo que rellena el dataGrid con la informacion del dato a modificarse.
        /// </summary>
        private void rellena_data_grid()
        {
            // Contamos el numero de columnas que se añadiran al dataGridView
            foreach(Atributo atr in ent.listaAtributos)
            {
                if(atr.apSigAtributo != -2 && atr.apSigAtributo != -4)
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

            for(int i = 0; i < atributosVigentes; i++)
            {
                String nombreAtr = new String(listaAtributosVigentes[i].nombre);
                dataGridView1.Columns[i].Name = nombreAtr;
            }

            dataGridView1.Columns[atributosVigentes].Name = "Pos. Dato";
            dataGridView1.Columns[atributosVigentes + 1].Name = "Ap. Sig. Dato.";

            // Rellena el dataGridView con la informacion del dato a modificar
            String[] fila = new string[atributosVigentes + 2];
            int count = 0;

            for(int j = 0; j < atributosVigentes; j++)
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

        /// <summary>
        /// Metodo con el que se validara si la nueva llave primaria no existe dentro de la lista de datos de la entidad.
        /// </summary>
        /// <param name="llave">El nuevo valor de llave primaria del dato modificado.</param>
        /// <returns>Bandera que indica si la llave primaria esta duplicada.</returns>
        private bool verifica_llave_primaria(dynamic llave, Dato modificado)
        {
            bool duplicada = false;

            foreach(Dato dat in ent.listaDatos)
            {
                if (dat != modificado)
                {
                    dynamic llaveComparar = dat.datos[indexLlavePrimaria];

                    if (llaveComparar == llave)
                    {
                        duplicada = true;
                        break;
                    }
                }
            }

            return duplicada;
        }

        /// <summary>
        /// Metodo que regresa el tipo de dato acorde al valor dado por el atributo.
        /// </summary>
        /// <param name="tatr">El caracter que define el tipo de atributo de cada valor del dato.</param>
        /// <returns>El tipo de dato del valor del dato.</returns>
        private dynamic valida_atributo(char tatr)
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
    }
}
