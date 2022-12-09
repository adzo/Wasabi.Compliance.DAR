

using Compliance.Defaults;
using Compliance.Entities;
using Compliance.Helpers;
using Compliance.Models;

namespace Compliance.Services
{
    internal class RegionService : IRegionServices
    {
        public RegionService()
        {
            Regions = new List<Region>();
        }

        internal IList<Region> Regions { get; private set; }

        async Task<IEnumerable<Region>> IRegionServices.GetAllRegionsAsync()
        {
            if (!Regions.Any())
            {
                 await LoadRegionsAsync();
            }

            return Regions.AsEnumerable();
        }

        private async Task LoadRegionsAsync()
        {
            Regions.Clear();
            var regions = new DescribeRegionsResult();

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(Urls.RegionBaseUrl);

                var result = await httpClient.GetAsync("?describeRegions");

                result.EnsureSuccessStatusCode();

                var content = await result.Content.ReadAsStringAsync();

                regions = XmlParser.Parse<DescribeRegionsResult>(content); 
            }
                
            if (regions is not null)
            {  
                var i = 0;
                foreach (var region in  regions.Item
                   .OrderBy(x => x.Region) 
                   .ToList())
                {
                    Regions.Add(new Region
                    {
                        Id = i,
                        Name = region.Region,
                        Description = region.RegionName,
                        Endpoint = region.Endpoint
                    });

                    i++;
                } 
            }
            else
            {
                throw new ApplicationException("Could not load regions");
            }
        }
    }
}