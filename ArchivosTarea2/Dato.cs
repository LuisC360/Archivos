using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchivosTarea2
{
    /// <summary>
    /// Clase que representara un dato o un registro.
    /// </summary>
    public class Dato
    {
        /// <summary>
        /// La posicion del dato en el archivo.
        /// </summary>
        public long posDato;
        /// <summary>
        /// El apuntador al siguiente dato.
        /// </summary>
        public long apSigDato = -1;
        /// <summary>
        /// Los atributos que posee el dato.
        /// </summary>
        public List<Atributo> listaAtributosDato = new List<Atributo>();
        /// <summary>
        /// Los datos que posee la clase Dato. Debido a que pueden ser de tipos distintos, esta lista sera de tipo object.
        /// </summary>
        public List<object> datos = new List<object>();
        /// <summary>
        /// El atributo considerado como llave primaria
        /// </summary>
        public Atributo atLlave = new Atributo();
        /// <summary>
        /// El indice del atributo llave primaria
        /// </summary>
        public int keyIndex;
        /// <summary>
        /// El indice ligado al dato.
        /// </summary>
        Indice ind;
        /// <summary>
        /// La lista de apuntadores que el dato puede tener en caso de que alguno de sus atributos sea llave de busqueda.
        /// </summary>
        public List<long> apuntadoresLlaveBusq = new List<long>();

        /// <summary>
        /// Construccion de una nueva instancia de la clase Dato.
        /// </summary>
        public Dato()
        {
            posDato = 0;
        }

        /// <summary>
        /// Construccion de una nueva instancia de la clase Dato.
        /// </summary>
        /// <param name="e">La entidad en la que se insertara el dato.</param>
        public Dato(Entidad e)
        {
            foreach(Atributo at in e.listaAtributos)
            {
                if(at.apSigAtributo != -2 && at.apSigAtributo != -4)
                {
                    listaAtributosDato.Add(at);

                    if(at.esLlavePrimaria == true)
                    {
                        atLlave = at;
                    }
                }
            }
        }

        /// <summary>
        /// Constructor de una nueva instancia de la clase Dato.
        /// </summary>
        /// <param name="e">La entidad en la que se insertara el dato.</param>
        /// <param name="i">El indice ligado al dato.</param>
        public Dato(Entidad e, Indice i)
        {
            foreach (Atributo at in e.listaAtributos)
            {
                if (at.apSigAtributo != -2 && at.apSigAtributo != -4)
                {
                    listaAtributosDato.Add(at);

                    if (at.esLlavePrimaria == true)
                    {
                        atLlave = at;
                    }
                }
            }

            ind = i;
        }

        /// <summary>
        /// Constructor de una nueva instancia de la clase Dato.
        /// </summary>
        /// <param name="e">La entidad en la que se insertara el dato.</param>
        /// <param name="apuntadores">Los apuntadores de llave de busqueda del dato.</param>
        public Dato(Entidad e, List<long> apuntadores)
        {
            foreach (Atributo at in e.listaAtributos)
            {
                if (at.apSigAtributo != -2 && at.apSigAtributo != -4)
                {
                    listaAtributosDato.Add(at);

                    if (at.esLlavePrimaria == true)
                    {
                        atLlave = at;
                    }
                }
            }

            apuntadoresLlaveBusq = apuntadores;
        }

        /// <summary>
        /// Metodo que regresara el atributo que sea la llave primaria.
        /// </summary>
        /// <returns>El atributo que sea la llave primaria.</returns>
        public Atributo regresa_llave_primaria()
        {
            Atributo llave = new Atributo();

            foreach(Atributo atr in listaAtributosDato)
            {
                if(atr.esLlavePrimaria == true)
                {
                    llave = atr;
                }
            }

            return llave;
        }

        /// <summary>
        /// Metodo que devuelve el indice de la llave primaria en la lista de atributos de la entidad.
        /// </summary>
        /// <returns>El indice de la llave primaria de la lista de atributos.</returns>
        public int indice_llave_primaria()
        {
            int index = 0;

            foreach (Atributo atr in listaAtributosDato)
            {
                if (atr.esLlavePrimaria == true)
                {
                    break;
                }
                index++;
            }

            return index;
        }

        /// <summary>
        /// Metodo con el cual se inicializaran los apuntadores para cada llave de busqueda.
        /// </summary>
        public void inicia_apuntadores_busqueda()
        {
            foreach(Atributo atr in listaAtributosDato)
            {
                apuntadoresLlaveBusq.Add(-1);
            }
        }
    }
}
