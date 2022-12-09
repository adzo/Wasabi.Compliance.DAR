

using BucketLifeCyclePolicies;
using DnsClient;
using System.Net;
using System.Text;

ServicePointManager
    .ServerCertificateValidationCallback +=
    (sender, cert, chain, sslPolicyErrors) => true;

#region Access Keys
var access_key_id = "Q1DVC07O1MYTCS0O7ADD";
var secretKey = "z1HmKEQagvSeZrHzMzqCOOvS79Y1AMvqrmFJz82t";
#endregion

var regions = await RegionsHelpers.LoadRegionsAsync();
var bucketName = Guid.NewGuid().ToString();
foreach (var region in regions)
{
    
    var regionUrl = $"https://s3.{region}.wasabisys.com";
    
    var bucketHandlerClient = new HttpClient(new S3AuthHandler(access_key_id, secretKey, "us-east-1"));
    bucketHandlerClient.BaseAddress = new Uri("https://s3.wasabisys.com");

    Console.WriteLine("**********************************************************************************");
    Console.WriteLine("                               " + region);
    Console.WriteLine("**********************************************************************************");

    // create the bucket
    using (Stream stream = typeof(Program).Assembly.GetManifestResourceStream("BucketLifeCyclePolicies.createBucket.txt"))
    using (StreamReader reader = new StreamReader(stream))
    {
        string content = reader.ReadToEnd();
        content = content.Replace("REGION_NAME", region);
        var stringContent = new StringContent(content, Encoding.UTF8, "application/xml");

        var put = await bucketHandlerClient.PutAsync($"{bucketName}", stringContent); 
    }

    await TestBucketPoliciesOfRegionAsync(bucketName, region, access_key_id, secretKey);

    // delete the bucket!
    var bucketdeletionClient = new HttpClient(new S3AuthHandler(access_key_id, secretKey, region));
    bucketdeletionClient.BaseAddress = new Uri(regionUrl);
    var deleteResponse = await bucketdeletionClient.DeleteAsync($"{bucketName}");
}

static async Task TestBucketPoliciesOfRegionAsync(string bucketName, string region, string keyId, string secret)
{
    var ipAddresses = NsLookupHelper.LoadIps(region);

    foreach (var ip in ipAddresses.OrderBy(i => i))
    {
        var url = $"http://{ip}";
        var requestUri = $"{bucketName}/?lifecycle";

        using var httpClient = new HttpClient(new S3AuthHandler(keyId, secret, region));
        httpClient.BaseAddress = new Uri(url);
         
        var responses = new Response();

        #region GET COnfiguration => 404
        var firstGet = await httpClient.GetAsync(requestUri);
        if (firstGet.Headers.TryGetValues("Server", out var values))
        {
            responses.ServerVersion = values.First();
            responses.ServerName = values.Skip(1).First();
        }
        responses.FirstGet = firstGet.StatusCode;
        #endregion

        #region PUT configuration => 200
        using (Stream stream = typeof(Program).Assembly.GetManifestResourceStream("BucketLifeCyclePolicies.TextFile1.txt"))
        using (StreamReader reader = new StreamReader(stream))
        {
            string content = reader.ReadToEnd();
            var stringContent = new StringContent(content, Encoding.UTF8, "application/xml");

            var put = await httpClient.PutAsync(requestUri, stringContent);
            responses.Put = put.StatusCode;
        }
        #endregion

        #region GET configuration => 200
        var secondGet = await httpClient.GetAsync(requestUri);
        responses.SecondGet = secondGet.StatusCode;
        #endregion

        #region DEL configuraiton => 206
        var delete = await httpClient.DeleteAsync(requestUri);
        responses.Delete = delete.StatusCode;
        #endregion

        Console.WriteLine($"IP: {ip.PadRight(16)} | Server Name : {responses.ServerName.PadRight(12)} | Version: {responses.ServerVersion} | GET: {(int)responses.FirstGet} | PUT: {(int)responses.Put} | GET: {(int)responses.SecondGet} | DELETE: {(int)responses.Delete}");
    }

} 



Console.ReadKey();







