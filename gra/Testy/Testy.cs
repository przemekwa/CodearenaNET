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

          //  Assert.AreEqual(true, listaPunktów.Equals(lista)); // Sprawdza też kolejność. 

            foreach (var l in listaPunktów)
            {
                var test = lista.Where(ws => ws.X == l.X).SingleOrDefault(ws => ws.Y == l.Y);
                Assert.AreNotEqual(test, null);
            }
        }


        [TestMethod]
        public void CheckDiamondalgorithm()
        {
            var map = new List<HexField>
            {
                new HexField(
                    new Coordinate{X=1,Y=2},
                    new List<Sees>
                    {
                        new Sees
                        { 
                            Background = BackgroundType.black,
                            Building = null,
                            Direction = DirectionType.E,
                            Object = null,
                            stan = Stan.odwiedzony,
                            wsporzedne = new Coordinate { X=1,Y=2}
                        }
                    })
            };

            var di = new DiamondAlgorithm(map,
                new Sees
                        {
                            Background = BackgroundType.black,
                            Building = null,
                            Direction = DirectionType.E,
                            Object = ObjectType.diamond,
                            stan = Stan.nieodwiedzony,
                            wsporzedne = new Coordinate { X = 1, Y = 2 }
                        });


            var acl = di.CaluculatPath();

            var testcl = new List<Coordinate>();


            Assert.AreEqual(true, testcl.Equals(acl)); // Kolejność musi być zachowana.
            
        }

    }
}
