using LunaSharp.Utils;
using System;
using System.Collections.Generic;

namespace LunaSharp
{
    public class Game
    {
        #region Fields
        private static readonly List<OnEventGameRender> OnEventGameRenderHandles;
        private static readonly List<OnEventGameSetupRender> OnEventGameSetupRenderHandles;
        #endregion

        #region Constructors

        static Game()
        {
            OnEventGameRenderHandles = new List<OnEventGameRender>();
            OnEventGameSetupRenderHandles = new List<OnEventGameSetupRender>();
        }

        #endregion

        #region Properties


        #endregion

        #region Other


        public static event OnEventGameRender OnEventGameRender
        {
            add => OnEventGameRenderHandles.Add(value);
            remove => OnEventGameRenderHandles.Remove(value);
        }
        

        public static event OnEventGameSetupRender OnEventGameSetupRender
        {
            add => OnEventGameSetupRenderHandles.Add(value);
            remove => OnEventGameSetupRenderHandles.Remove(value);
        }

        #endregion

        #region Static Methods

        internal static void OnGameRenderNative(IntPtr hdc)
        {
            try
            {
                var enumerator = OnEventGameRenderHandles.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var current = enumerator.Current;
                    try
                    {
                        current(hdc);
                    }
                    catch (Exception ex)
                    {
                        Logging.Write()("Failed Current Event OnGameRenderNative - " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Write()("Failed Events OnGameRenderNative - " + ex.Message);
            }
        }

        #endregion
    }
}
