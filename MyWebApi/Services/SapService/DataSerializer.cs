using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Services.SapService;

public class DataSerializer
{

    public static string XmlSerialize(Type dataType, object data, XmlSerializerNamespaces xmlns)
    {

        XmlSerializer xmlSerializer = new(dataType);
        var settings = new XmlWriterSettings
        {
            Encoding = Encoding.UTF8,
            Indent = true,
            OmitXmlDeclaration = true,
        };

        using (StringWriter textWriter = new StringWriter())
        {
            using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
            {
                xmlSerializer.Serialize(xmlWriter, data, xmlns);
            }

            return textWriter.ToString(); //This is the output as a string

        }
        //var builder = new StringBuilder();
        //using (var writer = XmlWriter.Create(builder, settings))
        //{
        //    xmlSerializer.Serialize(writer, data, xmlns);
        //}
        //return builder.ToString();

    }

}
