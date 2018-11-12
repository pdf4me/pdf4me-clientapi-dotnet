using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Pdf4meClient
{
    public partial class ImageClient
    {
        partial void UpdateJsonSerializerSettings(Newtonsoft.Json.JsonSerializerSettings settings)
        {
            settings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
        }
    }

    public static class Extension
    {
        //public static async Task<byte[]> OptimizeAsync(this LightClient pdfLightClient,  string profile, byte[] file, string fileName)
        //{

        //    var message = new HttpRequestMessage();
        //    var content = new MultipartFormDataContent();

        //    string controller = "Light/Optimize";
        //    string qparams = "profile=" + profile;

        //    string uriAddon = $"{controller}?{qparams}";

        //    using (var ms = new MemoryStream(file))
        //    {
        //        HttpClient client = Pdf4me.Instance.getApi();
                
        //        content.Add(new StreamContent(ms), "file", fileName);
                
        //        message.Method = HttpMethod.Post;
        //        message.Content = content;
        //        message.RequestUri = new Uri($"{client.BaseAddress.AbsoluteUri}{uriAddon}");


        //        var res = await client.SendAsync(message);

        //        //var optimizeRes = await res.Content.ReadAsAsync<OptimizeRes>();


        //        var fileStr = await res.Content.ReadAsStreamAsync();

        //        var msFile = new MemoryStream();
        //        await fileStr.CopyToAsync(msFile);

        //        var pobytes = msFile.ToArray();

        //        return pobytes;


        //        //return optimizeRes;

        //    }
            

        //}
               

    }
}
