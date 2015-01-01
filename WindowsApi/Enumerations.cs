using System;

namespace BorderlessGaming.WindowsApi
{
    [Flags]
    public enum WindowStyleFlags : uint
    {
        Overlapped = 0x00000000,
        Popup = 0x80000000,
        Child = 0x40000000,
        Minimize = 0x20000000,
        Visible = 0x10000000,
        Disabled = 0x08000000,
        ClipSiblings = 0x04000000,
        ClipChildren = 0x02000000,
        Maximize = 0x01000000,
        Border = 0x00800000,
        DialogFrame = 0x00400000,
        Vscroll = 0x00200000,
        Hscroll = 0x00100000,
        SystemMenu = 0x00080000,
        ThickFrame = 0x00040000,
        Group = 0x00020000,
        Tabstop = 0x00010000,

        MinimizeBox = 0x00020000,
        MaximizeBox = 0x00010000,

        Caption = Border | DialogFrame,
        Tiled = Overlapped,
        Iconic = Minimize,
        SizeBox = ThickFrame,
        TiledWindow = Overlapped,

        OverlappedWindow = Overlapped | Caption | SystemMenu | ThickFrame | MinimizeBox | MaximizeBox,
        ChildWindow = Child,

        ExtendedDlgmodalframe = 0x00000001,
        ExtendedNoparentnotify = 0x00000004,
        ExtendedTopmost = 0x00000008,
        ExtendedAcceptfiles = 0x00000010,
        ExtendedTransparent = 0x00000020,
        ExtendedMdichild = 0x00000040,
        ExtendedToolWindow = 0x00000080,
        ExtendedWindowEdge = 0x00000100,
        ExtendedClientEdge = 0x00000200,
        ExtendedContexthelp = 0x00000400,
        ExtendedRight = 0x00001000,
        ExtendedLeft = 0x00000000,
        ExtendedRtlreading = 0x00002000,
        ExtendedLtrreading = 0x00000000,
        ExtendedLeftscrollbar = 0x00004000,
        ExtendedRightscrollbar = 0x00000000,
        ExtendedControlParent = 0x00010000,
        ExtendedStaticEdge = 0x00020000,
        ExtendedAppWindow = 0x00040000,
        ExtendedOverlappedWindow = (ExtendedWindowEdge | ExtendedClientEdge),
        ExtendedPaletteWindow = (ExtendedWindowEdge | ExtendedToolWindow | ExtendedTopmost),
        ExtendedLayered = 0x00080000,
        ExtendedNoinheritlayout = 0x00100000,
        ExtendedLayoutRtl = 0x00400000,
        ExtendedComposited = 0x02000000,
        ExtendedNoactivate = 0x08000000
    }

    [Flags]
    public enum SetWindowPosFlags
    {
        AsyncWindowPos = 0x4000,
        DeferBase = 0x2000,
        DrawFrame = 0x0020,
        FrameChanged = 0x0020,
        HideWindow = 0x0080,
        NoActivate = 0x0010,
        NoCopyBits = 0x0100,
        NoMove = 0x0002,
        NoOwnerZOrder = 0x0200,
        NoReDraw = 0x0008,
        NoRePosition = 0x0200,
        NoSendChanging = 0x0400,
        NoSize = 0x0001,
        NoZOrder = 0x0004,
        ShowWindow = 0x0040
    }

    [Flags]
    public enum MenuFlags
    {
        ByPosition = 0x00000400,
        Remove = 0x00001000
    }

    public enum WindowLongIndex
    {
        ExtendedStyle = -20,
        HandleInstance = -6,
        HandleParent = -8,
        Identifier = -12,
        Style = -16,
        UserData = -21,
        WindowProc = -4
    }

