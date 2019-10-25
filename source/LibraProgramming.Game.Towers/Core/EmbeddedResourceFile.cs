using System.IO;
using System.Reflection;

namespace LibraProgramming.Game.Towers.Core
{
    internal class EmbeddedResourceFile : IFile
    {
        private readonly Assembly assembly;
        private readonly ManifestResourceInfo resourceInfo;
        private readonly string resourceName;

        public EmbeddedResourceFile(Assembly assembly, ManifestResourceInfo resourceInfo, string resourceName)
        {
            this.assembly = assembly;
            this.resourceInfo = resourceInfo;
            this.resourceName = resourceName;
        }

        public void Dispose()
        {
            ;
        }

        public Stream OpenRead()
        {
            return assembly.GetManifestResourceStream(resourceName);
        }
    }
}