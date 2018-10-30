﻿using System;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Collections.Generic;

namespace Pdf4meClient
{

    static class CustomHttp
    {
        public static async Task<byte[]> postWrapper(List<string> qparams, List<Tuple<byte[], string, string>> files, string controller, HttpClient client)
        {
            var message = new HttpRequestMessage();
            var content = new MultipartFormDataContent();

            // build the qparams
            string urlParams = "";
            for (int i = 0; i < qparams.Count; i += 2)
            {
                String elem = $"{qparams[i]}={qparams[i + 1]}";
                if (i == 0)
                {
                    urlParams += elem;
                }
                else
                {
                    urlParams += $"&{elem}";
                }
            }

            string uriAddon = $"{controller}";
            if (urlParams != "")
            {
                uriAddon += $"?{urlParams}";
            }

            MemoryStream[] ms = new MemoryStream[files.Count];
            try
            {
                // prepare memoryStreams for file uploads
                for (int i = 0; i < files.Count; i++)
                {
                    ms[i] = new MemoryStream(files[i].Item1);
                    int temp = i + 1;
                    content.Add(new StreamContent(ms[i]), files[i].Item2, files[i].Item3);
                }

                // prepare request
                message.Method = HttpMethod.Post;
                message.Content = content;
                message.RequestUri = new Uri($"{client.BaseAddress.AbsoluteUri}{uriAddon}");

                // send request
                var res = await client.SendAsync(message).ConfigureAwait(false);

                // check response
                int statusCode = (int) res.StatusCode;
                if(statusCode == 500)
                {
                    throw new Pdf4meBackendException($"HTTP 500: {res.ReasonPhrase}");
                }else if(statusCode != 200 && statusCode != 204)
                {
                    throw new Pdf4meBackendException($"HTTP {statusCode}: {res.ReasonPhrase}");
                }

                // extract response
                return await res.Content.ReadAsByteArrayAsync();
            }
            finally
            {
                // Dispose of all MemoryStreams
                for (int i = 0; i < ms.Length; i++)
                {
                    if (ms[i] != null)
                    {
                        ms[i].Dispose();
                    }
                }
            }
        }
    }

    public partial class ConvertClient
    {
        public async Task<byte[]> ConvertFileToPdfAsync(string fileName, byte[] file)
        {
            return await CustomHttp.postWrapper(
                new List<string> { "fileName", fileName },
                new List<Tuple<byte[], string, string>> { new Tuple<byte[], string, string>(file, "file", fileName) },
                "Convert/ConvertFileToPdf",
                _httpClient);
        }
    }

    public partial class ExtractClient
    {
        public async Task<byte[]> ExtractPagesAsync(string pageNrs, byte[] file)
        {
            return await CustomHttp.postWrapper(
                new List<string> { "pageNrs", pageNrs },
                new List<Tuple<byte[], string, string>> { new Tuple<byte[], string, string>(file, "file", "pdf.pdf") },
                "Extract/ExtractPages",
                _httpClient);
        }
    }

    public partial class ImageClient
    {
        public async Task<byte[]> CreateThumbnailAsync(int width, string pageNr, ImageActionImageExtension imageFormat, byte[] file)
        {
            return await CustomHttp.postWrapper(
                new List<string> { "width", width.ToString(), "pageNr", pageNr, "imageFormat", Enum.GetName(typeof(ImageActionImageExtension), imageFormat) },
                new List<Tuple<byte[], string, string>> { new Tuple<byte[], string, string>(file, "file", "pdf.pdf") },
                "Image/CreateThumbnail",
                _httpClient);
        }
    }

    public partial class MergeClient
    {
        public async Task<byte[]> Merge2PdfsAsync(byte[] file1, byte[] file2)
        {
            return await CustomHttp.postWrapper(
                new List<string> { },
                new List<Tuple<byte[], string, string>> { new Tuple<byte[], string, string>(file1, "file1", "pdf1.pdf"), new Tuple<byte[], string, string>(file2, "file2", "pdf2.pdf") },
                "Merge/Merge2Pdfs",
                _httpClient);
        }
    }

    public partial class OptimizeClient
    {
        public async Task<byte[]> OptimizeByProfileAsync(OptimizeActionProfile profile, byte[] file)
        {
            return await CustomHttp.postWrapper(
                new List<string> { "profile", Enum.GetName(typeof(OptimizeActionProfile), profile) },
                new List<Tuple<byte[], string, string>> { new Tuple<byte[], string, string>(file, "file", "pdf.pdf") },
                "Optimize/OptimizeByProfile",
                _httpClient);
        }
    }

    public partial class PdfAClient
    {
        public async Task<byte[]> CreatePdfAAsync(PdfAActionCompliance pdfCompliance, byte[] file)
        {
            return await CustomHttp.postWrapper(
                new List<string> { "pdfCompliance", Enum.GetName(typeof(PdfAActionCompliance), pdfCompliance) },
                new List<Tuple<byte[], string, string>> { new Tuple<byte[], string, string>(file, "file", "pdf.pdf") },
                "PdfA/CreatePdfA",
                _httpClient);
        }

        public async Task<byte[]> CreatePdfAAsync(byte[] file)
        {
            return await CustomHttp.postWrapper(
                new List<string> { "pdfCompliance", Enum.GetName(typeof(PdfAActionCompliance), PdfAActionCompliance.PdfA2b) },
                new List<Tuple<byte[], string, string>> { new Tuple<byte[], string, string>(file, "file", "pdf.pdf") },
                "PdfA/CreatePdfA",
                _httpClient);
        }
    }

    public partial class SplitClient
    {
        public async Task<List<byte[]>> SplitByPageNrAsync(int pageNr, byte[] file)
        {
            byte[] res = await CustomHttp.postWrapper(
                new List<string> { "pageNr", pageNr.ToString() },
                new List<Tuple<byte[], string, string>> { new Tuple<byte[], string, string>(file, "file", "pdf.pdf") },
                "Split/SplitByPageNr",
                _httpClient);


            // desirialization
            string respJson = System.Text.Encoding.Default.GetString(res);
            SplitRes splitRes = Newtonsoft.Json.JsonConvert.DeserializeObject<SplitRes>(respJson);

            // PDF extraction
            return new List<byte[]> { splitRes.Documents[0].DocData, splitRes.Documents[1].DocData };
        }
    }

    public partial class StampClient
    {
        public async Task<byte[]> TextStampAsync(string text, string pages, AlignX alignX, AlignY alignY, byte[] file)
        {
            return await CustomHttp.postWrapper(
                new List<string> { "text", text, "pages", pages, "alignX", Enum.GetName(typeof(AlignX), alignX), "alignY", Enum.GetName(typeof(AlignY), alignY) },
                new List<Tuple<byte[], string, string>> { new Tuple<byte[], string, string>(file, "file", "pdf.pdf") },
                "Stamp/TextStamp",
                _httpClient);
        }
    }

    public class Pdf4meBackendException : Exception
    {
        public Pdf4meBackendException()
        {
        }

        public Pdf4meBackendException(string message)
            : base(message)
        {
        }

        public Pdf4meBackendException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
