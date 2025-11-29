using Gh.Common.Utilities;
using System;
using System.Collections.Specialized;
using System.Net;

namespace Gh.Brillipaper.Wallpaper.Impl.Services.Bing
{
  internal class Service : Base
  {
    private readonly string baseAddress = "https://www.bing.com/";

    public override MetaData Locate()
    {
      using (WebClient client = new WebClient())
        return this.Parse(this.Query(client));
    }

    private string Query(WebClient client)
    {
      return client.DownloadString("https://bing.com/hpimagearchive.aspx?format=js&mkt=en-US&n=1");
    }

    private MetaData Parse(string response)
    {
      Uri uri = new Uri(new Uri(this.baseAddress), Serializer.Deserialize<ResponseInfo>(response).images[0].urlbase + "_UHD.jpg");
      return new MetaData() { Uri = uri };
    }
  }
}
