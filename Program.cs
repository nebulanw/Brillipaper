using Gh.Common.Utilities;
using Gh.Walliant.Forms;
using Gh.Walliant.Utilities;
using Gh.Walliant.Wallpaper;
using Gh.Walliant.Wallpaper.Impl.Services;
using System;
using System.Windows.Forms;

namespace Gh.Walliant
{
  internal static class Program
  {
    [STAThread]
    private static void Main()
    {
      new Runner("walliant", "Local\\{E8C20855-465C-411B-A0F9-6897240D9EB3}", "563f6dee0d52d0dd7c1a7cc4b9df5626364d2d5b", "https://25e5cccd5e35435e9399507e39a52b6d@o357035.ingest.sentry.io/5209089", (ConfigBase) Config.Instance).Run(new Runner.FormCreator(Program.CreateForm), new Warning()
      {
        Title = "Do you want to disable web indexing?",
        Text = "Without web indexing, the best wallpaper views won’t be available.\r\nKeep it enabled to enjoy the app FOR FREE!"
      });
    }

    private static Form CreateForm()
    {
      return (Form) new MainForm(Gh.Walliant.Utilities.Host.Modern ? (IChanger) new Gh.Walliant.Wallpaper.Impl.Changers.Modern.Changer() : (IChanger) new Gh.Walliant.Wallpaper.Impl.Changers.Legacy.Changer(), (IFactory) new Factory());
    }
  }
}
