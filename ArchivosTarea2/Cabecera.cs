using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchivosTarea2
{
    /// <summary>
    /// Clase que representara una cabecera para los datos en multilistas.
    /// </summary>
    public class Cabecera
    {
        /// <summary>
        /// El apuntador a los datos de la cabecera.
        /// </summary>
        public long apDatos { get; set; }
        /// <summary>
        /// La posicion de las cabeceras en el archivo.
        /// </summary>
        public long posCabecera { get; set; }
        
        /// <summary>
        /// Constructor de una nueva instancia de la clase Cabecera.
        /// </summary>
        /// <param name="apD">El apuntador a los datos de la cabecera.</param>
        public Cabecera(long apD)
        {
            apDatos = apD;
        }

        /// <summary>
        /// Funcion que le asigna a la cabecera actual el apuntador a datos correspondiente.
        /// </summary>
        /// <param name="apD">El nuevo valor de apuntador a datos.</param>
        public void str_apDatos(long apD)
        {
            apDatos = apD;
        }

        /// <summary>
        /// Funcion que le asigna a la cabecera actual la posicion en el archivo correspondiente.
        /// </summary>
        /// <param name="posC">La posicion en el archivo de la cabecera.</param>
        public void str_posCabecera(long posC)
        {
            posCabecera = posC;
        }

        /// <summary>
        /// Funcion que regresa el apuntador a datos de la cabecera.
        /// </summary>
        /// <returns>El apuntador a datos de la cabecera.</returns>
        public long return_apDatos()
        {
            return apDatos;
        }

        /// <summary>
        /// Funcion que regresa la posicion en el archivo de la cabecera actual.
        /// </summary>
        /// <returns>La posicion de la cabecera en el archivo.</returns>
        public long return_posCabecera()
        {
            return posCabecera;
        }
    }
}
