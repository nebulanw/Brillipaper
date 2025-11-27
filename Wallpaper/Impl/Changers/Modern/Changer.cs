using System.Drawing;
using System.IO;

namespace Gh.Walliant.Wallpaper.Impl.Changers.Modern
{
  internal class Changer : IChanger
  {
    public Image Image
    {
      get
      {
        string wallpaper;
        ((IDesktopWallpaper) new DesktopWallpaper()).GetWallpaper((string) null, out wallpaper);
        return !string.IsNullOrEmpty(wallpaper) ? Image.FromFile(wallpaper) : (Image) null;
      }
      set
      {
        IDesktopWallpaper desktopWallpaper = (IDesktopWallpaper) new DesktopWallpaper();
        if (value != null)
        {
          string tempFileName = Path.GetTempFileName();
          value.Save(tempFileName);
          desktopWallpaper.SetWallpaper((string) null, tempFileName);
        }
        else
          desktopWallpaper.Enable(false);
      }
    }

    public Style Style
    {
      get
      {
        DesktopWallpaperPosition position;
        ((IDesktopWallpaper) new DesktopWallpaper()).GetPosition(out position);
        return (Style) position;
      }
      set
      {
        ((IDesktopWallpaper) new DesktopWallpaper()).SetPosition((DesktopWallpaperPosition) value);
      }
    }
  }
}
