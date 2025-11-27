using System.Drawing;

namespace Gh.Walliant.Wallpaper
{
  internal interface IChanger
  {
    Image Image { get; set; }

    Style Style { get; set; }
  }
}
