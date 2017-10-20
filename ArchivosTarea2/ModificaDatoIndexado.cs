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
        Entidad ent;
        public Indice ind { get; set; }
        public Dato dat { get; set; }
        public bool llavePrimariaCambiada { get; set; }
        List<Atributo> listaAtributosVigentes = new List<Atributo>();
        int atributosVigentes;
        int indexLlavePrimaria;
        readonly Dato datoRespaldo;

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
                    default:
                        // DEFAULT VACIO
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

        private void button1_Click(object sender, EventArgs e)
        {
            // WIP
        }
    }
}
