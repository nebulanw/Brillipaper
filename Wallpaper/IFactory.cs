namespace Gh.Walliant.Wallpaper
{
  internal interface IFactory
  {
    IService Create(Provider type);
  }
}
