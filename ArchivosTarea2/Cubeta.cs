using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchivosTarea2
{
    public class Cubeta
    {
        public long apDato { get; set; }
        public long posCubeta { get; set; }
        public long apSigCubeta { get; set; }
        public Dato datoCubeta { get; set; }

        public Cubeta()
        {
            apDato = -1;
            apSigCubeta = 0;
        }

        public void str_apDato(long ap)
        {
            apDato = ap;
        }

        public void str_posCubeta(long pos)
        {
            posCubeta = pos;
        }

        public void str_apSigCubeta(long ap)
        {
            apSigCubeta = ap;
        }

        public void str_datoCubeta(Dato dato)
        {
            datoCubeta = dato;
        }

        public long regresa_apDato()
        {
            return apDato;
        }

        public long regresa_posCubeta()
        {
            return posCubeta;
        }

        public long regresa_apSigCubeta()
        {
            return apSigCubeta;
        }

        public Dato regresa_datoCubeta()
        {
            return datoCubeta;
        }
    }
}
