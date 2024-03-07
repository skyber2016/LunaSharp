using LunaSharp.Utils;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace LunaSharpKernel
{
    internal class Assemblies
    {
        #region Fields

        private static bool m_initialized;

        #endregion

        #region Static Methods

        internal static void Initialize()
        {
            if (m_initialized) return;

            m_initialized = true;

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += currentDomain_AssemblyResolve;

            var pathToPlugin = Path.Combine(ApplicationInfo.PathRoot, "Plugins");
            var allow = new string[] { "ESP.dll" };
            if (Directory.Exists(pathToPlugin))
            {
                var plugins = Directory.GetFiles(pathToPlugin, "*.dll");
                foreach (string assemblyDll in plugins)
                {
                    var file = Path.GetFileName(assemblyDll);
                    if (allow.Contains(file))
                    {
                        LoadAssembly(assemblyDll);
                    }
                }

            }

        }

        private static void LoadAssembly(string assemblyPath)
        {
            Assembly a = Assembly.LoadFile(assemblyPath);

            Logging.Write()($"Loading Assembly '{Path.GetFileNameWithoutExtension(assemblyPath)}' Version={a.GetName().Version}");

            Type myType = a.GetTypes().SingleOrDefault(t => t.Name == "Program");

            MethodInfo myMethod = myType.GetMethod("Main");

            object obj = Activator.CreateInstance(myType);

            try
            {
                myMethod.Invoke(obj, null);
                Logging.Write()($"Loaded plugin '{Path.GetFileNameWithoutExtension(assemblyPath)}' Version={a.GetName().Version}");
            }
            catch (Exception ex)
            {
                Logging.Write()("{0}", ex.ToString());
            }
        }

        private static Assembly currentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string strTempAssmbPath = "";

            if (args.Name.Substring(0, args.Name.IndexOf(",", StringComparison.Ordinal)) == $"LunaSharp")
            {
                return Assembly.GetExecutingAssembly();
            }

            Assembly myAssembly = Assembly.LoadFrom(strTempAssmbPath);

            return myAssembly;
        }

        #endregion
    }
}
