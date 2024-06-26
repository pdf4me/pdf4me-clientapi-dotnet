﻿using System;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

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
                    var error_message = await res.Content.ReadAsStringAsync();
                    throw new Pdf4meBackendException($"HTTP 500: {res.ReasonPhrase} : {error_message}");
                }
                else if (statusCode != 200 && statusCode != 204)
                {
                    var error_message = await res.Content.ReadAsStringAsync();
                    throw new Pdf4meBackendException($"HTTP {statusCode}: {res.ReasonPhrase}: {error_message}");
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

        public async Task<Stream> ConvertFileToPdfAsync(FileParameter file, string jobIdExt = null)
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

        public async Task<byte[]> DeletePagesAsync(byte[] file, string pageNrs)
        {
            return await CustomHttp.postWrapper(
                new List<string> { "pageNrs", pageNrs },
                new List<Tuple<byte[], string, string>> { new Tuple<byte[], string, string>(file, "file", "pdf.pdf") },
                "Extract/DeletePages",
                _httpClient);
        }

        //public async Task<Stream> ExtractPagesAsync(FileParameter file, string pageNrs, string jobIdExt = null)
        //{
        //    return await ExtractPagesAsync(pageNrs, jobIdExt, file);
        //}
    }

    public partial class ImageClient
    {
        public async Task<byte[]> CreateThumbnailAsync(byte[] file, int width, string pageNr, ImageExtension imageFormat)
        {
            return await CustomHttp.postWrapper(
                new List<string> { "width", width.ToString(), "pageNr", pageNr, "imageFormat", Enum.GetName(typeof(ImageExtension), imageFormat) },
                new List<Tuple<byte[], string, string>> { new Tuple<byte[], string, string>(file, "file", "pdf.pdf") },
                "Image/CreateThumbnail",
                _httpClient);
        }

        public async Task<List<byte[]>> CreateThumbnailsAsync(byte[] file, int width, string pageNrs, ImageExtension imageFormat)
        {
            var res = await CustomHttp.postWrapper(
                new List<string> { "width", width.ToString(), "pageNrs", pageNrs, "imageFormat", Enum.GetName(typeof(ImageExtension), imageFormat) },
                new List<Tuple<byte[], string, string>> { new Tuple<byte[], string, string>(file, "file", "pdf.pdf") },
                "Image/CreateThumbnails",
                _httpClient);

            // desirialization
            string respJson = System.Text.Encoding.Default.GetString(res);
            return JsonConvert.DeserializeObject<List<byte[]>>(respJson);
        }


        //public async Task<Stream> CreateThumbnailAsync(FileParameter file, int width, string pageNr, string imageFormat, string jobIdExt = null)
        //{
        //    return await CreateThumbnailAsync(width, pageNr, imageFormat, jobIdExt, file);
        //}

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

        public async Task<Stream> Merge2PdfsAsync(FileParameter file1, FileParameter file2, string jobIdExt = null)
        {
            return await Merge2PdfsAsync(jobIdExt, file1, file2);
        }
    }

    public partial class OptimizeClient
    {
        public async Task<byte[]> OptimizeByProfileAsync(byte[] file, PdfOptimizationProfile profile)
        {
            return await CustomHttp.postWrapper(
                new List<string> { "profile", Enum.GetName(typeof(PdfOptimizationProfile), profile) },
                new List<Tuple<byte[], string, string>> { new Tuple<byte[], string, string>(file, "file", "pdf.pdf") },
                "Optimize/OptimizeByProfile",
                _httpClient);
        }

        public async Task<Stream> OptimizeByProfileAsync(FileParameter file, PdfOptimizationProfile profile = PdfOptimizationProfile.Max, string jobIdExt = null)
        {
            return await OptimizeByProfileAsync(profile, file);
        }

    }

    public partial class DocumentClient
    {
        public async Task<UploadFileInfo> GetUploadFileUrl(byte[] docData)
        {
            var uploadUrlInfo = await this.CreateUploadUrlAsync();

            using (var stream = new MemoryStream(docData))
            {
                using (var content = new StreamContent(stream))
                {
                    using (var requestMessage = new HttpRequestMessage(HttpMethod.Put, uploadUrlInfo.UploadUrl))
                    {
                        requestMessage.Headers.Add("x-ms-blob-type", "BlockBlob");
                        //requestMessage.Headers.Add("Content-Type", "application/octet-stream");
                        requestMessage.Content = content;

                        using (var docHttpClient = new HttpClient())
                        {
                            docHttpClient.Timeout = new TimeSpan(0, 10, 0);
                            using (var httpResponse = await docHttpClient.SendAsync(requestMessage).ConfigureAwait(false))
                            {
                                var resString = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                            }
                        }
                    }
                }
            }
            return uploadUrlInfo;
        }
    }

    public partial class PdfAClient
    {
        public async Task<byte[]> RotatePageAsync(byte[] file, string pageNr, RotationType rotate)
        {
            return await CustomHttp.postWrapper(
                new List<string> { "pageNr", pageNr, "rotate", Enum.GetName(typeof(RotationType), rotate) },
                new List<Tuple<byte[], string, string>> { new Tuple<byte[], string, string>(file, "file", "pdf.pdf") },
                "PdfA/RotatePage",
                _httpClient);
        }

        public async Task<byte[]> RotateDocumentAsync(byte[] file, RotationType rotate)
        {
            return await CustomHttp.postWrapper(
                new List<string> { "rotate", Enum.GetName(typeof(RotationType), rotate) },
                new List<Tuple<byte[], string, string>> { new Tuple<byte[], string, string>(file, "file", "pdf.pdf") },
                "PdfA/RotateDocument",
                _httpClient);
        }

        public async Task<Stream> RotateDocumentAsync(FileParameter file, RotationType? rotate, string jobIdExt = null)
        {
            return await RotateDocumentAsync(rotate, file);
        }

        public async Task<byte[]> ProtectDocumentAsync(byte[] file, string password, string permissions)
        {
            return await CustomHttp.postWrapper(
                new List<string> { "password", password, "permissions", permissions },
                new List<Tuple<byte[], string, string>> { new Tuple<byte[], string, string>(file, "file", "pdf.pdf") },
                "PdfA/ProtectDocument",
                _httpClient);
        }

        //public async Task<Stream> ProtectDocumentAsync(FileParameter file, string password, string permissions, string jobIdExt = null)
        //{
        //    return await ProtectDocumentAsync(password, permissions, jobIdExt, file, System.Threading.CancellationToken.None);
        //}

        public async Task<byte[]> UnlockDocumentAsync(byte[] file, string password)
        {
            return await CustomHttp.postWrapper(
                new List<string> { "password", password },
                new List<Tuple<byte[], string, string>> { new Tuple<byte[], string, string>(file, "file", "pdf.pdf") },
                "PdfA/UnlockDocument",
                _httpClient);
        }

        //public async Task<Stream> UnlockDocumentAsync(FileParameter file, string password, string jobIdExt = null)
        //{
        //    return await UnlockDocumentAsync(password, jobIdExt, file);
        //}

        public async Task<ValidateRes> ValidateDocumentAsync(byte[] file, PdfCompliance pdfCompliance)
        {
            byte[] res = await CustomHttp.postWrapper(
                new List<string> { "pdfCompliance", Enum.GetName(typeof(PdfCompliance), pdfCompliance) },
                new List<Tuple<byte[], string, string>> { new Tuple<byte[], string, string>(file, "file", "pdf.pdf") },
                "PdfA/ValidateDocument",
                _httpClient);

            // desirialization
            string respJson = System.Text.Encoding.Default.GetString(res);
            return JsonConvert.DeserializeObject<ValidateRes>(respJson);
        }

        public async Task<byte[]> RepairDocumentAsync(byte[] file)
        {
            return await CustomHttp.postWrapper(
                new List<string>() { },
                new List<Tuple<byte[], string, string>> { new Tuple<byte[], string, string>(file, "file", "pdf.pdf") },
                "PdfA/RepairDocument",
                _httpClient);
        }

        public async Task<Stream> RepairDocumentAsync(FileParameter file, string jobIdExt = null)
        {
            return await RepairDocumentAsync(jobIdExt, file);
        }

        public async Task<byte[]> CreatePdfAAsync(byte[] file, PdfCompliance pdfCompliance)
        {
            return await CustomHttp.postWrapper(
                new List<string> { "pdfCompliance", Enum.GetName(typeof(PdfCompliance), pdfCompliance) },
                new List<Tuple<byte[], string, string>> { new Tuple<byte[], string, string>(file, "file", "pdf.pdf") },
                "PdfA/CreatePdfA",
                _httpClient);
        }

        //public async Task<Stream> CreatePdfAAsync(FileParameter file, PdfCompliance2? pdfCompliance, string jobIdExt = null)
        //{
        //    return await CreatePdfAAsync(pdfCompliance, jobIdExt, file);
        //}

        public async Task<byte[]> CreatePdfAAsync(byte[] file)
        {
            return await CustomHttp.postWrapper(
                new List<string> { "pdfCompliance", Enum.GetName(typeof(PdfCompliance), PdfCompliance.PdfA2b) },
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
            return JsonConvert.DeserializeObject<List<byte[]>>(respJson);
        }

        //public async Task<HashSet<byte[]>> SplitByPageNrAsync(FileParameter file, int pageNr, string jobIdExt = null)
        //{
        //    return await SplitByPageNrAsync(pageNr, jobIdExt, null, file);
        //}

        public async Task<List<byte[]>> SplitRecurringAsync(byte[] file, int pageNr)
        {
            byte[] res = await CustomHttp.postWrapper(
                new List<string> { "pageNr", pageNr.ToString() },
                new List<Tuple<byte[], string, string>> { new Tuple<byte[], string, string>(file, "file", "pdf.pdf") },
                "Split/SplitRecurring",
                _httpClient);


            // desirialization
            string respJson = System.Text.Encoding.Default.GetString(res);
            return JsonConvert.DeserializeObject<List<byte[]>>(respJson);
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

        public async Task<Stream> TextStampAsync(FileParameter file, string text, string pages, AlignX alignX, AlignY alignY, string jobIdExt = null)
        {
            var _alignX = (AlignX)Enum.Parse(typeof(AlignX), alignX.ToString(), true);
            var _alignY = (AlignY)Enum.Parse(typeof(AlignY), alignY.ToString(), true);
            return await TextStampAsync(text, pages, _alignX, _alignY, jobIdExt, file);
        }

    }

    public partial class BarcodeClient
    {
        public async Task<ReadBarcodesRes> ReadBarcodesByTypeAsync(byte[] file, BarcodeReadType barcodeType)
        {
            byte[] res = await CustomHttp.postWrapper(
                new List<string> { "barcodeType", barcodeType.ToString() },
                new List<Tuple<byte[], string, string>> { new Tuple<byte[], string, string>(file, "file", "pdf.pdf") },
                "Barcode/ReadBarcodesByType",
                _httpClient);

            // desirialization
            string respJson = System.Text.Encoding.Default.GetString(res);
            return JsonConvert.DeserializeObject<ReadBarcodesRes>(respJson);
        }

        public async Task<ReadBarcodesRes> ReadBarcodesByTypeAsync(FileParameter file, BarcodeReadType barcodeType, string jobIdExt = null)
        {
            return await ReadBarcodesByTypeAsync(barcodeType, jobIdExt, file);
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
