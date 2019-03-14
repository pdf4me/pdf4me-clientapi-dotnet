using System;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;

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
                int statusCode = (int)res.StatusCode;
                if (statusCode == 500)
                {
                    throw new Pdf4meBackendException($"HTTP 500: {res.ReasonPhrase}");
                }
                else if (statusCode != 200 && statusCode != 204)
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
        public async Task<byte[]> ConvertFileToPdfAsync(byte[] file, string fileName)
        {
            return await CustomHttp.postWrapper(
                new List<string> { "fileName", fileName },
                new List<Tuple<byte[], string, string>> { new Tuple<byte[], string, string>(file, "file", fileName) },
                "Convert/ConvertFileToPdf",
                _httpClient);
        }

        public async Task<FileResponse> ConvertFileToPdfAsync(FileParameter file, string jobIdExt = null)
        {
            return await ConvertFileToPdfAsync(file.FileName, jobIdExt, file);
        }

    }

    public partial class ExtractClient
    {
        public async Task<byte[]> ExtractPagesAsync(byte[] file, string pageNrs)
        {
            return await CustomHttp.postWrapper(
                new List<string> { "pageNrs", pageNrs },
                new List<Tuple<byte[], string, string>> { new Tuple<byte[], string, string>(file, "file", "pdf.pdf") },
                "Extract/ExtractPages",
                _httpClient);
        }

        public async Task<FileResponse> ExtractPagesAsync(FileParameter file, string pageNrs, string jobIdExt = null )
        {
            return await ExtractPagesAsync(pageNrs, jobIdExt, file);
        }
    }

    public partial class ImageClient
    {
        public async Task<byte[]> CreateThumbnailAsync(byte[] file, int width, string pageNr, ImageActionImageExtension imageFormat)
        {
            return await CustomHttp.postWrapper(
                new List<string> { "width", width.ToString(), "pageNr", pageNr, "imageFormat", Enum.GetName(typeof(ImageActionImageExtension), imageFormat) },
                new List<Tuple<byte[], string, string>> { new Tuple<byte[], string, string>(file, "file", "pdf.pdf") },
                "Image/CreateThumbnail",
                _httpClient);
        }

        public async Task<FileResponse> CreateThumbnailAsync( FileParameter file, int width, string pageNr, string imageFormat, string jobIdExt = null)
        {
            return await CreateThumbnailAsync(width, pageNr, imageFormat, jobIdExt, file);
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

        public async Task<FileResponse> Merge2PdfsAsync(FileParameter file1, FileParameter file2, string jobIdExt = null)
        {
            return await Merge2PdfsAsync(jobIdExt, file1, file2);
        }
    }

    public partial class OptimizeClient
    {
        public async Task<byte[]> OptimizeByProfileAsync(byte[] file, OptimizeActionProfile profile)
        {
            return await CustomHttp.postWrapper(
                new List<string> { "profile", Enum.GetName(typeof(OptimizeActionProfile), profile) },
                new List<Tuple<byte[], string, string>> { new Tuple<byte[], string, string>(file, "file", "pdf.pdf") },
                "Optimize/OptimizeByProfile",
                _httpClient);
        }

        public async Task<FileResponse> OptimizeByProfileAsync( FileParameter file, Profile profile = Profile.Default, string jobIdExt = null)
        {
            return await OptimizeByProfileAsync(profile, jobIdExt, file);
        }

    }

    public partial class DocumentClient
    {

    }

    public partial class PdfAClient
    {
        public async Task<byte[]> RotatePageAsync(byte[] file, string pageNr, PdfRotateRotationType rotate)
        {
            return await CustomHttp.postWrapper(
                new List<string> { "pageNr", pageNr, "rotate", Enum.GetName(typeof(PdfRotateRotationType), rotate) },
                new List<Tuple<byte[], string, string>> { new Tuple<byte[], string, string>(file, "file", "pdf.pdf") },
                "PdfA/RotatePage",
                _httpClient);
        }

        public async Task<byte[]> RotateDocumentAsync(byte[] file, PdfRotateRotationType rotate)
        {
            return await CustomHttp.postWrapper(
                new List<string> { "rotate", Enum.GetName(typeof(PdfRotateRotationType), rotate) },
                new List<Tuple<byte[], string, string>> { new Tuple<byte[], string, string>(file, "file", "pdf.pdf") },
                "PdfA/RotateDocument",
                _httpClient);
        }

        public async Task<FileResponse> RotateDocumentAsync(FileParameter file, Rotate3? rotate, string jobIdExt = null)
        {
            return await RotateDocumentAsync(rotate, jobIdExt, file);
        }

        public async Task<byte[]> ProtectDocumentAsync(byte[] file, string password, string permissions)
        {
            return await CustomHttp.postWrapper(
                new List<string> { "password", password, "permissions", permissions },
                new List<Tuple<byte[], string, string>> { new Tuple<byte[], string, string>(file, "file", "pdf.pdf") },
                "PdfA/ProtectDocument",
                _httpClient);
        }

        public async Task<FileResponse> ProtectDocumentAsync(FileParameter file, string password, string permissions, string jobIdExt = null)
        {
            return await ProtectDocumentAsync(password, permissions, jobIdExt, file, System.Threading.CancellationToken.None);
        }

        public async Task<byte[]> UnlockDocumentAsync(byte[] file, string password)
        {
            return await CustomHttp.postWrapper(
                new List<string> { "password", password },
                new List<Tuple<byte[], string, string>> { new Tuple<byte[], string, string>(file, "file", "pdf.pdf") },
                "PdfA/UnlockDocument",
                _httpClient);
        }

        public async Task<FileResponse> UnlockDocumentAsync(FileParameter file, string password, string jobIdExt = null)
        {
            return await UnlockDocumentAsync(password, jobIdExt, file);
        }

        public async Task<byte[]> ValidateDocumentAsync(byte[] file, PdfAActionCompliance pdfCompliance)
        {
            return await CustomHttp.postWrapper(
                new List<string> { "pdfCompliance", Enum.GetName(typeof(PdfAActionCompliance), pdfCompliance) },
                new List<Tuple<byte[], string, string>> { new Tuple<byte[], string, string>(file, "file", "pdf.pdf") },
                "PdfA/ValidateDocument",
                _httpClient);
        }

        public async Task<byte[]> RepairDocumentAsync(byte[] file)
        {
            return await CustomHttp.postWrapper(
                new List<string>() { },
                new List<Tuple<byte[], string, string>> { new Tuple<byte[], string, string>(file, "file", "pdf.pdf") },
                "PdfA/RepairDocument",
                _httpClient);
        }

        public async Task<FileResponse> RepairDocumentAsync(FileParameter file, string jobIdExt = null)
        {
            return await RepairDocumentAsync(jobIdExt, file);
        }

        public async Task<byte[]> CreatePdfAAsync(byte[] file, PdfAActionCompliance pdfCompliance)
        {
            return await CustomHttp.postWrapper(
                new List<string> { "pdfCompliance", Enum.GetName(typeof(PdfAActionCompliance), pdfCompliance) },
                new List<Tuple<byte[], string, string>> { new Tuple<byte[], string, string>(file, "file", "pdf.pdf") },
                "PdfA/CreatePdfA",
                _httpClient);
        }

        public async Task<FileResponse> CreatePdfAAsync(FileParameter file, PdfCompliance2? pdfCompliance, string jobIdExt = null)
        {
            return await CreatePdfAAsync(pdfCompliance, jobIdExt, file);
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
        public async Task<List<byte[]>> SplitByPageNrAsync(byte[] file, int pageNr)
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
            return splitRes.Documents.ToList().Select(a => a.DocData).ToList();

            //return new List<byte[]> { splitRes.Documents.fi .DocData, splitRes.Documents[1].DocData };
        }

        public async Task<HashSet<byte[]>> SplitByPageNrAsync(FileParameter file, int pageNr, string jobIdExt = null)
        {
            return await SplitByPageNrAsync(pageNr, jobIdExt, file);
        }

        public async Task<List<byte[]>> SplitRecurringAsync(byte[] file, int pageNr)
        {
            byte[] res = await CustomHttp.postWrapper(
                new List<string> { "pageNr", pageNr.ToString() },
                new List<Tuple<byte[], string, string>> { new Tuple<byte[], string, string>(file, "file", "pdf.pdf") },
                "Split/SplitRecurring",
                _httpClient);


            // desirialization
            string respJson = System.Text.Encoding.Default.GetString(res);
            SplitRes splitRes = Newtonsoft.Json.JsonConvert.DeserializeObject<SplitRes>(respJson);

            // PDF extraction
            return splitRes.Documents.ToList().Select(a => a.DocData).ToList();

            //return new List<byte[]> { splitRes.Documents.fi .DocData, splitRes.Documents[1].DocData };
        }

        public async Task<HashSet<byte[]>> SplitRecurringAsync(FileParameter file, int pageNr, string jobIdExt = null)
        {
            return await SplitRecurringAsync(pageNr, jobIdExt, file);
        }

    }

    public partial class StampClient
    {
        public async Task<byte[]> TextStampAsync(byte[] file, string text, string pages, AlignX alignX, AlignY alignY)
        {
            return await CustomHttp.postWrapper(
                new List<string> { "text", text, "pages", pages, "alignX", Enum.GetName(typeof(AlignX), alignX), "alignY", Enum.GetName(typeof(AlignY), alignY) },
                new List<Tuple<byte[], string, string>> { new Tuple<byte[], string, string>(file, "file", "pdf.pdf") },
                "Stamp/TextStamp",
                _httpClient);
        }

        public async Task<FileResponse> TextStampAsync( FileParameter file, string text, string pages, AlignX alignX, AlignY alignY, string jobIdExt = null)
        {
            return await TextStampAsync(text, pages, alignX, alignY, jobIdExt, file);
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
