using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace codearenaTCP
{
    public class SilnikTCP : IDisposable
    {
        private Stream stm;
        private TcpClient client;

        private NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        public SilnikTCP(string host, int port)
        {
            Console.Write("Próba połączenia z {0} na porcie {1}..........", host, port);
            log.Trace("Próba połączenia z {0} na porcie {1}..........", host, port);
            try
            {
                client = new TcpClient();
                
                client.Connect(host, port);

                Console.WriteLine("Połączony!");
                log.Info("Połączony");
                
                stm = client.GetStream();
               
            }
            catch (Exception e)
            {
                Console.WriteLine("Wystapił błąd podczas połączenia {0}", e);
                log.Trace("Wystapił błąd podczas połączenia {0}", e);
            }

        }
        

        public void WyśliKomunikat(string komunikat)
        {
            try
            {
                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes(komunikat);

                Console.WriteLine("Wysyłanie...{0}", komunikat);
                log.Info(komunikat);
                stm.Write(ba, 0, ba.Length);

            }
            catch (Exception e)
            {
                Console.WriteLine("Wystapił błąd podczas wysyłania komunikatu {0}", e);
                log.Error(e);
            }
        }

        public XmlDocument OdbierzKomunikat()
        {
            try
            {
                Console.WriteLine("Odbieranie...");

                byte[] bb = new byte[10000];
                int k = stm.Read(bb, 0, 10000);

                StringBuilder odp = new StringBuilder();

                for (int i = 0; i < k; i++)
                {
                    odp.Append(Convert.ToChar(bb[i]));
                }

                string[] separator = { "<?xml version=\"1.0\"?>" };

                var tablicaOdp = odp.ToString().Split(separator, StringSplitOptions.None);

                foreach (var s in tablicaOdp)
                {
                    if (s.Contains("response"))
                    {
                        var node = XmlHelper.DajXmla(s).SelectSingleNode("response");

                        Console.WriteLine("Odpowiedz z serwera o statusie gry {0}", node.Attributes[0].InnerText);
                        log.Trace(s);

                        continue;

                    }
                    else if (s.Contains("error"))
                    {
                        var node = XmlHelper.DajXmla(s).SelectSingleNode("error");

                        Console.WriteLine("Error {0}", node.Attributes[0].InnerText);
                        log.Trace(s);

                        continue;
                    }
                    else if (s.Contains("ok"))
                    {
                        var node = XmlHelper.DajXmla(s).SelectSingleNode("ok");

                        Console.WriteLine("Status ruchu: {0}", node.Name);
                        log.Trace(s);

                        continue;
                    }
                    else if (!string.IsNullOrEmpty(s))
                    {
                        log.Info(s);
                        return XmlHelper.DajXmla(s);
                    }

                }
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine("Wystapił błąd podczas odbierania komunikatu {0}", e);
                log.Error(e);
                
                return null;
            }
            
        }
        
        public void Dispose()
        {
            client.Close();
            log.Info("Rozłączony");
        }
    }
}
