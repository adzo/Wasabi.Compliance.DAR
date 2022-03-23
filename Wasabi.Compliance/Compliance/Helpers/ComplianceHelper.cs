using Compliance.Models;
using System.Text;

namespace Compliance.Helpers
{
    internal class ComplianceHelper
    {
        internal static async Task<BucketComplianceConfiguration> LoadBucketConfigurationAsync(S3Configuration configuration)
        {
            var baseUrl = $"https://s3.{configuration.Region}.wasabisys.com";
            var requestUri = $"{configuration.BucketName}?compliance";

            var httpClient = new HttpClient(new S3AuthHandler(configuration.AccessKeyId, configuration.SecretKey, configuration.Region));
            httpClient.BaseAddress = new Uri(baseUrl);
            var response = await httpClient.GetAsync(requestUri);

            response.EnsureSuccessStatusCode(); 
            
            return XmlParser.Parse<BucketComplianceConfiguration>(await response.Content.ReadAsStringAsync());
        }

        internal static async Task EnableDeleteAfterRetention(S3Configuration configuration)
        {
            var baseUrl = $"https://s3.{configuration.Region}.wasabisys.com";
            var requestUri = $"{configuration.BucketName}?compliance";

            var httpClient = new HttpClient(new S3AuthHandler(configuration.AccessKeyId, configuration.SecretKey, configuration.Region));
            httpClient.BaseAddress = new Uri(baseUrl);

            var body = new EnableDar();

            var httpContent = new StringContent(XmlParser.Serialize(body), Encoding.UTF8, "text/xml");

            var response = await httpClient.PutAsync(requestUri, httpContent);

            response.EnsureSuccessStatusCode();
        }

        internal static async Task DisableDeleteAfterRetention(S3Configuration configuration)
        {
            var baseUrl = $"https://s3.{configuration.Region}.wasabisys.com";
            var requestUri = $"{configuration.BucketName}?compliance";

            var httpClient = new HttpClient(new S3AuthHandler(configuration.AccessKeyId, configuration.SecretKey, configuration.Region));
            httpClient.BaseAddress = new Uri(baseUrl);

            var body = new DisableDar();

            var httpContent = new StringContent(XmlParser.Serialize(body), Encoding.UTF8, "text/xml");

            var response = await httpClient.PutAsync(requestUri, httpContent);

            response.EnsureSuccessStatusCode();
        }
    }
}
