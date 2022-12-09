using Compliance.Entities;
using Compliance.Models;

namespace Compliance.Services
{
    internal interface IRegionServices
    {
        Task<IEnumerable<Region>> GetAllRegionsAsync(); 
    }
}
