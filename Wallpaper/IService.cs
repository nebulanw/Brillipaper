namespace Gh.Walliant.Wallpaper
{
  internal interface IService
  {
    MetaData Locate();

    ImageData Retrieve(MetaData meta);
  }
}
