using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchivosTarea2
{
    /// <summary>
    /// Clase que representara un cajon en los archivos de Hash Dinamica.
    /// </summary>
    public class Cajon
    {
        /// <summary>
        /// La posicion actual del cajon en el archivo.
        /// </summary>
        long posCajon { get; set; }
        /// <summary>
        /// El apuntador a las cubetas del cajon.
        /// </summary>
        long apuntadorCubeta { get; set; }
        /// <summary>
        /// La lista de cubetas del cajon.
        /// </summary>
        public List<List<Cubeta>> listaCubetas = new List<List<Cubeta>>();

        /// <summary>
        /// Constructor de una nueva instancia de la clase Cajon.
        /// </summary>
        public Cajon()
        {
            apuntadorCubeta = -1;
        }

        /// <summary>
        /// Funcion que le asigna al cajon su posicion en el archivo.
        /// </summary>
        /// <param name="p">La posicion del cajon en el archivo.</param>
        public void str_posCajon(long p)
        {
            posCajon = p;
        }

        /// <summary>
        /// Funcion que asignara al cajon el apuntador a las cubetas.
        /// </summary>
        /// <param name="ap">El apuntador a las cubetas para el cajon actual.</param>
        public void str_apuntadorCubeta(long ap)
        {
            apuntadorCubeta = ap;
        }

        /// <summary>
        /// Funcion que regresa la posicion del cajon en el archivo.
        /// </summary>
        /// <returns>La posicion del cajon en el archivo</returns>
        public long regresa_posCajon()
        {
            return posCajon;
        }

        /// <summary>
        /// Funcion que regresa el apuntador a las cubetas del cajon.
        /// </summary>
        /// <returns>El apuntador a las cubetas para el cajon actual.</returns>
        public long regresa_apuntadorCubeta()
        {
            return apuntadorCubeta;
        }
    }
}
