using System;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using LunaSharp.Memory;

namespace LunaSharp
{
    internal static class MemoryManager
    {
        #region Static Methods

        internal static unsafe sbyte* StringToChar(string stringToChar)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(stringToChar);
            IntPtr temporaryMemory = MemoryTemp.GetTemporaryMemory((uint)(bytes.Length + 1));
            IntPtr ptr = temporaryMemory;
            Marshal.Copy(bytes, 0, temporaryMemory, bytes.Length);
            Marshal.WriteByte(ptr, bytes.Length, 0);

            return (sbyte*)ptr.ToPointer();
        }

        internal static IntPtr FindSignature(byte[] signature, string mask, string module, int size, int offset = 0)
        {
            SigScan scanMemory = new SigScan(Process.GetCurrentProcess(), GetModule(module), size);

            return scanMemory.FindPattern(signature, mask, offset);
        }

        internal static IntPtr Function(IntPtr pointer, int index = 0) =>
            ReadIntPtr(ReadIntPtr(pointer) + index * 0x04);

        internal static int ReadInt(IntPtr offset, int bytesToRead = 4) =>
            BitConverter.ToInt32(Read(offset, bytesToRead, out int _), 0);

        internal static IntPtr ReadIntPtr(IntPtr offset) => new IntPtr(ReadInt(offset));

        internal static float ReadFloat(IntPtr offset, int bytesToRead = 4) =>
            BitConverter.ToSingle(Read(offset, bytesToRead, out int _), 0);


        public static IntPtr ReadPointer(IntPtr addy)
        {
            var bytes = Read(addy, IntPtr.Size, out _);
            return (IntPtr)BitConverter.ToInt32(bytes, 0);
        }

        public static IntPtr ReadPointer(IntPtr addy, int offset)
        {
            var bytes = Read(addy + offset, IntPtr.Size, out _);
            return (IntPtr)BitConverter.ToInt32(bytes, 0);
        }

        public static IntPtr ReadPointer(IntPtr addy, int[] offsets)
        {
            IntPtr intPtr = addy;
            foreach (int offset in offsets)
            {
                intPtr = ReadPointer(intPtr, offset);
            }

            return intPtr;
        }

        public static IntPtr ReadPointer(IntPtr addy, IntPtr offset)
        {
            return ReadPointer(addy, new int[1] { (int)offset });
        }

        public static IntPtr ReadPointer(IntPtr addy, int offset1, int offset2)
        {
            return ReadPointer(addy, new int[2] { offset1, offset2 });
        }

        public static IntPtr ReadPointer(IntPtr addy, int offset1, int offset2, int offset3)
        {
            return ReadPointer(addy, new int[3] { offset1, offset2, offset3 });
        }

        public static IntPtr ReadPointer(IntPtr addy, int offset1, int offset2, int offset3, int offset4)
        {
            return ReadPointer(addy, new int[4] { offset1, offset2, offset3, offset4 });
        }

        public static IntPtr ReadPointer(IntPtr addy, int offset1, int offset2, int offset3, int offset4, int offset5)
        {
            return ReadPointer(addy, new int[5] { offset1, offset2, offset3, offset4, offset5 });
        }

        public static IntPtr ReadPointer(IntPtr addy, int offset1, int offset2, int offset3, int offset4, int offset5, int offset6)
        {
            return ReadPointer(addy, new int[6] { offset1, offset2, offset3, offset4, offset5, offset6 });
        }

        public static IntPtr ReadPointer(IntPtr addy, int offset1, int offset2, int offset3, int offset4, int offset5, int offset6, int offset7)
        {
            return ReadPointer(addy, new int[7] { offset1, offset2, offset3, offset4, offset5, offset6, offset7 });
        }
        public static byte[] ReadBytes(IntPtr addy, int len) => Read(addy, len, out _);

        public static byte[] ReadBytes(IntPtr addy, int offset, int len) => Read(addy + offset, len, out _);
        public static Vector3 ReadVec(IntPtr address)
        {
            byte[] value = ReadBytes(address, 12);
            Vector3 result = default(Vector3);
            result.X = BitConverter.ToSingle(value, 0);
            result.Y = BitConverter.ToSingle(value, 4);
            result.Z = BitConverter.ToSingle(value, 8);
            return result;
        }

        public static Vector3 ReadVec(IntPtr address, int offset)
        {
            byte[] value = ReadBytes(address + offset, 12);
            Vector3 result = default(Vector3);
            result.X = BitConverter.ToSingle(value, 0);
            result.Y = BitConverter.ToSingle(value, 4);
            result.Z = BitConverter.ToSingle(value, 8);
            return result;
        }

        internal static string ReadString(IntPtr offset, int bytesToRead = 128)
        {
            int endIndex = 0;
            byte[] buf = new byte[bytesToRead];
            buf = Read(offset, bytesToRead, out int _);

            for (int i = 0; i < buf.Length; i++)
                if (buf[i] == '\0')
                {
                    endIndex = i;
                    break;
                }

            return Encoding.ASCII.GetString(buf, 0, endIndex);
        }

        internal static T PtrToStructure<T>(IntPtr pointer)
        {
            return Marshal.PtrToStructure<T>(pointer);
        }

        private static IntPtr GetModule(string processModule)
        {
            ProcessModule module = null;

            foreach (ProcessModule proc in Process.GetCurrentProcess().Modules)
                if (proc.ModuleName == processModule)
                    module = proc;

            return WinImports.LoadLibraryEx(module.FileName, IntPtr.Zero,
                WinImports.LoadLibraryFlags.DontResolveDllReferences);
        }

        private static byte[] Read(IntPtr memoryAddress, int bytesToRead, out int bytesRead)
        {
            byte[] buffer = new byte[bytesToRead];
            WinImports.ReadProcessMemory(Process.GetCurrentProcess().Handle, memoryAddress, buffer, bytesToRead, out int ptrBytesRead);
            bytesRead = ptrBytesRead;

            return buffer;
        }



        #endregion
    }
}
