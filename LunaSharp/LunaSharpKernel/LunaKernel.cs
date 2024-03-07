﻿using LunaSharp.Memory;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace LunaSharp.LunaSharpKernel
{
    internal class LunaKernel
    {
        #region Fields

        private static Native.CreateInterface m_callCreateInterface;

        #endregion

        #region Static Methods

        internal static TClass CreateInterface<TClass>(string module, int offset = 0)
            where TClass : InteropHelp.INativeWrapper, new()
        {
            try
            {
                ProcessModule procModule = null;

                foreach (ProcessModule m in Process.GetCurrentProcess().Modules)
                    if (m.ModuleName == module)
                        procModule = m;

                if (procModule == null)
                    throw new LunaException($"Can't find Module '{module}'");

                IntPtr addrModule = WinImports.LoadLibraryEx(procModule.FileName, IntPtr.Zero,
                WinImports.LoadLibraryFlags.LoadWithAlteredSearchPath);

                if (addrModule == IntPtr.Zero)
                    throw new LunaException($"Can't load Module '{module}'");

                m_callCreateInterface = Native.GetExportFunction<Native.CreateInterface>(addrModule, "CreateInterface");

                if (m_callCreateInterface == null)
                    throw new LunaException($"Interface wasn't created - Module '{module}'");

                IntPtr address = m_callCreateInterface(InterfaceVersions.GetInterfaceIdentifier(typeof(TClass)),
                IntPtr.Zero);

                if (address == IntPtr.Zero)
                    throw new LunaException($"Interface Identifier failed - Module '{module}'");

                if (offset > 0) address = address + offset;


                TClass result = new TClass();
                result.SetupFunctions(address);

                return result;
            }
            catch (LunaException)
            {
                return default(TClass);
            }
        }

        #endregion

        #region Nested Types, Enums, Delegates

        internal struct Native
        {
            #region Static Methods

            public static TDelegate GetExportFunction<TDelegate>(IntPtr module, string name) where TDelegate : class
            {
                try
                {
                    IntPtr address = WinImports.GetProcAddress(module, name);

                    if (address == IntPtr.Zero)
                        throw new LunaException($"Can't get Module '{name}' Address");

                    return (TDelegate)(object)Marshal.GetDelegateForFunctionPointer(address, typeof(TDelegate));
                }
                catch (LunaException)
                {
                    return null;
                }
            }

            #endregion

            #region Nested Types, Enums, Delegates

            [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
            public delegate IntPtr CreateInterface(string version, IntPtr returnCode);

            #endregion
        }

        #endregion
    }
}
