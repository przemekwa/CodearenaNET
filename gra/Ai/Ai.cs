
using System.Collections.Generic;

namespace Ai
{
    public class Ai
    {
        static public readonly Dictionary<CommandType, string> CommandDictionary = new Dictionary<CommandType, string>
        {
            {CommandType.rotateRight, "<go rotate='rotateRight' />"},
            {CommandType.rotateLeft, "<go rotate='rotateLeft' />"},
            {CommandType.drop, "<go action='drop' />"},
            {CommandType.drag, "<go action='drag' />"},
            {CommandType.NW, "<go direction='NW' />s"},
            {CommandType.W, "<go direction='W' />s"},
            {CommandType.NE, "<go direction='NE' />"},
            {CommandType.E, "<go direction='E' />"},
            {CommandType.SE, "<go direction='SE' />"},
            {CommandType.SW, "<go direction='SW' />"},
            {CommandType.heal, "<go action='heal' />"}
        };
        
        private List<GlownyAlgorytmGry> listaSesji { get; set; }

        public Ai()
        {
            this.listaSesji = new List<GlownyAlgorytmGry>();
        }


        public string DajMiTenRuch(Game gra)
        {
            if (listaSesji.Count == 0)
                gra.listaJednostek.ForEach(unit => listaSesji.Add(new GlownyAlgorytmGry(unit)));

            foreach (var sessionUnit in listaSesji)
            {
                //[TODO] A co jak będą dwie jednostki?

                return sessionUnit.WyliczRuch(gra.listaJednostek[0]);
            }

            return null;
        }
    }
}
