
namespace Ai
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

   
   
    public class Wieszcholek
    {
       

        public Wsporzedne wsporzedne { get; set; }

        public List<Sees> listaLisci { get; private set; } 

        public Wieszcholek(Wsporzedne w, List<Sees> lista)
        {
            this.wsporzedne = w;
            this.listaLisci = lista;
            this.PoliczyWsporzedneLisci();
        }

        private void PoliczyWsporzedneLisci()
        {
            var czyParzystaCzescPolaX = (this.wsporzedne.y) % 2 == 0;

            foreach (var lisc in listaLisci)
            {
                switch (lisc.Direction)
                {
                    case DirectionType.E:
                        lisc.wsporzedne = new Wsporzedne { x = this.wsporzedne.x + 1, y = this.wsporzedne.y };
                        break;
                    case DirectionType.W:
                        lisc.wsporzedne = new Wsporzedne { x = this.wsporzedne.x - 1, y = this.wsporzedne.y };
                        break;
                    case DirectionType.NE:
                        if (czyParzystaCzescPolaX)
                        {
                            lisc.wsporzedne = new Wsporzedne { x = this.wsporzedne.x, y = this.wsporzedne.y - 1 };
                        }
                        else
                        {
                            lisc.wsporzedne = new Wsporzedne { x = this.wsporzedne.x +1, y = this.wsporzedne.y - 1 };
                        }
                        break;
                    case DirectionType.NW:
                        if (czyParzystaCzescPolaX)
                        {
                            lisc.wsporzedne = new Wsporzedne { x = this.wsporzedne.x-1, y = this.wsporzedne.y - 1 };
                        }
                        else
                        {
                            lisc.wsporzedne = new Wsporzedne { x = this.wsporzedne.x , y = this.wsporzedne.y - 1 };
                        }
                        break;

                    case DirectionType.SE:
                        if (czyParzystaCzescPolaX)
                        {
                            lisc.wsporzedne = new Wsporzedne { x = this.wsporzedne.x, y = this.wsporzedne.y + 1 };
                        }
                        else
                        {
                            lisc.wsporzedne = new Wsporzedne { x = this.wsporzedne.x + 1, y = this.wsporzedne.y + 1 };
                        }
                        break;

                    case DirectionType.SW:
                        if (czyParzystaCzescPolaX)
                        {
                            lisc.wsporzedne = new Wsporzedne { x = this.wsporzedne.x - 1, y = this.wsporzedne.y + 1 };
                        }
                        else
                        {
                            lisc.wsporzedne = new Wsporzedne { x = this.wsporzedne.x, y = this.wsporzedne.y + 1 };
                        }
                        break;


                }
            }
        }

       

        public int p { get; set; }
        public Stan stan { get; set; }
        
    }
}
