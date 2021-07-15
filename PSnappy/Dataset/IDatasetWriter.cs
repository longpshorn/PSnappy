using System.Threading.Tasks;

namespace PSnappy
{
    public interface IDatasetWriter
    {
        void Reset(string server, string database);
        Task SaveAsync(string server, string database);
    }
}
