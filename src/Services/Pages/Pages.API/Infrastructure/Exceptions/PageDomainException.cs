using System;
namespace Pages.API.Infrastructure.Exceptions
{
    public class PageDomainException : Exception
    {
        public PageDomainException()
        {
        }

        public PageDomainException(string message)
            : base(message)
        { }

        public PageDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
