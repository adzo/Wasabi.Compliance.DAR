using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance.Defaults
{
    internal class Urls
    {
        internal static string RegionBaseUrl => "https://s3.wasabisys.com";
        internal static string DescribeRegionRequestUri => "?describeRegions";
        internal static string S3BaseUrl(string regionName)
            => $"https://s3.{regionName}.wasabisys.com";
        internal static string ComplianceRequestUri(string bucketName)
            => $"{bucketName}?compliance";
    }
}
