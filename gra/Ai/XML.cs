using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TestyDRAC;

namespace TestyDRAC
{
    static class XmlDoObiektu
    {
        public static Game Konwersja(XmlDocument xml)
        {
            var general = xml.SelectSingleNode("/game/general");

            var game = new Game
            {
                timeSec = general["timeSec"].InnerText,
                roundNum = general["roundNum"].InnerText,
                amountOfPoints = general["amountOfPoints"].InnerText
            };

            var units = xml.SelectNodes("/game/units/unit");

            foreach (XmlNode unit in units)
            {
                var tempUnit = new Unit
                {
                    id = unit.Attributes["id"].InnerText,
                    x = Int32.Parse(unit.Attributes["x"].InnerText),
                    y = Int32.Parse(unit.Attributes["y"].InnerText),
                    status = unit.Attributes["status"].InnerText,
                    action = string.IsNullOrEmpty(unit.Attributes["action"].InnerText) ? (ActionType?)null : (ActionType)Enum.Parse(typeof(ActionType), unit.Attributes["action"].InnerText),
                    orientation = (DirectionType)Enum.Parse(typeof(DirectionType), unit.Attributes["orientation"].InnerText),
                    player = Int32.Parse(unit.Attributes["player"].InnerText),
                    hp = Int32.Parse(unit.Attributes["hp"].InnerText)

                };

                var sees = unit.SelectNodes("./sees");

                foreach (XmlNode see in sees)
                {
                    var tempSee = new Sees
                    {
                        Direction = (DirectionType)Enum.Parse(typeof(DirectionType), see.Attributes["direction"].InnerText),
                    };

                    var tlo = see.SelectSingleNode("./background");

                    if (tlo == null)
                    {
                        tempSee.Background = BackgroundType.black;
                    }
                    else
                    {
                        tempSee.Background = (BackgroundType)Enum.Parse(typeof(BackgroundType), tlo.InnerText);
                    }
                    var tempBuilding = see.SelectSingleNode("./building");

                    if (tempBuilding != null)
                    {
                        var building = new Building
                        {
                            player = Int32.Parse(tempBuilding.Attributes["player"].InnerText),
                            buildingType = (BuildingType)Enum.Parse(typeof(BuildingType), tempBuilding.InnerText == "base" ? "altar" : tempBuilding.InnerText)
                        };

                        tempSee.Building = building;
                    }

                    var tempObject = see.SelectSingleNode("./object");

                    if (tempObject != null)
                        tempSee.Object = (ObjectType)Enum.Parse(typeof(ObjectType), tempObject.InnerText);

                    tempUnit.seesList.Add(tempSee);
                }

                game.listaJednostek.Add(tempUnit);
            }


            return game;

        }
    }
}
