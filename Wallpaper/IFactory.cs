namespace Gh.Brillipaper.Wallpaper
{
  internal interface IFactory
  {
    IService Create(Provider type);
  }
}
