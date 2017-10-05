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
    public partial class ModificadorAtributo : Form
    {
        List<char> tiposDato = new List<char>();
        public String newNombre { get; set; }
        public char newTipo { get; set; }
        public long newBytes { get; set; }
        public int esLlave { get; set; }

        /// <summary>
        /// Construccion de la ventana para modificar un atributo.
        /// </summary>
        public ModificadorAtributo()
        {
            InitializeComponent();
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            rellena_lista_tipo();
        }

        // Boton para aceptar los cambios.
        private void button1_Click(object sender, EventArgs e)
        {
            this.newNombre = textBox1.Text;
            this.newTipo = comboBox1.SelectedItem.ToString()[0];
            this.newBytes = escoje_num_bytes();
            this.esLlave = comboBox2.SelectedIndex;
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
