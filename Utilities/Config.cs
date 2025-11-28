using Gh.Common.Utilities;
using Gh.Walliant.Wallpaper;
using System;
using System.Configuration;

namespace Gh.Walliant.Utilities
{
  internal sealed class Config : ConfigBase
  {
    private Config()
    {
    }

    public static Config Instance { get; } = new Config();

    [UserScopedSetting]
    [DefaultSettingValue("True")]
    public bool AppEnabled
    {
      get => (bool) this[nameof (AppEnabled)];
      set => this[nameof (AppEnabled)] = (object) value;
    }

    [UserScopedSetting]
    [DefaultSettingValue("True")]
    public bool Active
    {
      get => (bool) this[nameof (Active)];
      set => this[nameof (Active)] = (object) value;
    }

    [UserScopedSetting]
    [DefaultSettingValue("")]
    public DateTime LastRefresh
    {
      get => (DateTime) this[nameof (LastRefresh)];
      set => this[nameof (LastRefresh)] = (object) value;
    }

    public DateTime NextRefresh
    {
      get
      {
        DateTime dateTime = this.LastRefresh;
        dateTime = dateTime.Date;
        return dateTime.AddDays(1.0);
      }
    }

    [UserScopedSetting]
    [DefaultSettingValue("Spotlight")]
    public Provider Service
    {
      get => !Host.Supported ? Provider.Bing : (Provider) this[nameof (Service)];
      set => this[nameof (Service)] = (object) value;
    }
  }
}
