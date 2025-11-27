using System;

namespace Gh.Walliant.Wallpaper.Impl.Changers.Legacy
{
  [Flags]
  internal enum ApplyFlag
  {
    Save = 1,
    HtmlGen = 2,
    Refresh = 4,
    All = Refresh | HtmlGen | Save, // 0x00000007
    Force = 8,
    BufferedRefresh = 16, // 0x00000010
    DynamicRefresh = 32, // 0x00000020
  }
}
