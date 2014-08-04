﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gra
{
    public enum DirectionType { NE, E, SE, SW, W, NW  }

    public enum BackgroundType { green, orange, blue, red, black, stone }

    public enum ObjectType { stone, diamond }

    public enum BuildingType { altar } 

    public class Game
    {
        public string timeSec { get; set; }
        public string roundNum { get; set; }
        public string amountOfPoints { get; set; }
        public List<Unit> listaJednostek = new List<Unit>();
    }

    public class Sees
    {
        public DirectionType Direction { get; set; }
        public BackgroundType Background { get; set; }
        public Building Building { get; set; }
        public ObjectType Object { get; set; }
    }

    public class Unit
    {
        public string id { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int hp { get; set; }
        public string status { get; set; }
        public string action { get; set; }
        public DirectionType orientation { get; set; }
        public int player { get; set; }
        public List<Sees> seesList = new List<Sees>();
    }

    public class Building
    {
        public string player { get; set; }
        public BuildingType buildingType { get; set; }
    }
}
