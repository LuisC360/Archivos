using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchivosTarea2
{
    public class Dato
    {
        public long posDato = 0;
        public long apSigDato = -1;
        // Los atributos que posee el dato
        public List<Atributo> listaAtributosDato = new List<Atributo>();
        // Los datos que posee la clase Dato. Debido a que pueden ser de tipos distintos, esta lista sera de tipo object.
        public List<object> datos = new List<object>();
        // El atributo considerado como llave primaria
        public Atributo atLlave = new Atributo();
        // El indice del atributo llave primaria
        public int keyIndex = 0;

        /// <summary>
        /// Construccion de una nueva instancia de la clase Dato.
        /// </summary>
        public Dato()
        {

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
    }
}
