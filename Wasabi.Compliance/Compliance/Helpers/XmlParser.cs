using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Compliance.Helpers
{
    internal class XmlParser
    {
        internal static T Parse<T>(string input)
        {
            T result = default;
            XmlSerializer serializer = new(typeof(T));
            using (StringReader reader = new(input))
            {
                result = (T)serializer.Deserialize(reader);
            }

            return result;
        }

        internal static string Serialize(object input)
        {
            using (var stringwriter = new Utf8StringWriter())
            {
                var serializer = new XmlSerializer(input.GetType());
                serializer.Serialize(stringwriter, input);
                return stringwriter.ToString();
            }
        }
    }

    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }
}
