

using System.Net.Configuration;
using System.Security.Authentication.ExtendedProtection.Configuration;

namespace Ai
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;


    public struct Wsporzedne
    {
        public int x { get; set; }
        public int y { get; set; }
    }

    public class sessionUnit
    {
        private Wsporzedne Alter;
        private int wlasciciel;

        private bool czyJestemKoloSwojejBazy;
        private bool czyMogeSieLeczyc;
        
        Unit unit { get; set; }

        public sessionUnit(Unit jednostka)
        {
            this.unit = jednostka;
            this.wlasciciel = jednostka.player;
        }

        private readonly List<Sees> listaPolGdzieMogeIsc = new List<Sees>();

        private List<DirectionType> skadPrzyzedłem = new List<DirectionType>();

        int[,] gdzieBylem = new int[50,50];

        public string WyliczRuch()
        {
            Zamelduj();

            CzyJestemKołoBazy();

            listaPolGdzieMogeIsc.Clear();
            
            return RuchJednostek.CommandDictionary[WyliczKierunek()];
        }

        private CommandType WyliczKierunek()
        {
            foreach (var pole in unit.seesList)
            {
                if (CzyMogeTamIsc(pole.Direction) )
                {
                    listaPolGdzieMogeIsc.Add(pole);
                }
            }

            if (listaPolGdzieMogeIsc.Count == 0)
            {
                return CommandType.rotateLeft;
            }

            if (listaPolGdzieMogeIsc.Count == 1)
            {
                return Konwersje.GetActionType(listaPolGdzieMogeIsc[0].Direction);
            }

            
            foreach (var seese in listaPolGdzieMogeIsc)
            {
                if (unit.x == unit.y)
                {
                    return Konwersje.GetActionType(seese.Direction);
                }
               
                if (unit.x > unit.y)
                {
                    if (seese.Direction == DirectionType.SE || seese.Direction == DirectionType.SW || seese.Direction == DirectionType.E)
                    {
                        return Konwersje.GetActionType(seese.Direction);
                    }

                }
                
                if (unit.x < unit.y)
                {
                    if (seese.Direction == DirectionType.NE || seese.Direction == DirectionType.NW ||  seese.Direction == DirectionType.W)
                    {
                        return Konwersje.GetActionType(seese.Direction);
                    }
                }
            }

            return Konwersje.GetActionType(listaPolGdzieMogeIsc.First().Direction);

        }


        private void Zamelduj()
        {
            gdzieBylem[unit.x, unit.y] = 1;
        }

        private bool CzyMogeTamIsc(DirectionType kierunek)
        {
            var pole = unit.seesList.Single(p => p.Direction == kierunek);

            if (pole.Background == BackgroundType.black) return false;
            if (pole.Background == BackgroundType.stone) return false;
            if (pole.Object != null && pole.Object == ObjectType.diamond) return false;
            if (pole.Object != null && pole.Object == ObjectType.stone) return false;
            return pole.Building == null || pole.Building.buildingType != BuildingType.altar;
        }

        CommandType ZnalazłemDiamend(DirectionType dt)
        {
            if (unit.orientation == dt)
            {
                return CommandType.drag;
            }

            return CommandType.rotateLeft;
        }


        bool CzyJednostkaWidziKrawedz()
        {
            return unit.seesList.Any(pole => pole.Background == BackgroundType.black);
        }

        bool CzyJestemKołoBazy()
        {
            var poleZBaza =
                unit.seesList.SingleOrDefault(
                    pole =>
                        pole.Building != null && pole.Building.buildingType == BuildingType.altar &&
                        pole.Building.player == wlasciciel);

            if (poleZBaza != null)
            {
                Alter.x = unit.x;
                Alter.y = unit.y;
                czyJestemKoloSwojejBazy = true;

                if (unit.orientation == poleZBaza.Direction)
                    czyMogeSieLeczyc = true;

                return true;
            }
           
            czyMogeSieLeczyc = false;
            czyJestemKoloSwojejBazy = false;
                
            return false;
        }
    }
}
