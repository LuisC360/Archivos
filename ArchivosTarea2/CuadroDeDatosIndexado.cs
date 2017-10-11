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
        Atributo atrLlave;
        readonly List<Atributo> atributosVigentes = new List<Atributo>();
        public long posMemoria { get; set; }
        long tamDato;
        long apDato { get; set; }
        long rango;


        public CuadroDeDatosIndexado()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
