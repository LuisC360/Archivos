using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchivosTarea2
{
    /// <summary>
    /// La clase que representara un indice para archivos en secuencial indexado.
    /// </summary>
    public class Indice
    {
        /// <summary>
        /// Valor inicial del rango del indice.
        /// </summary>
        public dynamic valorInicial { get; set; }
        /// <summary>
        /// Valor final del rango del indice.
        /// </summary>
        public dynamic valorFinal { get; set; }
        /// <summary>
        /// Posicion de memoria del indice.
        /// </summary>
        public long posIndice { get; set; }
        /// <summary>
        /// Apuntador al siguiente indice.
        /// </summary>
        public long apSigIndice { get; set; }
        /// <summary>
        /// Apuntador a los datos.
        /// </summary>
        public long apDatos { get; set; }
        /// <summary>
        /// Lista de datos relacionados a este indice.
        /// </summary>
        public List<Dato> datosIndice = new List<Dato>();

        /// <summary>
        /// Constructor de una nueva instancia de la clase Indice.
        /// </summary>
        public Indice()
        {
            apSigIndice = -1;
        }

        /// <summary>
        /// Constructor de una nueva instancia de la clase Indice.
        /// </summary>
        /// <param name="vI"></param>
        /// <param name="vF"></param>
        public Indice(dynamic vI, dynamic vF)
        {
            valorInicial = vI;
            valorFinal = vF;
        }

        /// <summary>
        /// Funcion que asigna la posicion del indice.
        /// </summary>
        /// <param name="pI">La posicion del indice.</param>
        public void srt_posIndice(long pI)
        {
            posIndice = pI;
        }

        /// <summary>
        /// Funcion que asigna el apuntador al siguiente indice.
        /// </summary>
        /// <param name="apIn">El apuntador al siguiente indice.</param>
        public void srt_apSigIndice(long apIn)
        {
            apSigIndice = apIn;
        }

        /// <summary>
        /// Funcion que asigna el apuntador de datos del indice.
        /// </summary>
        /// <param name="apD">El apuntador de datos del indice.</param>
        public void srt_apDatos(long apD)
        {
            apDatos = apD;
        }

        /// <summary>
        /// Funcion que asigna el valor inicial del indice.
        /// </summary>
        /// <param name="vI">El valor inicial del indice.</param>
        public void srt_valorInicial(dynamic vI)
        {
            valorInicial = vI;
        }

        /// <summary>
        /// Funcion que asigna el valor final del indice.
        /// </summary>
        /// <param name="vF">El valor final del indice.</param>
        public void srt_valorFinal(dynamic vF)
        {
            valorFinal = vF;
        }

        /// <summary>
        /// Funcion que regresa el valor inicial del indice.
        /// </summary>
        /// <returns>El valor inicial del indice.</returns>
        public dynamic regresa_valInicial()
        {
            return valorInicial;
        }

        /// <summary>
        /// Funcion que regresa el valor final del indice.
        /// </summary>
        /// <returns>El valor final del indice.</returns>
        public dynamic regresa_valFinal()
        {
            return valorFinal;
        }

        /// <summary>
        /// Funcion que regresa la posicion del indice.
        /// </summary>
        /// <returns>La posicion del indice.</returns>
        public long regresa_posIndice()
        {
            return posIndice;
        }

        /// <summary>
        /// Funcion que regresa el apuntador al siguiente indice.
        /// </summary>
        /// <returns>El apuntador al siguiente indice.</returns>
        public long regresa_apSigIndice()
        {
            return apSigIndice;
        }

        /// <summary>
        /// Funcion que regresa el apuntador a datos del indice.
        /// </summary>
        /// <returns>El apuntador a datos del indice.</returns>
        public long regresa_apDatos()
        {
            return apDatos;
        }
    }
}
