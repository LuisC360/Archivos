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
    public partial class CuadroDeDatos : Form
    {
        public Entidad ent;
        int numAtributos = 0;
        Atributo atrLlave;
        List<Atributo> atributosVigentes = new List<Atributo>();
        public long posMemoria = 0;
        long tamDato = 0;
        public long apDatos = 0;
        public bool bandChanged = false;
        List<Dato> datosVigentes = new List<Dato>();
        /** NOTA: Intentarlo con una lista tipo dynamic **/

        int valorMasBajoInt = 0;
        List<int> valoresInt = new List<int>(); 

        float valorMasBajoFloat = 0;
        List<float> valoresFloat = new List<float>();

        double valorMasBajoDouble = 0;
        List<double> valoresDouble = new List<double>();

        long valorMasBajoLong = 0;
        List<long> valoresLong = new List<long>();

        char valorMasBajoChar = ' ';
        List<char> valoresChar = new List<char>();

        string valorMasBajoString = "";
        List<string> valoresString = new List<string>();

        Dato datoValorMasBajo = new Dato();
        int posDatoValorMasBajo = 0;

        Atributo attr = new Atributo();
        public int indiceLlave = 0;

        /// <summary>
        /// Constructor del cuadro de datos.
        /// </summary>
        /// <param name="e">La entidad en la que se insertaran datos.</param>
        /// <param name="pMem">La posicion de memoria actual.</param>
        /// <param name="apDat">El apuntador a datos de la entidad.</param>
        /// <param name="tamDat">El tamaño del dato en bytes.</param>
        public CuadroDeDatos(Entidad e, long pMem, long apDat, long tamDat)
        {
            ent = e;
            posMemoria = pMem;
            apDatos = apDat;

            foreach(Atributo atr in ent.listaAtributos)
            {
                if (atr.apSigAtributo != -2 && atr.apSigAtributo != -4)
                {
                    numAtributos++;
                }
            }

            foreach(Dato dat in ent.listaDatos)
            {
                if(dat.apSigDato != -3 && dat.apSigDato != -4)
                {
                    datosVigentes.Add(dat);
                }
            }

            InitializeComponent();

            rellena_dataGrid();

            tamDato = tamDat;

            // Si la lista no esta vacia
            if(datosVigentes.Count > 0)
            {
                pobla_dataGrid();
            }

            button4.Enabled = false;
        }

        // Boton para añadir un dato
        private void button1_Click(object sender, EventArgs e)
        {           
            int celdaSeleccionada = dataGridView1.CurrentRow.Index;
            bool incompatible = false;
            List<object> datos = new List<object>();

            valoresInt = new List<int>();
            valoresFloat = new List<float>();
            valoresDouble = new List<double>();
            valoresLong = new List<long>();
            valoresChar = new List<char>();

            // Recorremos cada celda de la fila del dataGridView sobre la que se inserto el valor
            for (int i = 0; i < dataGridView1.CurrentRow.Cells.Count - 1; i++ )
            {
                // Verifica que la celda no este vacia
                if (dataGridView1.CurrentRow.Cells[i].ToString() != "")
                {
                    Atributo atr = atributosVigentes[i];

                    // Validar el tipo de atributo
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

                            if (resultado.ToString().Length > (atr.bytes / 2))
                            {
                                resultado.ToString().Remove((Convert.ToInt32(atr.bytes) / 2), resultado.ToString().Length);
                                return;
                            }

                            datos.Add(resultado.ToString().ToLower());
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
                    
                    if(incompatible == true)
                    {
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Error, no se pueden dejar campos vacios");
                    return;
                }
            }

            Dato nuevoDato = new Dato(ent);

            attr = nuevoDato.regresa_llave_primaria();

            if(attr.posAtributo == 0)
            {
                MessageBox.Show("Error, no existe una llave primaria.");
                return;
            }

            indiceLlave = nuevoDato.indice_llave_primaria();
            int posDatoComparar = 0;

            switch(attr.tipo)
            {
                case 'I': foreach (Dato dt in datosVigentes)
                          {
                              if (dt.apSigDato != -3 && dt.apSigDato != -4)
                              {
                                  valoresInt.Add(Convert.ToInt32(dt.datos[indiceLlave]));
                              }
                          }
                          break;
                case 'F': foreach (Dato dt in datosVigentes)
                          {
                              if (dt.apSigDato != -3 && dt.apSigDato != -4)
                              {
                                  valoresFloat.Add((float)dt.datos[indiceLlave]);
                              }
                          }
                          break;
                case 'D': foreach (Dato dt in datosVigentes)
                          {
                              if (dt.apSigDato != -3 && dt.apSigDato != -4)
                              {
                                  valoresDouble.Add(Convert.ToDouble(dt.datos[indiceLlave]));
                              }
                          }
                          break;
                case 'L': foreach (Dato dt in datosVigentes)
                          {
                              if (dt.apSigDato != -3 && dt.apSigDato != -4)
                              {
                                  valoresLong.Add(Convert.ToInt64(dt.datos[indiceLlave]));
                              }
                          }
                          break;
                case 'C': foreach (Dato dt in datosVigentes)
                          {
                              if (dt.apSigDato != -3 && dt.apSigDato != -4)
                              {
                                  valoresChar.Add(Convert.ToChar(dt.datos[indiceLlave]));
                              }
                          }
                          break;
                case 'S': foreach(Dato dt in datosVigentes)
                          {
                              if (dt.apSigDato != -3 && dt.apSigDato != -4)
                              {
                                  valoresString.Add(Convert.ToString(dt.datos[indiceLlave]));
                              }
                          }
                          break;
            }
            
            foreach(Object obj in datos)
            {
                nuevoDato.datos.Add(obj);
            }

            // Si la lista tiene mas de 1 atributo, se calculara el valor mas bajo 
            if(datosVigentes.Count > 1)
            {
                switch(attr.tipo)
                {
                    case 'I': valorMasBajoInt = encuentra_valor_mas_bajo(indiceLlave);                       
                        break;
                    case 'F': valorMasBajoFloat = encuentra_valor_mas_bajo(indiceLlave);
                        break;
                    case 'D': valorMasBajoDouble = encuentra_valor_mas_bajo(indiceLlave);
                        break;
                    case 'L': valorMasBajoLong = encuentra_valor_mas_bajo(indiceLlave);
                        break;
                    case 'C': valorMasBajoChar = encuentra_valor_mas_bajo(indiceLlave);
                        break;
                    case 'S': valorMasBajoString = encuentra_valor_mas_bajo(indiceLlave);
                        break;
                }

                datoValorMasBajo = encuentra_dato_valor_mas_bajo(indiceLlave);
            }

            // Ahora se validara el dato mediante su llave primaria
            if (datosVigentes.Count == 0)
            {
                nuevoDato.posDato = posMemoria;
                posMemoria += tamDato;
                ent.listaDatos.Add(nuevoDato);
                datosVigentes.Add(nuevoDato);
                datoValorMasBajo = nuevoDato;
                ent.apDatos = nuevoDato.posDato;
                apDatos = ent.apDatos;

                switch(attr.tipo)
                {
                    case 'I': valorMasBajoInt = Convert.ToInt32(nuevoDato.datos[indiceLlave]);
                              posDatoValorMasBajo = 0;
                              break;
                    case 'F': valorMasBajoFloat = (float)nuevoDato.datos[indiceLlave];
                              posDatoValorMasBajo = 0;
                              break;
                    case 'D': valorMasBajoDouble = Convert.ToDouble(nuevoDato.datos[indiceLlave]);
                              posDatoValorMasBajo = 0;
                              break;
                    case 'L': valorMasBajoLong = Convert.ToInt64(nuevoDato.datos[indiceLlave]);
                              posDatoValorMasBajo = 0;
                              break;
                    case 'C': valorMasBajoChar = Convert.ToChar(nuevoDato.datos[indiceLlave]);
                              posDatoValorMasBajo = 0;
                              break;
                    case 'S': valorMasBajoString = Convert.ToString(nuevoDato.datos[indiceLlave]);
                              posDatoValorMasBajo = 0;
                              break;
                }

                dataGridView1.CurrentRow.Cells[atributosVigentes.Count].Value = nuevoDato.apSigDato;
            }
            else
            {
                foreach (Dato dat in datosVigentes)
                {
                    if (dat.apSigDato != -3 && dat.apSigDato != -4)
                    {
                        if (attr.tipo == 'I' || attr.tipo == 'F' || attr.tipo == 'D' || attr.tipo == 'L')
                        {
                            int datoA = 0;
                            int datoB = 0;
                            float datoC = 0;
                            float datoD = 0;
                            double datoE = 0;
                            double datoF = 0;
                            long datoG = 0;
                            long datoH = 0;

                            switch (attr.tipo)
                            {
                                case 'I': datoA = Convert.ToInt32(dat.datos[indiceLlave]);
                                    datoB = Convert.ToInt32(nuevoDato.datos[indiceLlave]);
                                    break;
                                case 'F': datoC = (float)dat.datos[indiceLlave];
                                    datoD = (float)nuevoDato.datos[indiceLlave];
                                    break;
                                case 'D': datoE = Convert.ToDouble(dat.datos[indiceLlave]);
                                    datoF = Convert.ToDouble(nuevoDato.datos[indiceLlave]);
                                    break;
                                case 'L': datoG = Convert.ToInt64(dat.datos[indiceLlave]);
                                    datoH = Convert.ToInt64(nuevoDato.datos[indiceLlave]);
                                    break;
                            }

                            //**---------- ENTEROS ----------**//
                            if (datoA != 0 && datoB != 0)
                            {
                                if (datoA == datoB)
                                {
                                    MessageBox.Show("Error, llave primaria duplicada.");
                                    break;
                                }
                                // Si el dato insertado es el valor mas pequeño
                                else if (datoB < datoA && dat == datoValorMasBajo)
                                {
                                    nuevoDato.posDato = posMemoria;
                                    posMemoria += tamDato;

                                    nuevoDato.apSigDato = dat.posDato;
                                    ent.apDatos = nuevoDato.posDato;
                                    apDatos = ent.apDatos;

                                    ent.listaDatos.Add(nuevoDato);
                                    datosVigentes.Add(nuevoDato);

                                    valorMasBajoInt = Convert.ToInt32(nuevoDato.datos[indiceLlave]);
                                    posDatoValorMasBajo = ent.listaDatos.IndexOf(nuevoDato);
                                    datoValorMasBajo = nuevoDato;

                                    dataGridView1.CurrentRow.Cells[atributosVigentes.Count].Value = nuevoDato.apSigDato;

                                    for (int dg = 0; dg < dataGridView1.Rows.Count - 1; dg++)
                                    {
                                        if (dataGridView1.Rows[dg].Cells[indiceLlave].Value.ToString() == dat.datos[indiceLlave].ToString())
                                        {
                                            dataGridView1.Rows[dg].Cells[atributosVigentes.Count].Value = dat.apSigDato;
                                            break;
                                        }
                                    }

                                    break;
                                }
                                else if (datoB < datoA)
                                {
                                    nuevoDato.posDato = posMemoria;
                                    posMemoria += tamDato;

                                    List<int> valoresResta = new List<int>();
                                    List<int> sustraendos = new List<int>();
                                    Dato datoPredecesor = new Dato();

                                    foreach (Dato dt in datosVigentes)
                                    {
                                        if (dt.apSigDato != -3 && dt.apSigDato != -4)
                                        {
                                            int sustraendo = Convert.ToInt32(dt.datos[indiceLlave]);
                                            int resta = datoB - sustraendo;

                                            if (resta >= 0)
                                            {
                                                valoresResta.Add(resta);
                                                sustraendos.Add(sustraendo);
                                            }
                                        }
                                    }

                                    var valorRestaMenor = valoresResta.Min();

                                    for (int i = 0; i < sustraendos.Count; i++)
                                    {
                                        if (valoresResta[i] == valorRestaMenor)
                                        {
                                            foreach (Dato data in datosVigentes)
                                            {
                                                if (data.apSigDato != -3 && data.apSigDato != -4)
                                                {
                                                    if (Convert.ToInt32(data.datos[indiceLlave]) == sustraendos[i])
                                                    {
                                                        datoPredecesor = data;
                                                        break;
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    }

                                    datoPredecesor.apSigDato = nuevoDato.posDato;
                                    nuevoDato.apSigDato = dat.posDato;
                                    ent.listaDatos.Add(nuevoDato);
                                    datosVigentes.Add(nuevoDato);

                                    dataGridView1.CurrentRow.Cells[atributosVigentes.Count].Value = nuevoDato.apSigDato;

                                    for (int dg = 0; dg < dataGridView1.Rows.Count - 1; dg++)
                                    {
                                        if (dataGridView1.Rows[dg].Cells[indiceLlave].Value.ToString() == dat.datos[indiceLlave].ToString())
                                        {
                                            dataGridView1.Rows[dg].Cells[atributosVigentes.Count].Value = dat.apSigDato;
                                            break;
                                        }
                                    }

                                    for (int dg = 0; dg < dataGridView1.Rows.Count - 1; dg++)
                                    {
                                        if (dataGridView1.Rows[dg].Cells[indiceLlave].Value.ToString() == datoPredecesor.datos[indiceLlave].ToString())
                                        {
                                            dataGridView1.Rows[dg].Cells[atributosVigentes.Count].Value = datoPredecesor.apSigDato;
                                            break;
                                        }
                                    }

                                    break;
                                }
                                // Si es el valor mas grande de todos
                                else if (datosVigentes.Count - 1 == posDatoComparar && datoB > datoA)
                                {
                                    nuevoDato.posDato = posMemoria;
                                    posMemoria += tamDato;

                                    Dato datoValorMasGrande = new Dato();

                                    var valorMasGrande = valoresInt.Max();

                                    // Buscamos el dato con ese valor
                                    foreach (Dato dt in datosVigentes)
                                    {
                                        if (dt.apSigDato != -3 && dt.apSigDato != -4)
                                        {
                                            if (Convert.ToInt32(dt.datos[indiceLlave]) == valorMasGrande)
                                            {
                                                datoValorMasGrande = dt;
                                                break;
                                            }
                                        }
                                    }

                                    datoValorMasGrande.apSigDato = nuevoDato.posDato;
                                    ent.listaDatos.Add(nuevoDato);
                                    datosVigentes.Add(nuevoDato);

                                    dataGridView1.CurrentRow.Cells[atributosVigentes.Count].Value = nuevoDato.apSigDato;

                                    for (int dg = 0; dg < dataGridView1.Rows.Count - 1; dg++)
                                    {
                                        if (dataGridView1.Rows[dg].Cells[indiceLlave].Value.ToString() == datoValorMasGrande.datos[indiceLlave].ToString())
                                        {
                                            dataGridView1.Rows[dg].Cells[atributosVigentes.Count].Value = datoValorMasGrande.apSigDato;
                                            break;
                                        }
                                    }

                                    break;
                                }
                                else if (datoB > datoA)
                                {
                                    posDatoComparar++;
                                }
                            }
                            //**---------- FLOTANTES ----------**//
                            else if (datoC != 0 && datoD != 0)
                            {
                                if (datoC == datoD)
                                {
                                    MessageBox.Show("Error, llave primaria duplicada.");
                                    break;
                                }
                                // Si el dato insertado es el valor mas pequeño
                                else if (datoD < datoC && dat == datoValorMasBajo)
                                {
                                    nuevoDato.posDato = posMemoria;
                                    posMemoria += tamDato;

                                    nuevoDato.apSigDato = dat.posDato;
                                    ent.apDatos = nuevoDato.posDato;
                                    apDatos = ent.apDatos;

                                    ent.listaDatos.Add(nuevoDato);
                                    datosVigentes.Add(nuevoDato);

                                    valorMasBajoFloat = (float)nuevoDato.datos[indiceLlave];
                                    posDatoValorMasBajo = ent.listaDatos.IndexOf(nuevoDato);
                                    datoValorMasBajo = nuevoDato;

                                    dataGridView1.CurrentRow.Cells[atributosVigentes.Count].Value = nuevoDato.apSigDato;

                                    for (int dg = 0; dg < dataGridView1.Rows.Count - 1; dg++)
                                    {
                                        if (dataGridView1.Rows[dg].Cells[indiceLlave].Value.ToString() == dat.datos[indiceLlave].ToString())
                                        {
                                            dataGridView1.Rows[dg].Cells[atributosVigentes.Count].Value = dat.apSigDato;
                                            break;
                                        }
                                    }

                                    break;
                                }
                                else if (datoD < datoC)
                                {
                                    nuevoDato.posDato = posMemoria;
                                    posMemoria += tamDato;

                                    List<float> valoresResta = new List<float>();
                                    List<float> sustraendos = new List<float>();
                                    Dato datoPredecesor = new Dato();

                                    foreach (Dato dt in datosVigentes)
                                    {
                                        if (dt.apSigDato != -3 && dt.apSigDato != -4)
                                        {
                                            float sustraendo = (float)dt.datos[indiceLlave];
                                            float resta = datoD - sustraendo;

                                            if (resta >= 0)
                                            {
                                                valoresResta.Add(resta);
                                                sustraendos.Add(sustraendo);
                                            }
                                        }
                                    }

                                    var valorRestaMenor = valoresResta.Min();

                                    for (int i = 0; i < sustraendos.Count; i++)
                                    {
                                        if (valoresResta[i] == valorRestaMenor)
                                        {
                                            foreach (Dato data in datosVigentes)
                                            {
                                                if (data.apSigDato != -3 && data.apSigDato != -4)
                                                {
                                                    if (((float)(data.datos[indiceLlave])) == sustraendos[i])
                                                    {
                                                        datoPredecesor = data;
                                                        break;
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    }

                                    datoPredecesor.apSigDato = nuevoDato.posDato;
                                    nuevoDato.apSigDato = dat.posDato;
                                    ent.listaDatos.Add(nuevoDato);
                                    datosVigentes.Add(nuevoDato);

                                    dataGridView1.CurrentRow.Cells[atributosVigentes.Count].Value = nuevoDato.apSigDato;

                                    for (int dg = 0; dg < dataGridView1.Rows.Count - 1; dg++)
                                    {
                                        if (dataGridView1.Rows[dg].Cells[indiceLlave].Value.ToString() == dat.datos[indiceLlave].ToString())
                                        {
                                            dataGridView1.Rows[dg].Cells[atributosVigentes.Count].Value = dat.apSigDato;
                                            break;
                                        }
                                    }

                                    for (int dg = 0; dg < dataGridView1.Rows.Count - 1; dg++)
                                    {
                                        if (dataGridView1.Rows[dg].Cells[indiceLlave].Value.ToString() == datoPredecesor.datos[indiceLlave].ToString())
                                        {
                                            dataGridView1.Rows[dg].Cells[atributosVigentes.Count].Value = datoPredecesor.apSigDato;
                                            break;
                                        }
                                    }

                                    break;
                                }
                                // Si es el valor mas grande de todos
                                else if (datosVigentes.Count - 1 == posDatoComparar && datoD > datoC)
                                {
                                    nuevoDato.posDato = posMemoria;
                                    posMemoria += tamDato;

                                    Dato datoValorMasGrande = new Dato();

                                    var valorMasGrande = valoresFloat.Max();

                                    // Buscamos el dato con ese valor
                                    foreach (Dato dt in datosVigentes)
                                    {
                                        if (dt.apSigDato != -3 && dt.apSigDato != -4)
                                        {
                                            if (((float)dt.datos[indiceLlave]) == valorMasGrande)
                                            {
                                                datoValorMasGrande = dt;
                                                break;
                                            }
                                        }
                                    }

                                    datoValorMasGrande.apSigDato = nuevoDato.posDato;
                                    ent.listaDatos.Add(nuevoDato);
                                    datosVigentes.Add(nuevoDato);

                                    dataGridView1.CurrentRow.Cells[atributosVigentes.Count].Value = nuevoDato.apSigDato;

                                    for (int dg = 0; dg < dataGridView1.Rows.Count - 1; dg++)
                                    {
                                        if (dataGridView1.Rows[dg].Cells[indiceLlave].Value.ToString() == datoValorMasGrande.datos[indiceLlave].ToString())
                                        {
                                            dataGridView1.Rows[dg].Cells[atributosVigentes.Count].Value = datoValorMasGrande.apSigDato;
                                            break;
                                        }
                                    }

                                    break;
                                }
                                else if (datoD > datoC)
                                {
                                    posDatoComparar++;
                                }
                            }
                            //**---------- DOBLES ----------**//
                            else if (datoE != 0 && datoF != 0)
                            {
                                if (datoE == datoF)
                                {
                                    MessageBox.Show("Error, llave primaria duplicada.");
                                    break;
                                }
                                // Si el dato insertado es el valor mas pequeño
                                else if (datoF < datoE && dat == datoValorMasBajo)
                                {
                                    nuevoDato.posDato = posMemoria;
                                    posMemoria += tamDato;

                                    nuevoDato.apSigDato = dat.posDato;
                                    ent.apDatos = nuevoDato.posDato;
                                    apDatos = ent.apDatos;

                                    ent.listaDatos.Add(nuevoDato);
                                    datosVigentes.Add(nuevoDato);

                                    valorMasBajoDouble = Convert.ToDouble(nuevoDato.datos[indiceLlave]);
                                    posDatoValorMasBajo = ent.listaDatos.IndexOf(nuevoDato);
                                    datoValorMasBajo = nuevoDato;

                                    dataGridView1.CurrentRow.Cells[atributosVigentes.Count].Value = nuevoDato.apSigDato;

                                    for (int dg = 0; dg < dataGridView1.Rows.Count - 1; dg++)
                                    {
                                        if (dataGridView1.Rows[dg].Cells[indiceLlave].Value.ToString() == dat.datos[indiceLlave].ToString())
                                        {
                                            dataGridView1.Rows[dg].Cells[atributosVigentes.Count].Value = dat.apSigDato;
                                            break;
                                        }
                                    }

                                    break;
                                }
                                else if (datoF < datoE)
                                {
                                    nuevoDato.posDato = posMemoria;
                                    posMemoria += tamDato;

                                    List<double> valoresResta = new List<double>();
                                    List<double> sustraendos = new List<double>();
                                    Dato datoPredecesor = new Dato();

                                    foreach (Dato dt in datosVigentes)
                                    {
                                        if (dt.apSigDato != -3 && dt.apSigDato != -4)
                                        {
                                            double sustraendo = Convert.ToDouble(dt.datos[indiceLlave]);
                                            double resta = datoF - sustraendo;

                                            if (resta >= 0)
                                            {
                                                valoresResta.Add(resta);
                                                sustraendos.Add(sustraendo);
                                            }
                                        }
                                    }

                                    var valorRestaMenor = valoresResta.Min();

                                    for (int i = 0; i < sustraendos.Count; i++)
                                    {
                                        if (valoresResta[i] == valorRestaMenor)
                                        {
                                            foreach (Dato data in datosVigentes)
                                            {
                                                if (data.apSigDato != -3 && data.apSigDato != -4)
                                                {
                                                    if (Convert.ToDouble(data.datos[indiceLlave]) == sustraendos[i])
                                                    {
                                                        datoPredecesor = data;
                                                        break;
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    }

                                    datoPredecesor.apSigDato = nuevoDato.posDato;
                                    nuevoDato.apSigDato = dat.posDato;
                                    ent.listaDatos.Add(nuevoDato);
                                    datosVigentes.Add(nuevoDato);

                                    dataGridView1.CurrentRow.Cells[atributosVigentes.Count].Value = nuevoDato.apSigDato;

                                    for (int dg = 0; dg < dataGridView1.Rows.Count - 1; dg++)
                                    {
                                        if (dataGridView1.Rows[dg].Cells[indiceLlave].Value.ToString() == dat.datos[indiceLlave].ToString())
                                        {
                                            dataGridView1.Rows[dg].Cells[atributosVigentes.Count].Value = dat.apSigDato;
                                            break;
                                        }
                                    }

                                    for (int dg = 0; dg < dataGridView1.Rows.Count - 1; dg++)
                                    {
                                        if (dataGridView1.Rows[dg].Cells[indiceLlave].Value.ToString() == datoPredecesor.datos[indiceLlave].ToString())
                                        {
                                            dataGridView1.Rows[dg].Cells[atributosVigentes.Count].Value = datoPredecesor.apSigDato;
                                            break;
                                        }
                                    }

                                    break;
                                }
                                // Si es el valor mas grande de todos
                                else if (datosVigentes.Count - 1 == posDatoComparar && datoF > datoE)
                                {
                                    nuevoDato.posDato = posMemoria;
                                    posMemoria += tamDato;

                                    Dato datoValorMasGrande = new Dato();

                                    var valorMasGrande = valoresDouble.Max();

                                    // Buscamos el dato con ese valor
                                    foreach (Dato dt in datosVigentes)
                                    {
                                        if (dt.apSigDato != -3 && dt.apSigDato != -4)
                                        {
                                            if (Convert.ToDouble(dt.datos[indiceLlave]) == valorMasGrande)
                                            {
                                                datoValorMasGrande = dt;
                                                break;
                                            }
                                        }
                                    }

                                    datoValorMasGrande.apSigDato = nuevoDato.posDato;
                                    ent.listaDatos.Add(nuevoDato);
                                    datosVigentes.Add(nuevoDato);

                                    dataGridView1.CurrentRow.Cells[atributosVigentes.Count].Value = nuevoDato.apSigDato;

                                    for (int dg = 0; dg < dataGridView1.Rows.Count - 1; dg++)
                                    {
                                        if (dataGridView1.Rows[dg].Cells[indiceLlave].Value.ToString() == datoValorMasGrande.datos[indiceLlave].ToString())
                                        {
                                            dataGridView1.Rows[dg].Cells[atributosVigentes.Count].Value = datoValorMasGrande.apSigDato;
                                            break;
                                        }
                                    }

                                    break;
                                }
                                else if (datoF > datoE)
                                {
                                    posDatoComparar++;
                                }
                            }
                            //**---------- LONG ----------**//
                            else if (datoG != 0 && datoH != 0)
                            {
                                if (datoG == datoH)
                                {
                                    MessageBox.Show("Error, llave primaria duplicada.");
                                    break;
                                }
                                // Si el dato insertado es el valor mas pequeño
                                else if (datoH < datoG && dat == datoValorMasBajo)
                                {
                                    nuevoDato.posDato = posMemoria;
                                    posMemoria += tamDato;

                                    nuevoDato.apSigDato = dat.posDato;
                                    ent.apDatos = nuevoDato.posDato;
                                    apDatos = ent.apDatos;

                                    ent.listaDatos.Add(nuevoDato);
                                    datosVigentes.Add(nuevoDato);

                                    valorMasBajoLong = Convert.ToInt64(nuevoDato.datos[indiceLlave]);
                                    posDatoValorMasBajo = ent.listaDatos.IndexOf(nuevoDato);
                                    datoValorMasBajo = nuevoDato;

                                    dataGridView1.CurrentRow.Cells[atributosVigentes.Count].Value = nuevoDato.apSigDato;

                                    for (int dg = 0; dg < dataGridView1.Rows.Count - 1; dg++)
                                    {
                                        if (dataGridView1.Rows[dg].Cells[indiceLlave].Value.ToString() == dat.datos[indiceLlave].ToString())
                                        {
                                            dataGridView1.Rows[dg].Cells[atributosVigentes.Count].Value = dat.apSigDato;
                                            break;
                                        }
                                    }

                                    break;
                                }
                                else if (datoH < datoG)
                                {
                                    nuevoDato.posDato = posMemoria;
                                    posMemoria += tamDato;

                                    List<long> valoresResta = new List<long>();
                                    List<long> sustraendos = new List<long>();
                                    Dato datoPredecesor = new Dato();

                                    foreach (Dato dt in datosVigentes)
                                    {
                                        if (dt.apSigDato != -3 && dt.apSigDato != -4)
                                        {
                                            long sustraendo = Convert.ToInt64(dt.datos[indiceLlave]);
                                            long resta = datoH - sustraendo;

                                            if (resta >= 0)
                                            {
                                                valoresResta.Add(resta);
                                                sustraendos.Add(sustraendo);
                                            }
                                        }
                                    }

                                    var valorRestaMenor = valoresResta.Min();

                                    for (int i = 0; i < sustraendos.Count; i++)
                                    {
                                        if (valoresResta[i] == valorRestaMenor)
                                        {
                                            foreach (Dato data in datosVigentes)
                                            {
                                                if (data.apSigDato != -3 && data.apSigDato != -4)
                                                {
                                                    if (Convert.ToInt64(data.datos[indiceLlave]) == sustraendos[i])
                                                    {
                                                        datoPredecesor = data;
                                                        break;
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    }

                                    datoPredecesor.apSigDato = nuevoDato.posDato;
                                    nuevoDato.apSigDato = dat.posDato;
                                    ent.listaDatos.Add(nuevoDato);
                                    datosVigentes.Add(nuevoDato);

                                    dataGridView1.CurrentRow.Cells[atributosVigentes.Count].Value = nuevoDato.apSigDato;

                                    for (int dg = 0; dg < dataGridView1.Rows.Count - 1; dg++)
                                    {
                                        if (dataGridView1.Rows[dg].Cells[indiceLlave].Value.ToString() == dat.datos[indiceLlave].ToString())
                                        {
                                            dataGridView1.Rows[dg].Cells[atributosVigentes.Count].Value = dat.apSigDato;
                                            break;
                                        }
                                    }

                                    for (int dg = 0; dg < dataGridView1.Rows.Count - 1; dg++)
                                    {
                                        if (dataGridView1.Rows[dg].Cells[indiceLlave].Value.ToString() == datoPredecesor.datos[indiceLlave].ToString())
                                        {
                                            dataGridView1.Rows[dg].Cells[atributosVigentes.Count].Value = datoPredecesor.apSigDato;
                                            break;
                                        }
                                    }

                                    break;
                                }
                                // Si es el valor mas grande de todos
                                else if (datosVigentes.Count - 1 == posDatoComparar && datoH > datoG)
                                {
                                    nuevoDato.posDato = posMemoria;
                                    posMemoria += tamDato;

                                    Dato datoValorMasGrande = new Dato();

                                    var valorMasGrande = valoresLong.Max();

                                    // Buscamos el dato con ese valor
                                    foreach (Dato dt in datosVigentes)
                                    {
                                        if (dt.apSigDato != -3 && dt.apSigDato != -4)
                                        {
                                            if (Convert.ToInt64(dt.datos[indiceLlave]) == valorMasGrande)
                                            {
                                                datoValorMasGrande = dt;
                                                break;
                                            }
                                        }
                                    }

                                    datoValorMasGrande.apSigDato = nuevoDato.posDato;
                                    ent.listaDatos.Add(nuevoDato);
                                    datosVigentes.Add(nuevoDato);

                                    dataGridView1.CurrentRow.Cells[atributosVigentes.Count].Value = nuevoDato.apSigDato;

                                    for (int dg = 0; dg < dataGridView1.Rows.Count - 1; dg++)
                                    {
                                        if (dataGridView1.Rows[dg].Cells[indiceLlave].Value.ToString() == datoValorMasGrande.datos[indiceLlave].ToString())
                                        {
                                            dataGridView1.Rows[dg].Cells[atributosVigentes.Count].Value = datoValorMasGrande.apSigDato;
                                            break;
                                        }
                                    }

                                    break;
                                }
                                else if (datoH > datoG)
                                {
                                    posDatoComparar++;
                                }
                            }
                        }
                        //**---------- CARACTERES ----------**//
                        else if (attr.tipo == 'C')
                        {
                            char datoI = ' ';
                            char datoJ = ' ';

                            datoI = Convert.ToChar(dat.datos[indiceLlave]);
                            datoJ = Convert.ToChar(nuevoDato.datos[indiceLlave]);

                            if (datoI == datoJ)
                            {
                                MessageBox.Show("Error, llave primaria duplicada.");
                                break;
                            }
                            else if (datoJ < datoI && dat == datoValorMasBajo)
                            {
                                nuevoDato.posDato = posMemoria;
                                posMemoria += tamDato;

                                nuevoDato.apSigDato = dat.posDato;
                                ent.apDatos = nuevoDato.posDato;
                                apDatos = ent.apDatos;

                                ent.listaDatos.Add(nuevoDato);
                                datosVigentes.Add(nuevoDato);

                                valorMasBajoChar = Convert.ToChar(nuevoDato.datos[indiceLlave]);
                                posDatoValorMasBajo = ent.listaDatos.IndexOf(nuevoDato);
                                datoValorMasBajo = nuevoDato;

                                dataGridView1.CurrentRow.Cells[atributosVigentes.Count].Value = nuevoDato.apSigDato;

                                for (int dg = 0; dg < dataGridView1.Rows.Count - 1; dg++)
                                {
                                    if (dataGridView1.Rows[dg].Cells[indiceLlave].Value.ToString() == dat.datos[indiceLlave].ToString())
                                    {
                                        dataGridView1.Rows[dg].Cells[atributosVigentes.Count].Value = dat.apSigDato;
                                        break;
                                    }
                                }

                                break;
                            }
                            else if (datoJ < datoI)
                            {
                                nuevoDato.posDato = posMemoria;
                                posMemoria += tamDato;

                                List<int> valoresResta = new List<int>();
                                List<int> sustraendos = new List<int>();
                                Dato datoPredecesor = new Dato();

                                foreach (Dato dt in datosVigentes)
                                {
                                    if (dt.apSigDato != -3 && dt.apSigDato != -4)
                                    {
                                        int sustraendo = Convert.ToInt32(dt.datos[indiceLlave]);
                                        int resta = datoJ - sustraendo;

                                        if (resta >= 0)
                                        {
                                            valoresResta.Add(resta);
                                            sustraendos.Add(sustraendo);
                                        }
                                    }
                                }

                                var valorRestaMenor = valoresResta.Min();

                                for (int i = 0; i < sustraendos.Count; i++)
                                {
                                    if (valoresResta[i] == valorRestaMenor)
                                    {
                                        foreach (Dato data in datosVigentes)
                                        {
                                            if (data.apSigDato != -3 && data.apSigDato != -4)
                                            {
                                                if (Convert.ToInt32(data.datos[indiceLlave]) == sustraendos[i])
                                                {
                                                    datoPredecesor = data;
                                                    break;
                                                }
                                            }
                                        }
                                        break;
                                    }
                                }

                                datoPredecesor.apSigDato = nuevoDato.posDato;
                                nuevoDato.apSigDato = dat.posDato;
                                ent.listaDatos.Add(nuevoDato);
                                datosVigentes.Add(nuevoDato);

                                dataGridView1.CurrentRow.Cells[atributosVigentes.Count].Value = nuevoDato.apSigDato;

                                for (int dg = 0; dg < dataGridView1.Rows.Count - 1; dg++)
                                {
                                    if (dataGridView1.Rows[dg].Cells[indiceLlave].Value.ToString() == dat.datos[indiceLlave].ToString())
                                    {
                                        dataGridView1.Rows[dg].Cells[atributosVigentes.Count].Value = dat.apSigDato;
                                        break;
                                    }
                                }

                                for (int dg = 0; dg < dataGridView1.Rows.Count - 1; dg++)
                                {
                                    if (dataGridView1.Rows[dg].Cells[indiceLlave].Value.ToString() == datoPredecesor.datos[indiceLlave].ToString())
                                    {
                                        dataGridView1.Rows[dg].Cells[atributosVigentes.Count].Value = datoPredecesor.apSigDato;
                                        break;
                                    }
                                }

                                break;
                            }
                            // Si es el valor mas grande de todos
                            else if (datosVigentes.Count - 1 == posDatoComparar && datoJ > datoI)
                            {
                                nuevoDato.posDato = posMemoria;
                                posMemoria += tamDato;

                                Dato datoValorMasGrande = new Dato();

                                var valorMasGrande = valoresChar.Max();

                                // Buscamos el dato con ese valor
                                foreach (Dato dt in datosVigentes)
                                {
                                    if (dt.apSigDato != -3 && dt.apSigDato != -4)
                                    {
                                        if (Convert.ToChar(dt.datos[indiceLlave]) == valorMasGrande)
                                        {
                                            datoValorMasGrande = dt;
                                            break;
                                        }
                                    }
                                }

                                datoValorMasGrande.apSigDato = nuevoDato.posDato;
                                ent.listaDatos.Add(nuevoDato);
                                datosVigentes.Add(nuevoDato);

                                dataGridView1.CurrentRow.Cells[atributosVigentes.Count].Value = nuevoDato.apSigDato;

                                for (int dg = 0; dg < dataGridView1.Rows.Count - 1; dg++)
                                {
                                    if (dataGridView1.Rows[dg].Cells[indiceLlave].Value.ToString() == datoValorMasGrande.datos[indiceLlave].ToString())
                                    {
                                        dataGridView1.Rows[dg].Cells[atributosVigentes.Count].Value = datoValorMasGrande.apSigDato;
                                        break;
                                    }
                                }

                                break;
                            }
                            else if (datoJ > datoI)
                            {
                                posDatoComparar++;
                            }
                        }
                        //**---------- CADENAS ----------**//
                        else if (attr.tipo == 'S')
                        {
                            String datoK = ""; // I
                            String datoL = ""; // J

                            if (dat.datos[indiceLlave] is char[])
                            {
                                char[] arregloK = (char[])dat.datos[indiceLlave];
                                datoK = new string(arregloK);
                                datoK = datoK.Replace("\0", "");
                            }
                            else
                            {
                                datoK = Convert.ToString(dat.datos[indiceLlave]);
                            }

                            datoL = Convert.ToString(nuevoDato.datos[indiceLlave]);

                            datoK.ToLower();
                            datoL.ToLower();

                            if (String.Compare(datoK, datoL) == 0)
                            {
                                MessageBox.Show("Error, llave primaria duplicada.");
                                break;
                            }
                            else if (String.Compare(datoL, datoK) < 0 && dat == datoValorMasBajo)
                            {
                                nuevoDato.posDato = posMemoria;
                                posMemoria += tamDato;

                                nuevoDato.apSigDato = dat.posDato;
                                ent.apDatos = nuevoDato.posDato;
                                apDatos = ent.apDatos;

                                ent.listaDatos.Add(nuevoDato);
                                datosVigentes.Add(nuevoDato);

                                valorMasBajoString = Convert.ToString(nuevoDato.datos[indiceLlave]);
                                posDatoValorMasBajo = ent.listaDatos.IndexOf(nuevoDato);
                                datoValorMasBajo = nuevoDato;

                                dataGridView1.CurrentRow.Cells[atributosVigentes.Count].Value = nuevoDato.apSigDato;

                                for (int dg = 0; dg < dataGridView1.Rows.Count - 1; dg++)
                                {
                                    String str1 = dataGridView1.Rows[dg].Cells[indiceLlave].Value.ToString();
                                    char[] crtemp = (char[])dat.datos[indiceLlave];
                                    String str2 = new string(crtemp);
                                    str1.ToLower();
                                    str2.ToLower();
                                    str1 = str1.Replace("\0", "");
                                    str2 = str2.Replace("\0", "");

                                    if (String.Compare(str1, str2) == 0)
                                    {
                                        dataGridView1.Rows[dg].Cells[atributosVigentes.Count].Value = dat.apSigDato;
                                        break;
                                    }
                                }

                                break;
                            }
                            else if (String.Compare(datoL, datoK) < 0)
                            {
                                nuevoDato.posDato = posMemoria;
                                posMemoria += tamDato;
                                Dato datoPredecesor = new Dato();

                                List<string> listaRespaldo = new List<string>();

                                foreach (Dato d in datosVigentes)
                                {
                                    if (d.apSigDato != -3 && d.apSigDato != -4)
                                    {
                                        char[] cadena = (char[])d.datos[indiceLlave];
                                        string item = new string(cadena);
                                        item = item.Replace("\0", "");
                                        listaRespaldo.Add(item);
                                    }
                                }

                                listaRespaldo.Sort();

                                foreach (string str1 in listaRespaldo)
                                {
                                    if (String.Compare(datoL, str1) > 0)
                                    {
                                        // datoL va despues de str1
                                        int indiceAntecesor = listaRespaldo.IndexOf(str1);

                                        datoPredecesor = ent.listaDatos[indiceAntecesor];
                                    }
                                }

                                datoPredecesor.apSigDato = nuevoDato.posDato;
                                nuevoDato.apSigDato = dat.posDato;
                                ent.listaDatos.Add(nuevoDato);
                                datosVigentes.Add(nuevoDato);

                                dataGridView1.CurrentRow.Cells[atributosVigentes.Count].Value = nuevoDato.apSigDato;

                                for (int dg = 0; dg < dataGridView1.Rows.Count - 1; dg++)
                                {
                                    String str1 = dataGridView1.Rows[dg].Cells[indiceLlave].Value.ToString();
                                    char[] crtemp = (char[])dat.datos[indiceLlave];
                                    String str2 = new string(crtemp);
                                    str1.ToLower();
                                    str2.ToLower();
                                    str1 = str1.Replace("\0", "");
                                    str2 = str2.Replace("\0", "");

                                    if (String.Compare(str1, str2) == 0)
                                    {
                                        dataGridView1.Rows[dg].Cells[atributosVigentes.Count].Value = dat.apSigDato;
                                        break;
                                    }
                                }

                                for (int dg = 0; dg < dataGridView1.Rows.Count - 1; dg++)
                                {
                                    String str1 = dataGridView1.Rows[dg].Cells[indiceLlave].Value.ToString();
                                    char[] crtemp = (char[])datoPredecesor.datos[indiceLlave];
                                    String str2 = new string(crtemp);
                                    str1.ToLower();
                                    str2.ToLower();
                                    str1 = str1.Replace("\0", "");
                                    str2 = str2.Replace("\0", "");

                                    if (String.Compare(str1, str2) == 0)
                                    {
                                        dataGridView1.Rows[dg].Cells[atributosVigentes.Count].Value = datoPredecesor.apSigDato;
                                        break;
                                    }
                                }

                                break;
                            }
                            else if (datosVigentes.Count - 1 == posDatoComparar && String.Compare(datoL, datoK) > 0)
                            {
                                nuevoDato.posDato = posMemoria;
                                posMemoria += tamDato;

                                Dato datoValorMasGrande = new Dato();

                                var valorMasGrande = valoresString.Max();

                                // Buscamos el dato con ese valor
                                foreach (Dato dt in datosVigentes)
                                {
                                    if (dt.apSigDato != -3 && dt.apSigDato != -4)
                                    {
                                        if (Convert.ToString(dt.datos[indiceLlave]) == valorMasGrande)
                                        {
                                            datoValorMasGrande = dt;
                                            break;
                                        }
                                    }
                                }

                                datoValorMasGrande.apSigDato = nuevoDato.posDato;
                                ent.listaDatos.Add(nuevoDato);
                                datosVigentes.Add(nuevoDato);

                                dataGridView1.CurrentRow.Cells[atributosVigentes.Count].Value = nuevoDato.apSigDato;

                                for (int dg = 0; dg < dataGridView1.Rows.Count - 1; dg++)
                                {
                                    String str1 = dataGridView1.Rows[dg].Cells[indiceLlave].Value.ToString();
                                    char[] crtemp = (char[])datoValorMasGrande.datos[indiceLlave];
                                    String str2 = new string(crtemp);
                                    str1.ToLower();
                                    str2.ToLower();
                                    str1 = str1.Replace("\0", "");
                                    str2 = str2.Replace("\0", "");

                                    if (String.Compare(str1, str2) == 0)
                                    {
                                        dataGridView1.Rows[dg].Cells[atributosVigentes.Count].Value = datoValorMasGrande.apSigDato;
                                        break;
                                    }
                                }

                                break;
                            }
                            else if (String.Compare(datoK, datoL) < 0)
                            {
                                posDatoComparar++;
                            }
                        }
                    }
                }
            }

            bandChanged = true;
            toolStripStatusLabel1.Text = "Dato añadido con exito.";
        }

        /// <summary>
        /// Metodo que inicia los nombres y la cantidad de columnas necesarias en el dataGridView.
        /// </summary>
        private void rellena_dataGrid()
        {
            dataGridView1.ColumnCount = numAtributos + 1;
            dataGridView1.ColumnHeadersVisible = true;
            int j = 0;

            for (int i = 0; i < ent.listaAtributos.Count; i++ )
            {
                if(ent.listaAtributos[i].apSigAtributo != -2 && ent.listaAtributos[i].apSigAtributo != -4)
                {
                    atributosVigentes.Add(ent.listaAtributos[i]);

                    char[] nombre = new char[30];

                    for(int k = 0; k < ent.listaAtributos[i].nombre.Length;k++)
                    {
                        nombre[k] = ent.listaAtributos[i].nombre[k];
                    }

                    string nombreAtributo = new string(nombre);

                    if(ent.listaAtributos[i].esLlavePrimaria == true)
                    {
                        atrLlave = ent.listaAtributos[i];
                    }

                    dataGridView1.Columns[j].Name = nombreAtributo;
                    j++;
                }
            }

            dataGridView1.Columns[j].Name = "Ap. Sig. Dato";
        }

        /// <summary>
        /// Metodo que rellena filas del dataGrid con los datos que contiene la entidad.
        /// </summary>
        private void pobla_dataGrid()
        {
            List<String[]> filas = new List<string[]>();
            String[] fila = new string[atributosVigentes.Count + 1];
            int count = 0;
            Dato datoAnterior = new Dato();

            foreach (Dato dat in ent.listaDatos)
            {                
                foreach(object obj in dat.datos)
                {
                    if (obj is char[])
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
                if (dat.apSigDato != -2)
                {
                    fila[count] = dat.apSigDato.ToString();
                }
                else
                {
                    fila[count] = "-1";
                }

                if(dat == ent.listaDatos[0])
                {
                    dat.posDato = ent.apDatos;
                    datoAnterior = dat;
                }
                else
                {
                    dat.posDato = datoAnterior.apSigDato;
                    datoAnterior = dat;
                }

                if (dat.apSigDato != -3 && dat.apSigDato != -4)
                {
                    filas.Add(fila);
                }
                fila = new string[atributosVigentes.Count + 1];
                count = 0;
            }

            foreach (string[] arr in filas)
            {
                dataGridView1.Rows.Add(arr);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tatr"></param>
        /// <returns></returns>
        private dynamic valida_atributo(char tatr)
        {
            var tipoAtr = typeof(int);

            switch(tatr)
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
        /// 
        /// </summary>
        /// <param name="indice"></param>
        /// <returns></returns>
        private Dato encuentra_dato_valor_mas_bajo(int indice)
        {
            Dato bajo = new Dato();
            dynamic lowestVal = 0;
            List<dynamic> listaValores = new List<dynamic>();
            List<string> listaRespaldo = new List<string>();

            foreach (Dato dat in datosVigentes)
            {
                if (dat.apSigDato != -3 && dat.apSigDato != -4)
                {
                    listaValores.Add(dat.datos[indice]);
                }
            }

            if (attr.tipo == 'I' || attr.tipo == 'F' || attr.tipo == 'D' || attr.tipo == 'L')
            {
                lowestVal = listaValores.Min();               
            }
            else if (attr.tipo == 'C')
            {
                lowestVal = listaValores.Min();
            }
            else if (attr.tipo == 'S')
            {
                foreach (dynamic i in listaValores)
                {
                    if (i is char[])
                    {
                        String s = new string(i);
                        listaRespaldo.Add(s);
                    }
                    else
                    {
                        listaRespaldo.Add(i);
                    }
                }
                listaRespaldo.Sort();
                lowestVal = listaRespaldo[0];
            }

            bool encontrado = false;

            foreach(Dato dt in ent.listaDatos)
            {
                if (dt.apSigDato != -3 && dt.apSigDato != -4)
                {
                    switch (attr.tipo)
                    {
                        case 'I': if (Convert.ToInt32(dt.datos[indice]) == lowestVal)
                            {
                                bajo = dt;
                                encontrado = true;
                            }
                            break;
                        case 'F': if ((float)dt.datos[indice] == lowestVal)
                            {
                                bajo = dt;
                                encontrado = true;
                            }
                            break;
                        case 'D': if (Convert.ToDouble(dt.datos[indice]) == lowestVal)
                            {
                                bajo = dt;
                                encontrado = true;
                            }
                            break;
                        case 'L': if (Convert.ToInt64(dt.datos[indice]) == lowestVal)
                            {
                                bajo = dt;
                                encontrado = true;
                            }
                            break;
                        case 'C': if (Convert.ToChar(dt.datos[indice]) == lowestVal)
                            {
                                bajo = dt;
                                encontrado = true;
                            }
                            break;
                        case 'S':
                            String strn = "";

                            if (dt.datos[indice] is char[])
                            {
                                strn = new string((char[])dt.datos[indice]);
                            }
                            else
                            {
                                strn = dt.datos[indice].ToString();
                            }

                            if (String.Compare(lowestVal, strn) == 0)
                            {
                                bajo = dt;
                                encontrado = true;
                            }
                            break;
                    }
                }

                if(encontrado == true)
                {
                    break;
                }
            }

            return bajo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="indice"></param>
        /// <returns></returns>
        private dynamic encuentra_valor_mas_bajo(int indice)
        {
            dynamic lowestVal = 0;
            List<dynamic> listaValores = new List<dynamic>();
            List<string> listaRespaldo = new List<string>();

            foreach(Dato dat in datosVigentes)
            {
                if (dat.apSigDato != -3 && dat.apSigDato != -4)
                {
                    listaValores.Add(dat.datos[indice]);
                }
            }

            if(attr.tipo == 'I' || attr.tipo == 'F' || attr.tipo == 'D' || attr.tipo == 'L')
            {
                lowestVal = listaValores.Min();
                return lowestVal;
            }
            else if(attr.tipo == 'C')
            {
                lowestVal = listaValores.Min();
                return lowestVal;
            }
            else if(attr.tipo == 'S')
            {
                foreach (dynamic i in listaValores)
                {
                    if (i is char[])
                    {
                        String s = new string(i);
                        listaRespaldo.Add(s);
                    }
                    else
                    {
                        listaRespaldo.Add(i);
                    }
                }
                listaRespaldo.Sort();
                lowestVal = listaRespaldo[0];
                return lowestVal;
            }

            return lowestVal;      
        }

        // Boton que actualiza el dataGridView 
        private void button2_Click(object sender, EventArgs e)
        {
            ordena_datos();
        }

        /// <summary>
        /// Metodo con el cual se actualizara el dataGridView de visualizacion de datos.
        /// </summary>
        private void ordena_datos()
        {
            List<Dato> ordenada = ordena_lista_por_llave(ent.listaDatos);

            dataGridView1.Rows.Clear();

            List<String[]> filas = new List<string[]>();
            String[] fila = new string[atributosVigentes.Count + 1];
            int count = 0;
            Dato datoAnterior = new Dato();

            foreach (Dato dat in ordenada)
            {
                foreach (object obj in dat.datos)
                {
                    if (obj is char[])
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
                fila = new string[atributosVigentes.Count + 1];
                count = 0;
            }

            foreach (string[] arr in filas)
            {
                dataGridView1.Rows.Add(arr);
            }

            actualiza_listas(ordenada);
        }

        /// <summary>
        /// Metodo con el cual se ordenara la lista de datos de la entidad mediante su llave primaria. El orden sera ascendente.
        /// </summary>
        /// <param name="listaD">La lista de datos de la entidad.</param>
        /// <returns>La misma lista de datos, pero ordenada segun su llave primaria.</returns>
        private List<Dato> ordena_lista_por_llave(List<Dato> listaD)
        {
            List<Dato> ordenada = new List<Dato>();

            indiceLlave = regresa_indice_llave_primaria();

            if (attr.tipo != 'S')
            {
                ordenada = ent.listaDatos.OrderBy(o => o.datos[indiceLlave]).ToList();
            }
            else
            {
                List<Dato> listaRespaldo = new List<Dato>();

                int c = 0;
                foreach (Dato dat in ent.listaDatos)
                {
                    listaRespaldo.Add(dat);

                    if (ent.listaDatos[c].datos[indiceLlave] is char[])
                    {
                        String s = new string((char[])ent.listaDatos[c].datos[indiceLlave]);
                        s.Replace("\0", "");
                        listaRespaldo[c].datos[indiceLlave] = s;
                        c++;
                    }
                }

                ordenada = listaRespaldo.OrderBy(o => o.datos[indiceLlave]).ToList();
            }

            return ordenada;
        }

        /// <summary>
        /// Metodo que actualiza las listas de datos vigentes (que no han sido eliminados) y de datos en la entidad.
        /// </summary>
        /// <param name="datosOrdenados"></param>
        private void actualiza_listas(List<Dato> datosOrdenados)
        {
            datosVigentes.Clear();

            foreach (Dato dat in datosOrdenados)
            {
                if (dat.apSigDato != -3 && dat.apSigDato != -4)
                {
                    datosVigentes.Add(dat);
                }
            }

            ent.listaDatos.Clear();

            foreach (Dato dat in datosOrdenados)
            {
                ent.listaDatos.Add(dat);
            }
        }

        /// <summary>
        /// Metodo que actualiza los apuntadores de los datos en la lista de la entidad, en este caso, la lista ya estara ordenada de
        /// acuerdo a la llave primaria.
        /// </summary>
        /// <param name="listaOrdenada">La lista de datos ya ordenada.</param>
        private void actualiza_lista_datos(List<Dato> listaOrdenada)
        {
            Dato datoAnterior = new Dato();

            foreach(Dato dat in listaOrdenada)
            {
                if(dat.apSigDato != -3 && dat.apSigDato != -4)
                {
                    if(datoAnterior.datos.Count == 0)
                    {
                        ent.apDatos = dat.posDato;
                        datoAnterior = dat;
                    }
                    else
                    {
                        datoAnterior.apSigDato = dat.posDato;
                        datoAnterior = dat;
                    }

                    if(dat == listaOrdenada[listaOrdenada.Count - 1])
                    {
                        dat.apSigDato = -1;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textboxtext"></param>
        /// <returns></returns>
        private dynamic regresa_llave_primaria(string textboxtext)
        {
            dynamic llaveEncontrada = 0;
            char tipo = atrLlave.tipo;

            switch(tipo)
            {
                case 'I': llaveEncontrada = Convert.ToInt32(textboxtext);
                    break;
                case 'L': llaveEncontrada = Convert.ToInt64(textboxtext);
                    break;
                case 'D': llaveEncontrada = Convert.ToDouble(textboxtext);
                    break;
                case 'F': llaveEncontrada = Convert.ToSingle(textboxtext);
                    break;
                case 'C': llaveEncontrada = Convert.ToChar(textboxtext);
                    break;
                case 'S': llaveEncontrada = Convert.ToString(textboxtext);
                    break;
            }

            return llaveEncontrada;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="llave"></param>
        /// <returns></returns>
        private Dato regresa_dato_llave_primaria(Entidad e, String llave)
        {
            Dato dato = new Dato();
            dynamic llaveComparar = 0;
            bool breakLoop = false;

            foreach(Dato dtr in e.listaDatos)
            {
                if(dtr.apSigDato != -3 && dtr.apSigDato != -4)
                {
                    dynamic datoComparar = 0;
                    char tipo = this.atrLlave.tipo;
                    int indexLlave = dtr.indice_llave_primaria();

                    switch(tipo)
                    {
                        case 'C': datoComparar = Convert.ToChar(dtr.datos[indexLlave]);
                            llaveComparar = Convert.ToChar(llave);
                            if (Convert.ToChar(dtr.datos[indexLlave]) == llaveComparar)
                            {
                                dato = dtr;
                                breakLoop = true;
                            }
                            break;
                        case 'D': datoComparar = Convert.ToDouble(dtr.datos[indexLlave]);
                            llaveComparar = Convert.ToDouble(llave);
                            if (Convert.ToDouble(dtr.datos[indexLlave]) == llaveComparar)
                            {
                                dato = dtr;
                                breakLoop = true;
                            }
                            break;
                        case 'I': datoComparar = Convert.ToInt32(dtr.datos[indexLlave]);
                            llaveComparar = Convert.ToInt32(llave);
                            if (Convert.ToInt32(dtr.datos[indexLlave]) == llaveComparar)
                            {
                                dato = dtr;
                                breakLoop = true;
                            }
                            break;
                        case 'F': datoComparar = Convert.ToSingle(dtr.datos[indexLlave]);
                            llaveComparar = Convert.ToSingle(llave);
                            if (Convert.ToSingle(dtr.datos[indexLlave]) == llaveComparar)
                            {
                                dato = dtr;
                                breakLoop = true;
                            }
                            break;
                        case 'L': datoComparar = Convert.ToInt64(dtr.datos[indexLlave]);
                            llaveComparar = Convert.ToInt64(llave);
                            if (Convert.ToInt64(dtr.datos[indexLlave]) == llaveComparar)
                            {
                                dato = dtr;
                                breakLoop = true;
                            }
                            break;
                        case 'S': datoComparar = Convert.ToString(dtr.datos[indexLlave]);
                            llaveComparar = Convert.ToString(llave);
                            if (Convert.ToString(dtr.datos[indexLlave]) == llaveComparar)
                            {
                                dato = dtr;
                                breakLoop = true;
                            }
                            break;
                    }
                   
                    if(breakLoop == true)
                    {
                        break;
                    }
                }
            }

            return dato;
        }

        /// <summary>
        /// Metodo que devuelve el indice de la llave primaria en la lista de atributos de la entidad.
        /// </summary>
        /// <returns>El indice de la llave primaria de la lista de atributos de la entidad.</returns>
        private int regresa_indice_llave_primaria()
        {
            int indexLlave = 0;

            foreach(Atributo atri in ent.listaAtributos)
            {
                if(atri.apSigAtributo != -2 && atri.apSigAtributo != 4)
                {
                    if(atri.esLlavePrimaria == false)
                    {
                        indexLlave++;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return indexLlave;
        }

        // Boton para eliminar un dato
        private void button5_Click(object sender, EventArgs e)
        {
            if(textBox1.Text.Length > 0)
            {
                Dato datoEliminar = regresa_dato_llave_primaria(ent, textBox1.Text);

                if(datoEliminar.datos.Count > 0)
                {
                    datoEliminar.apSigDato = -3;

                    for (int i = 0; i < this.ent.listaDatos.Count; i++)
                    {
                        // Si era el unico
                        if (datoEliminar == this.ent.listaDatos[i] && this.ent.listaDatos.Count == 1)
                        {
                            this.ent.apDatos = -2;
                            break;
                        }
                        // Si era el ultimo
                        else if (datoEliminar == this.ent.listaDatos[i] && i == this.ent.listaDatos.Count - 1)
                        {
                            datoEliminar.apSigDato = -4;
                            bool encontrado = false;

                            for (int a = i - 1; a > 0; a--)
                            {
                                if (this.ent.listaDatos[a].apSigDato != -3 && this.ent.listaDatos[a].apSigDato != -4)
                                {
                                    this.ent.listaDatos[a].apSigDato = -2;
                                    encontrado = true;
                                    break;
                                }
                            }

                            if(encontrado == true)
                            {
                                break;
                            }
                            else
                            {
                                this.ent.apDatos = -2;
                                break;
                            }
                        }
                        // Si era el primero
                        else if (datoEliminar == this.ent.listaDatos[i] && i == 0 && this.ent.listaDatos.Count > 1)
                        {
                            bool encontrado = false;

                            for (int b = i + 1; b < this.ent.listaDatos.Count; b++)
                            {
                                if (this.ent.listaDatos[b].apSigDato != -3 && this.ent.listaDatos[b].apSigDato != -4)
                                {
                                    this.ent.apDatos = this.ent.listaDatos[b].posDato;
                                    encontrado = true;
                                    break;
                                }
                            }

                            if(encontrado == true)
                            {
                                break;
                            }
                            else 
                            {
                                this.ent.apDatos = -2;
                                break;
                            }
                        }
                        // Si esta entre 2 datos
                        else if (datoEliminar == this.ent.listaDatos[i])
                        {
                            Dato datoAnt = new Dato();
                            Dato datoSuc = new Dato();
                            for (int a = i - 1; a > 0; a--)
                            {
                                if (this.ent.listaDatos[a].apSigDato != -3)
                                {
                                    datoAnt = this.ent.listaDatos[a];
                                    break;
                                }
                            }
                            for (int b = i + 1; b < this.ent.listaDatos.Count; b++)
                            {
                                if (this.ent.listaDatos[b].apSigDato != -3 && this.ent.listaDatos[b].apSigDato != -4)
                                {
                                    datoSuc = this.ent.listaDatos[b];
                                    break;
                                }
                            }

                            if(datoAnt.datos.Count > 0 && datoSuc.datos.Count > 0)
                            {
                                datoAnt.apSigDato = datoSuc.posDato;
                            }
                            else if(datoAnt.datos.Count > 0 && datoSuc.datos.Count == 0)
                            {
                                datoAnt.apSigDato = -2;
                            }
                            else if(datoSuc.datos.Count > 0 && datoAnt.datos.Count == 0)
                            {
                                ent.apDatos = datoSuc.posDato;
                            }
                            else
                            {
                                ent.apDatos = -2;
                            }
                            
                            break;
                        }
                    }
                    this.bandChanged = true;

                    toolStripStatusLabel1.Text = "Dato eliminado con exito.";
                }
                else
                {
                    MessageBox.Show("Error, llave primaria no encontrada.");
                }
            }
            else
            {
                MessageBox.Show("Error, no se ha introducido una llave primaria");
            }
        }

        // Boton para modificar un dato
        private void button3_Click(object sender, EventArgs e)
        {
            if(textBox1.Text.Length > 0)
            {
                Dato datoModificar = regresa_dato_llave_primaria(ent, textBox1.Text);

                if(datoModificar.datos.Count > 0)
                {
                    int idiceKey = regresa_indice_llave_primaria();
                    bool llaveCambiada = false;

                    using(ModificaDato modificador = new ModificaDato(ent, datoModificar, idiceKey))
                    {
                        var cuadroModifica = modificador.ShowDialog();

                        if(cuadroModifica == DialogResult.OK)
                        {
                            datoModificar = modificador.dat;
                            llaveCambiada = modificador.llavePrimariaCambiada;

                            // Si se cambio la llave primaria, hay que invocar al metodo que reordena la lista
                            if(llaveCambiada == true)
                            {
                                ordena_datos();
                                actualiza_lista_datos(ent.listaDatos);
                                ordena_datos();
                            }

                            this.bandChanged = true;
                            toolStripStatusLabel1.Text = "Dato modificado con exito.";
                        }
                    }                  
                }
                else
                {
                    MessageBox.Show("Error, llave primaria no encontrada.");
                }
            }
            else
            {
                MessageBox.Show("Error, no se ha introducido una llave primaria.");
            }
        }
    }
}
