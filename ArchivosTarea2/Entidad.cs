using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchivosTarea2
{ 
    /// <summary>
    /// La clase que representa una entidad.
    /// </summary>
    public class Entidad
    {
        /// <summary>
        /// El nombre de la entidad.
        /// </summary>
        public char[] nombre = { '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0',  
                                    '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0' ,'\0' ,'\n'};
        /// <summary>
        /// Apuntador a atributos.
        /// </summary>
        public long apAtributos = -1;
        /// <summary>
        /// Apuntador a datos.
        /// </summary>
        public long apDatos = -1;
        /// <summary>
        /// Posicion de la entidad.
        /// </summary>
        public long posEntidad = 0;
        /// <summary>
        /// Apuntador a la siguiente entidad.
        /// </summary>
        public long apSigEntidad = -1;
        /// <summary>
        /// Apuntador a indices.
        /// </summary>
        public long apIndices = -1;
        /// <summary>
        /// Apuntador a cajones.
        /// </summary>
        public long apCajones = -1;
        /// <summary>
        /// Apuntador a cabeceras.
        /// </summary>
        public long apCabeceras = -1;
        /// <summary>
        /// Lista de atributos.
        /// </summary>
        public List<Atributo> listaAtributos = new List<Atributo>();
        /// <summary>
        /// Lista de datos.
        /// </summary>
        public List<Dato> listaDatos = new List<Dato>();
        /// <summary>
        /// Lista de indices.
        /// </summary>
        public List<Indice> listaIndices = new List<Indice>();
        /// <summary>
        /// Lista de cajones.
        /// </summary>
        public List<Cajon> listaCajones = new List<Cajon>();
        /// <summary>
        /// Lista de cabeceras.
        /// </summary>
        public List<Cabecera> listaCabeceras = new List<Cabecera>();
        /// <summary>
        /// El tamaño del dato de la entidad.
        /// </summary>
        public long tamDato;

        /// <summary>
        /// Construccion de una nueva instancia de la clase Entidad.
        /// </summary>
        /// <param name="n">El nombre de la entidad.</param>
        public Entidad(String n)
        {
            for (int i = 0; i < n.Length; i++ )
            {
                nombre[i] = n[i];
            }
        }

        /// <summary>
        /// Construccion de una nueva instancia de la clase Entidad.
        /// </summary>
        /// <param name="n">El nombre de la entidad.</param>
        /// <param name="apAt">El apuntador a atributos de la entidad.</param>
        /// <param name="apDat">El apuntador a datos de la entidad.</param>
        /// <param name="posIn">La posicion inicial de la entidad.</param>
        /// <param name="posSigEnt">El apuntador a la siguiente entidad.</param>
        public Entidad(char[] n, long apAt, long apDat, long posIn, long posSigEnt)
        {
            nombre = n;
            apAtributos = apAt;
            apDatos = apDat;
            posEntidad = posIn;
            apSigEntidad = posSigEnt;
        }

        /// <summary>
        /// Construccion de una nueva instancia de la clase Entidad.
        /// </summary>
        /// <param name="n">El nombre de la entidad.</param>
        /// <param name="apAt">El apuntador a atributos de la entidad.</param>
        /// <param name="apInd">El apuntador a indices de la entidad.</param>
        /// <param name="posIn">La posicion inicial de la entidad.</param>
        /// <param name="posSigEnt">El apuntador a la siguiente entidad.</param>
        /// <param name="dif">Parametro que solo sirve para diferenciar el constructor de entidad.</param>
        public Entidad(char[] n, long apAt, long apInd, long posIn, long posSigEnt, int dif)
        {
            nombre = n;
            apAtributos = apAt;
            apIndices = apInd;
            posEntidad = posIn;
            apSigEntidad = posSigEnt;
        }

        /// <summary>
        /// Construccion de una nueva instancia de la clase Entidad.
        /// </summary>
        /// <param name="n">El nombre de la entidad.</param>
        /// <param name="apAt">El apuntador a atributos de la entidad.</param>
        /// <param name="apCaj">El apuntador a los cajones de la entidad.</param>
        /// <param name="posIn">La posicion inicial de la entidad.</param>
        /// <param name="posSigEnt">El apuntador a la siguiente entidad.</param>
        /// <param name="dif">Parametro que solo sirve para diferenciar el constructor de entidad.</param>
        public Entidad(char[] n, long apAt, long apCaj, long posIn, long posSigEnt, long dif)
        {
            nombre = n;
            apAtributos = apAt;
            apCajones = apCaj;
            posEntidad = posIn;
            apSigEntidad = posSigEnt;
        }

        /// <summary>
        /// Construccion de una nueva instancia de la clase Entidad.
        /// </summary>
        /// <param name="n">El nombre de la entidad.</param>
        /// <param name="apAt">El apuntador a atributos de la entidad.</param>
        /// <param name="apCab">El apuntador a las cabeceras de la entidad.</param>
        /// <param name="posIn">La posicion inicial de la entidad.</param>
        /// <param name="posSigEnt">El apuntador a la siguiente entidad.</param>
        /// <param name="dif">Parametro que solo sirve para diferenciar el constructor de entidad.</param>
        public Entidad(char[] n, long apAt, long apCab, long posIn, long posSigEnt, double dif)
        {
            nombre = n;
            apAtributos = apAt;
            apCabeceras = apCab;
            posEntidad = posIn;
            apSigEntidad = posSigEnt;
        }

        /// <summary>
        /// Construccion de una nueva instancia de la clase Entidad.
        /// </summary>
        public Entidad()
        {

        }
    }
}
