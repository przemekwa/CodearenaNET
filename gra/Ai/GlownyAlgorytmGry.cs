﻿
using System.Net.Configuration;
using System.Security.Authentication.ExtendedProtection.Configuration;

namespace Ai
{
       using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    
    public class GlownyAlgorytmGry
    {
        const int POZIOM_LECZENIA = 80;

        private Wsporzedne Alter;
        private int wlasciciel;
 
        private int IndexAktualnegoWieszcholka;
        private List<Wieszcholek> listaWieszcholkow;
        
        private DirectionType PoprzedniKierunek { get; set; }

        private List<DirectionType> historiaRuchow;
      
        private DirectionType NamiaryNaDiament;
        private DirectionType NamiaryNaBaze;
        Unit unit { get; set; }
        private BackgroundType TypPolaNaKtorymStoje;

        private Graph graf { get; set; }

        public GlownyAlgorytmGry(Unit jednostka)
        {
            this.unit = jednostka;
            this.wlasciciel = jednostka.player;
            this.listaWieszcholkow = new List<Wieszcholek>();
            this.historiaRuchow = new List<DirectionType>();
            graf = new Graph();
        }

        private readonly List<Sees> listaPolGdzieMogeIsc = new List<Sees>();
    
        public string WyliczRuch(Unit jednostka)
        {
            this.unit = jednostka;
            DodajWieszchołek();
       
            //
            // Leczenie.
            //
            
            if (CzyJestemKołoBazy())
            {
                

                if (CzyJestDiament() && unit.action == ActionType.dragging)
                {
                    return Ai.CommandDictionary[OdłózDiament()];
                }
                if (CzyMamSieLeczyc())
                {
                    if (unit.action != ActionType.dragging)
                    {
                        return Ai.CommandDictionary[LeczSie()];
                    }
                }
            }

            //
            // Szukanie diamentu
            //

            if (CzyJestDiament())
            {
                return Ai.CommandDictionary[ZnalazłemDiamend(NamiaryNaDiament)];
            }

            //
            // Chodzenie
            //

            var komenda = AlgorytmEksploracji();

            return Ai.CommandDictionary[komenda];
        }
        private CommandType OdłózDiament()
        {
            if (unit.orientation == NamiaryNaBaze)
            {
                return CommandType.drop;
            }
            return ZmienKierunekPatrzenia(NamiaryNaBaze);
        }
        private bool CzyJestDiament()
        {
            //foreach (var pole in unit.seesList)
            //{
            //    if (pole.Object != null && pole.Object == ObjectType.diamond)
            //    {
            //        NamiaryNaDiament = pole.Direction;
            //        return true;
            //    }
            //}
            return false;
        }

