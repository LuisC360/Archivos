using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchivosTarea2
{
    public class Indice
    {
        // Valor inicial del rango del indice.
        public dynamic valorInicial { get; set; }
        // Valor final del rango del indice.
        public dynamic valorFinal { get; set; }
        // Posicion de memoria del indice.
        public long posIndice { get; set; }
        // Apuntador al siguiente indice.
        public long apSigIndice { get; set; }
        // Apuntador a los datos.
        public long apDatos { get; set; }

        public Indice()
        {

        }

        public Indice(dynamic vI, dynamic vF)
        {
            valorInicial = vI;
            valorFinal = vF;
        }

        public void srt_posIndice(long pI)
        {
            posIndice = pI;
        }

        public void srt_apSigIndice(long apIn)
        {
            apSigIndice = apIn;
        }

        public void srt_apDatos(long apD)
        {
            apDatos = apD;
        }

        public dynamic get_valInicial()
        {
            return valorInicial;
        }

        public dynamic get_valFinal()
        {
            return valorFinal;
        }

        public long get_posIndice()
        {
            return posIndice;
        }

        public long get_apSigIndice()
        {
            return apSigIndice;
        }

        public long get_apDatos()
        {
            return apDatos;
        }
    }
}
