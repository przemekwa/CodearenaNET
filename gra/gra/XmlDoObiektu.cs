using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace gra
{
    static class XmlDoObiektu
    {
        public static gra Konwersja(XmlDocument xml)
        {

            var node = xml.SelectSingleNode("/game/general");

            var gra = new gra();

            gra.timeSec = node["timeSec"].InnerText;
            gra.roundNum = node["roundNum"].InnerText;
            gra.amountOfPoints = node["amountOfPoints"].InnerText;

            var nodes = xml.SelectNodes("/game/units/unit");

            foreach (XmlNode unit in nodes)
            {
                var jednostka = new Jednostka();

                jednostka.id = unit.Attributes["id"].InnerText;
                jednostka.x = Int32.Parse(unit.Attributes["x"].InnerText);
                jednostka.y = Int32.Parse(unit.Attributes["y"].InnerText);
                jednostka.status = unit.Attributes["status"].InnerText;
                jednostka.akcja = unit.Attributes["action"].InnerText;
                jednostka.kierunekPatrzenia = unit.Attributes["orientation"].InnerText;
                jednostka.gracz = Int32.Parse(unit.Attributes["player"].InnerText);
                jednostka.hp = Int32.Parse(unit.Attributes["hp"].InnerText);

                var pola = unit.SelectNodes("./sees");


              

                foreach (XmlNode sees in pola)
                {
                    var pole = new Pole();

                    pole.kierunekMarszu = sees.Attributes["direction"].InnerText;

                    var tlo = sees.SelectSingleNode("./background");

                    if (tlo == null)
                    {
                        pole.rodzajPola = "krawedz";
                    }
                    else
                    {
                        pole.rodzajPola = tlo.InnerText;
                    }
                    var budynek = sees.SelectSingleNode("./building");

                    if (budynek != null)
                    {
                        var bud = new budynek();
                        bud.gracz = budynek.Attributes["player"].InnerText;
                        bud.rodzaj = budynek.InnerText;
                        pole.budynek = bud;
                    }

                    var objekt = sees.SelectSingleNode("./object");

                    if (objekt != null)
                    {
                        var obj = new Objekt();
                        obj.typ = objekt.InnerText;
                        pole.objekt = obj;
                    }


                    jednostka.pola.Add(pole);
                     

                }


                gra.listaJednostek.Add(jednostka);
            }


            return gra;

        }
    }
}
