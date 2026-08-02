using Shared.Failures.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Failures.Exceptions.WebApi.ClientErrors
{
    public class UniqueConstraitException : ConflictException
    {
        public UniqueConstraitException(ErrorsCollection errors) : base(errors)
            => Error = HttpErrorWorkbench.Craft(errors);

        public UniqueConstraitException(Error error) : base(error)
            => Error = HttpErrorWorkbench.Craft(error);

        public UniqueConstraitException(string message = "Conflict!") : base(message)
            => Error = HttpErrorWorkbench.Craft(message);
    }
}
