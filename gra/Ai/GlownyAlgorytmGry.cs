
using System.Net.Configuration;
using System.Security.Authentication.ExtendedProtection.Configuration;

namespace Ai
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class sessionUnit
    {
        #region private_Property

        private const int POZIOM_LECZENIA = 80;

        private List<Coordinate> CoordinateList { get; set; }

        private List<HexField> HexMap { get; set; }

        private int Player { get; set; }

        private int CurrentHexIndex;

        private List<DirectionType> DirectionHistoryList;

        private Sees Baza { get; set; }

        private Sees Diament { get; set; }

        private Unit unit { get; set; }

        private Graph DijkstrasAlgorithm { get; set; }

        #endregion

        public sessionUnit(Unit currentUnit)
        {
            this.unit = currentUnit;
            this.Player = currentUnit.player;
            this.HexMap = new List<HexField>();
            this.DirectionHistoryList = new List<DirectionType>();
            this.CoordinateList = new List<Coordinate>();
            this.DijkstrasAlgorithm = new Graph();
        }

        public string GetMove(Unit currentUnit)
        {
            this.unit = currentUnit;

            AddHexField();

            //
            // Leczenie i odstawianie diamentu. + Pewnie też tu będzie odstawianie kamienia.
            //

            if (SetUpBaza())
            {
                if (SetUpDiament() && unit.action == ActionType.dragging)
                {
                    return Ai.CommandDictionary[OdłózDiament()];
                }                                                                   //[TODO] A jak się nie będe mógł uleczyć? Będzie cały czas się leczył. BŁĄD!!!
                if (CheckLiveLevel())
                {
                    if (unit.action != ActionType.dragging)
                    {
                        return Ai.CommandDictionary[Heal()];
                    }
                }
            }

            //
            // Czy mam ustawiony cel podróży.
            //

            if (CoordinateList.Count > 0 && unit.action != ActionType.dragging)
            {
                return Ai.CommandDictionary[(CommandType)Enum.Parse(typeof(CommandType), GetDirectionFormCoordinateList().ToString())];
            }

            //
            // Szukanie diamentu
            //

            if (SetUpDiament())
            {
                if (CoordinateList.Count == 0)
                {
                    GetCoordinatesFrom(CurrentHexIndex - 1);
                }

                // Ai.CommandDictionary[(CommandType)Enum.Parse(typeof(CommandType), GetDirectionFormCoordinateList().ToString())];

                this.CoordinateList =  new DiamondAlgorithm(HexMap, this.Diament).CaluculatPath();

                return Ai.CommandDictionary[(CommandType)Enum.Parse(typeof(CommandType), GetDirectionFormCoordinateList().ToString())];
            }

            //
            // Jeśli nie ma diamentu, nie musze się leczyć wtedy zwiedzam.
            //

            var komenda = ExploreAlgorithm();

            return Ai.CommandDictionary[komenda];
        }

        private DirectionType GetDirectionFormCoordinateList()
        {
            var hexToGo = HexMap[CurrentHexIndex].listaLisci.SingleOrDefault(p => p.wsporzedne.Equals(CoordinateList.Last()));

            if (hexToGo == null) return DirectionHistoryList.Last();

            CoordinateList.Remove(CoordinateList.Last());

            return hexToGo.Direction;
        }

        private DirectionType ReadDirectionFromCoordinateList()
        {
            var hexToGo = unit.seesList.SingleOrDefault(p => p.wsporzedne.Equals(CoordinateList.Last()));

            return hexToGo == null ? DirectionHistoryList.Last() : hexToGo.Direction;
        }

        private CommandType OdłózDiament()
        {
            return unit.orientation == Baza.Direction ? CommandType.drop : ZmienKierunekPatrzenia(Baza.Direction);
        }

        private bool SetUpDiament()
        {
            this.Diament = CheckObjectExisting(ObjectType.diamond);
            
            return Diament != null;
        }

        private bool SetUpBaza()
        {
            var poleZBaza =
            unit.seesList.SingleOrDefault(
            pole =>
            pole.Building != null && pole.Building.buildingType == BuildingType.altar &&
            pole.Building.player == Player);

            if (poleZBaza == null) return false;

            Baza = poleZBaza;

            var bazaVertex = DijkstrasAlgorithm.AllNodes.Where(p => p.XCoord == unit.wsporzedne.X
                ).SingleOrDefault(p => p.YCoord == unit.wsporzedne.Y);

            if (bazaVertex == null) return false;
            
            DijkstrasAlgorithm.SourceVertex = bazaVertex;

            return true;
        }

        private Sees CheckObjectExisting(ObjectType objectType)
        {
            return unit.seesList.FirstOrDefault(see => see.Object != null && see.Object == objectType);
        }

        private void GetCoordinatesFrom(int index)
        {
            if (!DijkstrasAlgorithm.CalculateShortestPath()) return;

            var shortestPathCoordinates = DijkstrasAlgorithm.RetrieveShortestPath(DijkstrasAlgorithm.AllNodes[index]);

            foreach (var coordinate in shortestPathCoordinates)
            {
                CoordinateList.Add(new Coordinate { X = coordinate.XCoord, Y = coordinate.YCoord });
            }
        }

        private CommandType ExploreAlgorithm()
        {
            //
            // Zapisz wieszchołek, na którym jestem. [TODO] Co zrobić z tymi na których jest kamień, diamend i mogą się zrobić wolne w trakcje gry.
            //
            var lisceOdwiedzone = true;
            //
            // Weź pierwszy nie odwiedzony liść.
            //
            if (HexMap[CurrentHexIndex].Stan == Stan.nieodwiedzony)
            {
                foreach (var liść in HexMap[CurrentHexIndex].listaLisci)
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

                        DirectionHistoryList.Add(liść.Direction);

                        return (CommandType)Enum.Parse(typeof(CommandType), liść.Direction.ToString());
                    }
                }
                if (lisceOdwiedzone)
                {
                    HexMap[CurrentHexIndex].Stan = Stan.odwiedzony;
                }
            }

            //Nie było nic do odwiedzenia.
            //Idz do poprzedniego wieszchołka


            var kierunek = CofnijSię(DirectionHistoryList.Last());

            if (CzyMogeTamIsc(kierunek)) // Niby idiotyczne ale może ktos tam się pojawić i jakiś obiekt albo player
            {
                DirectionHistoryList.Remove(DirectionHistoryList.Last());
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

        private bool AddHexField()
        {
            //
            // Sprawdzam czy wieszchołek już istnieje.
            //

            var existingHex = HexMap.SingleOrDefault(w => w.wsporzedne.Equals(unit.wsporzedne));

            //
            // Jeśli tak, to łącze wieszchołki razem.
            //

            if (existingHex != null)
            {
                CurrentHexIndex = HexMap.IndexOf(existingHex);
                PołączWieszchołki(CurrentHexIndex);

                return false;
            }

            //
            // Jeśli nie, to dodaje nowy wieszchołek.
            //

            var tempWieszch = new HexField(this.unit.wsporzedne, unit.seesList)
            {
                Stan = Stan.nieodwiedzony,
                Priorytet = 0
            };

            //
            // Dodaje wieszchołek do algorytmu szukania ścieżki.
            //

            var tempVertex = new Vector2D(tempWieszch.wsporzedne.X, tempWieszch.wsporzedne.Y, false);

            DijkstrasAlgorithm.AddVertex(tempVertex);

            //
            // I łącze z pozostałymi
            //

            HexMap.Add(tempWieszch);
            CurrentHexIndex = HexMap.IndexOf(tempWieszch);
            PołączWieszchołki(CurrentHexIndex);

            return true;
        }

        private void PołączWieszchołki(int index)
        {
            //
            // Łącze wieszchołki porównując ich pozycje x i y. Jeśli się zgadzają to kopiuje status wieszchołka.
            //

            foreach (var l in HexMap[index].listaLisci)
            {
                var temp = HexMap.SingleOrDefault(p => p.wsporzedne.Equals(l.wsporzedne));

                if (temp != null)
                {
                    HexMap[CurrentHexIndex].listaLisci.SingleOrDefault(p => p.wsporzedne.Equals(l.wsporzedne)).stan = Stan.odwiedzony;
                }
            }

            if (index > 0)
            {
                DijkstrasAlgorithm.AddEdge(new Edge(DijkstrasAlgorithm.AllNodes[index - 1], DijkstrasAlgorithm.AllNodes[index], 0));
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

        private bool CzyDiamentMozeSiePoruszyc(DirectionType d)
        {
            var prawo = HexMap[CurrentHexIndex].listaLisci[((int)d + 1) == 6 ? 1 : (int)d + 1].Background;
            var lewo = HexMap[CurrentHexIndex].listaLisci[((int)d - 1) == -1 ? 5 : (int)d - 1].Background;
            var przod =
                HexMap[CurrentHexIndex - 1].listaLisci.SingleOrDefault(
                    l => l.Direction == DirectionHistoryList.Last()).Background;

            if ((((int)d + 3) >= 6 ? (int)d - 3 : (int)d + 3) == (int)Diament.Direction)
            {
                if (przod == BackgroundType.orange) return false;
                return true;
            }
            if ((((int)d + 2) >= 6 ? (int)d - 4 : (int)d + 2) == (int)Diament.Direction)
            {
                if (prawo == BackgroundType.orange) return false;
                if (prawo == BackgroundType.stone) return false;
                return true;
            }
            if ((((int)d - 2) <= -1 ? (int)d + 4 : (int)d - 2) == (int)Diament.Direction)
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
            return lewo != (int)Diament.Direction && prawo != (int)Diament.Direction && przod != (int)Diament.Direction;
        }

        private CommandType UstawDiamentTakAbyByloMoznaGoporuszyc(DirectionType d)
        {
            return CommandType.rotateRight;
        }

        private CommandType UstawDiamentWodpowiedniejPozycji(DirectionType d)
        {
            var prawo = HexMap[CurrentHexIndex].listaLisci[((int)Diament.Direction + 1) == 6 ? 1 : (int)d + 1].Background;
            var lewo = HexMap[CurrentHexIndex].listaLisci[((int)Diament.Direction - 1) == -1 ? 5 : (int)d - 1].Background;
            for (int i = 1; i < 3; i++)
            {
                if (
                HexMap[CurrentHexIndex].listaLisci[
                ((int)Diament.Direction + i) == 6 ? 1 : (int)d + i].Background == BackgroundType.orange)
                {
                }
            }
            return CommandType.rotateRight;
        }

        private bool CheckLiveLevel()
        {
            return unit.hp < POZIOM_LECZENIA;
        }

        private CommandType Heal()
        {
            var kierunekDoBazy =
            unit.seesList.SingleOrDefault(
            pole =>
            pole.Building != null && pole.Building.buildingType == BuildingType.altar &&
            pole.Building.player == Player);
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
