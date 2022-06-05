using System.Threading.Tasks;

namespace PSnappy
{
    public interface IDatasetBuilder
    {
        Task BuildAsync(string connectionString);
    }
}
