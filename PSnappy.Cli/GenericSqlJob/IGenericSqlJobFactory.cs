using PSnappy;

namespace PSnappy.Cli.GenericSqlJob
{
    public interface IGenericSqlJobFactory<T>
    {
        T CreateJob(SqlJobArguments args);
    }
}
