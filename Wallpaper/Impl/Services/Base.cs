using System.Drawing;
using System.IO;
using System.Net;

namespace Gh.Brillipaper.Wallpaper.Impl.Services
{
  internal abstract class Base : IService
  {
    public abstract MetaData Locate();

    public ImageData Retrieve(MetaData meta)
    {
      using (WebClient webClient = new WebClient())
      {
        using (Stream stream = webClient.OpenRead(meta.Uri))
        {
          Image image = Image.FromStream(stream);
          return new ImageData() { Image = image };
        }
      }
    }
  }
}
