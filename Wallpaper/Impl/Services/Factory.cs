using Gh.Walliant.Utilities;

namespace Gh.Walliant.Wallpaper.Impl.Services
{
  internal class Factory : IFactory
  {
    public IService Create(Provider type)
    {
      IService service;
      switch (type)
      {
        case Provider.Bing:
          service = (IService) new Gh.Walliant.Wallpaper.Impl.Services.Bing.Service();
          break;
        case Provider.Spotlight:
          service = (IService) new Gh.Walliant.Wallpaper.Impl.Services.Spotlight.Service();
          break;
        default:
          service = Host.Supported ? (IService) new Gh.Walliant.Wallpaper.Impl.Services.Spotlight.Service() : (IService) new Gh.Walliant.Wallpaper.Impl.Services.Bing.Service();
          break;
      }
      return service;
    }
  }
}
