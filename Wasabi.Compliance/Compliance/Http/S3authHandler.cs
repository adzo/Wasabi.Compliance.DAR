using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web; 
    class S3AuthHandler : DelegatingHandler
    {
        private readonly string _accessKeyId;
        private readonly string _secretAccessKey;
        private readonly string _region;
        private readonly string _service; 
        private static readonly string EmptySha256 = Sha256(new byte[0]);

        public S3AuthHandler(string accessKeyId, string secretAccessKey, string region, string service = "s3") : base(new HttpClientHandler())
        {
            _accessKeyId = accessKeyId;
            _secretAccessKey = secretAccessKey;
            _region = region;
            _service = service;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await SignAsync(request);

            return await base.SendAsync(request, cancellationToken);
        }

        protected async Task<string> SignAsync(HttpRequestMessage request, DateTimeOffset? signDate = null)
        {
            
            DateTimeOffset dateToUse = signDate ?? DateTimeOffset.UtcNow;
            string nowDate = dateToUse.ToString("yyyyMMdd");
            string amzNowDate = GetAmzDate(dateToUse);

            request.Headers.Add("x-amz-date", amzNowDate); 

            string payloadHash = await AddPayloadHashHeader(request);

            string canonicalRequest = request.Method + "\n" +
               GetCanonicalUri(request) + "\n" +   
               GetCanonicalQueryString(request) + "\n" +
               GetCanonicalHeaders(request, out string signedHeaders) + "\n" +   
               signedHeaders + "\n" +
               payloadHash;
         

            string stringToSign = "AWS4-HMAC-SHA256\n" +
                  amzNowDate + "\n" +
                  nowDate + "/" + _region + "/s3/aws4_request\n" +
                  Sha256(Encoding.UTF8.GetBytes(canonicalRequest)); 
         

            byte[] kSecret = Encoding.UTF8.GetBytes(("AWS4" + _secretAccessKey).ToCharArray());
            byte[] kDate = HmacSha256(nowDate, kSecret);
            byte[] kRegion = HmacSha256(_region, kDate);
            byte[] kService = HmacSha256(_service, kRegion);
            byte[] kSigning = HmacSha256("aws4_request", kService);
         
            byte[] signatureRaw = HmacSha256(stringToSign, kSigning);
            string signature = BitConverter.ToString(signatureRaw).Replace("-", string.Empty); 

            string auth = $"Credential={_accessKeyId}/{nowDate}/{_region}/s3/aws4_request,SignedHeaders={signedHeaders},Signature={signature}";
            request.Headers.Authorization = new AuthenticationHeaderValue("AWS4-HMAC-SHA256", auth);

            return signature;
        }

        private static string GetAmzDate(DateTimeOffset date)
        {
            return date.ToString("yyyyMMddTHHmmssZ");
        }

        private string GetCanonicalUri(HttpRequestMessage request)
        {
            string path = request.RequestUri.AbsolutePath.TrimStart('/');

            return "/" + path;//.UrlEncode();
        }

        private string GetCanonicalQueryString(HttpRequestMessage request)
        { 

            NameValueCollection values = HttpUtility.ParseQueryString(request.RequestUri.Query);
            var sb = new StringBuilder();

            foreach (string key in values.AllKeys.OrderBy(k => k))
            {
                if (sb.Length > 0)
                {
                    sb.Append('&');
                }

                string value = HttpUtility.UrlEncode(values[key]);

                if (key == null)
                {
                    sb
                       .Append(value)
                       .Append("=");
                }
                else
                {
                    sb
                       .Append(HttpUtility.UrlEncode(key.ToLower()))
                       .Append("=")
                       .Append(value);

                }
            }

            return sb.ToString();
        }

        private string GetCanonicalHeaders(HttpRequestMessage request, out string signedHeaders)
        { 
            var headers = from kvp in request.Headers
                          where kvp.Key.StartsWith("x-amz-", StringComparison.OrdinalIgnoreCase)
                          orderby kvp.Key
                          select new { Key = kvp.Key.ToLowerInvariant(), kvp.Value };

            var sb = new StringBuilder();
            var signedHeadersList = new List<string>();
 
            if (request.Headers.Contains("date"))
            {
                sb.Append("date:").Append(request.Headers.GetValues("date").First()).Append("\n");
                signedHeadersList.Add("date");
            }

            sb.Append("host:").Append(request.RequestUri.Host).Append("\n");
            signedHeadersList.Add("host");

            string contentType = request.Content?.Headers.ContentType?.ToString();
            if (contentType != null)
            {
                sb.Append("content-type:").Append(contentType).Append("\n");
                signedHeadersList.Add("content-type");
            }

            if (request.Headers.Contains("range"))
            {
                sb.Append("range:").Append(request.Headers.GetValues("range").First()).Append("\n");
                signedHeadersList.Add("range");
            } 
            foreach (var kvp in headers)
            {
                sb.Append(kvp.Key).Append(":");
                signedHeadersList.Add(kvp.Key);

                foreach (string hv in kvp.Value)
                {
                    sb.Append(hv);
                }

                sb.Append("\n");
            }

            signedHeaders = string.Join(";", signedHeadersList);

            return sb.ToString();
        }
 
        private async Task<string> AddPayloadHashHeader(HttpRequestMessage request)
        {
            string hash;

            if (request.Content != null)
            {
                byte[] content = await request.Content.ReadAsByteArrayAsync();
                hash = Sha256(content);// content.SHA256().ToHexString();
            }
            else
            {
                hash = EmptySha256;
            }

            request.Headers.Add("x-amz-content-sha256", hash);

            return hash;
        }

        private static byte[] HmacSha256(string data, byte[] key)
        {
            var alg = KeyedHashAlgorithm.Create("HmacSHA256");
            alg.Key = key;
            return alg.ComputeHash(Encoding.UTF8.GetBytes(data));
        }

        private static string Sha256(byte[] input)
        {
            var crypt = new SHA256Managed();
            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(input);
            foreach (byte theByte in crypto)
            {
                hash += theByte.ToString("x2");
            }
            return hash;
        } 
    }
