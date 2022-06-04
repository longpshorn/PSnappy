using System.Threading.Tasks;

namespace PSnappy
{
    public interface IDatasetWriter
    {
        void Reset(string connectionString);
        Task SaveAsync(string connectionString);
    }
}
