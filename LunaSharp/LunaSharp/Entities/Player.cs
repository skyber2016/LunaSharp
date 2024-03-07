using LunaSharp.LunaSharpKernel.Offsets;
using System;

namespace LunaSharp.LunaSharp.Entities
{
    public class Player : Entity
    {
        internal Player(IntPtr entityPointer)
        {
            this.EntityPointer = MemoryManager.ReadPointer(entityPointer);
        }

        public int Health() => MemoryManager.ReadInt(EntityPointer + (int)PlayerOffsets.m_Health);
    }
}
