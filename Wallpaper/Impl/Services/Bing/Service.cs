using Gh.Common.Utilities;
using System;
using System.Collections.Specialized;
using System.Net;

namespace Gh.Walliant.Wallpaper.Impl.Services.Bing
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
      client.BaseAddress = this.baseAddress;
      client.QueryString = new NameValueCollection()
      {
        {
          "format",
          "js"
        },
        {
          "idx",
          "0"
        },
        {
          "mkt",
          "en-US"
        },
        {
          "n",
          "1"
        }
      };
      Uri address = new Uri("/HPImageArchive.aspx", UriKind.Relative);
      return client.DownloadString(address);
    }

    private MetaData Parse(string response)
    {
      Uri uri = new Uri(new Uri(this.baseAddress), Serializer.Deserialize<ResponseInfo>(response).images[0].url);
      return new MetaData() { Uri = uri };
    }
  }
}
