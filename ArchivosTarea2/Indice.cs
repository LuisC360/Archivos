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
        // Lista de datos relacionados a este indice
        public List<Dato> datosIndice = new List<Dato>();

        public Indice()
        {
            apSigIndice = -1;
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

        public void srt_valorInicial(dynamic vI)
        {
            valorInicial = vI;
        }

        public void srt_valorFinal(dynamic vF)
        {
            valorFinal = vF;
        }

        public dynamic regresa_valInicial()
        {
            return valorInicial;
        }

        public dynamic regresa_valFinal()
        {
            return valorFinal;
        }

        public long regresa_posIndice()
        {
            return posIndice;
        }

        public long regresa_apSigIndice()
        {
            return apSigIndice;
        }

        public long regresa_apDatos()
        {
            return apDatos;
        }
    }
}
