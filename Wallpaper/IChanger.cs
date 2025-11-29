using System.Drawing;

namespace Gh.Brillipaper.Wallpaper
{
  internal interface IChanger
  {
    Image Image { get; set; }

    Style Style { get; set; }
  }
}
