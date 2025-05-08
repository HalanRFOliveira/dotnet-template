using Microsoft.Extensions.Localization;

namespace Dotnet.Template.Infra.Resources
{
    public static class ResourceFactory
    {
        private static string _asm;

        public static void SetAssembly(string asm)
        {
            _asm = asm;
        }

        public static IStringLocalizerFactory Factory { get; set; }

        private static Dictionary<string, IStringLocalizer> localizers = new();

        public static IStringLocalizer Build(string resource)
        {
            if (!localizers.Any(k => k.Key == resource))
                localizers[resource] = Factory.Create(resource, _asm);

            return localizers[resource];
        }
    }
}
