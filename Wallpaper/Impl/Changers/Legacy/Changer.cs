using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Gh.Walliant.Wallpaper.Impl.Changers.Legacy
{
  internal class Changer : IChanger
  {
    public Image Image
    {
      get
      {
        IActiveDesktop activeDesktop = (IActiveDesktop) new ActiveDesktop();
        StringBuilder stringBuilder = new StringBuilder((int) short.MaxValue);
        StringBuilder wallpaper = stringBuilder;
        int capacity = stringBuilder.Capacity;
        activeDesktop.GetWallpaper(wallpaper, (uint) capacity, GetWallpaperFlag.Bmp);
        string filename = stringBuilder.ToString();
        return !string.IsNullOrEmpty(filename) ? Image.FromFile(filename) : (Image) null;
      }
      set
      {
        IActiveDesktop activeDesktop = (IActiveDesktop) new ActiveDesktop();
        string str = value != null ? Path.GetTempFileName() : string.Empty;
        value?.Save(str);
        activeDesktop.SetWallpaper(str, 0U);
        activeDesktop.ApplyChanges(ApplyFlag.All);
      }
    }

    public Style Style
    {
      get
      {
        IActiveDesktop activeDesktop = (IActiveDesktop) new ActiveDesktop();
        int num = Marshal.SizeOf(typeof (WallpaperOptions));
        WallpaperOptions wallpaperOptions = new WallpaperOptions()
        {
          Size = (uint) num
        };
        ref WallpaperOptions local = ref wallpaperOptions;
        activeDesktop.GetWallpaperOptions(ref local, 0U);
        return (Style) wallpaperOptions.Style;
      }
      set
      {
        IActiveDesktop activeDesktop = (IActiveDesktop) new ActiveDesktop();
        int num = Marshal.SizeOf(typeof (WallpaperOptions));
        activeDesktop.SetWallpaperOptions(new WallpaperOptions()
        {
          Size = (uint) num,
          Style = (WallpaperStyle) value
        }, 0U);
        activeDesktop.ApplyChanges(ApplyFlag.All);
      }
    }
  }
}
