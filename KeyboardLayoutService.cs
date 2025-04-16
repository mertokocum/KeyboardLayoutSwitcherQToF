using KeyboardLayoutSwitcherQToF.Services;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace KeyboardLayoutSwitcherQToF
{
    public class KeyboardLayoutService : IKeyboardLayoutService
    {
        private const uint KLF_ACTIVATE = 0x00000001;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr LoadKeyboardLayout(string pwszKLID, uint Flags);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool GetKeyboardLayoutName(StringBuilder pwszKLID);

        public void SetKeyboardLayout(string layoutHex)
        {
            // LoadKeyboardLayout kullanılarak istediğimiz düzen yükleniyor.
            IntPtr hkl = LoadKeyboardLayout(layoutHex, KLF_ACTIVATE);
            if (hkl == IntPtr.Zero)
            {
                int error = Marshal.GetLastWin32Error();
                throw new InvalidOperationException($"Klavye düzeni yüklenemedi. Hata kodu: {error}");
            }
        }

        public string DetectActiveLayout()
        {
            // GetKeyboardLayoutName, aktif klavyenin 8 haneli layout ID'sini verir.
            StringBuilder layoutName = new StringBuilder(9);
            if (GetKeyboardLayoutName(layoutName))
            {
                return layoutName.ToString().ToUpper();
            }
            else
            {
                throw new InvalidOperationException("Aktif klavye düzeni algılanamadı.");
            }
        }
    }
}
