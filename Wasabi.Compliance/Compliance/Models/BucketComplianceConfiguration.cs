using System.Xml.Serialization;

namespace Compliance.Models
{
    [XmlRoot(ElementName = "BucketComplianceConfiguration")]
	public class DisableDar
	{
		public DisableDar()
		{ 
			DeleteAfterRetention = false; 
		} 

		[XmlElement(ElementName = "DeleteAfterRetention")]
		public bool DeleteAfterRetention { get; set; }
	}

	[XmlRoot(ElementName = "BucketComplianceConfiguration")]
	public class EnableDar
	{
		public EnableDar()
		{
			DeleteAfterRetention = true;
		}

		[XmlElement(ElementName = "DeleteAfterRetention")]
		public bool DeleteAfterRetention { get; set; }
	}

	[XmlRoot(ElementName = "BucketComplianceConfiguration",
		Namespace = "http://s3.amazonaws.com/doc/2006-03-01/")]
	public class BucketComplianceConfiguration
	{

		[XmlElement(ElementName = "Status")]
		public string Status { get; set; }

		[XmlElement(ElementName = "LockTime")]
		public string LockTime { get; set; }

		[XmlElement(ElementName = "IsLocked")]
		public bool IsLocked { get; set; }

		[XmlElement(ElementName = "RetentionDays")]
		public int RetentionDays { get; set; }

		[XmlElement(ElementName = "ConditionalHold")]
		public bool ConditionalHold { get; set; }

		[XmlElement(ElementName = "DeleteAfterRetention")]
		public bool DeleteAfterRetention { get; set; }

		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }

		[XmlText]
		public string Text { get; set; }
	}
}
