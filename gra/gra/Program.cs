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
                codeArenaTcp.WyśliKomunikat("");

                Thread.Sleep(4000);


                while (true)
                {
                    codeArenaTcp.OdbierzKomunikat();
                }

            };

            



            Console.Read();

        }
    }
}
