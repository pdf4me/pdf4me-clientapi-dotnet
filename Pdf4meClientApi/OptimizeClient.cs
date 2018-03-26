using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Pdf4meClient
{
    public class OptimizeClient
    {

        private PdfClient _pdfClient;

        public OptimizeClient(PdfClient pdfClient)
        {
            _pdfClient = pdfClient;
        }

        public static async Task<byte[]> OptimizeAsync(OptimizeActionProfile profile, byte[] file, string fileName)
        {

            var message = new HttpRequestMessage();
            var content = new MultipartFormDataContent();

            string controller = "Light/Optimize";
            string qparams = "profile=" + profile;

            string uriAddon = $"{controller}?{qparams}";

            using (var ms = new MemoryStream(file))
            {
                HttpClient client = Pdf4me.Instance.getApi();

                content.Add(new StreamContent(ms), "file", fileName);

                message.Method = HttpMethod.Post;
                message.Content = content;
                message.RequestUri = new Uri($"{client.BaseAddress.AbsoluteUri}{uriAddon}");


                var res = await client.SendAsync(message);

                //var optimizeRes = await res.Content.ReadAsAsync<OptimizeRes>();


                var fileStr = await res.Content.ReadAsStreamAsync();

                var msFile = new MemoryStream();
                await fileStr.CopyToAsync(msFile);

                var pobytes = msFile.ToArray();

                return pobytes;


                //return optimizeRes;

            }


        }



        public async Task<byte[]> OptimizeAsync( OptimizeActionProfile profile, byte[] document)
        {

            var req = new Optimize()
            {
                Document = new Document()
                {
                    DocData = document
                },
                OptimizeAction = new OptimizeAction()
                {
                    UseProfile = true,
                    Profile = profile,
                }
            };

            var res = await _pdfClient.OptimizeAsync(req);

            if (res != null && res.Document != null)
            {
                return res.Document.DocData;
            }

            return null;
        }

    }
}
