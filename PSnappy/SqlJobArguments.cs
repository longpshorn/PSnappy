using System;

namespace PSnappy
{
    public interface ISqlJobArguments
    {
        string ConnectionString { get; set; }
        string Name { get; set; }
        string UserName { get; set; }
        Guid ProcessId { get; set; }
    }

    public class SqlJobArguments : ISqlJobArguments
    {
        public string ConnectionString { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public Guid ProcessId { get; set; } = Guid.NewGuid();
    }
}
