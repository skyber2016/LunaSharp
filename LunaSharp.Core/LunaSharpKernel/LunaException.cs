using LunaSharp.Utils;
using System;

namespace LunaSharp.LunaSharpKernel
{
    internal sealed class LunaException : ApplicationException
    {
        #region Constructors

        internal LunaException(string msg) : base(msg)
        {
            Logging.Write($"CreateInterface")(Message);
        }

        #endregion
    }
}
