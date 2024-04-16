using System.Runtime.Serialization;

namespace Dotnet.Template.Infra.Exceptions
{
    public class GeneralException : Exception
    {
        public GeneralException(string message) : base(message) { }

        public GeneralException(string message, Exception innerException) : base(message, innerException) { }

    }
}
