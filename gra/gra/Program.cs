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
                Thread.Sleep(2000);

                while (true)
                {
                    var test = codeArenaTcp.OdbierzKomunikat();

                    if (test != null)
                    {
                        var ruch = XmlDoObiektu.Konwersja(test);

                        codeArenaTcp.WyśliKomunikat("<unit id='" + ruch.listaJednostek[0].id + "'>" + ""+ "</unit>");
                    }

                    Thread.Sleep(2000);

                };
                Console.Read();
            }
        }
    }
}
