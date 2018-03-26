using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pdf4meClient
{
    public class StampClient
    {

        private PdfClient _pdfClient;

        public StampClient(PdfClient pdfClient)
        {
            _pdfClient = pdfClient;
        }

        public async Task<byte[]> StampAsync(string text, string pages, StampActionAlignX alignX, StampActionAlignY alignY, byte[] document)
        {

            var req = new Stamp()
            {
                Document = new Document()
                {
                    DocData = document
                },
                
                StampAction = new StampAction()
                {
                    Text = new Text()
                    {
                        Value = text,
                        
                    },
                    
                    PageSequence = pages,

                    AlignX = alignX,
                    AlignY = alignY
                }
            };

            var res = await _pdfClient.StampAsync(req);

            if (res != null && res.Document != null)
            {
                return res.Document.DocData;
            }

            return null;
        }

    }
}
