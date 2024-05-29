using System;

namespace LunaSharp.Utils
{
    internal static class ThrowUtils
    {
        public static InvalidOperationException DeviceNotInitialized()
        {
            return new InvalidOperationException("The DirectX device is not initialized");
        }

        public static InvalidOperationException UseBeginScene()
        {
            return new InvalidOperationException("Use BeginScene before drawing anything");
        }
    }
}
