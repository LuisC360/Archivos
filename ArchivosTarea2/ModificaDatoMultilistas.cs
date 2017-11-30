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
    /// Clase que representa la ventana de modificacion de un dato en multilistas.
    /// </summary>
    public partial class ModificaDatoMultilistas : Form
    {
        /// <summary>
        /// Bandera que nos dira si se cambio o no la llave primaria del dato.
        /// </summary>
        public bool llavePrimariaCambiada { get; set; }
        /// <summary>
        /// La entidad que posee el dato a modificarse.
        /// </summary>
        readonly Entidad ent;
        /// <summary>
        /// El dato a modificarse.
        /// </summary>
        public Dato dato { get; set; }
        /// <summary>
        /// Bandera que nos dira si se cambio algun atributo que sea llave de busqueda, independientemente de cual haya sido.
        /// </summary>
        public bool llaveBusquedaCambiada { get; set; }
        /// <summary>
        /// El numero de atributos vigentes de la entidad (aquellos que no han sido eliminados).
        /// </summary>
        int atributosVigentes;
        /// <summary>
        /// La lista de atributos vigentes de la entidad (aquellos que no han sido eliminados).
        /// </summary>
        readonly List<Atributo> listaAtributosVigentes = new List<Atributo>();
        /// <summary>
        /// El indice de la llave primaria en la lista de atributos del dato.
        /// </summary>
        int indiceLlavePrimaria;
        /// <summary>
        /// Un respaldo del dato a modificarse para verificar si se cambio la llave primaria o alguna llave de busqueda.
        /// </summary>
        readonly Dato datoRespaldo;

        /// <summary>
        /// Constructor de una nueva ventana de modificacion de dato.
        /// </summary>
        public ModificaDatoMultilistas(Dato d, Entidad e, int indK)
        {
            dato = d;
            ent = e;
            indiceLlavePrimaria = indK;
            datoRespaldo = new Dato();

            InitializeComponent();
            rellena_dataGrid();
            crea_referencia_nuevo_dato();
        }

        /// <summary>
        /// Boton que capturara la nueva informacion del dato y cerrara la ventana actual.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">EventArgs.</param>
        private void button1_Click(object sender, EventArgs e)
        {
            int celdaSeleccionada = dataGridView1.CurrentRow.Index;
            bool incompatible = false;

            for (int i = 0; i < dataGridView1.CurrentRow.Cells.Count - listaAtributosVigentes.Count - 2; i++)
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

            dataGridView1.ColumnCount = listaAtributosVigentes.Count + listaAtributosVigentes.Count + 2;
            dataGridView1.ColumnHeadersVisible = true;
            int columnCount = 0;

            for (int i = 0; i < listaAtributosVigentes.Count; i++)
            {
                String columnName = new string(listaAtributosVigentes[i].nombre);
                dataGridView1.Columns[i].Name = columnName;
                columnCount++;
            }

            dataGridView1.Columns[columnCount].Name = "Pos. Dato";
            columnCount++;
            dataGridView1.Columns[columnCount].Name = "Ap. Sig. Dato";
            columnCount++;

            for (int j = 0; j < listaAtributosVigentes.Count; j++)
            {
                String columnName1 = new string(listaAtributosVigentes[j].nombre);
                String columnName2 = "Ap_" + columnName1;
                dataGridView1.Columns[columnCount].Name = columnName2;
                columnCount++;
            }

            string[] fila = new string[listaAtributosVigentes.Count + listaAtributosVigentes.Count + 2];
            int count = 0;

            for (int i = 0; i < dato.datos.Count; i++)
            {
                if (dato.datos[i] is char[])
                {
                    char[] arr = (char[])dato.datos[i];
                    String objeto = new string(arr);
                    fila[i] = objeto;
                }
                else
                {
                    fila[i] = dato.datos[i].ToString();
                }
                count++;
            }

            fila[count] = dato.posDato.ToString();
            count++;

            if (dato.apSigDato == -3)
            {
                String menos = "-1";
                fila[count] = menos;
            }
            else
            {
                fila[count] = dato.apSigDato.ToString();
            }
            count++;

            for (int j = 0; j < dato.apuntadoresLlaveBusq.Count; j++)
            {
                fila[count] = dato.apuntadoresLlaveBusq[j].ToString();
                count++;
            }

            dataGridView1.Rows.Add(fila);
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
        ///  Funcion que verifica si la llave primaria del dato que se cambio no es la misma que la llave primaria de otro dato.
        /// </summary>
        /// <param name="llave">La nueva llave primaria del dato modificado.</param>
        /// <param name="datoModificado">El dato modificado.</param>
        /// <returns>Booleano que sera 'true' si la llave primaria del nuevo dato esta ya asignada a otro dato.</returns>
        private bool verifica_llave_primaria(dynamic llave, Dato datoModificado)
        {
            bool duplicada = false;

            List<Dato> datosOrdenados = ent.listaDatos.OrderBy(o => o.datos[indiceLlavePrimaria]).ToList();

            foreach(Dato dat in datosOrdenados)
            {
                if(dat.apSigDato != -2 && dat.apSigDato != -4 && dat != datoModificado)
                {
                    dynamic llaveComparar = dat.datos[indiceLlavePrimaria];

                    if(llaveComparar == llave)
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
        /// Funcion que regresa el dato o registro modificado.
        /// </summary>
        /// <returns>El dato o registro modificado.</returns>
        public Dato regresa_dato_multilistas()
        {
            return dato;
        }

        /// <summary>
        /// Funcion que regresa la bandera de si se cambio la llave primaria.
        /// </summary>
        /// <returns>La bandera de si se cambio la llave primaria.</returns>
        public bool regresa_llave_primaria_cambiada()
        {
            return llavePrimariaCambiada;
        }

        /// <summary>
        /// Funcion que regresa la bandera de si se cambio una llave de busqueda, cualquiera.
        /// </summary>
        /// <returns>La bandera de si se cambio una llave de busqueda</returns>
        public bool regresa_llave_busqueda_cambiada()
        {
            return llaveBusquedaCambiada;
        }
    }
}
