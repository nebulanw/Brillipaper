using System.Runtime.InteropServices;

namespace Gh.Brillipaper.Wallpaper.Impl.Changers.Legacy
{
  internal struct Component
  {
    [MarshalAs(UnmanagedType.U4)]
    public uint Size;
    [MarshalAs(UnmanagedType.U4)]
    public uint ID;
    [MarshalAs(UnmanagedType.I4)]
    public int ComponentType;
    [MarshalAs(UnmanagedType.Bool)]
    public bool Checked;
    [MarshalAs(UnmanagedType.Bool)]
    public bool Dirty;
    [MarshalAs(UnmanagedType.Bool)]
    public bool NoScroll;
    [MarshalAs(UnmanagedType.Struct)]
    private ComponentPosition Position;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
    public string FriendlyName;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2084)]
    public string Source;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2084)]
    public string SubscribedURL;
    [MarshalAs(UnmanagedType.U4)]
    public uint CurrentItemState;
    [MarshalAs(UnmanagedType.Struct)]
    private ComponentStateInfo Original;
    [MarshalAs(UnmanagedType.Struct)]
    private ComponentStateInfo Restored;
  }
}
