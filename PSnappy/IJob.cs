using System.Collections.Generic;
using System.Threading.Tasks;

namespace PSnappy
{
    public interface IJob
    {
        IEnumerable<StatusEventArgs> StatusLog { get; }
        void Commit();
        void Reset();
        Task<bool> RunAsync();
    }
}
