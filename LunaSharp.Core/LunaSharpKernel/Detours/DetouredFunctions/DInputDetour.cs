using LunaSharp.Core.Utils;
using LunaSharpKernel.Detours;
using SharpDX.Direct3D9;
using SharpDX.DirectInput;
using System;
using System.Runtime.InteropServices;

namespace LunaSharp.Core.LunaSharpKernel.Detours.DetouredFunctions
{
    internal class DInputDetour
    {
        private static DInputDetour _instance { get; set; }
        public static DInputDetour Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DInputDetour();
                }
                return _instance;
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
        public delegate int GetDeviceStateDelegate(IntPtr hDevice, int cbData, IntPtr lpvData);

        [StructLayout(LayoutKind.Sequential)]
        public struct LPDIMOUSESTATE
        {
            public int lX;
            public int lY;
            public int lZ;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public int[] rgbButtons;
        }


        public IntPtr OrigAddr = IntPtr.Zero;
        public GetDeviceStateDelegate Hook_orig;

        public event GetDeviceStateDelegate GetDeviceState;

        public bool BlockInput = false;

        private IntPtr Handle;

        public IntPtr WindowHandle   // property
        {
            get { return Handle; }   // get Handle
            set { Handle = value; }  // set Handle
        }
        private string FunctionName = "WinProc:DInputHook";
        public  void CreateHook()
        {
            IntPtr getDeviceStatePtr = GetVTableAdress(9);
            Hook_orig = Marshal.GetDelegateForFunctionPointer<GetDeviceStateDelegate>(getDeviceStatePtr);
            GetDeviceStateDelegate newFunction = new GetDeviceStateDelegate(GetDeviceStateHooked);
            DetoursMgr.GetDetours.Create(Hook_orig, newFunction, FunctionName);
        }

        private int GetDeviceStateHooked(IntPtr hDevice, int cbData, IntPtr lpvData)
        {
            if (GetDeviceState != null)
            {
                return GetDeviceState.Invoke(hDevice, cbData, lpvData);
            }
            else
            {
                return this.CallOrgin(hDevice, cbData, lpvData);
            }
        }

        public int CallOrgin(IntPtr hDevice, int cbData, IntPtr lpvData)
        {
            return (int)DetoursMgr.GetDetours[FunctionName].CallOriginal(hDevice, cbData, lpvData);
        }

        public IntPtr GetVTableAdress(int Funtion_Index)
        {
            IntPtr Result = IntPtr.Zero;
            var adapter = new DirectInput();
            var devices = adapter.GetDevices(DeviceClass.All, DeviceEnumerationFlags.AttachedOnly);

            if (devices.Count <= 0)
                return IntPtr.Zero;

            using (var joystick = new Joystick(adapter, devices[0].InstanceGuid))
            {
                IntPtr vTable = joystick.NativePointer;
                IntPtr entry = Helper.GetVTblAddresses(vTable, Funtion_Index + 1)[Funtion_Index];

                Result = entry;
            }
            return Result;
        }

        public LPDIMOUSESTATE GetMouseData(IntPtr lpvData)
        {
            return Marshal.PtrToStructure<LPDIMOUSESTATE>(lpvData);
        }
    }
}
