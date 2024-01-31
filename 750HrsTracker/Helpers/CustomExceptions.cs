using System.Globalization;

namespace _750HrsTracker.Helpers
{
    public class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException() : base() { }

        public ForbiddenAccessException(string message) : base(message) { }

        public ForbiddenAccessException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }

    public class EmailNotConfirmedException : Exception
    {
        public EmailNotConfirmedException() : base() { }

        public EmailNotConfirmedException(string message) : base(message) { }

        public EmailNotConfirmedException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }


    public class RequestValidationException : Exception
    {
        public RequestValidationException() : base() { }

        public RequestValidationException(string message) : base(message) { }

        public RequestValidationException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
