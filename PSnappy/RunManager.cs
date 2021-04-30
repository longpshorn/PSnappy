using System;

namespace PSnappy
{
    public interface IRunManager
    {
        IRunOptions GetRunOptions();
    }

    public class RunManager : IRunManager
    {
        private readonly ISqlOptions _sqlOptions;

        public RunManager(
            ISqlOptions sqlOptions
        )
        {
            _sqlOptions = sqlOptions;
        }

        public IRunOptions GetRunOptions()
        {
            var processId = Guid.NewGuid();
            return new RunOptions(_sqlOptions, processId);
        }
    }
}
