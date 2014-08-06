using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using codearenaTCP;
using System.Threading;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using Ai;

namespace gra
{
    class Program
    {
        static void Main(string[] args)
        {
            //List<string> bot = new List<string>() { "<go direction='SE' />", 
            //    "<go direction='SE' />", 
            //    "<go direction='SE' />", 
            //    "<go direction='SE' />", 
            //    "<go direction='SE' />", 
            //    "<go direction='E' />",
            //    "<go rotate='rotateRight' />", 
            //    "<go action='drag' />", 
            //    "<go direction='NW' />", 
            //    "<go direction='NW' />", 
            //    "<go direction='NW' />", 
            //    "<go direction='NW' />", 
            //    "<go direction='NW' />", 
            //    "<go rotate='rotateLeft' />", 
            //    "<go rotate='rotateLeft' />", 
            //    "<go rotate='rotateLeft' />",
            //    "<go direction='W' />",
            //    "<go action='drop' />",
            //    "<go rotate='rotateRight' />",
            //    "<go rotate='rotateRight' />",
            //    "<go direction='E' />",
            //    "<go direction='E' />",
            //    "<go direction='E' />",
            //    "<go direction='NE' />", 
            //    "<go direction='NE' />", 
            //    "<go action='drag' />", 
            //    "<go direction='SW' />", 
            //    "<go direction='SW' />",
            //    "<go direction='W' />",
            //    "<go direction='W' />",
            //    "<go direction='W' />",
            //    "<go rotate='rotateLeft' />", 
            //    "<go rotate='rotateLeft' />",
            //    "<go action='drop' />",
            //    "<go direction='SW' />",
            //    "<go direction='SW' />",
            //    "<go direction='SW' />",
            //    "<go direction='W' />",
            //    "<go direction='W' />",
            //    "<go action='drag' />", 
            //    "<go direction='E' />",
            //    "<go direction='E' />",
            //    "<go direction='NE' />",
            //    "<go direction='NE' />",
            //    "<go direction='NE' />",
            //    "<go action='drop' />",
            //};

            using (var codeArenaTcp = new SilnikTCP("codearena.pl", 7654))
            {
                codeArenaTcp.WyśliKomunikat("<connect userid=\"329\" hashid=\"ff48b0788df7f00d4a341db86c0c0a81\" /> ");

                Console.Read();
                                
                var silnikSztucznejInteligencji = new Ai.Ai();

                var xml = new XmlDocument();

                while (xml != null)
                {
                    xml = codeArenaTcp.OdbierzXml();

                    var graXMl = XmlDoObiektu.Konwersja(xml);

                    //[TODO] Włożyć obsługę drugiej jednostki

                    codeArenaTcp.WyśliKomunikat("<unit id='" + graXMl.listaJednostek[0].id + "'>" + silnikSztucznejInteligencji.DajMiTenRuch(graXMl) + "</unit>");

                    Thread.Sleep(1000);
                };

                Console.Read();
            }
        }
    }
}
