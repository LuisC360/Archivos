using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchivosTarea2
{
    public class Indice
    {
        dynamic valorInicial;
        dynamic valorFinal;
        long apSigIndice = 0;
        long apDatos = 0;

        public Indice()
        {

        }

        public Indice(dynamic vI, dynamic vF)
        {
            valorInicial = vI;
            valorFinal = vF;
        }
    }
}
