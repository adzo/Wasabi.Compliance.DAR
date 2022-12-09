using Compliance.Helpers;
using Compliance.Services;

namespace Compliance
{
    public class Program
    {
        public static async Task Main()
        {
            PrintWelcomeMessage();
            

            await LoadRegionsAsync();
            

            await Task.Delay(50000);

            ProgressHelper.EndProgress();
        }

        private static async Task LoadRegionsAsync()
        {
            ProgressHelper.ShowProgress("Downloading list of regions");

            IRegionServices regionsService = new RegionService(); 
            var regions = await regionsService.GetAllRegionsAsync();


        }

        private static void PrintWelcomeMessage()
        {
            Console.WriteLine("");
        }
    }
}
