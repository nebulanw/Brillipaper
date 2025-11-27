using System;
using System.Runtime.InteropServices;

namespace Gh.Walliant.Wallpaper.Impl.Changers.Modern
{
  [Guid("B92B56A9-8B55-4E14-9A89-0199BBB6F93B")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  internal interface IDesktopWallpaper
  {
    void SetWallpaper([MarshalAs(UnmanagedType.LPWStr)] string monitorID, [MarshalAs(UnmanagedType.LPWStr)] string wallpaper);

    void GetWallpaper([MarshalAs(UnmanagedType.LPWStr)] string monitorID, [MarshalAs(UnmanagedType.LPWStr)] out string wallpaper);

    void GetMonitorDevicePathAt([MarshalAs(UnmanagedType.U4)] uint monitorIndex, [MarshalAs(UnmanagedType.LPWStr)] out string monitorID);

    void GetMonitorDevicePathCount(out uint count);

    void GetMonitorRECT([MarshalAs(UnmanagedType.LPWStr)] string monitorID, [MarshalAs(UnmanagedType.Struct)] out Rect displayRect);

    void SetBackgroundColor([MarshalAs(UnmanagedType.U4)] uint color);

    void GetBackgroundColor([MarshalAs(UnmanagedType.U4)] out uint color);

    void SetPosition([MarshalAs(UnmanagedType.I4)] DesktopWallpaperPosition position);

    void GetPosition([MarshalAs(UnmanagedType.I4)] out DesktopWallpaperPosition position);

    void SetSlideshow(IntPtr items);

    void GetSlideshow(out IntPtr items);

    void SetSlideshowOptions([MarshalAs(UnmanagedType.I4)] DesktopSlideshowOptions options, uint slideshowTick);

    void GetSlideshowOptions([MarshalAs(UnmanagedType.I4)] out DesktopSlideshowOptions options, out uint slideshowTick);

    void AdvanceSlideshow([MarshalAs(UnmanagedType.LPWStr)] string monitorID, [MarshalAs(UnmanagedType.I4)] DesktopSlideshowDirection direction);

    void GetStatus([MarshalAs(UnmanagedType.I4)] out DesktopSlideshowState state);

    void Enable([MarshalAs(UnmanagedType.Bool)] bool enable);
  }
}
