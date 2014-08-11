using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ai;
using System.Collections.Generic;
using System.Linq;

namespace Testy
{
    [TestClass]
    public class Testy
    {
        [TestMethod]
        public void Help_ZnajdzSąsiadów()
        {
            var lista = Ai.Help.ZnajdzSąsiadów(new Coordinate { X = 3, Y = 2 });

            Assert.AreNotEqual(lista.Count, 0);

            var listaPunktów = new List<Coordinate>
            {
                new Coordinate{X=3,Y=1},
           //     new Wsporzedne{X=3,Y=2},
                new Coordinate{X=3,Y=3},
                new Coordinate{X=4,Y=1},
                new Coordinate{X=4,Y=2},
                new Coordinate{X=4,Y=3},
                new Coordinate{X=2,Y=1},
                new Coordinate{X=2,Y=2},
                new Coordinate{X=2,Y=3}
            };


            foreach (var l in listaPunktów)
            {
                var test = lista.Where(ws => ws.X == l.X).SingleOrDefault(ws => ws.Y == l.Y);
                Assert.AreNotEqual(test, null);
            }
        }
    }
}
