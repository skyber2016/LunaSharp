using System.IO;
using System.Reflection;

namespace LunaSharp.Utils
{
    internal class ApplicationInfo
    {
        /// <summary>
        ///     Application Path
        /// </summary>
        public static readonly string PathRoot = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }
}
