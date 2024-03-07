using System;

namespace LunaSharp.LunaSharpKernel.EntitySystem
{
    public interface ICacheService<out TObjectType>
    {
        #region Public Methods

        TObjectType CreateGet(IntPtr baseEntity);

        void Remove(IntPtr pointer);

        #endregion
    }
}
