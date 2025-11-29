using System.Runtime.InteropServices;

namespace Gh.Brillipaper.Wallpaper.Impl.Changers.Legacy
{
  internal struct ComponentPosition
  {
    [MarshalAs(UnmanagedType.U4)]
    public uint Size;
    [MarshalAs(UnmanagedType.I4)]
    public int Left;
    [MarshalAs(UnmanagedType.I4)]
    public int Top;
    [MarshalAs(UnmanagedType.U4)]
    public uint Width;
    [MarshalAs(UnmanagedType.U4)]
    public uint Height;
    [MarshalAs(UnmanagedType.I4)]
    public int Index;
    [MarshalAs(UnmanagedType.Bool)]
    public bool CanResize;
    [MarshalAs(UnmanagedType.Bool)]
    public bool CanResizeX;
    [MarshalAs(UnmanagedType.Bool)]
    public bool CanResizeY;
    [MarshalAs(UnmanagedType.I4)]
    public int PreferredLeftPercent;
    [MarshalAs(UnmanagedType.I4)]
    public int PreferredTopPercent;
  }
}
