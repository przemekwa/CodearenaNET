using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ai
{
    public static class Help
    {
        public static List<Wsporzedne> ZnajdzSąsiadów(Wsporzedne ws)
        {
            var lista = new List<Wsporzedne>();

            var tablica1 = new int[3] { ws.x, ws.x + 1, ws.x - 1 };
            var tablica2 = new int[3] { ws.y, ws.y + 1, ws.y - 1 };

            foreach (var x in tablica1)
            {
                foreach (var y in tablica2)
                {
                    lista.Add(new Wsporzedne { x=x,y=y });
                }
            }

            lista.Remove(ws);

            return lista;
        }
    }
}
