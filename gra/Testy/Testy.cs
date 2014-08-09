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
            var lista = Ai.Help.ZnajdzSąsiadów(new Wsporzedne { x = 3, y = 2 });

            Assert.AreNotEqual(lista.Count, 0);

            var listaPunktów = new List<Wsporzedne>
            {
                new Wsporzedne{x=3,y=1},
           //     new Wsporzedne{x=3,y=2},
                new Wsporzedne{x=3,y=3},
                new Wsporzedne{x=4,y=1},
                new Wsporzedne{x=4,y=2},
                new Wsporzedne{x=4,y=3},
                new Wsporzedne{x=2,y=1},
                new Wsporzedne{x=2,y=2},
                new Wsporzedne{x=2,y=3}
            };


            foreach (var l in listaPunktów)
            {
                var test = lista.Where(ws => ws.x == l.x).SingleOrDefault(ws => ws.y == l.y);
                Assert.AreNotEqual(test, null);
            }
        }
    }
}
