namespace PSnappy
{
    public interface ISqlOptions
    {
        string ConnectionString { get; }
        string UserName { get; }
    }

    public class SqlOptions : ISqlOptions
    {
        public string ConnectionString { get; }
        public string UserName { get; }

        public SqlOptions(string connectionString, string userName)
        {
            this.ConnectionString = connectionString;
            this.UserName = !string.IsNullOrEmpty(userName) ? userName : "psnappy";
        }
    }
}
