using Gh.Brillipaper.Utilities;

namespace Gh.Brillipaper.Wallpaper.Impl.Services
{
  internal class Factory : IFactory
  {
    public IService Create(Provider type)
    {
      IService service;
      switch (type)
      {
        case Provider.Bing:
          service = (IService) new Gh.Brillipaper.Wallpaper.Impl.Services.Bing.Service();
          break;
        case Provider.Spotlight:
          service = (IService) new Gh.Brillipaper.Wallpaper.Impl.Services.Spotlight.Service();
          break;
        default:
          service = Host.Supported ? (IService) new Gh.Brillipaper.Wallpaper.Impl.Services.Spotlight.Service() : (IService) new Gh.Brillipaper.Wallpaper.Impl.Services.Bing.Service();
          break;
      }
      return service;
    }
  }
}
