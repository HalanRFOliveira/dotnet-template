using Dotnet.Template.Infra.Exceptions;
using Dotnet.Template.Infra.Resources;

namespace Dotnet.Template.Infra.Extensions
{
    public static class StringExtensions
    {
        public static string Resource(this string chave, params object[] args)
        {
            var resource = ResourceFactory.Build("Globalization")[chave];

            if (resource.ResourceNotFound)
                throw new GeneralException($"Globalization Failed!. {chave} not found.");

            return args == null || args.Length == 0 ? resource.Value : string.Format(resource.Value, args);
        }
    }
}
