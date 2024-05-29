using System;

namespace LunaSharp.LunaSharp.Entities
{
    public class BaseEntity
    {
        #region Fields

        protected IntPtr EntityPointer;

        #endregion

        #region Constructors

        internal BaseEntity(IntPtr entityPointer) => EntityPointer = entityPointer;

        public BaseEntity()
        {
        }

        #endregion
    }
}
