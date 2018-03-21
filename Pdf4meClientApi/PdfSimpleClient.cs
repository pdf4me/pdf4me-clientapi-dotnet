using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pdf4me.Client
{
    public class PdfSimpleClient
    {

        private PdfClient _pdfClient;

        public PdfSimpleClient(PdfClient pdfClient)
        {
            _pdfClient = pdfClient;
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
