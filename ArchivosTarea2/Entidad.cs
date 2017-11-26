﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchivosTarea2
{
    public class Entidad
    {
        public char[] nombre = { '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0',  
                                    '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0' ,'\0' ,'\n'};
        public long apAtributos = -1;
        public long apDatos = -1;
        public long posEntidad = 0;
        public long apSigEntidad = -1;
        public long apIndices = -1;
        public long apCajones = -1;
        public long apCabeceras = -1;
        public List<Atributo> listaAtributos = new List<Atributo>();
        public List<Dato> listaDatos = new List<Dato>();
        public List<Indice> listaIndices = new List<Indice>();
        public List<Cajon> listaCajones = new List<Cajon>();
        public List<Cabecera> listaCabeceras = new List<Cabecera>();

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
        /// <param name="n">El nombre de la entidad</param>
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
        public Entidad()
        {

        }
    }
}
