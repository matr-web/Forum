using Newtonsoft.Json;
using System.Text;

namespace Forum.Tests.Helpers;
public static class HttpContentHelper
{
    /// <summary>
    /// A extension method for the object type, that serializes the object into a StringContent type.
    /// </summary>
    public static HttpContent ToJsonHttpContent(this object obj)
    {
        var json = JsonConvert.SerializeObject(obj);

        var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

        return httpContent;
    }
}
