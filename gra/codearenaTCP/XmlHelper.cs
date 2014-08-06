using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace codearenaTCP
{
     static class XmlHelper
    {
        public static XmlDocument DajXmla(string tresc)
        {
            XmlDocument xml = new XmlDocument();

            if (xml != null)
            {
                xml.LoadXml(tresc);
            }
            return null;
        }
    }
}
