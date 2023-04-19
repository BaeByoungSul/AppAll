using System.Xml;

namespace Models.Sap;

public class IgnoreNamespaceXmlTextReader : XmlTextReader
{
    public IgnoreNamespaceXmlTextReader(TextReader reader) : base(reader)
    {
    }
    public override string NamespaceURI => "";
}
