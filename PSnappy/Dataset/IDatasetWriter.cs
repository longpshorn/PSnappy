using System.Threading.Tasks;

namespace PSnappy
{
    public interface IDatasetWriter
    {
        void Reset(string server, string database);
        Task SaveAsync(IDatasetContext dataset, string server, string database);
    }
}
