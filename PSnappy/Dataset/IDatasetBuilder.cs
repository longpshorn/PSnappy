using System.Threading.Tasks;

namespace PSnappy
{
    public interface IDatasetBuilder
    {
        Task BuildAsync(IDatasetContext context, string server, string database);
    }
}
