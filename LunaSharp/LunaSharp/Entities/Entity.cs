using LunaSharp.LunaSharpKernel.Offsets;
using System;
using System.Numerics;

namespace LunaSharp.LunaSharp.Entities
{
    public class Entity : BaseEntity
    {
        internal Entity(IntPtr entityPointer) : base(entityPointer)
        {
            
        }

        public Entity() { }


        public Vector3 NetworkOrigin => MemoryManager.ReadVec(EntityPointer + (int)EntityOffsets.VecNetworkOrigin);


        #region Internal Methods

        internal IntPtr BasePointer() => EntityPointer;

        internal float PossitionX() => NetworkOrigin.X;

        internal float PossitionY() => NetworkOrigin.Y;

        internal float PossitionZ() => NetworkOrigin.Z;

        #endregion
    }
}
