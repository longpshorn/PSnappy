using System.Threading.Tasks;
using PSnappy;

namespace PSnappy.Cli.GenericSqlJob
{
    public interface IGenericSqlJobService
    {
        void Commit(GenericSqlJobCommitRequest request);
        void Reset(GenericSqlJobResetRequest request);
        Task RunAsync(GenericSqlJobRunRequest request);
    }

    public class GenericSqlJobService<T> : IGenericSqlJobService
        where T : IJob
    {
        private readonly IGenericSqlJobFactory<T> _factory;
        private readonly IValidator _validator;

        public GenericSqlJobService(
            IGenericSqlJobFactory<T> factory,
            IValidator validator
        )
        {
            _factory = factory;
            _validator = validator;
        }

        public void Commit(GenericSqlJobCommitRequest request)
        {
            if (!_validator.Validate(request)) return;
            var job = _factory.CreateJob(request.ToArgs());
            job.Commit();
        }

        public void Reset(GenericSqlJobResetRequest request)
        {
            if (!_validator.Validate(request)) return;
            var job = _factory.CreateJob(request.ToArgs());
            job.Reset();
        }

        public async Task RunAsync(GenericSqlJobRunRequest request)
        {
            if (!_validator.Validate(request)) return;
            var job = _factory.CreateJob(request.ToArgs());
            await job.RunAsync();
        }
    }
}
