using Gh.Common.Utilities;
using Gh.Brillipaper.Forms;
using Gh.Brillipaper.Utilities;
using Gh.Brillipaper.Wallpaper;
using Gh.Brillipaper.Wallpaper.Impl.Services;
using System;
using System.Windows.Forms;

namespace Gh.Brillipaper
{
  internal static class Program
  {
    [STAThread]
    private static void Main()
    {
      new Runner("brillipaper", "Local\\{E8C20855-465C-411B-A0F9-6897240D9EB3}", (ConfigBase) Config.Instance).Run(new Runner.FormCreator(Program.CreateForm));
    }

    private static Form CreateForm()
    {
      return (Form) new MainForm(Gh.Brillipaper.Utilities.Host.Modern ? (IChanger) new Gh.Brillipaper.Wallpaper.Impl.Changers.Modern.Changer() : (IChanger) new Gh.Brillipaper.Wallpaper.Impl.Changers.Legacy.Changer(), (IFactory) new Factory());
    }
  }
}
