using Compliance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Compliance.Helpers
{
    internal static class RegionsHelpers
    {
        internal static IEnumerable<string> Regions { get; private set;} = new List<string>();

        internal static async Task LoadRegionsAsync()
        {
            Console.WriteLine();
            Console.WriteLine("Loading the available regions...");
            var url = "https://s3.wasabisys.com";

            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(url);

            var result = await httpClient.GetAsync("?describeRegions");

            result.EnsureSuccessStatusCode();

            var regions = new DescribeRegionsResult();

            var content = await result.Content.ReadAsStringAsync();

            regions = XmlParser.Parse<DescribeRegionsResult>(content);

            if (regions is not null)
            {
                Regions = regions.Item
                   .OrderBy(x => x.Region)
                   .Select(x => x.Region)
                   .ToList();
                Console.WriteLine("Regions loaded successfully.");
                Console.WriteLine("Available regions:");
                foreach (var region in regions.Item.OrderBy(x => x.Region))
                {
                    Console.WriteLine($"        {region.Region.PadRight(15)} | {region.RegionName}");
                }
                Console.WriteLine();
            }
            else
            {
                Console.Error.WriteLine("Could not load regions!");
            }
           
        }
    }
}
