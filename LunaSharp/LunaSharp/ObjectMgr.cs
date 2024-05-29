using LunaSharp.LunaSharp.Entities;
using LunaSharp.LunaSharpKernel;
using LunaSharp.LunaSharpKernel.EntitySystem;
using LunaSharp.LunaSharpKernel.Offsets;
using System;

namespace LunaSharp.LunaSharp
{
    public static class ObjectMgr
    {
        #region Properties


        public static Player LocalPlayer
        {
            get
            {
                IntPtr localPlayerPtr = EntitySystem.GetLocalPlayerPtr();
                if(localPlayerPtr == IntPtr.Zero)
                {
                    throw new LunaException("Could not found localPlayerPtr");
                }
                var player =  (Player)CacheObject.Instance.CreateGet(localPlayerPtr);
                return player;
            }
        }

        #endregion

        #region Static Methods
        internal static Entity CreateEntityFromPointer(IntPtr entityPointer)
        {
            return null;
        }
        #endregion
    }
}
