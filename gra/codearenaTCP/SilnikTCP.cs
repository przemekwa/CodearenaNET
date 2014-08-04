using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace codearenaTCP
{
    public class SilnikTCP : IDisposable
    {
        private Stream stm;
        private TcpClient client;

        public SilnikTCP(string host, int port)
        {
            Console.WriteLine("Próba połączenia z {0} na porcie {1}", host, port);

            try
            {
                client = new TcpClient();
                
                client.Connect(host, port);

                Console.WriteLine("Połączony");

                stm = client.GetStream();
               
            }
            catch (Exception e)
            {
                Console.WriteLine("Wystapił błąd podczas połączenia {0}", e);
            }

        }
        

        public void WyśliKomunikat(string komunikat)
        {
            ASCIIEncoding asen = new ASCIIEncoding();
            byte[] ba = asen.GetBytes(komunikat);

            Console.WriteLine("Wysyłanie...{0}",komunikat);

            stm.Write(ba, 0, ba.Length);
            
        }

        public void OdbierzKomunikat()
        {
            Console.WriteLine("Odbieranie...");

            byte[] bb = new byte[1000];
            int k = stm.Read(bb, 0, 1000);

            for (int i = 0; i < k; i++)
                Console.Write(Convert.ToChar(bb[i]));
        }
        
        public void Dispose()
        {
            client.Close();
        }
    }
}
