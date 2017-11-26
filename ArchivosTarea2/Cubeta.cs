using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchivosTarea2
{
    public class Cubeta
    {
        // El apuntador a dato de la cubeta.
        public long apDato { get; set; }
        // La posicion de la cubeta en memoria.
        public long posCubeta { get; set; }
        // El apuntador a la siguiente cubeta.
        public long apSigCubeta { get; set; }
        // El dato de la cubeta.
        public Dato datoCubeta { get; set; }

        /// <summary>
        /// Constructor de una nueva instancia de la clase cubeta.
        /// </summary>
        public Cubeta()
        {
            apDato = -1;
            apSigCubeta = 0;
        }

        // Funciones para inicializar informacion de la cubeta.
        public void str_apDato(long ap)
        {
            apDato = ap;
        }

        public void str_posCubeta(long pos)
        {
            posCubeta = pos;
        }

        public void str_apSigCubeta(long ap)
        {
            apSigCubeta = ap;
        }

        public void str_datoCubeta(Dato dato)
        {
            datoCubeta = dato;
        }

        // Funciones de retorno de informacion de la cubeta.
        public long regresa_apDato()
        {
            return apDato;
        }

        public long regresa_posCubeta()
        {
            return posCubeta;
        }

        public long regresa_apSigCubeta()
        {
            return apSigCubeta;
        }

        public Dato regresa_datoCubeta()
        {
            return datoCubeta;
        }
    }
}
