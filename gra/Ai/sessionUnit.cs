
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
        const int POZIOM_LECZENIA = 80;

        private Wsporzedne Alter;
        private int wlasciciel;

        private bool czyJestemKoloSwojejBazy;
        private bool czyMogeSieLeczyc;
        private int IndexAktualnegoWieszcholka;
        private List<Wieszcholek> listaWieszcholkow;
        private DirectionType poprzedniRuch;
        private List<DirectionType> historiaRuchow;
        private int IndexCofaniaRuchow;
        private DirectionType NamiaryNaDiament;

        Unit unit { get; set; }

        public sessionUnit(Unit jednostka)
        {
            this.unit = jednostka;
            this.wlasciciel = jednostka.player;
            this.listaWieszcholkow = new List<Wieszcholek>();
            this.historiaRuchow = new List<DirectionType>();
        }

        private readonly List<Sees> listaPolGdzieMogeIsc = new List<Sees>();

        private Wieszcholek skadPrzyzedłem;



        public string WyliczRuch(Unit jednostka)
        {
            this.unit = jednostka;
            DodajWieszchołek();
            GdzieMogeIsc();

            //
            // Leczenie.
            //
            if (CzyJestemKołoBazy())
            {
                if (CzyMamSieLeczyc())
                {
                    if (unit.action != ActionType.dragging)
                    {
                        return Ai.CommandDictionary[LeczSie()];
                    }
                }
            }

            ////
            //// Szukanie diamentu
            ////
            //if (CzyJestDiament())
            //{
            //    return Ai.CommandDictionary[ZnalazłemDiamend(NamiaryNaDiament)];
            //}

            var komenda = AlgorytmEksploracji();
            

          //  return RuchJednostek.CommandDictionary[WyliczKierunek()];
          // [TODO] Co to jest ten ruch jednostek?
            return Ai.CommandDictionary[komenda];
        }

      

        private bool CzyJestDiament()
        {
            foreach (var pole in unit.seesList)
            {
                if (pole.Object != null && pole.Object == ObjectType.diamond)
                {
                    NamiaryNaDiament = pole.Direction;
                    return true;
                }
            }
            return false;
        }

        private void GdzieMogeIsc()
        {
            listaPolGdzieMogeIsc.Clear();

            foreach (var pole in unit.seesList)
            {
                if (CzyMogeTamIsc(pole.Direction))
                {
                    listaPolGdzieMogeIsc.Add(pole);
                }
            }
        }

        private CommandType AlgorytmEksploracji()
        {
            //
            // Zapisz wieszchołek, na którym jestem. [TODO] Co zrobić z tymi na których jest kamień, diamend i mogą się zrobić wolne w trakcje gry.
            //

            var lisceOdwiedzone = true;

            //
            // Weź pierwszy nie odwiedzony liść.
            //

            if (listaWieszcholkow[IndexAktualnegoWieszcholka].stan == Stan.nieodwiedzony)
            {
                foreach (var liść in listaWieszcholkow[IndexAktualnegoWieszcholka].listaLisci)
                {
                    ///
                    /// Jeśli nie mogę wejść na pole to uzaje, że nie da się tam wejść i ustawiam, że to pole odwiedziłem.
                    ///

                    if (liść.stan == Stan.nieodwiedzony)
                    {
                        liść.stan = SprawdzCzyWogleMamSzanseWejscNaToPole(liść);
                        lisceOdwiedzone = false;
                    }

                    //
                    // Wybieram pole jeśli mogę na nie iść. Zapisuje historię ruchów aby móc jakoś wrócić [TODO] Coś wymyśleć jak chodzić z punktu A do B!!!
                    //

                    if (liść.stan == Stan.nieodwiedzony && CzyMogeTamIsc(liść.Direction))
                    {
                        liść.stan = Stan.odwiedzony;
                        historiaRuchow.Add(liść.Direction);
                        IndexCofaniaRuchow++;
                        return (CommandType) Enum.Parse(typeof (CommandType), liść.Direction.ToString());
                    }
                }

                if (lisceOdwiedzone)
                {
                    listaWieszcholkow[IndexAktualnegoWieszcholka].stan = Stan.odwiedzony;
                }
            }

            //Nie było nic do odwiedzenia. 

            //Idz do poprzedniego wieszchołka

            var kierunek = CofnijSię(historiaRuchow[IndexCofaniaRuchow]);
            IndexCofaniaRuchow--;

            //[TODO] Ideeks cofania zmienijszyć.

            if (CzyMogeTamIsc(kierunek)) // Niby idiotyczne ale może ktos tam się pojawić i jakiś obiekt albo player
            {
                return (CommandType)Enum.Parse(typeof(CommandType), kierunek.ToString());
            }
            

            // To znaczy, że jestem zablokowany. Same kamienie dookoła, lub jestem zablokowany przez kogoś

            // [TODO] Co zrobić jak mnie ktoś zablokuje.

            return CommandType.rotateRight;

        }

        private Stan SprawdzCzyWogleMamSzanseWejscNaToPole(Sees pole)
        {
            if (pole.Background == BackgroundType.black) return Stan.odwiedzony;
            if (pole.Background == BackgroundType.stone) return Stan.odwiedzony;
            if (pole.Object != null && pole.Object == ObjectType.stone) return Stan.odwiedzony;
            if (pole.Building != null && pole.Building.buildingType == BuildingType.altar) return Stan.odwiedzony;

            return Stan.nieodwiedzony;
        }

        private DirectionType CofnijSię(DirectionType d)
        {
            switch (d)
            {
                case DirectionType.NE:
                    return DirectionType.SW;
                case DirectionType.NW:
                    return DirectionType.SE;
                case DirectionType.SE:
                    return DirectionType.NW;
                case DirectionType.SW:
                    return DirectionType.NE;
                case DirectionType.E:
                    return DirectionType.W;
                case DirectionType.W:
                    return DirectionType.E;
                default:
                    return d;
            }
        }

        private bool DodajWieszchołek()
        {
            for (var i = 0; i < listaWieszcholkow.Count; i++)
            {
                if ((listaWieszcholkow[i].x == unit.x) && (listaWieszcholkow[i].y == unit.y))
                {
                    IndexAktualnegoWieszcholka = i;
                    return false;
                }
            }

            var tempWieszch = new Wieszcholek
            {
                x = unit.x,
                y = unit.y,
                stan = Stan.nieodwiedzony,
                listaLisci = unit.seesList,
                p = 0
            };

            listaWieszcholkow.Add(tempWieszch);

            IndexAktualnegoWieszcholka = listaWieszcholkow.IndexOf(tempWieszch);

            return true;
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

        private CommandType ZnalazłemDiamend(DirectionType dt)
        {
            if (unit.action != ActionType.dragging)
            {
                if (unit.orientation == dt)
                {
                    return CommandType.drag; 
                }
                
                return ZmienKierunekPatrzenia(dt);
            }
            else
            {
                var k = (CommandType) Enum.Parse(typeof (CommandType), CofnijSię(historiaRuchow[IndexCofaniaRuchow-1]).ToString());
                IndexCofaniaRuchow--;
                return k;
            }
        }

        private bool CzyJednostkaWidziKrawedz()
        {
            return unit.seesList.Any(pole => pole.Background == BackgroundType.black);
        }

        private bool CzyJestemKołoBazy()
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

        private bool CzyMamSieLeczyc()
        {
            return unit.hp < POZIOM_LECZENIA;
        }

        private CommandType LeczSie()
        {
            var kierunekDoBazy =
                unit.seesList.SingleOrDefault(
                    pole =>
                        pole.Building != null && pole.Building.buildingType == BuildingType.altar &&
                        pole.Building.player == wlasciciel);

            if (kierunekDoBazy != null)
            {
                return kierunekDoBazy.Direction == unit.orientation ? CommandType.heal : ZmienKierunekPatrzenia(kierunekDoBazy.Direction);
            }
            return CommandType.rotateRight;
        }

        private CommandType ZmienKierunekPatrzenia(DirectionType d)
        {
            //[TODO] Obsługa akcji dragging.

            var docelowaOrientacja = (int) unit.orientation;
            var prawo = false;

            for (var i = 0; i < 2; i++)
            {
                docelowaOrientacja++;

                if (docelowaOrientacja == 6)
                {
                    docelowaOrientacja = 0;
                }

                if (docelowaOrientacja == (int) d)
                {
                    prawo = true;
                }
            }

            return  prawo?CommandType.rotateRight:CommandType.rotateLeft;
        }
    }
}
