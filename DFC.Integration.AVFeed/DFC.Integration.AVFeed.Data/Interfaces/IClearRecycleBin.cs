using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Data.Interfaces
{
    public interface IClearRecycleBin
    {
        Task ClearRecycleBinAsync();
    }
}
