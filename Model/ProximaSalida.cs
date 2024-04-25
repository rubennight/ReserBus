using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReserBus.Model
{
    public class ProximaSalida
    {
        public string Destino { get; set; }
        public DateTime Salida { get; set; }
        public DateTime Llegada { get; set; }
        public int LugaresLibres { get; set; }
        public string Escalas {  get; set; }

    }
}
