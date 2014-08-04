using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using codearenaTCP;
using System.Threading;
namespace gra
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var codeArenaTcp = new SilnikTCP("codearena.pl",7654))
            {
                codeArenaTcp.WyśliKomunikat("<connect userid=\"329\" hashid=\"ff48b0788df7f00d4a341db86c0c0a81\" /> ");

                while (true)
                {
                    Thread.Sleep(4000);
                    codeArenaTcp.OdbierzKomunikat();
                }

            };

            



            Console.Read();

        }
    }
}
