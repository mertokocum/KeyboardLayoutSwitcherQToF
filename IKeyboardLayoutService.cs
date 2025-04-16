using System;
using System.Runtime.InteropServices;

namespace KeyboardLayoutSwitcherQToF.Services
{
    public interface IKeyboardLayoutService
    {

        void SetKeyboardLayout(string layoutHex);
        string DetectActiveLayout();
    }

}
