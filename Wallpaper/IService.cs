namespace Gh.Brillipaper.Wallpaper
{
  internal interface IService
  {
    MetaData Locate();

    ImageData Retrieve(MetaData meta);
  }
}
