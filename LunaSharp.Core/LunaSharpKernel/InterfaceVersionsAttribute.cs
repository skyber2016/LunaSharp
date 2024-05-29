﻿using System;

namespace LunaSharp.LunaSharpKernel
{
    [AttributeUsage(AttributeTargets.Class)]
    internal class InterfaceVersionsAttribute : Attribute
    {
        #region Constructors

        public InterfaceVersionsAttribute(string versionIdentifier) => Identifier = versionIdentifier;

        #endregion

        #region Properties

        public string Identifier { get; set; }

        #endregion
    }
}
