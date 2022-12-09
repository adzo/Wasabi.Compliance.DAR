using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Wasabi.IP.Loader
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

    internal class RegionsHelpers
    {
        internal async static Task<List<(string, string)>> LoadRegionsAsync()
        {
            var describeRegionsClient = new HttpClient();
            describeRegionsClient.BaseAddress = new Uri("https://s3.us-east-1.wasabisys.com");
            var result = await describeRegionsClient.GetAsync("?describeRegions");

            XmlSerializer serializer = new XmlSerializer(typeof(DescribeRegionsResult));
            using (StringReader reader = new StringReader(await result.Content.ReadAsStringAsync()))
            {
                var test = (DescribeRegionsResult)serializer.Deserialize(reader);

                return test.Item.Select(item => (item.Region, item.RegionName)).ToList();
            }
        }
    }

}
