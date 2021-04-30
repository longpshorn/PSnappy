namespace PSnappy
{
    public interface ISqlOptions
    {
        string Database { get; }
        string Server { get; }
        string UserName { get; }
    }

    public class SqlOptions : ISqlOptions
    {
        public string Server { get; }
        public string Database { get; }
        public string UserName { get; }

        public SqlOptions(string server, string database, string userName)
        {
            this.Server = server;
            this.Database = database;
            this.UserName = !string.IsNullOrEmpty(userName) ? userName : "psnappy";
        }
    }
}
