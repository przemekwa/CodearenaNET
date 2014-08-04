using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gra
{
    class Jednostka
    {
        public string id { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int hp { get; set; }
        public string status { get; set; }
        public string akcja { get; set; }
        public string kierunekPatrzenia { get; set; }
        public int gracz { get; set; }
        public List<Pole> pola = new List<Pole>();
    }
}
