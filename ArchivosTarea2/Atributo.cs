using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchivosTarea2
{
    public class Atributo
    {
        public char[] nombre = { '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0',  
                                    '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0' ,'\0' ,'\n'};
        public char tipo;
        public long bytes;
        public long posAtributo;
        public Boolean esLlavePrimaria;
        public long apSigAtributo = -1;
        public Boolean esLlaveDeBusqueda;

        /// <summary>
        /// Construccion de una nueva instancia de la clase Atributo.
        /// </summary>
        /// <param name="nom">El nombre del atributo.</param>
        /// <param name="t">El tipo de dato del atributo.</param>
        /// <param name="by">El numero de bytes del atributo.</param>
        /// <param name="esLl">Bandera que representa si el atributo es llave primaria o no.</param>
        public Atributo(String nom, String t, long by, String esLl)
        {
            for (int i = 0; i < nom.Length; i++)
            {
                nombre[i] = nom[i];
            }
            tipo = t[0];
            bytes = by;

            if(esLl == "Si")
            {
                esLlavePrimaria = true;
            }
            else 
            {
                esLlavePrimaria = false;
            }
        }

        /// <summary>
        /// Construccion de una nueva instancia de la clase Atributo.
        /// </summary>
        /// <param name="nom">El nombre del atributo.</param>
        /// <param name="t">El tipo de dato del atributo.</param>
        /// <param name="by">El tamaño en bytes del atributo.</param>
        /// <param name="posAt">La posicion en memoria del atributo.</param>
        /// <param name="esLl">Bandera que indica si el atributo es o no llave primaria.</param>
        /// <param name="esLlB">Bandera que indica si el atributo si es o no llave de busqueda.</param>
        public Atributo(String nom, String t, long by, String esLl, String esLlB)
        {
            for (int i = 0; i < nom.Length; i++)
            {
                nombre[i] = nom[i];
            }

            tipo = t[0];
            bytes = by;

            if (esLl == "Si")
            {
                esLlavePrimaria = true;
            }
            else
            {
                esLlavePrimaria = false;
            }

            if(esLlB == "Si")
            {
                esLlaveDeBusqueda = true;
            }
            else
            {
                esLlaveDeBusqueda = false;
            }
        }

        /// <summary>
        /// Construccion de una nueva instancia de la clase Atributo.
        /// </summary>
        /// <param name="nom">El nombre del atributo.</param>
        /// <param name="t">El tipo de dato del atributo.</param>
        /// <param name="by">El tamaño en bytes del atributo.</param>
        /// <param name="posAt">La posicion en memoria del atributo.</param>
        /// <param name="esLl">Bandera que indica si el atributo es o no llave primaria.</param>
        /// <param name="apSig">El apuntador al siguiente atributo.</param>
        public Atributo(char[] nom, char t, long by, long posAt, bool esLl, long apSig)
        {
            nombre = nom;
            tipo = t;
            bytes = by;
            posAtributo = posAt;
            esLlavePrimaria = esLl;
            apSigAtributo = apSig;
        }

        /// <summary>
        /// Construccion de una nueva instancia de la clase Atributo.
        /// </summary>
        /// <param name="nom">El nombre del atributo.</param>
        /// <param name="t">El tipo de dato del atributo.</param>
        /// <param name="by">El tamaño en bytes del atributo.</param>
        /// <param name="posAt">La posicion en memoria del atributo.</param>
        /// <param name="esLl">Bandera que indica si el atributo es o no llave primaria.</param>
        /// <param name="esLlB">Bandera que indica si el atributo si es o no llave de busqueda.</param>
        /// <param name="apSig">El apuntador al siguiente atributo.</param>
        public Atributo(char[] nom, char t, long by, long posAt, bool esLl, bool esLlB, long apSig)
        {
            nombre = nom;
            tipo = t;
            bytes = by;
            posAtributo = posAt;
            esLlavePrimaria = esLl;
            esLlaveDeBusqueda = esLlB;
            apSigAtributo = apSig;
        }

        /// <summary>
        /// Construccion de una nueva instancia de la clase Atributo.
        /// </summary>
        public Atributo()
        {

        }
    }
}
