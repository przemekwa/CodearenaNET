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
            Console.Read();

            using (var codeArenaTcp = new SilnikTCP("codearena.pl", 7654))
            {
                codeArenaTcp.WyśliKomunikat("<connect userid=\"329\" hashid=\"ff48b0788df7f00d4a341db86c0c0a81\" /> ");

                Thread.Sleep(2000);

                while (true)
                {
                    var test = codeArenaTcp.OdbierzKomunikat();
                                      

                    if (test != null)
                    {
                        var ruch = XmlDoObiektu.Konwersja(test);

                        codeArenaTcp.WyśliKomunikat("<unit id='" + ruch.listaJednostek[0].id + "'><go direction='SE' /></unit>");

                    }

                    Thread.Sleep(2000);

                }  
                

                Console.Read();
              
            };

          



           

        }
    }
}