        private void WyliczDrogę(Wieszcholek w)
        {
            if (graf.CalculateShortestPath())
            {
                var r = graf.RetrieveShortestPath(new Vector2D(w.wsporzedne.x,w.wsporzedne.y,false));
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
            if (listaWieszcholkow[IndexAktualnegoWieszcholka].Stan == Stan.nieodwiedzony)
            {
                foreach (var liść in listaWieszcholkow[IndexAktualnegoWieszcholka].listaLisci)
                {
                    //
                    // Jeśli nie mogę wejść na pole to uzaje, że nie da się tam wejść i ustawiam, że to pole odwiedziłem.
                    //
                    if (liść.stan == Stan.nieodwiedzony)
                    {
                        liść.stan = SprawdzCzyWogleMamSzanseWejscNaToPole(liść);
                        // [TODO] Dodać priorytet do algorytmu
                        lisceOdwiedzone = false;
                    }
                    //
                    // Wybieram pole jeśli mogę na nie iść. Zapisuje historię ruchów aby móc jakoś wrócić [TODO] Coś wymyśleć jak chodzić z punktu A do B!!!
                    //
                    if (liść.stan == Stan.nieodwiedzony && CzyMogeTamIsc(liść.Direction))
                    {
                        liść.stan = Stan.odwiedzony;

                        historiaRuchow.Add(liść.Direction);

                        return (CommandType)Enum.Parse(typeof(CommandType), liść.Direction.ToString());
                    }
                }
                if (lisceOdwiedzone)
                {
                    listaWieszcholkow[IndexAktualnegoWieszcholka].Stan = Stan.odwiedzony;
                }
            }
            
            //Nie było nic do odwiedzenia.
            //Idz do poprzedniego wieszchołka

            var kierunek = CofnijSię(historiaRuchow.Last());
            if (CzyMogeTamIsc(kierunek)) // Niby idiotyczne ale może ktos tam się pojawić i jakiś obiekt albo player
            {
                historiaRuchow.Remove(historiaRuchow.Last());
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
           //
           // Sprawdzam czy wieszchołek już istnieje.
           //

           var nowyWieszchołek = listaWieszcholkow.SingleOrDefault(w=>w.wsporzedne.Equals(unit.wsporzedne));

            //
            // Jeśli tak, to łącze wieszchołki razem.
            //

           if (nowyWieszchołek != null)
           {
               IndexAktualnegoWieszcholka = listaWieszcholkow.IndexOf(nowyWieszchołek);
               PołączWieszchołki(IndexAktualnegoWieszcholka);

               return false;
           }

            //
            // Jeśli nie, to dodaje nowy wieszchołek.
            //
           
           var tempWieszch = new Wieszcholek(this.unit.wsporzedne, unit.seesList)
           {
               Stan = Stan.nieodwiedzony,
               Priorytet = 0
           };

            //
            // Dodaje wieszchołek do algorytmu szukania ścieżki.
            //

           graf.AddVertex(new Vector2D(tempWieszch.wsporzedne.x, tempWieszch.wsporzedne.y, false));

            //
            // I łącze z pozostałymi
            //

            listaWieszcholkow.Add(tempWieszch);
            IndexAktualnegoWieszcholka = listaWieszcholkow.IndexOf(tempWieszch);
            PołączWieszchołki(IndexAktualnegoWieszcholka);

            return true;
        }
        private void PołączWieszchołki(int index)
        {
            //
            // Łącze wieszchołki porównując ich pozycje x i y. Jeśli się zgadzają to kopiuje status wieszchołka.
            //

            foreach (var l in listaWieszcholkow[index].listaLisci)
            {
                var temp = listaWieszcholkow.SingleOrDefault(p => p.wsporzedne.Equals(l.wsporzedne));

                if (temp != null)
                {
                    listaWieszcholkow[IndexAktualnegoWieszcholka].listaLisci.SingleOrDefault(p => p.wsporzedne.Equals(l.wsporzedne)).stan = Stan.odwiedzony;
                }
            }

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
            //[TODO] ustaw odpowiednia diament
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
                //[ToDo] Numery wieszchiołów.
                var kierunek = CofnijSię(historiaRuchow.Last());
                var komenda = (CommandType)Enum.Parse(typeof(CommandType), CofnijSię(historiaRuchow.Last()).ToString());
                if (CzyDianemStoiWOdpowiedniejPozycji(kierunek))
                {
                    if (CzyDiamentMozeSiePoruszyc(kierunek))
                    {
                        historiaRuchow.Remove(historiaRuchow.Last());
                        return komenda;
                    }
                    return UstawDiamentWodpowiedniejPozycji(historiaRuchow.Last());
                }
                return UstawDiamentTakAbyByloMoznaGoporuszyc(kierunek);
            }
        }
        private bool CzyDiamentMozeSiePoruszyc(DirectionType d)
        {
            var prawo = listaWieszcholkow[IndexAktualnegoWieszcholka].listaLisci[((int)d + 1) == 6 ? 1 : (int)d + 1].Background;
            var lewo = listaWieszcholkow[IndexAktualnegoWieszcholka].listaLisci[((int)d - 1) == -1 ? 5 : (int)d - 1].Background;
            var przod = TypPolaNaKtorymStoje;
            if ((((int)d + 3) >= 6 ? (int)d - 3 : (int)d + 3) == (int)NamiaryNaDiament)
            {
                if (przod == BackgroundType.orange) return false;
                return true;
            }
            if ((((int)d + 2) >= 6 ? (int)d - 4 : (int)d + 2) == (int)NamiaryNaDiament)
            {
                if (prawo == BackgroundType.orange) return false;
                if (prawo == BackgroundType.stone) return false;
                return true;
            }
            if ((((int)d - 2) <= -1 ? (int)d + 4 : (int)d - 2) == (int)NamiaryNaDiament)
            {
                if (lewo == BackgroundType.orange) return false;
                if (prawo == BackgroundType.stone) return false;
                return true;
            }
            return false;
        }
        private bool CzyDianemStoiWOdpowiedniejPozycji(DirectionType d)
        {
            var lewo = ((int)d + 1) == 6 ? 1 : (int)d + 1;
            var prawo = ((int)d - 1) == -1 ? 5 : (int)d - 1;
            var przod = (int)d;
            return lewo != (int)NamiaryNaDiament && prawo != (int)NamiaryNaDiament && przod != (int)NamiaryNaDiament;
        }
        private CommandType UstawDiamentTakAbyByloMoznaGoporuszyc(DirectionType d)
        {
            return CommandType.rotateRight;
        }
        private CommandType UstawDiamentWodpowiedniejPozycji(DirectionType d)
        {
            var prawo = listaWieszcholkow[IndexAktualnegoWieszcholka].listaLisci[((int)NamiaryNaDiament + 1) == 6 ? 1 : (int)d + 1].Background;
            var lewo = listaWieszcholkow[IndexAktualnegoWieszcholka].listaLisci[((int)NamiaryNaDiament - 1) == -1 ? 5 : (int)d - 1].Background;
            for (int i = 1; i < 3; i++)
            {
                if (
                listaWieszcholkow[IndexAktualnegoWieszcholka].listaLisci[
                ((int)NamiaryNaDiament + i) == 6 ? 1 : (int)d + i].Background == BackgroundType.orange)
                {
                }
            }
            return CommandType.rotateRight;
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
                Alter = this.unit.wsporzedne;
                this.graf.SourceVertex = new Vector2D(Alter.x, Alter.y, false);
                NamiaryNaBaze = poleZBaza.Direction;
            
               
                return true;
            }
        
      
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
            var docelowaOrientacja = (int)unit.orientation;
            var prawo = false;
            for (var i = 0; i < 2; i++)
            {
                docelowaOrientacja++;
                if (docelowaOrientacja == 6)
                {
                    docelowaOrientacja = 0;
                }
                if (docelowaOrientacja == (int)d)
                {
                    prawo = true;
                }
            }
            return prawo ? CommandType.rotateRight : CommandType.rotateLeft;
        }
        private DirectionType Obróć(DirectionType d, int a)
        {
            var wynik = (int)d;
            if (a > 0)
            {
                for (var i = 0; i < a - 1; i++)
                {
                    wynik++;
                    if (wynik >= 6)
                    {
                        wynik = 0;
                    }
                }
            }
            else if (a < 0)
            {
                for (var i = 0; i < Math.Abs(a) - 1; i++)
                {
                    wynik--;
                    if (wynik < 0)
                    {
                        wynik = 5;
                    }
                }
            }
            return (DirectionType)wynik;
        }
    }
}