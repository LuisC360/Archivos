using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchivosTarea2
{
    /// <summary>
    /// Clase que representa una cubeta en Hash Estatica.
    /// </summary>
    public class Cubeta
    {
        /// <summary>
        /// El apuntador a dato de la cubeta.
        /// </summary>
        public long apDato { get; set; }
        /// <summary>
        /// La posicion de la cubeta en memoria.
        /// </summary>
        public long posCubeta { get; set; }
        /// <summary>
        /// El apuntador a la siguiente cubeta.
        /// </summary>
        public long apSigCubeta { get; set; }
        /// <summary>
        /// El dato de la cubeta.
        /// </summary>
        public Dato datoCubeta { get; set; }

        /// <summary>
        /// Constructor de una nueva instancia de la clase cubeta.
        /// </summary>
        public Cubeta()
        {
            apDato = -1;
            apSigCubeta = 0;
        }

        /// <summary>
        /// Funcion que asigna el apuntador a datos de la cubeta.
        /// </summary>
        /// <param name="ap">El apuntador a datos de la cubeta.</param>
        public void str_apDato(long ap)
        {
            apDato = ap;
        }

        /// <summary>
        /// Funcion que asigna la posicion de la cubeta.
        /// </summary>
        /// <param name="pos">La posicion de la cubeta.</param>
        public void str_posCubeta(long pos)
        {
            posCubeta = pos;
        }

        /// <summary>
        /// Funcion que asigna el apuntador a la siguiente cubeta.
        /// </summary>
        /// <param name="ap">El apuntador a la siguiente cubeta.</param>
        public void str_apSigCubeta(long ap)
        {
            apSigCubeta = ap;
        }

        /// <summary>
        /// Funcion que asigna el dato de la cubeta.
        /// </summary>
        /// <param name="dato">El dato de la cubeta.</param>
        public void str_datoCubeta(Dato dato)
        {
            datoCubeta = dato;
        }

        /// <summary>
        /// Funcion que regresa el apuntador al dato de la cubeta.
        /// </summary>
        /// <returns>El apuntador al dato de la cubeta.</returns>
        public long regresa_apDato()
        {
            return apDato;
        }

        /// <summary>
        /// Funcion que regresara la posicion de la cubeta.
        /// </summary>
        /// <returns>La posicion de la cubeta.</returns>
        public long regresa_posCubeta()
        {
            return posCubeta;
        }

        /// <summary>
        /// Funcion que regresa el apuntador a la siguiente cubeta.
        /// </summary>
        /// <returns>El apuntador a la siguiente cubeta.</returns>
        public long regresa_apSigCubeta()
        {
            return apSigCubeta;
        }

        /// <summary>
        /// Funcion que regresa el dato de la cubeta.
        /// </summary>
        /// <returns>El dato de la cubeta.</returns>
        public Dato regresa_datoCubeta()
        {
            return datoCubeta;
        }
    }
}
