using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Gh.Walliant.Properties
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
        if (Gh.Walliant.Properties.Resources.resourceMan == null)
          Gh.Walliant.Properties.Resources.resourceMan = new ResourceManager("Gh.Walliant.Properties.Resources", typeof (Gh.Walliant.Properties.Resources).Assembly);
        return Gh.Walliant.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => Gh.Walliant.Properties.Resources.resourceCulture;
      set => Gh.Walliant.Properties.Resources.resourceCulture = value;
    }

    internal static Icon logo_off
    {
      get => (Icon) Gh.Walliant.Properties.Resources.ResourceManager.GetObject(nameof (logo_off), Gh.Walliant.Properties.Resources.resourceCulture);
    }

    internal static Icon logo_on
    {
      get => (Icon) Gh.Walliant.Properties.Resources.ResourceManager.GetObject(nameof (logo_on), Gh.Walliant.Properties.Resources.resourceCulture);
    }
  }
}
