using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchivosTarea2
{
    public class Cajon
    {
        long posCajon { get; set; }
        long apuntadorCubeta { get; set; }
        public List<Cubeta> listaCubetas = new List<Cubeta>();

        public Cajon()
        {
            apuntadorCubeta = -1;
        }

        public void str_posCajon(long p)
        {
            posCajon = p;
        }

        public void str_apuntadorCubeta(long ap)
        {
            apuntadorCubeta = ap;
        }

        public long regresa_posCajon()
        {
            return posCajon;
        }

        public long regresa_apuntadorCubeta()
        {
            return apuntadorCubeta;
        }
    }
}
