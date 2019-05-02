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
            };

            if (action is OptimizeAction)
                pdf4meAction.ActionType = Pdf4meActionActionType.Optimize;

            if (action is ConvertToPdfAction)
                pdf4meAction.ActionType = Pdf4meActionActionType.ConvertToPdf;

            //if (action is ConvertAction)
            //    pdf4meAction.ActionType = Pdf4meActionActionType.Converter;

            //if (action is CreateBarcodeAction)
            //    pdf4meAction.ActionType = Pdf4meActionActionType.CreateBarcode;

            if (action is ReadBarcodeAction)
                pdf4meAction.ActionType = Pdf4meActionActionType.ReadBarcode;

            if (action is ExtractAction)
                pdf4meAction.ActionType = Pdf4meActionActionType.Extract;

            if (action is ExtractResourcesAction)
                pdf4meAction.ActionType = Pdf4meActionActionType.ExtractResources;

            if (action is ImageAction)
                pdf4meAction.ActionType = Pdf4meActionActionType.Image;

            if (action is MergeAction)
                pdf4meAction.ActionType = Pdf4meActionActionType.Merge;

            if (action is OcrAction)
                pdf4meAction.ActionType = Pdf4meActionActionType.Ocr;

            if (action is PdfAAction)
                pdf4meAction.ActionType = Pdf4meActionActionType.PdfA;

            if (action is ProtectAction)
                pdf4meAction.ActionType = Pdf4meActionActionType.Protect;

            if (action is RepairAction)
                pdf4meAction.ActionType = Pdf4meActionActionType.Repair;

            if (action is RotateAction)
                pdf4meAction.ActionType = Pdf4meActionActionType.Rotate;

            if (action is SignAction)
                pdf4meAction.ActionType = Pdf4meActionActionType.Sign;

            if (action is SplitAction)
                pdf4meAction.ActionType = Pdf4meActionActionType.Split;

            if (action is StampAction)
                pdf4meAction.ActionType = Pdf4meActionActionType.Stamp;

            //if (action is CreateThumbnailAction)
            //    pdf4meAction.ActionType = Pdf4meActionActionType.Thumbnail;

            //if (action is UserAction)
            //    pdf4meAction.ActionType = Pdf4meActionActionType.User;


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
