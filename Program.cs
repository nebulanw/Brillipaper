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
      new Runner("walliant", "Local\\{E8C20855-465C-411B-A0F9-6897240D9EB3}", (ConfigBase) Config.Instance).Run(new Runner.FormCreator(Program.CreateForm));
    }

    private static Form CreateForm()
    {
      return (Form) new MainForm(Gh.Walliant.Utilities.Host.Modern ? (IChanger) new Gh.Walliant.Wallpaper.Impl.Changers.Modern.Changer() : (IChanger) new Gh.Walliant.Wallpaper.Impl.Changers.Legacy.Changer(), (IFactory) new Factory());
    }
  }
}
