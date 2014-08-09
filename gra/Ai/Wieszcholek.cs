
namespace Ai
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

   
   
    public class Wieszcholek
    {
        public int x { get; private set; }
        public int y { get; private set; }

        public List<Sees> listaLisci { get; private set; } 

        public Wieszcholek(int x, int y, List<Sees> lista)
        {
            this.x = x;
            this.y = y;
            this.listaLisci = lista;
            this.PoliczyWsporzedneLisci();
        }

        private void PoliczyWsporzedneLisci()
        {
            var czyParzystaCzescPolaX = (x + y) % 2 == 0;

            foreach (var lisc in listaLisci)
            {
                switch (lisc.Direction)
                {
                    case DirectionType.E:
                        lisc.wsporzedne = new Wsporzedne { x = this.x + 1, y = this.y };
                        break;
                    case DirectionType.W:
                        lisc.wsporzedne = new Wsporzedne { x = this.x - 1, y = this.y };
                        break;
                    case DirectionType.NE:
                        if (czyParzystaCzescPolaX)
                        {
                            lisc.wsporzedne = new Wsporzedne { x = this.x + 1, y = this.y-1 };
                        }
                        else
                        {
                            lisc.wsporzedne = new Wsporzedne { x = this.x, y = this.y - 1 };
                        }
                        break;
                    case DirectionType.NW:
                        if (czyParzystaCzescPolaX)
                        {
                            lisc.wsporzedne = new Wsporzedne { x = this.x, y = this.y - 1 };
                        }
                        else
                        {
                            lisc.wsporzedne = new Wsporzedne { x = this.x-1, y = this.y - 1 };
                        }
                        break;

                    case DirectionType.SE:
                        if (czyParzystaCzescPolaX)
                        {
                            lisc.wsporzedne = new Wsporzedne { x = this.x, y = this.y + 1 };
                        }
                        else
                        {
                            lisc.wsporzedne = new Wsporzedne { x = this.x - 1, y = this.y + 1 };
                        }
                        break;

                    case DirectionType.SW:
                        if (czyParzystaCzescPolaX)
                        {
                            lisc.wsporzedne = new Wsporzedne { x = this.x+1, y = this.y + 1 };
                        }
                        else
                        {
                            lisc.wsporzedne = new Wsporzedne { x = this.x, y = this.y + 1 };
                        }
                        break;


                }
            }
        }

       

        public int p { get; set; }
        public Stan stan { get; set; }
        
    }
}
