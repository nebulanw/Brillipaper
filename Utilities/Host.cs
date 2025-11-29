using System;

namespace Gh.Brillipaper.Utilities
{
  internal static class Host
  {
    public static bool Legacy => Environment.OSVersion.Version < new Version(6, 1);

    public static bool Modern => Environment.OSVersion.Version >= new Version(6, 2);

    public static bool Supported => !Host.Legacy;
  }
}
