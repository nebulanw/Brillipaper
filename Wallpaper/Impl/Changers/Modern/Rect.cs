using System.Runtime.InteropServices;

namespace Gh.Brillipaper.Wallpaper.Impl.Changers.Modern
{
  internal struct Rect
  {
    [MarshalAs(UnmanagedType.I4)]
    public int Left;
    [MarshalAs(UnmanagedType.I4)]
    public int Top;
    [MarshalAs(UnmanagedType.I4)]
    public int Right;
    [MarshalAs(UnmanagedType.I4)]
    public int Bottom;
  }
}
