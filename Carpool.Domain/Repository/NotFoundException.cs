using System;

namespace Carpool.Domain.Repository
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException() : base() { }

        public NotFoundException(string message = "Object is not found.") : base(message) { }
    }
}
