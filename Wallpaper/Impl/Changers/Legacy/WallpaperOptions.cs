using System.Runtime.InteropServices;

namespace Gh.Walliant.Wallpaper.Impl.Changers.Legacy
{
  internal struct WallpaperOptions
  {
    [MarshalAs(UnmanagedType.U4)]
    public uint Size;
    [MarshalAs(UnmanagedType.U4)]
    public WallpaperStyle Style;
  }
  }
