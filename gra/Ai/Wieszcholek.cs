namespace Ai
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
   
    public class HexField
    {
        public Coordinate wsporzedne { get; set; }


        public List<Sees> listaLisci { get; private set; }
        public int Priorytet { get; set; }
        public Stan Stan { get; set; }

        public HexField(Coordinate w, List<Sees> lista)
        {
            this.wsporzedne = w;
            this.listaLisci = lista;
            this.PoliczyWsporzedneLisci();
        }

        private void PoliczyWsporzedneLisci()
        {
            var czyParzystaCzescPolaX = (this.wsporzedne.Y) % 2 == 0;

            foreach (var lisc in listaLisci)
            {
                switch (lisc.Direction)
                {
                    case DirectionType.E:
                        lisc.wsporzedne = new Coordinate { X = this.wsporzedne.X + 1, Y = this.wsporzedne.Y };
                        break;

                    case DirectionType.W:
                        lisc.wsporzedne = new Coordinate { X = this.wsporzedne.X - 1, Y = this.wsporzedne.Y };
                        break;

                    case DirectionType.NE:
                        if (czyParzystaCzescPolaX)
                        {
                            lisc.wsporzedne = new Coordinate { X = this.wsporzedne.X, Y = this.wsporzedne.Y- 1 };
                        }
                        else
                        {
                            lisc.wsporzedne = new Coordinate { X = this.wsporzedne.X + 1, Y = this.wsporzedne.Y - 1 };
                        }
                        break;

                    case DirectionType.NW:
                        if (czyParzystaCzescPolaX)
                        {
                            lisc.wsporzedne = new Coordinate { X = this.wsporzedne.X - 1, Y = this.wsporzedne.Y - 1 };
                        }
                        else
                        {
                            lisc.wsporzedne = new Coordinate { X = this.wsporzedne.X , Y = this.wsporzedne.Y - 1 };
                        }
                        break;

                    case DirectionType.SE:
                        if (czyParzystaCzescPolaX)
                        {
                            lisc.wsporzedne = new Coordinate { X = this.wsporzedne.X, Y = this.wsporzedne.Y + 1 };
                        }
                        else
                        {
                            lisc.wsporzedne = new Coordinate { X = this.wsporzedne.X + 1, Y = this.wsporzedne.Y + 1 };
                        }
                        break;

                    case DirectionType.SW:
                        if (czyParzystaCzescPolaX)
                        {
                            lisc.wsporzedne = new Coordinate { X = this.wsporzedne.X - 1, Y = this.wsporzedne.Y + 1 };
                        }
                        else
                        {
                            lisc.wsporzedne = new Coordinate { X = this.wsporzedne.X, Y = this.wsporzedne.Y + 1 };
                        }
                        break;
                }
            }
        }
    }
}
