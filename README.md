# Brillipaper!

A tiny wallpaper app for Windows, written in C#.

It's based on Walliant v1.0.16.0, but with all Globalhop SDK and update functionality removed. Inspired by Endermanch's original video ([YouTube link](https://www.youtube.com/watch?v=91w4rzBTP5o) / [Odysee backup](https://odysee.com/@endermanch:d/wallpaper-engine-virus-(+-removal):2)) covering Walliant.

## Changes

### Tracking

- Globalhop SDK removed
- Auto-updater removed
- Countly and Sentry removed

### Features

- Welcome notification for first startup
- "Change wallpapers daily" enabled by default
- Source and style submenus re-enabled
- Spotlight and Bing wallpaper sources updated to request 4K images

## Building from source

Assuming you've got Visual Studio with the .NET desktop environment workflow installed, you can clone this repo with submodules:

```
git clone --recurse-submodules https://github.com/nebulanw/Brillipaper.git 
```

This will also pull in the modified `Gh.Common` library at [https://github.com/nebulanw/Gh.Common](https://github.com/nebulanw/Gh.Common)

Before you build, you may need to go to Project > Manage NuGet Packages and click Restore to add the Newtonsoft.Json library.

If Visual Studio complains about `Gh.Common` references not being found, you'll need to remove the `Gh.Common` reference from the Brillipaper project and add it back.

You're now able to build by selecting Build > Build Solution.

