using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Gh.Brillipaper.Properties
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (Gh.Brillipaper.Properties.Resources.resourceMan == null)
          Gh.Brillipaper.Properties.Resources.resourceMan = new ResourceManager("Gh.Brillipaper.Properties.Resources", typeof (Gh.Brillipaper.Properties.Resources).Assembly);
        return Gh.Brillipaper.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => Gh.Brillipaper.Properties.Resources.resourceCulture;
      set => Gh.Brillipaper.Properties.Resources.resourceCulture = value;
    }

    internal static Icon logo_off
    {
      get => (Icon) Gh.Brillipaper.Properties.Resources.ResourceManager.GetObject(nameof (logo_off), Gh.Brillipaper.Properties.Resources.resourceCulture);
    }

    internal static Icon logo_on
    {
      get => (Icon) Gh.Brillipaper.Properties.Resources.ResourceManager.GetObject(nameof (logo_on), Gh.Brillipaper.Properties.Resources.resourceCulture);
    }
  }
}
