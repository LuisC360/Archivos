using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchivosTarea2
{
    public class Cabecera
    {
        // El apuntador a los datos de la cabecera.
        public long apDatos { get; set; }
        // La posicion de las cabeceras en el archivo.
        public long posCabecera { get; set; }
        
        /// <summary>
        /// Constructor de una nueva instancia de la clase Cabecera.
        /// </summary>
        /// <param name="apD">El apuntador a los datos de la cabecera.</param>
        public Cabecera(long apD)
        {
            apDatos = apD;
        }

        // Funciones para asignacion de variables.
        public void str_apDatos(long apD)
        {
            apDatos = apD;
        }

        public void str_posCabecera(long posC)
        {
            posCabecera = posC;
        }

        // Funciones de retorno de variables.
        public long return_apDatos()
        {
            return apDatos;
        }

        public long return_posCabecera()
        {
            return posCabecera;
        }
    }
}
