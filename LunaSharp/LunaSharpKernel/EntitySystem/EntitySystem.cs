using LunaSharpKernel.Misc;
using System;

namespace LunaSharp.LunaSharpKernel.EntitySystem
{
    internal static class EntitySystem
    {
        internal static IntPtr GetLocalPlayerPtr() => Signatures.LocalPlayer;
    }
}
