namespace PSnappy
{
    public interface IDatasetCommitter
    {
        void Commit(IDatasetContext context);
    }
}
