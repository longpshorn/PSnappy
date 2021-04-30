using System;
using System.Collections.Generic;
using System.Linq;
using PSnappy;

namespace PSnappy.Cli
{
    public interface IValidateableRequest
    {
        IEnumerable<string> Validate();
    }

    public interface IValidator
    {
        bool Validate(IValidateableRequest request);
    }

    public class Validator : IValidator
    {
        private readonly CLILogger _logger;

        public Validator(CLILogger logger)
        {
            _logger = logger;
        }

        public bool Validate(IValidateableRequest request)
        {
            var errors = request.Validate();

            if (errors.Any())
            {
                foreach (var e in errors)
                {
                    _logger.LogStatus(e, TimeSpan.Zero, StatusType.Error);
                }

                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
