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

            if (tresc != null)
            {
                xml.LoadXml(tresc);
                return xml;
            }
            return null;
        }
    }
}
