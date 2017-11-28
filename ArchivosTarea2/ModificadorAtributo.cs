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
    /// Clase que representa la ventana donde se modificara un atributo.
    /// </summary>
    public partial class ModificadorAtributo : Form
    {
        /// <summary>
        /// La lista con los tipos de dato disponibles.
        /// </summary>
        List<char> tiposDato = new List<char>();
        /// <summary>
        /// El nuevo nombre del atributo.
        /// </summary>
        public String newNombre { get; set; }
        /// <summary>
        /// El nuevo tipo de dato del atributo.
        /// </summary>
        public char newTipo { get; set; }
        /// <summary>
        /// El nuevo tamaño en bytes del atributo.
        /// </summary>
        public long newBytes { get; set; }
        /// <summary>
        /// Bandera que representa si el atributo sera o no llave primaria.
        /// </summary>
        public int esLlave { get; set; }
        /// <summary>
        /// Bandera que representa si el atributo sera o no llave de busqueda.
        /// </summary>
        public int esBusqueda { get; set; }
        /// <summary>
        /// El tipo de ordenamiento de datos actual.
        /// </summary>
        readonly long tipo;

        /// <summary>
        /// Construccion de la ventana para modificar un atributo.
        /// </summary>
        public ModificadorAtributo(long t)
        {
            InitializeComponent();
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox3.Enabled = false;
            tipo = t;

            if(tipo == 3)
            {
                comboBox3.Enabled = true;
                comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
            }

            rellena_lista_tipo();
        }

        /// <summary>
        /// Boton para aceptar los cambios.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">EventArgs.</param>
        private void button1_Click(object sender, EventArgs e)
        {
            this.newNombre = textBox1.Text;
            this.newTipo = comboBox1.SelectedItem.ToString()[0];
            this.newBytes = escoje_num_bytes();
            this.esLlave = comboBox2.SelectedIndex;
            if(tipo == 3)
            {
                this.esBusqueda = comboBox3.SelectedIndex;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Metodo que rellena los comboBox con su informacion respectiva. El primero sera llenado con los tipos de dato disponibles, y el
        /// segundo con banderas de si el atributo sera o no llave primaria.
        /// </summary>
        public void rellena_lista_tipo()
        {
            char[] tipos = { 'I', 'F', 'C', 'S', 'D', 'L', 'B' };

            for (int i = 0; i < tipos.Length; i++)
            {
                tiposDato.Add(tipos[i]);
            }

            for (int i = 0; i < tiposDato.Count; i++)
            {
                comboBox1.Items.Add(tiposDato[i]);
            }

            comboBox2.Items.Add("Si");
            comboBox2.Items.Add("No");

            if(tipo == 3)
            {
                comboBox3.Items.Add("Si");
                comboBox3.Items.Add("No");
            }
        }

        /// <summary>
        /// Metodo con el que se elegira el nuevo numero de bytes del atributo dependiendo de lo que se haya seleccionado en el primer
        /// textBox.
        /// </summary>
        /// <returns>El nuevo numero de bytes del atributo.</returns>
        public long escoje_num_bytes()
        {
            long numBytes = 0;
            int selectedIndex;

            selectedIndex = comboBox1.SelectedIndex;

            switch (selectedIndex)
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
    }
}
