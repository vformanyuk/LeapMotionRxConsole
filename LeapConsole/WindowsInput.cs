using System;
using System.Runtime.InteropServices;

namespace LeapConsole
{
    public class WindowsInput
    {
        // VK_LWIN (0x5b)
        // VK_TAB  (0x09)
        // VK_MENU (0x12) - ALT
        // VK_RIGHT (0x27)
        // VK_LEFT  (0x25)
        // VK_UP    (0x26)
        // VK_DOWN  (0x28)
        // VK_RETURN (0x0D)
        // VK_DELETE (0x2E)


        // Virtual keys: https://msdn.microsoft.com/ru-ru/library/windows/desktop/dd375731(v=vs.85).aspx
        // INPUT struct: https://msdn.microsoft.com/ru-ru/library/windows/desktop/ms646270(v=vs.85).aspx

        /// https://msdn.microsoft.com/ru-ru/library/windows/desktop/ms646273(v=vs.85).aspx
        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        /// https://msdn.microsoft.com/ru-ru/library/windows/desktop/ms646271(v=vs.85).aspx
        [StructLayout(LayoutKind.Sequential)]
        public struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;

        }

        [StructLayout(LayoutKind.Sequential)]
        public struct HARDWAREINPUT
        {
            public uint uMsg;
            public ushort wParamL;
            public ushort wParamH;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct INPUT
        {
            public int type;
            public InputBatch u;
        }

        // UNION
        [StructLayout(LayoutKind.Explicit)]
        public struct InputBatch
        {
            [FieldOffset(0)]
            public HARDWAREINPUT Hardware;
            [FieldOffset(0)]
            public KEYBDINPUT Keyboard;
            [FieldOffset(0)]
            public MOUSEINPUT Mouse;
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetMessageExtraInfo();

        private static readonly INPUT[] _winPlusTab = new INPUT[2]
            {
                new INPUT()
                {
                    type=1,
                    u=new InputBatch()
                    {
                        Keyboard=new KEYBDINPUT()
                        {
                            wVk=0x5b,
                            wScan=0,
                            dwFlags=0,
                            time=0,
                            dwExtraInfo=GetMessageExtraInfo()
                        }
                    }
                },
                new INPUT()
                {
                    type=1,
                    u=new InputBatch()
                    {
                        Keyboard=new KEYBDINPUT()
                        {
                            wVk=0x09,
                            wScan=0,
                            dwFlags=0,
                            time=0,
                            dwExtraInfo=GetMessageExtraInfo()
                        }
                    }
                }
            };
        private static readonly INPUT[] _winPlusD = new INPUT[2]
            {
                new INPUT()
                {
                    type=1,
                    u=new InputBatch()
                    {
                        Keyboard=new KEYBDINPUT()
                        {
                            wVk=0x5b,
                            wScan=0,
                            dwFlags=0,
                            time=0,
                            dwExtraInfo=GetMessageExtraInfo()
                        }
                    }
                },
                new INPUT()
                {
                    type=1,
                    u=new InputBatch()
                    {
                        Keyboard=new KEYBDINPUT()
                        {
                            wVk=0x44,
                            wScan=0,
                            dwFlags=0,
                            time=0,
                            dwExtraInfo=GetMessageExtraInfo()
                        }
                    }
                }
            };
        private static readonly INPUT[] _winPlusS = new INPUT[2]
            {
                new INPUT()
                {
                    type=1,
                    u=new InputBatch()
                    {
                        Keyboard=new KEYBDINPUT()
                        {
                            wVk=0x5b,
                            wScan=0,
                            dwFlags=0,
                            time=0,
                            dwExtraInfo=GetMessageExtraInfo()
                        }
                    }
                },
                new INPUT()
                {
                    type=1,
                    u=new InputBatch()
                    {
                        Keyboard=new KEYBDINPUT()
                        {
                            wVk=0x53,
                            wScan=0,
                            dwFlags=0,
                            time=0,
                            dwExtraInfo=GetMessageExtraInfo()
                        }
                    }
                }
            };
        private static readonly INPUT[] _winUp = new INPUT[1]
            {
                new INPUT()
                {
                    type = 1,
                    u = new InputBatch()
                    {
                        Keyboard = new KEYBDINPUT()
                        {
                            wVk = 0x5b,
                            wScan = 0,
                            dwFlags = 0x2,
                            time = 0,
                            dwExtraInfo = GetMessageExtraInfo()
                        }
                    }
                }
            };

        private static readonly INPUT[] _altPlusTab = new INPUT[2]
            {
                new INPUT()
                {
                    type=1,
                    u=new InputBatch()
                    {
                        Keyboard=new KEYBDINPUT()
                        {
                            wVk=0x12,
                            wScan=0,
                            dwFlags=0,
                            time=0,
                            dwExtraInfo=GetMessageExtraInfo()
                        }
                    }
                },
                new INPUT()
                {
                    type=1,
                    u=new InputBatch()
                    {
                        Keyboard=new KEYBDINPUT()
                        {
                            wVk=0x09,
                            wScan=0,
                            dwFlags=0,
                            time=0,
                            dwExtraInfo=GetMessageExtraInfo()
                        }
                    }
                }
            };
        private static readonly INPUT[] _altUp = new INPUT[1]
            {
                new INPUT()
                {
                    type = 1,
                    u = new InputBatch()
                    {
                        Keyboard = new KEYBDINPUT()
                        {
                            wVk = 0x12,
                            wScan = 0,
                            dwFlags = 0x2,
                            time = 0,
                            dwExtraInfo = GetMessageExtraInfo()
                        }
                    }
                }
            };

