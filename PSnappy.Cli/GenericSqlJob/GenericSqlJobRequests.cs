using System;
using System.Collections.Generic;

namespace PSnappy.Cli.GenericSqlJob
{
    public class GenericSqlJobRequests : IValidateableRequest
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }

        public virtual IEnumerable<string> Validate()
        {
            if (this.Server.IsNullOrEmpty()) yield return "Server is required.";
            if (this.Database.IsNullOrEmpty()) yield return "Database is required.";
        }

        public virtual SqlJobArguments ToArgs() => new SqlJobArguments()
        {
            Server = this.Server,
            Database = this.Database,
            Name = this.Name,
            UserName = this.UserName
        };
    }

    public class GenericSqlJobResetRequest : GenericSqlJobRequests
    {
        private Guid _processId;

        public Guid ProcessId
        {
            get => _processId;
            set => _processId = value == Guid.Empty ? Guid.NewGuid() : value;
        }

        public override SqlJobArguments ToArgs()
        {
            var args = base.ToArgs();
            args.ProcessId = this.ProcessId;
            return args;
        }
    }

    public class GenericSqlJobCommitRequest : GenericSqlJobRequests
    {
        public override SqlJobArguments ToArgs()
        {
            var args = base.ToArgs();
            return args;
        }
    }

    public class GenericSqlJobRunRequest : GenericSqlJobCommitRequest
    {
        public override SqlJobArguments ToArgs()
        {
            var args = base.ToArgs();
            return args;
        }
    }
}
