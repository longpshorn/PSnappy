using System.Threading.Tasks;

namespace PSnappy
{
    public interface IDatasetWriter
    {
        Task ResetAsync(string connectionString);
        Task SaveAsync(string connectionString);
    }
}