        private static readonly INPUT[] _tab = new INPUT[1]
            {
                new INPUT()
                {
                    type = 1,
                    u = new InputBatch()
                    {
                        Keyboard = new KEYBDINPUT()
                        {
                            wVk = 0x09,
                            wScan = 0,
                            dwFlags = 0x0,
                            time = 0,
                            dwExtraInfo = GetMessageExtraInfo()
                        }
                    }
                }
            };

        private static readonly INPUT[] _right = new INPUT[1]
            {
                new INPUT()
                {
                    type = 1,
                    u = new InputBatch()
                    {
                        Keyboard = new KEYBDINPUT()
                        {
                            wVk = 0x27,
                            wScan = 0,
                            dwFlags = 0x0,
                            time = 0,
                            dwExtraInfo = GetMessageExtraInfo()
                        }
                    }
                }
            };
        private static readonly INPUT[] _left = new INPUT[1]
            {
                new INPUT()
                {
                    type = 1,
                    u = new InputBatch()
                    {
                        Keyboard = new KEYBDINPUT()
                        {
                            wVk = 0x25,
                            wScan = 0,
                            dwFlags = 0x0,
                            time = 0,
                            dwExtraInfo = GetMessageExtraInfo()
                        }
                    }
                }
            };
        private static readonly INPUT[] _up = new INPUT[1]
            {
                new INPUT()
                {
                    type = 1,
                    u = new InputBatch()
                    {
                        Keyboard = new KEYBDINPUT()
                        {
                            wVk = 0x26,
                            wScan = 0,
                            dwFlags = 0x0,
                            time = 0,
                            dwExtraInfo = GetMessageExtraInfo()
                        }
                    }
                }
            };
        private static readonly INPUT[] _down = new INPUT[1]
            {
                new INPUT()
                {
                    type = 1,
                    u = new InputBatch()
                    {
                        Keyboard = new KEYBDINPUT()
                        {
                            wVk = 0x28,
                            wScan = 0,
                            dwFlags = 0x0,
                            time = 0,
                            dwExtraInfo = GetMessageExtraInfo()
                        }
                    }
                }
            };
        private static readonly INPUT[] _enter = new INPUT[1]
            {
                new INPUT()
                {
                    type = 1,
                    u = new InputBatch()
                    {
                        Keyboard = new KEYBDINPUT()
                        {
                            wVk = 0x0D,
                            wScan = 0,
                            dwFlags = 0x0,
                            time = 0,
                            dwExtraInfo = GetMessageExtraInfo()
                        }
                    }
                }
            };
        private static readonly INPUT[] _del = new INPUT[2]
            {
                new INPUT()
                {
                    type = 1,
                    u = new InputBatch()
                    {
                        Keyboard = new KEYBDINPUT()
                        {
                            wVk = 0x2E,
                            wScan = 0,
                            dwFlags = 0x00,
                            time = 0,
                            dwExtraInfo = GetMessageExtraInfo()
                        }
                    }
                },
                new INPUT()
                {
                    type = 1,
                    u = new InputBatch()
                    {
                        Keyboard = new KEYBDINPUT()
                        {
                            wVk = 0x2E,
                            wScan = 0,
                            dwFlags = 0x02,
                            time = 0,
                            dwExtraInfo = GetMessageExtraInfo()
                        }
                    }
                }
            };

        public static void SwitchDesktops()
        {
            SendInput((uint)_winPlusTab.Length, _winPlusTab, Marshal.SizeOf<INPUT>());
            SendInput((uint)_winUp.Length, _winUp, Marshal.SizeOf<INPUT>());
        }

        public static void ShowSwitchApplications()
        {
            SendInput((uint)_altPlusTab.Length, _altPlusTab, Marshal.SizeOf<INPUT>());
        }

        public static void GotoSearch()
        {
            SendInput((uint)_winPlusS.Length, _winPlusS, Marshal.SizeOf<INPUT>());
            SendInput((uint)_winUp.Length, _winUp, Marshal.SizeOf<INPUT>());
        }

        //public static void WinUp()
        //{
        //    SendInput((uint)_winUp.Length, _winUp, Marshal.SizeOf<INPUT>());
        //}

        public static void MinimizeAll()
        {
            SendInput((uint)_winPlusD.Length, _winPlusD, Marshal.SizeOf<INPUT>());
            SendInput((uint)_winUp.Length, _winUp, Marshal.SizeOf<INPUT>());
        }

        public static void Left()
        {
            WindowsInput.SendInput((uint)_left.Length, _left, Marshal.SizeOf<INPUT>());
        }

        public static void Right()
        {
            WindowsInput.SendInput((uint)_right.Length, _right, Marshal.SizeOf<INPUT>());
        }

        public static void Up()
        {
            WindowsInput.SendInput((uint)_up.Length, _up, Marshal.SizeOf<INPUT>());
        }

        public static void Down()
        {
            WindowsInput.SendInput((uint)_down.Length, _down, Marshal.SizeOf<INPUT>());
        }

        public static void Tab()
        {
            WindowsInput.SendInput((uint)_tab.Length, _tab, Marshal.SizeOf<INPUT>());
        }

        public static void Del()
        {
            WindowsInput.SendInput((uint)_del.Length, _del, Marshal.SizeOf<INPUT>());
        }

        public static void Enter()
        {
            WindowsInput.SendInput((uint)_enter.Length, _enter, Marshal.SizeOf<INPUT>());
        }

        public static void CloseSwitchApplication()
        {
            WindowsInput.SendInput((uint)_altUp.Length, _altUp, Marshal.SizeOf<INPUT>());
        }
    }
}
