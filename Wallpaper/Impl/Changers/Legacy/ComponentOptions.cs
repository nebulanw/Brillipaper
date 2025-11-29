using System.Runtime.InteropServices;

namespace Gh.Brillipaper.Wallpaper.Impl.Changers.Legacy
{
  internal struct ComponentOptions
  {
    [MarshalAs(UnmanagedType.U4)]
    public uint Size;
    [MarshalAs(UnmanagedType.Bool)]
    public bool EnableComponents;
    [MarshalAs(UnmanagedType.Bool)]
    public bool ActiveDesktop;
  }
}