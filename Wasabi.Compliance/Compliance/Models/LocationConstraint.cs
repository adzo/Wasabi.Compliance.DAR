using System.Xml.Serialization;

namespace Compliance.Models
{
    [XmlRoot(ElementName = "LocationConstraint")]
	public class LocationConstraint
	{

		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }

		[XmlText]
		public string Text { get; set; }
	}
}
