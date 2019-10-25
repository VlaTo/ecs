using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace LibraProgramming.Game.Towers.Core
{
    internal sealed class EmbeddedResourceFileProvider : IFileProvider
    {
        private readonly string namespacePrefix;
        private readonly Assembly assembly;

        public EmbeddedResourceFileProvider(string namespacePrefix, Assembly assembly)
        {
            this.namespacePrefix = namespacePrefix;
            this.assembly = assembly;
        }

        public IFile GetFile(string path)
        {
            var resourceName = Path.ChangeExtension(namespacePrefix, path);
            var resourceInfo = assembly.GetManifestResourceInfo(resourceName);

            if (null == resourceInfo)
            {
                return null;
            }

            return new EmbeddedResourceFile(assembly, resourceInfo, resourceName);
        }
    }
}