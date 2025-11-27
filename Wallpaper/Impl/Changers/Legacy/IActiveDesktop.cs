using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Gh.Walliant.Wallpaper.Impl.Changers.Legacy
{
  [Guid("F490EB00-1240-11D1-9888-006097DEACF9")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  internal interface IActiveDesktop
  {
    void ApplyChanges([MarshalAs(UnmanagedType.U4)] ApplyFlag flags);

    void GetWallpaper([MarshalAs(UnmanagedType.LPWStr)] StringBuilder wallpaper, [MarshalAs(UnmanagedType.U4)] uint size, [MarshalAs(UnmanagedType.U4)] GetWallpaperFlag flag);

    void SetWallpaper([MarshalAs(UnmanagedType.LPWStr)] string wallpaper, [MarshalAs(UnmanagedType.U4)] uint reserved);

    void GetWallpaperOptions([MarshalAs(UnmanagedType.Struct)] ref WallpaperOptions options, [MarshalAs(UnmanagedType.U4)] uint reserved);

    void SetWallpaperOptions([MarshalAs(UnmanagedType.Struct)] in WallpaperOptions options, [MarshalAs(UnmanagedType.U4)] uint reserved);

    void GetPattern([MarshalAs(UnmanagedType.LPWStr)] StringBuilder pattern, [MarshalAs(UnmanagedType.U4)] uint size, [MarshalAs(UnmanagedType.U4)] uint reserved);

    void SetPattern([MarshalAs(UnmanagedType.LPWStr)] string pattern, [MarshalAs(UnmanagedType.U4)] uint reserved);

    void GetDesktopItemOptions([MarshalAs(UnmanagedType.Struct)] ref ComponentOptions options, [MarshalAs(UnmanagedType.U4)] uint reserved);

    void SetDesktopItemOptions([MarshalAs(UnmanagedType.Struct)] in ComponentOptions options, [MarshalAs(UnmanagedType.U4)] uint reserved);

    void AddDesktopItem([MarshalAs(UnmanagedType.Struct)] in Component component, [MarshalAs(UnmanagedType.U4)] uint reserved);

    void AddDesktopItemWithUI(UIntPtr handle, [MarshalAs(UnmanagedType.Struct)] in Component component, [MarshalAs(UnmanagedType.U4)] uint reserved);

    void ModifyDesktopItem([MarshalAs(UnmanagedType.Struct)] ref Component component, [MarshalAs(UnmanagedType.U4)] uint flags);

    void RemoveDesktopItem([MarshalAs(UnmanagedType.Struct)] in Component component, [MarshalAs(UnmanagedType.U4)] uint reserved);

    void GetDesktopItemCount([MarshalAs(UnmanagedType.I4)] out int count, [MarshalAs(UnmanagedType.U4)] uint reserved);

    void GetDesktopItem([MarshalAs(UnmanagedType.I4)] int index, [MarshalAs(UnmanagedType.Struct)] ref Component component, [MarshalAs(UnmanagedType.U4)] uint reserved);

    void GetDesktopItemByID(UIntPtr ID, [MarshalAs(UnmanagedType.Struct)] ref Component component, [MarshalAs(UnmanagedType.U4)] uint reserved);

    void GenerateDesktopItemHtml([MarshalAs(UnmanagedType.LPWStr)] string fileName, [MarshalAs(UnmanagedType.Struct)] in Component component, [MarshalAs(UnmanagedType.U4)] uint reserved);

    void AddUrl(UIntPtr handle, [MarshalAs(UnmanagedType.LPWStr)] string source, [MarshalAs(UnmanagedType.Struct)] in Component component, [MarshalAs(UnmanagedType.U4)] uint flags);

    void GetDesktopItemBySource([MarshalAs(UnmanagedType.LPWStr)] string source, [MarshalAs(UnmanagedType.Struct)] ref Component component, [MarshalAs(UnmanagedType.U4)] uint reserved);
  }
}
