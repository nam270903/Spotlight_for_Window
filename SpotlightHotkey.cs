using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace SpotlightClone
{
    public static class SpotlightHotkey
    {
        private const int HOTKEY_ID = 9000;
        private const uint MOD_ALT = 0x0001;
        private const uint VK_SPACE = 0x20;

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public static void Register(Window window)
        {
            var helper = new WindowInteropHelper(window);
            helper.EnsureHandle();
            RegisterHotKey(helper.Handle, HOTKEY_ID, MOD_ALT, VK_SPACE);

            ComponentDispatcher.ThreadFilterMessage += (ref MSG msg, ref bool handled) =>
            {
                if (msg.message == 0x0312 && msg.wParam.ToInt32() == HOTKEY_ID)
                {
                    ((MainWindow)Application.Current.MainWindow).ToggleVisibility();
                    handled = true;
                }
            };
        }
    }
}
