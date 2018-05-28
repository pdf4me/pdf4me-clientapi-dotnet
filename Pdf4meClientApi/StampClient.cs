using Pdf4me.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pdf4meClient
{
    public partial class StampClient
    {

        //public async Task<byte[]> StampAsync(string text, string pages, StampActionAlignX alignX, StampActionAlignY alignY, byte[] document)
        //{

        //    var req = new Stamp()
        //    {
        //        Document = new Document()
        //        {
        //            DocData = document
        //        },
                
        //        StampAction = new StampAction()
        //        {
        //            Text = new Text()
        //            {
        //                Value = text,
                        
        //            },
                    
        //            PageSequence = pages,

        //            AlignX = alignX,
        //            AlignY = alignY
        //        }
        //    };

        //    var res = await StampAsync(req);

        //    if (res != null && res.Document != null)
        //    {
        //        return res.Document.DocData;
        //    }

        //    return null;
        //}

    }
}
