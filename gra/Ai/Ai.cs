

using System.Collections.Generic;


namespace Ai
{
    public class RuchJednostek
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
            {CommandType.SW, "<go direction='SW' />"}
        };
        
        private List<sessionUnit> listaSesji { get; set; }

        public RuchJednostek()
        {
            this.listaSesji = new List<sessionUnit>();
        }


        public string DajMiTenRuch(Game gra)
        {
            if (listaSesji.Count == 0)
             gra.listaJednostek.ForEach(unit=>listaSesji.Add(new sessionUnit(unit)));

            foreach (var sessionUnit in listaSesji)
            {
                return sessionUnit.WyliczRuch();
            }

            return CommandDictionary[CommandType.drag];
        }
    }
}
