// using System.Xml.Serialization;
// XmlSerializer serializer = new XmlSerializer(typeof(DescribeRegionsResult));
// using (StringReader reader = new StringReader(xml))
// {
//    var test = (DescribeRegionsResult)serializer.Deserialize(reader);
// }

using System.Xml.Serialization;

namespace Compliance.Models
{

    [XmlRoot(ElementName = "item")]
    public class Item
    {

        [XmlElement(ElementName = "Region")]
        public string Region { get; set; }

        [XmlElement(ElementName = "RegionName")]
        public string RegionName { get; set; }

        [XmlElement(ElementName = "Endpoint")]
        public string Endpoint { get; set; }

        [XmlElement(ElementName = "Status")]
        public string Status { get; set; }

        [XmlElement(ElementName = "IsDefault")]
        public bool IsDefault { get; set; }
    }

    [XmlRoot(
        ElementName = "DescribeRegionsResult",
        Namespace = "http://s3.amazonaws.com/doc/2006-03-01/")]
    public class DescribeRegionsResult
    {

        [XmlElement(ElementName = "item")]
        public List<Item> Item { get; set; }

        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

}

