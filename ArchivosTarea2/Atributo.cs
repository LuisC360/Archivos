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
        public long bytes = 0;
        public long posAtributo = 0;
        public Boolean esLlavePrimaria;
        public long apSigAtributo = -1;

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

        public Atributo(char[] nom, char t, long by, long posAt, bool esLl, long apSig)
        {
            nombre = nom;
            tipo = t;
            bytes = by;
            posAtributo = posAt;
            esLlavePrimaria = esLl;
            apSigAtributo = apSig;
        }

        public Atributo()
        {

        }
    }
}
