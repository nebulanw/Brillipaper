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
      client.BaseAddress = "https://arc.msn.com/";
      WebClient webClient = client;
      NameValueCollection nameValueCollection = new NameValueCollection();
      nameValueCollection.Add("pid", "338387");
      nameValueCollection.Add("fmt", "json");
      nameValueCollection.Add("rafb", "0");
      nameValueCollection.Add("cdm", "1");
      nameValueCollection.Add("disphorzres", "1920");
      nameValueCollection.Add("dispvertres", "1080");
      nameValueCollection.Add("lo", "80217");
      nameValueCollection.Add("pl", "en-US");
      nameValueCollection.Add("lc", "en-US");
      nameValueCollection.Add("ctry", "US");
      DateTime dateTime = DateTime.UtcNow;
      dateTime = dateTime.Date;
      nameValueCollection.Add("time", dateTime.ToString("yyyy-MM-ddTHH:mm:ssK", (IFormatProvider) CultureInfo.InvariantCulture));
      webClient.QueryString = nameValueCollection;
      Uri address = new Uri("/v3/Delivery/Placement", UriKind.Relative);
      return client.DownloadString(address);
    }

    private MetaData Parse(string response)
    {
      Uri uri = new Uri(Serializer.Deserialize<EntryInfo>(Serializer.Deserialize<ResponseInfo>(response).batchrsp.items[0].item).ad.image_fullscreen_001_landscape.u);
      return new MetaData() { Uri = uri };
    }
  }
}
