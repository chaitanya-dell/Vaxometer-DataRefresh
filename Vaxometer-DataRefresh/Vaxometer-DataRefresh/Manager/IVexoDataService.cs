using System.Threading.Tasks;

namespace Vaxometer_DataRefresh.Manager
{
    public interface IVexoDataService
    {
        Task WatchSession();
    }
}
