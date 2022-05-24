using Sgml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FlagActivityTracker.Parsers
{
    public class PageParserHelper
    {
        public static XmlDocument ParsePage(string pageHtml)
        {
            var sgmlReader = new SgmlReader();

            using var reader = new StringReader(pageHtml);

            sgmlReader.DocType = "HTML";
            sgmlReader.WhitespaceHandling = WhitespaceHandling.All;
            sgmlReader.CaseFolding = Sgml.CaseFolding.ToLower;
            sgmlReader.InputStream = reader;

            var xmlDoc = new XmlDocument();
            xmlDoc.Load(sgmlReader);

            return xmlDoc;
        }
    }
}
