using System;
using System.Runtime.InteropServices;

namespace KeyboardLayoutSwitcherQToF.Services
{
    public interface IKeyboardLayoutService
    {

        void SetKeyboardLayout(string layoutHex);
        string DetectActiveLayout();
    }

    public class KeyboardLayoutService : IKeyboardLayoutService
    {
        private const uint KLF_ACTIVATE = 0x00000001;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr LoadKeyboardLayout(string pwszKLID, uint Flags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);

        [DllImport("user32.dll")]
        private static extern IntPtr GetKeyboardLayout(uint idThread);

        public void SetKeyboardLayout(string layoutHex)
        {
            IntPtr hkl = LoadKeyboardLayout(layoutHex, KLF_ACTIVATE); //hkl: handle of keyboard layout
            if (hkl == IntPtr.Zero)
            {
                int error = Marshal.GetLastWin32Error();
                throw new InvalidOperationException($"Klavye düzeni yüklenemedi. Hata kodu: {error}");
            }
        }

        public string DetectActiveLayout()
        {
            IntPtr hWnd = GetForegroundWindow();
            uint threadId = GetWindowThreadProcessId(hWnd, IntPtr.Zero);
            IntPtr hkl = GetKeyboardLayout(threadId);
            return ((uint)hkl & 0xFFFFFFFF).ToString("X8").ToUpper();
        }
    }
}
