using Newtonsoft.Json;
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

        public static void AddAction<T>(this ActionFlow actionFlow, T action)
        {
            if (actionFlow.Actions == null)
                actionFlow.Actions = new HashSet<Pdf4meAction>();

            var pdf4meAction = new Pdf4meAction()
            {
                ActionConfig = JsonConvert.SerializeObject(action),
                //ActionType = Pdf4meActionActionType.Optimize
            };

            switch(typeof(T).GetType().Name.ToString())
            {
                case "OptimizeAction":
                    pdf4meAction.ActionType = Pdf4meActionActionType.Optimize;
                    break;
                case "ConvertToPdfAction":
                    pdf4meAction.ActionType = Pdf4meActionActionType.ConvertToPdf;
                    break;
                case "ConverterAction":
                    pdf4meAction.ActionType = Pdf4meActionActionType.Converter;
                    break;
                case "CreateBarcodeAction":
                    pdf4meAction.ActionType = Pdf4meActionActionType.CreateBarcode;
                    break;
                case "ReadBarcodeAction":
                    pdf4meAction.ActionType = Pdf4meActionActionType.ReadBarcode;
                    break;
                case "ExtractAction":
                    pdf4meAction.ActionType = Pdf4meActionActionType.Extract;
                    break;
                case "ExtractResourcesAction":
                    pdf4meAction.ActionType = Pdf4meActionActionType.ExtractResources;
                    break;
                case "ImageAction":
                    pdf4meAction.ActionType = Pdf4meActionActionType.Image;
                    break;
                case "MergeAction":
                    pdf4meAction.ActionType = Pdf4meActionActionType.Merge;
                    break;
                case "OcrAction":
                    pdf4meAction.ActionType = Pdf4meActionActionType.Ocr;
                    break;
                case "PdfAAction":
                    pdf4meAction.ActionType = Pdf4meActionActionType.PdfA;
                    break;
                case "ProtectAction":
                    pdf4meAction.ActionType = Pdf4meActionActionType.Protect;
                    break;
                case "RepairAction":
                    pdf4meAction.ActionType = Pdf4meActionActionType.Repair;
                    break;
                case "RotateAction":
                    pdf4meAction.ActionType = Pdf4meActionActionType.Rotate;
                    break;
                case "SignAction":
                    pdf4meAction.ActionType = Pdf4meActionActionType.Sign;
                    break;
                case "SplitAction":
                    pdf4meAction.ActionType = Pdf4meActionActionType.Split;
                    break;
                case "StampAction":
                    pdf4meAction.ActionType = Pdf4meActionActionType.Stamp;
                    break;
                case "ThumbnailAction":
                    pdf4meAction.ActionType = Pdf4meActionActionType.Thumbnail;
                    break;
                case "UserAction":
                    pdf4meAction.ActionType = Pdf4meActionActionType.User;
                    break;


            }

            actionFlow.Actions.Add(pdf4meAction);

        }

       

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
