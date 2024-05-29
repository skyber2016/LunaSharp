using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace LunaSharp.Utils
{
    internal static class Logging
    {
        #region Static Methods

        public static WriteDelegate Write([CallerMemberName] string memberName = "") =>
            (value, args) =>
            {
                object finalMessage = value;
                if (args.Length > 0)
                    try
                    {
                        finalMessage = string.Format(value.ToString(), args);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{memberName} {ex.GetBaseException().Message}");
                    }

                Write(finalMessage.ToString(), memberName: memberName);
            };

        public static void Write(object value, object[] args, [CallerMemberName] string memberName = "")
        {
            object finalMessage = value;
            if (args.Length > 0)
                try
                {
                    finalMessage = string.Format(value.ToString(), args);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{memberName} {ex.GetBaseException().Message}");
                }

            Write(finalMessage.ToString(), memberName);
        }

        internal static void LogAllExceptions()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                if (args.ExceptionObject is Exception exception)
                {
                    Write()(exception.Message);
                }
            };
        }

        private static void Write(string message, string memberName)
        {
            string format = $"[{DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss")}]: [{memberName.PadLeft(30)}] -> {message}";
            Console.WriteLine(format);

            try
            {
                using (StreamWriter writer = new StreamWriter(Path.Combine(ApplicationInfo.PathRoot, "info.log"), true))
                {
                    writer.WriteLine(format);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{memberName} {ex.GetBaseException().Message}");
            }
        }

        #endregion

        #region Nested Types, Enums, Delegates

        public delegate void WriteDelegate(object value, params object[] args);

        #endregion
    }
}
