using System.Runtime.InteropServices;

namespace Gh.Walliant.Wallpaper.Impl.Changers.Legacy
{
  internal struct ComponentStateInfo
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
    [MarshalAs(UnmanagedType.U4)]
    public uint ItemState;
  }
}
