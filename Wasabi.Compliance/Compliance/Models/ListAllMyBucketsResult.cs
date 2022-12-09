using System.Xml.Serialization;

namespace Compliance.Models
{
    // using System.Xml.Serialization;
    // XmlSerializer serializer = new XmlSerializer(typeof(ListAllMyBucketsResult));
    // using (StringReader reader = new StringReader(xml))
    // {
    //    var test = (ListAllMyBucketsResult)serializer.Deserialize(reader);
    // }

    [XmlRoot(ElementName = "Owner")]
	public class Owner
	{

		[XmlElement(ElementName = "ID")]
		public string ID { get; set; }

		[XmlElement(ElementName = "DisplayName")]
		public string DisplayName { get; set; }
	}

	[XmlRoot(ElementName = "Bucket")]
	public class Bucket
	{

		[XmlElement(ElementName = "Name")]
		public int Name { get; set; }

		[XmlElement(ElementName = "CreationDate")]
		public DateTime CreationDate { get; set; }
	}

	[XmlRoot(ElementName = "Buckets")]
	public class Buckets
	{

		[XmlElement(ElementName = "Bucket")]
		public List<Bucket> Bucket { get; set; }
	}

	[XmlRoot(ElementName = "ListAllMyBucketsResult")]
	public class ListAllMyBucketsResult
	{

		[XmlElement(ElementName = "Owner")]
		public Owner Owner { get; set; }

		[XmlElement(ElementName = "Buckets")]
		public Buckets Buckets { get; set; }

		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }

		[XmlText]
		public string Text { get; set; }
	}
}
