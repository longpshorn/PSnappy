using System.Threading.Tasks;

namespace PSnappy
{
    public interface IDatasetBuilder
    {
        Task BuildAsync(string server, string database);
    }

    public interface IDatasetBuilder<T>
    {
        Task BuildAsync(T config);
    }
}
