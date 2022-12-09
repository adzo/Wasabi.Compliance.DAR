// See https://aka.ms/new-console-template for more information


using Wasabi.IP.Loader;

var regions = await RegionsHelpers.LoadRegionsAsync();

Console.WriteLine("Region,RegionName,Ip");
regions.ForEach(region =>
{
    var ips = NsLookupHelper.LoadIps(region.Item1);

     
    Console.WriteLine();
    foreach (var ip in ips.OrderBy(i => i))
    {
        Console.WriteLine($"{region.Item1},{region.Item2},{ip}"); 
    }
     
});