    public enum SystemMetric
    {
        CXScreen = 0, // 0x00
        CYScreen = 1, // 0x01
        CXVScroll = 2, // 0x02
        CYHScroll = 3, // 0x03
        CYCaption = 4, // 0x04
        CXBorder = 5, // 0x05
        CYBorder = 6, // 0x06
        CXDlgFrame = 7, // 0x07
        CXFixedFrame = 7, // 0x07
        CYDlgFrame = 8, // 0x08
        CYFixedFrame = 8, // 0x08
        CYVThumb = 9, // 0x09
        CXHThumb = 10, // 0x0A
        CXIcon = 11, // 0x0B
        CYIcon = 12, // 0x0C
        CXCursor = 13, // 0x0D
        CYCursor = 14, // 0x0E
        CYMenu = 15, // 0x0F
        CXFullScreen = 16, // 0x10
        CYFullScreen = 17, // 0x11
        CYKanjiWindow = 18, // 0x12
        MousePresent = 19, // 0x13
        CYVScroll = 20, // 0x14
        CXHScroll = 21, // 0x15
        Debug = 22, // 0x16
        SwapButton = 23, // 0x17
        CXMinimum = 28, // 0x1C
        CYMinimum = 29, // 0x1D
        CXSize = 30, // 0x1E
        CYSize = 31, // 0x1F
        CXSizeFrame = 32, // 0x20
        CXFrame = 32, // 0x20
        CYSizeFrame = 33, // 0x21
        CYFrame = 33, // 0x21
        CXMinimumTrack = 34, // 0x22
        CYMinimumTrack = 35, // 0x23
        CXDoubleClick = 36, // 0x24
        CYDoubleClick = 37, // 0x25
        CXIconSpacing = 38, // 0x26
        CYIconSpacing = 39, // 0x27
        MenuDropAlignment = 40, // 0x28
        PenWindowS = 41, // 0x29
        DBCSEnabled = 42, // 0x2A
        CMouseButtonS = 43, // 0x2B
        Secure = 44, // 0x2C
        CXEdge = 45, // 0x2D
        CYEdge = 46, // 0x2E
        CXMinimumSpacing = 47, // 0x2F
        CYMinimumSpacing = 48, // 0x30
        CXSMIcon = 49, // 0x31
        CYSMIcon = 50, // 0x32
        CYSMCaption = 51, // 0x33
        CXSMSize = 52, // 0x34
        CYSMSize = 53, // 0x35
        CXMenuSize = 54, // 0x36
        CYMenuSize = 55, // 0x37
        ARRange = 56, // 0x38
        CXMinimized = 57, // 0x39
        CYMinimized = 58, // 0x3A
        CXMaxTrack = 59, // 0x3B
        CYMaxTrack = 60, // 0x3C
        CXMaxIMIZED = 61, // 0x3D
        CYMaxIMIZED = 62, // 0x3E
        NETWORK = 63, // 0x3F
        CLEANBOOT = 67, // 0x43
        CXDRAG = 68, // 0x44
        CYDRAG = 69, // 0x45
        SHOWSOUNDS = 70, // 0x46
        CXMenuCHECK = 71, // 0x47
        CYMenuCHECK = 72, // 0x48
        SLOWMACHINE = 73, // 0x49
        MIDEASTEnabled = 74, // 0x4A
        MouseWHEELPresent = 75, // 0x4B
        XVIRTUALScreen = 76, // 0x4C
        YVIRTUALScreen = 77, // 0x4D
        CXVIRTUALScreen = 78, // 0x4E
        CYVIRTUALScreen = 79, // 0x4F
        CMONITORS = 80, // 0x50
        SAMEDISPLAYFORMAT = 81, // 0x51
        IMMEnabled = 82, // 0x52
        CXFOCUSBorder = 83, // 0x53
        CYFOCUSBorder = 84, // 0x54
        TABLETPC = 86, // 0x56
        MEDIACENTER = 87, // 0x57
        STARTER = 88, // 0x58
        SERVERR2 = 89, // 0x59
        MouseHORIZONTALWHEELPresent = 91, // 0x5B
        CXPADDEDBorder = 92, // 0x5C
        DIGITIZER = 94, // 0x5E
        MaxIMUMTOUCHES = 95, // 0x5F

        REMOTESESSION = 0x1000, // 0x1000
        SHUTTINGDOWN = 0x2000, // 0x2000
        REMOTECONTROL = 0x2001, // 0x2001
    }

    /// <summary>Enumeration of the different ways of showing a window using 
    /// ShowWindow</summary>
    public enum WindowShowStyle : uint
    {
        /// <summary>Hides the window and activates another window.</summary>
        /// <remarks>See SW_HIDE</remarks>
        Hide = 0,
        /// <summary>Activates and displays a window. If the window is minimized 
        /// or maximized, the system restores it to its original size and 
        /// position. An application should specify this flag when displaying 
        /// the window for the first time.</summary>
        /// <remarks>See SW_SHOWNORMAL</remarks>
        ShowNormal = 1,
        /// <summary>Activates the window and displays it as a minimized window.</summary>
        /// <remarks>See SW_SHOWMINIMIZED</remarks>
        ShowMinimized = 2,
        /// <summary>Activates the window and displays it as a maximized window.</summary>
        /// <remarks>See SW_SHOWMAXIMIZED</remarks>
        ShowMaximized = 3,
        /// <summary>Maximizes the specified window.</summary>
        /// <remarks>See SW_MAXIMIZE</remarks>
        Maximize = 3,
        /// <summary>Displays a window in its most recent size and position. 
        /// This value is similar to "ShowNormal", except the window is not 
        /// actived.</summary>
        /// <remarks>See SW_SHOWNOACTIVATE</remarks>
        ShowNormalNoActivate = 4,
        /// <summary>Activates the window and displays it in its current size 
        /// and position.</summary>
        /// <remarks>See SW_SHOW</remarks>
        Show = 5,
        /// <summary>Minimizes the specified window and activates the next 
        /// top-level window in the Z order.</summary>
        /// <remarks>See SW_MINIMIZE</remarks>
        Minimize = 6,
          /// <summary>Displays the window as a minimized window. This value is 
          /// similar to "ShowMinimized", except the window is not activated.</summary>
        /// <remarks>See SW_SHOWMINNOACTIVE</remarks>
        ShowMinNoActivate = 7,
        /// <summary>Displays the window in its current size and position. This 
        /// value is similar to "Show", except the window is not activated.</summary>
        /// <remarks>See SW_SHOWNA</remarks>
        ShowNoActivate = 8,
        /// <summary>Activates and displays the window. If the window is 
        /// minimized or maximized, the system restores it to its original size 
        /// and position. An application should specify this flag when restoring 
        /// a minimized window.</summary>
        /// <remarks>See SW_RESTORE</remarks>
        Restore = 9,
        /// <summary>Sets the show state based on the SW_ value specified in the 
        /// STARTUPINFO structure passed to the CreateProcess function by the 
        /// program that started the application.</summary>
        /// <remarks>See SW_SHOWDEFAULT</remarks>
        ShowDefault = 10,
        /// <summary>Windows 2000/XP: Minimizes a window, even if the thread 
        /// that owns the window is hung. This flag should only be used when 
        /// minimizing windows from a different thread.</summary>
        /// <remarks>See SW_FORCEMINIMIZE</remarks>
        ForceMinimized = 11
    }
}