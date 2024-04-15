using Microsoft.Extensions.Localization;

namespace Dotnet.Template.Infra.Resources
{
    public class ResourceFactory
    {
        private static string _asm;

        public static void SetAssembly(string asm)
        {
            _asm = asm;
        }

        public static IStringLocalizerFactory Factory { get; set; }

        private static readonly Dictionary<string, IStringLocalizer> _localizers = new();

        public static IStringLocalizer Build(string resource)
        {
            if (!_localizers.Any(k => k.Key == resource))
                _localizers[resource] = Factory.Create(resource, _asm);

            return _localizers[resource];
        }
    }
}
