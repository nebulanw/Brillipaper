using Gh.Common.Utilities;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Net;

namespace Gh.Walliant.Wallpaper.Impl.Services.Spotlight
{
  internal class Service : Base
  {
    public override MetaData Locate()
    {
      using (WebClient client = new WebClient())
        return this.Parse(this.Query(client));
    }

    private string Query(WebClient client)
    {
      return client.DownloadString("https://fd.api.iris.microsoft.com/v4/api/selection?placement=88000820&bcnt=1&country=US&locale=en-US&fmt=json");
    }

    private MetaData Parse(string response)
    {
      Uri uri = new Uri(Serializer.Deserialize<EntryInfo>(Serializer.Deserialize<ResponseInfo>(response).batchrsp.items[0].item).ad.landscapeImage.asset);
      return new MetaData() { Uri = uri };
    }
  }
}
