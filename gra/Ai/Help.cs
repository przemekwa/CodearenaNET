using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ai
{
    public static class Help
    {
        public static List<Coordinate> ZnajdzSąsiadów(Coordinate ws)
        {
            var lista = new List<Coordinate>();

            var tablica1 = new int[3] { ws.X, ws.X + 1, ws.X - 1 };
            var tablica2 = new int[3] { ws.Y, ws.Y + 1, ws.Y - 1 };

            foreach (var x in tablica1)
            {
                foreach (var y in tablica2)
                {
                    lista.Add(new Coordinate { X = x, Y = y });
                }
            }

            lista.Remove(ws);

            return lista;
        }
    }
}
